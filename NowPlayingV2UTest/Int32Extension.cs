using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowPlayingV2UTest
{
    static class Int32Extension
    {
        public static void Times(this int loopCount, System.Action<int> loop)
        {
            for (int i = 0; i < loopCount; i++)
            {
                loop(i);
            }
        }
    }
}
