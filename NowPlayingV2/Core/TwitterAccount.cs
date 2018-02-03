using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTweet;

namespace NowPlayingV2.Core
{
    class TwitterAccount : AccountContainer
    {
        public Tokens AuthToken { get; set; }

        //Will be called on Json.NET deserialization
        private TwitterAccount()
        {
        }

        public TwitterAccount(Tokens token)
        {
            AuthToken = token;
            UpdateName();
        }

        public override string ID => AuthToken.ScreenName;

        public override int MaxTweetLength => 280;

        public override void UpdateCache()
        {
            AuthToken.Account.VerifyCredentials();
            UpdateName();
        }

        public override void UpdateStatus(string UpdateText)
        {
            AuthToken.Statuses.Update(status: UpdateText);
        }

        public override void UpdateStatus(string UpdateText, string base64image)
        {
            var imgresult = AuthToken.Media.Upload(media_data: base64image);
            AuthToken.Statuses.Update(status: UpdateText, media_ids: new[] {imgresult.MediaId});
        }

        private void UpdateName() => Name = AuthToken.Users.Show(user_id: AuthToken.UserId).Name;
    }
}