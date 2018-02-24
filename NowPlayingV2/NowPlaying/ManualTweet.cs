using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NowPlayingV2.Core;

namespace NowPlayingV2.NowPlaying
{
    public class ManualTweet
    {
        public static Task RunManualTweet()
        {
            return Task.Run(() =>
            {
                //check playing song
                if (!(NowPlaying.PipeListener.staticpipelistener.LastPlayedSong?.Clone() is SongInfo songcache))
                    throw new Exception("何を再生されていなかったため、投稿できませんでした。");
                //list up all accounts
                if (!ConfigStore.StaticConfig.accountList.Any())
                    throw new Exception("アカウントが何も追加されていません。アカウントを追加してからこの操作を行ってください。");
                if (!ConfigStore.StaticConfig.accountList.Any(itm => itm.Enabled))
                    throw new Exception("有効化されているアカウントが一つもありません。最低限一つのアカウントを有効化してください。");
                ConfigStore.StaticConfig.accountList.Where(itm => itm.Enabled).ToList().ForEach(account =>
                {
                    //make tweet string
                    var tweettext = Tsumugi.TweetConverter.SongInfoToString(ConfigStore.StaticConfig.TweetFormat,
                        songcache,
                        ConfigStore.StaticConfig.EnableAutoDeleteText140, account);
                    if (account.CountText(tweettext) > account.MaxTweetLength)
                        throw new Exception($"投稿可能な最大文字数({account.MaxTweetLength}文字)を超えたため、投稿出来ませんでした。");
                    //tweet
                    if (ConfigStore.StaticConfig.EnableTweetWithAlbumArt)
                    {
                        account.UpdateStatus(tweettext, songcache.AlbumArtBase64);
                    }
                    else
                    {
                        account.UpdateStatus(tweettext);
                    }
                });
            });
        }
    }
}