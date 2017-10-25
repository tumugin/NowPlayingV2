using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NowPlayingV2.Core;
using NowPlayingV2.Matsuri;

namespace NowPlayingV2.NowPlaying
{
    public class AutoTweet
    {
        public AutoTweet Autotweetsigleton = new AutoTweet();
        private Config appconfig => ConfigStore.StaticConfig;
        private Task tweetTask;
        private AutoResetEvent resetevent = new AutoResetEvent(false);
        private CancellationTokenSource cancellationToken = new CancellationTokenSource();
        private SongInfo lastplayedsong;

        public void InitListner(PipeListener pl)
        {
            pl.OnMusicPlay += OnMusicPlay;
        }

        private void OnMusicPlay(SongInfo songInfo)
        {
            Task.Run(() =>
            {
                if (!appconfig.EnableAutoTweet) return;
                if (appconfig.EnablePostDelay && !tweetTask.IsCompleted)
                {
                    //cancel tweet job
                    cancellationToken.Cancel();
                    resetevent.Set();
                    tweetTask.Wait();
                }
                //can't reuse this
                cancellationToken = new CancellationTokenSource();
                //start task
                tweetTask = Task.Run(() =>
                {
                    try
                    {
                        //make tweet string
                        var tweettext = Tsumugi.TweetConverter.SongInfoToString(appconfig.TweetFormat, songInfo,appconfig.EnableAutoDeleteText140);
                        if (SeaSlug.CountText(tweettext) > 140) throw new Exception("Tweet text was over 140 chars.");
                        //check for album art
                        var enablealbumart = appconfig.EnableTweetWithAlbumArt;
                        if (appconfig.EnableNoAlbumArtworkOnSameAlbum)
                        {
                            if (songInfo.Album == lastplayedsong?.Album) enablealbumart = false;
                        }
                        //same album check
                        if (appconfig.EnableNoTweetOnSameAlbum)
                        {
                            if (songInfo.Album == lastplayedsong?.Album) return;
                        }
                        //post delay
                        if(appconfig.EnablePostDelay) resetevent.WaitOne(appconfig.PostDelaySecond * 1000);
                        cancellationToken.Token.ThrowIfCancellationRequested();
                        lastplayedsong = (SongInfo) songInfo.Clone();
                        //tweet it!
                        Task.Run(() =>
                        {
                            try
                            {
                                //get account list(only enabled)
                                appconfig.accountList.Where(itm => itm.Enabled).ToList().ForEach((accCont) =>
                                {
                                    var acc = accCont.AuthToken;
                                    var imgresult = acc.Media.Upload(media_data: songInfo.AlbumArtBase64);
                                    acc.Statuses.Update(status: tweettext, media_ids: new[] {imgresult.MediaId});
                                });
                            }
                            catch
                            {
                            }
                        });
                    }
                    catch (OperationCanceledException)
                    {
                    }
                });
            });
        }

        public void StopAllTask()
        {
            cancellationToken.Cancel();
            resetevent.Set();
        }
    }
}