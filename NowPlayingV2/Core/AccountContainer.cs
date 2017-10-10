using CoreTweet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NowPlayingV2.Core
{
    public class AccountContainer : INotifyPropertyChanged
    {
        public AccountContainer()
        {
            
        }

        public AccountContainer(Tokens tk)
        {
            AuthToken = tk;
            UpdateName();
        }

        public Tokens AuthToken { get; set; }

        public bool Enabled { get; set; } = true;

        public string ID => AuthToken.ScreenName;

        public string Name { get; set; }

        public void UpdateCache()
        {
            AuthToken.Account.VerifyCredentials();
            UpdateName();
        }

        public void UpdateName() => Name = AuthToken.Users.Show(user_id: AuthToken.UserId).Name;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ReDrawAllProperty() => OnPropertyChanged("");
    }
}
