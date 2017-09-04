using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowPlayingV2.NowPlaying
{
    public class SongInfo
    {
        [JsonProperty("title")]
        public string Title { get; protected set; }
        [JsonProperty("albumartist")]
        public string AlbumArtist { get; protected set; }
        [JsonProperty("album")]
        public string Album { get; protected set; }
        [JsonProperty("trackcount")]
        public string TrackCount { get; protected set; }
        [JsonProperty("albumart")]
        public string AlbumArtBase64 { get; protected set; }
        [JsonProperty("artist")]
        public string Artist { get; protected set; }
    }
}
