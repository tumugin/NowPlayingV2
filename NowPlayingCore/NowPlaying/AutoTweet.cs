using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NowPlayingCore.Core;

namespace NowPlayingCore.NowPlaying
{
    public class AutoTweet
    {
        private ConfigBase appConfig = default!;
        private Task? tweetTask;
        private readonly AutoResetEvent resetEvent = new AutoResetEvent(false);
        private CancellationTokenSource cancellationToken = new CancellationTokenSource();
        private SongInfo? lastPlayedSong;
        private DateTime lastTweetTime;
        private PipeListener? pipeListener;
        private static AutoTweet? singletonAutoTweet;

        public static AutoTweet AutoTweetSingleton => singletonAutoTweet ?? (singletonAutoTweet = new AutoTweet());
        public bool IsInitialized { get; private set; } = false;

        public void InitListner(PipeListener pl, ConfigBase cb)
        {
            if (IsInitialized)
            {
                throw new Exception("DO NOT RE-INITIALIZE.");
            }

            pipeListener = pl;
            pipeListener.OnMusicPlay += OnMusicPlay;
            appConfig = cb;
            IsInitialized = true;
        }

        public void UnInitListner()
        {
            if (pipeListener != null)
            {
                pipeListener.OnMusicPlay -= OnMusicPlay;
            }

            IsInitialized = false;
        }

        private void OnMusicPlay(SongInfo songInfo)
        {
            Task.Run(async () =>
            {
                if (!appConfig.EnableAutoTweet)
                {
                    return;
                }

                if (appConfig.EnablePostDelay && !(tweetTask?.IsCompleted ?? true))
                {
                    //cancel tweet job
                    Trace.WriteLine($"[AutoTweet PostDelay]Cancel tweet task start.");
                    cancellationToken.Cancel();
                    resetEvent.Set();
                    if (tweetTask != null)
                    {
                        await tweetTask;
                    }

                    Trace.WriteLine($"[AutoTweet PostDelay]Cancel tweet task OK.");
                }

                //can't reuse this
                cancellationToken = new CancellationTokenSource();
                //start task
                tweetTask = Task.Run(() =>
                {
                    try
                    {
                        //check for album art
                        var enableAlbumArt = appConfig.EnableTweetWithAlbumArt;
                        if (appConfig.EnableNoAlbumArtworkOnSameAlbum)
                        {
                            if (songInfo.Album == lastPlayedSong?.Album)
                            {
                                Trace.WriteLine($"[AutoTweet]Disabled album art tweet.(reason=same album)");
                                enableAlbumArt = false;
                            }
                        }

                        //same album check
                        if (appConfig.EnableNoTweetOnSameAlbum)
                        {
                            if (songInfo.Album == lastPlayedSong?.Album)
                            {
                                Trace.WriteLine($"[AutoTweet]Canceled tweet.(reason=EnableNoTweetOnSameAlbum)");
                                return;
                            }
                        }

                        //post delay
                        if (appConfig.EnablePostDelay)
                        {
                            Trace.WriteLine(
                                $"[AutoTweet PostDelay]Waiting for {appConfig.PostDelaySecond * 1000}msec.");
                            resetEvent.WaitOne(appConfig.PostDelaySecond * 1000);
                        }

                        //post delay(by last tweet time)
                        if (appConfig.EnableTimePostDelay)
                        {
                            if (lastTweetTime != default(DateTime))
                            {
                                if (!(DateTime.Now - lastTweetTime >=
                                      new TimeSpan(0, 0, appConfig.TimePostDelayMin, appConfig.TimePostDelaySec)))
                                {
                                    Trace.WriteLine($"[AutoTweet]Canceled tweet.(reason=EnableTimePostDelay)");
                                    return;
                                }
                            }
                        }

                        cancellationToken.Token.ThrowIfCancellationRequested();
                        lastPlayedSong = (SongInfo) songInfo.Clone();
                        lastTweetTime = DateTime.Now;

                        //tweet it!
                        try
                        {
                            //get account list(only enabled)
                            appConfig.accountList.Where(itm => itm.Enabled).ToList().ForEach(async (accCont) =>
                            {
                                //make tweet string
                                var tweetText = Tsumugi.TweetConverter.SongInfoToString(appConfig.TweetFormat,
                                    songInfo,
                                    appConfig.EnableAutoDeleteText140, accCont);
                                if (accCont.CountText(tweetText) > accCont.MaxTweetLength)
                                    throw new Exception(
                                        $"[AutoTweet]Tweet text was over {accCont.MaxTweetLength} chars.");
                                //tweet
                                if (enableAlbumArt && songInfo.IsAlbumArtAvaliable())
                                {
                                    if (accCont is MastodonAccount mastodonAccount)
                                    {
                                        await mastodonAccount.UpdateStatus(tweetText, songInfo.AlbumArtBase64,
                                            appConfig.MastodonTootVisibility);
                                    }
                                    else
                                    {
                                        await accCont.UpdateStatus(tweetText, songInfo.AlbumArtBase64);
                                    }
                                }
                                else
                                {
                                    if (accCont is MastodonAccount mastodonAccount)
                                    {
                                        await mastodonAccount.UpdateStatus(tweetText, appConfig.MastodonTootVisibility);
                                    }
                                    else
                                    {
                                        await accCont.UpdateStatus(tweetText);
                                    }
                                }

                                Trace.WriteLine($"[AutoTweet]Sent tweet for account @{accCont.ID}.");
                            });
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine($"[AutoTweet]Tweet error.\n{ex.Message}");
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        Trace.WriteLine($"[AutoTweet] OperationCanceledException");
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine($"[AutoTweet] {ex.Message}");
                    }
                });
            });
        }

        public void StopAllTask()
        {
            cancellationToken.Cancel();
            resetEvent.Set();
        }
    }
}