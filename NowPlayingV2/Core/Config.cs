using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowPlayingV2.Core
{
    public class Config
    {
        public ObservableCollection<AccountContainer> accountList = new ObservableCollection<AccountContainer>();
    }
}
