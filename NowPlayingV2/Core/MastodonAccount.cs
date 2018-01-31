using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastonet;
using Newtonsoft.Json;

namespace NowPlayingV2.Core
{
    public class MastodonAccount : AccountContainer
    {
        public MastodonClient mastodonClient;

        //Will be called on Json.NET deserialization
        private MastodonAccount()
        {
        }

        public MastodonAccount(MastodonClient client)
        {
            mastodonClient = client;
            UpdateCache();
        }

        [JsonProperty] private string IDCache = "";
        public override string ID => $"{IDCache}({mastodonClient.Instance})";

        public override void UpdateCache()
        {
            var account = mastodonClient.GetCurrentUser().Result;
            IDCache = account.UserName;
            Name = account.DisplayName;
        }

        public override void UpdateStatus(string UpdateText)
        {
            mastodonClient.PostStatus(UpdateText, Visibility.Public).Wait();
        }

        public override void UpdateStatus(string UpdateText, string base64image)
        {
            byte[] data = System.Convert.FromBase64String(base64image);
            MemoryStream ms = new MemoryStream(data);
            var filetype = Matsuri.ImageTool.GetFileTypeFromBytes(data);
            var attachment = mastodonClient.UploadMedia(ms, $"nowplaying.{filetype}").Result;
            ms.Dispose();
            mastodonClient.PostStatus(UpdateText, mediaIds: new[] {attachment.Id}, visibility: Visibility.Public).Wait();
        }
    }
}