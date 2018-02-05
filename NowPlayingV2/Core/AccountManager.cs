using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTweet;

namespace NowPlayingV2.Core
{
    public class AccountManager
    {
        public static Task UpdateAccountAsync(IEnumerable<AccountContainer> tklst)
        {
            return Task.Run(() =>
            {
                tklst.ToList().ForEach(acc => acc.UpdateCache());
            });
        }
    }
}
