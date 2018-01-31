using CoreTweet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NowPlayingV2.Core
{
    public abstract class AccountContainer : INotifyPropertyChanged
    {
        public bool Enabled { get; set; } = true;

        [JsonIgnore]
        public abstract string ID { get; }

        public string Name { get; set; }

        public abstract void UpdateCache();

        public abstract void UpdateStatus(string UpdateText);

        public abstract void UpdateStatus(string UpdateText, string base64image);

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ReDrawAllProperty() => OnPropertyChanged("");
    }
}