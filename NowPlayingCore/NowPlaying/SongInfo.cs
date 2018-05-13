using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NowPlayingCore.NowPlaying
{
    [Serializable()]
    public class SongInfo : ICloneable
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
        [JsonProperty("composer")]
        public string Composer { get; protected set; }
        [JsonProperty("year")]
        public string Year { get; protected set; }
        [JsonProperty("group")]
        public string Group { get; protected set; }

        private Bitmap cachebitmap = null;

        public Bitmap GetAlbumArt()
        {
            if (cachebitmap != null) return cachebitmap;
            var bary = Convert.FromBase64String(AlbumArtBase64);
            var ms = new MemoryStream(bary);
            return new Bitmap(ms);
        }

        public bool IsAlbumArtAvaliable()
        {
            if (AlbumArtBase64.Length == 0) return false;
            if (cachebitmap != null) return true;
            try
            {
                var bary = Convert.FromBase64String(AlbumArtBase64);
                var ms = new MemoryStream(bary);
                cachebitmap = new Bitmap(ms);
                return true;
            }
            catch {
                return false;
            }
        }

        public object Clone()
        {
            using (var ms = new MemoryStream())
            {
                var bformatter = new BinaryFormatter();
                bformatter.Serialize(ms,this);
                ms.Position = 0;
                return bformatter.Deserialize(ms);
            }
        }
    }
}
