using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NowPlayingV2.UI.Theme;

namespace NowPlayingV2.Core
{
    public class Config
    {
        public ObservableCollection<AccountContainer> accountList = new ObservableCollection<AccountContainer>();
        public bool CheckUpdate { get; set; } = true;
        public bool EnableAutoTweet { get; set; } = false;
        public string TweetFormat { get; set; } = "Nowplaing $Title - $Artist #NowPlaying";
        public ThemeManager Theme {get; set;} = new ThemeManager();
}
}
