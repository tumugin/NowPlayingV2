using CoreTweet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowPlayingV2.Core
{
    public class AccountContainer
    {
        public AccountContainer()
        {
            
        }

        public AccountContainer(Tokens tk)
        {
            AuthToken = tk;
            UpdateCache();
        }

        public Tokens AuthToken { get; set; }

        public bool Enabled { get; set; } = true;

        public string ID => AuthToken.ScreenName;

        private string _Name;
        public string Name
        {
            get
            {
                if (AuthToken.UserId == 0) UpdateCache();
                return _Name?.Length == 0 ? AuthToken.Users.Show(user_id: AuthToken.UserId).Name : _Name;
            }
            set => _Name = Name;
        }

        public void UpdateCache()
        {
            AuthToken.Account.VerifyCredentials();
            _Name = AuthToken.Users.Show(user_id: AuthToken.UserId).Name;
        } 
    }
}
