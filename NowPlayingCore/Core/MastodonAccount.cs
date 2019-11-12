using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mastonet;
using Mastonet.Entities;
using Newtonsoft.Json;
using NowPlayingCore.ConfigConverter;

namespace NowPlayingCore.Core
{
    public class MastodonAccount : AccountContainer
    {
        [JsonConverter(typeof(MastodonClientConverter))]
        public MastodonClient MastodonClient { get; set; }

        //Will be called on Json.NET deserialization
        [JsonConstructor]
#nullable disable
        private MastodonAccount()
        {
        }
#nullable enable

        public MastodonAccount(MastodonClient client)
        {
            MastodonClient = client;
        }

        [JsonProperty] private string IDCache = "";
        public override string ID => $"{IDCache}({MastodonClient.Instance})";

        public override int MaxTweetLength => 500;

        public override async Task UpdateCache()
        {
            var account = await MastodonClient.GetCurrentUser();
            IDCache = account.UserName;
            Name = account.DisplayName;
        }

        public override async Task UpdateStatus(string UpdateText)
        {
            await MastodonClient.PostStatus(UpdateText, Visibility.Public);
        }

        public override async Task UpdateStatus(string UpdateText, string base64image)
        {
            await UpdateStatus(UpdateText, base64image, Visibility.Public);
        }

        public async Task UpdateStatus(string UpdateText, Visibility visibility)
        {
            await MastodonClient.PostStatus(UpdateText, visibility);
        }

        public async Task UpdateStatus(string UpdateText, string base64image, Visibility visibility)
        {
            byte[] data = System.Convert.FromBase64String(base64image);
            var ms = new MemoryStream(data);
            var filetype = Matsuri.ImageTool.GetFileTypeFromBytes(data);
            var attachment = await MastodonClient.UploadMedia(ms, $"nowplaying.{filetype}");
            ms.Dispose();
            await MastodonClient.PostStatus(UpdateText, mediaIds: new[] {attachment.Id}, visibility: visibility);
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