using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NowPlayingV2.Core;

namespace NowPlayingV2.UI.View
{
    public class AccountListViewModel : INotifyPropertyChanged
    {
        //must have public constructor
        public AccountListViewModel()
        {
            
        }

        //Use this for DisplayMemberBinding
        public ObservableCollection<AccountContainer> StarryMelody => ConfigStore.StaticConfig.accountList;

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}