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
        [JsonProperty("title")] public string Title { get; protected set; }
        [JsonProperty("albumartist")] public string AlbumArtist { get; protected set; }
        [JsonProperty("album")] public string Album { get; protected set; }
        [JsonProperty("trackcount")] public string TrackCount { get; protected set; }
        [JsonProperty("albumart")] public string AlbumArtBase64 { get; protected set; }
        [JsonProperty("albumartpath")] public string AlbumArtPath { get; protected set; }
        [JsonProperty("artist")] public string Artist { get; protected set; }
        [JsonProperty("composer")] public string Composer { get; protected set; }
        [JsonProperty("year")] public string Year { get; protected set; }
        [JsonProperty("group")] public string Group { get; protected set; }

        private Bitmap? cacheBitmap = null;

#nullable disable
        public SongInfo()
        {
        }
#nullable enable

#nullable disable
        public SongInfo(Dictionary<String, String> kvp)
        {
            kvp.ToList().ForEach(item =>
            {
                var property = GetType().GetProperty(item.Key);
                property.SetValue(this, item.Value);
            });
        }
#nullable enable

        public Bitmap? GetAlbumArt()
        {
            if (cacheBitmap != null)
            {
                return cacheBitmap;
            }

            if (!IsAlbumArtAvaliable())
            {
                return null;
            }

            CreateAlbumArt();
            return cacheBitmap;
        }

        public bool IsAlbumArtAvaliable()
        {
            if (String.IsNullOrEmpty(AlbumArtBase64) && String.IsNullOrEmpty(AlbumArtPath))
            {
                return false;
            }

            if (cacheBitmap != null)
            {
                return true;
            }

            try
            {
                CreateAlbumArt();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void CreateAlbumArt()
        {
            if (!String.IsNullOrEmpty(AlbumArtPath))
            {
                var bary = File.ReadAllBytes(AlbumArtPath);
                var ms = new MemoryStream(bary);
                cacheBitmap = new Bitmap(ms);
            }
            else
            {
                var bary = Convert.FromBase64String(AlbumArtBase64);
                var ms = new MemoryStream(bary);
                cacheBitmap = new Bitmap(ms);
            }
        }

        public object Clone()
        {
            using (var ms = new MemoryStream())
            {
                var bformatter = new BinaryFormatter();
                bformatter.Serialize(ms, this);
                ms.Position = 0;
                return bformatter.Deserialize(ms);
            }
        }
    }
}