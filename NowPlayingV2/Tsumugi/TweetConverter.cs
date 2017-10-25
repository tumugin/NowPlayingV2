using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NowPlayingV2.Matsuri;
using NowPlayingV2.NowPlaying;
using NowPlayingV2.UI.Extension;

namespace NowPlayingV2.Tsumugi
{
    public class TweetConverter
    {
        public static string SongInfoToString(string pattern, SongInfo songinfo)
        {
            var npstr = pattern;
            npstr = npstr.Replace("$Title", songinfo.Title);
            npstr = npstr.Replace("$AlbumArtist", songinfo.AlbumArtist);
            npstr = npstr.Replace("$Album", songinfo.Album);
            npstr = npstr.Replace("$PCount", songinfo.TrackCount);
            npstr = npstr.Replace("$Artist", songinfo.Artist);
            npstr = npstr.Replace("$Composer", songinfo.Composer);
            return npstr;
        }

        public static string SongInfoToString(string pattern, SongInfo songInfo, bool enableAutoShorten)
        {
            var tweet = SongInfoToString(pattern, songInfo);
            if (enableAutoShorten && SeaSlug.CountText(tweet) > 140)
            {
                tweet = tweet.Remove(140 - 3);
                tweet += "...";
            }
            return tweet;
        }
    }
}
