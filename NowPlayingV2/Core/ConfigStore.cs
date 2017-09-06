using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowPlayingV2.Core
{
    public class ConfigStore
    {
        public static Config StaticConfig { get; private set; } = new Config();

        public static string ConfigPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + "/config.json";
            }
        }

        public static bool ConfigExists()
        {
            return System.IO.File.Exists(ConfigPath);
        }

        public static void LoadConfig(Config cfg)
        {
            var bary = System.IO.File.ReadAllBytes(ConfigPath);
            var rawjson = Encoding.UTF8.GetString(bary);
            cfg = JsonConvert.DeserializeObject<Config>(rawjson);
        }

        public static void SaveConfig(Config cfg)
        {
            var rawjson = JsonConvert.SerializeObject(cfg);
            var bary = Encoding.UTF8.GetBytes(rawjson);
            System.IO.File.WriteAllBytes(ConfigPath, bary);
        }
    }
}
