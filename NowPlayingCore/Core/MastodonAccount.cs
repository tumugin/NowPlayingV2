using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mastonet;
using Newtonsoft.Json;

namespace NowPlayingCore.Core
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

        public override int MaxTweetLength => 500;

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
            UpdateStatus(UpdateText, base64image, Visibility.Public);
        }

        public void UpdateStatus(string UpdateText, Visibility visibility)
        {
            mastodonClient.PostStatus(UpdateText, visibility).Wait();
        }

        public void UpdateStatus(string UpdateText, string base64image, Visibility visibility)
        {
            byte[] data = System.Convert.FromBase64String(base64image);
            MemoryStream ms = new MemoryStream(data);
            var filetype = Matsuri.ImageTool.GetFileTypeFromBytes(data);
            var attachment = mastodonClient.UploadMedia(ms, $"nowplaying.{filetype}").Result;
            ms.Dispose();
            mastodonClient.PostStatus(UpdateText, mediaIds: new[] {attachment.Id}, visibility: visibility)
                .Wait();
        }

        public override int CountText(string text) => CountTextStatic(text);

        public static int CountTextStatic(string text)
        {
            //CRLF is counted as 2 chars(should be counted as 1 char)
            var repchar = text.Replace(Environment.NewLine, " ");
            var sinfo = new StringInfo(repchar);
            return sinfo.LengthInTextElements;
        }
    }
}