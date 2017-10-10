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
        public static void UpdateAccounts(IEnumerable<Tokens> tklst)
        {
            tklst.ToList().ForEach(i => i?.Account.VerifyCredentials());
        }

        public static Task UpdateAccountAsync(IEnumerable<AccountContainer> tklst)
        {
            return Task.Run(() =>
            {
                UpdateAccounts(tklst.Select(konomi => konomi.AuthToken));
                tklst.ToList().ForEach(acc => acc.UpdateName());
            });
        }
    }
}
