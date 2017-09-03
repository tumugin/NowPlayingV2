using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowPlayingV2.Core
{
    public class ConfigStore
    {
        public static bool ConfigExists()
        {
            return System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/config.xml");
        }
    }
}
