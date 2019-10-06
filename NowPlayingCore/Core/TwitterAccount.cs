using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoreTweet;
using Newtonsoft.Json;
using NowPlayingCore.ConfigConverter;

namespace NowPlayingCore.Core
{
    public class TwitterAccount : AccountContainer
    {
        [JsonConverter(typeof(TwitterAuthTokenConverter))]
        public Tokens AuthToken { get; set; }

        //Will be called on Json.NET deserialization
        private TwitterAccount()
        {
        }

        public TwitterAccount(Tokens token)
        {
            AuthToken = token;
        }

        public override string ID => AuthToken.ScreenName;

        public override int MaxTweetLength => 280;

        public override async Task UpdateCache()
        {
            await AuthToken.Account.VerifyCredentialsAsync();
            await UpdateName();
        }

        public override async Task UpdateStatus(string UpdateText)
        {
            await AuthToken.Statuses.UpdateAsync(status: UpdateText);
        }

        public override async Task UpdateStatus(string UpdateText, string base64image)
        {
            byte[] data = Convert.FromBase64String(base64image);
            var imgresult = await AuthToken.Media.UploadAsync(media => data);
            await AuthToken.Statuses.UpdateAsync(status: UpdateText, media_ids: new[] {imgresult.MediaId});
        }

        public override int CountText(string text) => CountTextStatic(text);

        public static int CountTextStatic(string text)
        {
            //CRLF is counted as 2 chars(should be counted as 1 char)
            var repchar = text.Replace(Environment.NewLine, " ");
            var sinfo = new StringInfo(repchar);
            //count all chars as 2 chars
            var scount = sinfo.LengthInTextElements * 2;
            //count only Hankaku chars
            var regexcount = Regex.Matches(repchar, "[ -~｡-ﾟ]").Count;
            //minus Hankaku chars(Because all char are counted as 2 chars on the code above)
            scount -= regexcount;
            return scount;
        }

        private async Task UpdateName() => Name = (await AuthToken.Users.ShowAsync(user_id: AuthToken.UserId)).Name;
    }
}