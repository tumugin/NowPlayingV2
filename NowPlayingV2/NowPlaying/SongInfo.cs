using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowPlayingV2.NowPlaying
{
    public class SongInfo
    {
        public string Title { get; protected set; }
        public string AlbumArtist { get; protected set; }
        public string Album { get; protected set; }
        public string TrackCount { get; protected set; }
        public string AlbumArtBase64 { get; protected set; }
        public string Artist { get; protected set; }
    }
}
