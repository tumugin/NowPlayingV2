using CoreTweet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NowPlayingCore.Core
{
    public abstract class AccountContainer : INotifyPropertyChanged
    {
        public bool Enabled { get; set; } = true;

        [JsonIgnore] public abstract string ID { get; }

        public string Name { get; set; } = default!;

        [JsonIgnore] public abstract int MaxTweetLength { get; }

        public abstract Task UpdateCache();

        public abstract Task UpdateStatus(string UpdateText);

        public abstract Task UpdateStatus(string UpdateText, string base64image);

        public abstract int CountText(string text);

        public event PropertyChangedEventHandler PropertyChanged = default!;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ReDrawAllProperty() => OnPropertyChanged("");
    }
}