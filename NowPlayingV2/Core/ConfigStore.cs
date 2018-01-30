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

        public static string ConfigPath => AppDomain.CurrentDomain.BaseDirectory + "/config.json";

        public static bool ConfigExists()
        {
            return System.IO.File.Exists(ConfigPath);
        }

        public static Config LoadConfig()
        {
            var bary = System.IO.File.ReadAllBytes(ConfigPath);
            var rawjson = Encoding.UTF8.GetString(bary);
            return JsonConvert.DeserializeObject<Config>(rawjson,new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        public static void SaveConfig(Config cfg)
        {
            var rawjson = JsonConvert.SerializeObject(cfg, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.Auto
                });
            var bary = Encoding.UTF8.GetBytes(rawjson);
            System.IO.File.WriteAllBytes(ConfigPath, bary);
        }

        public static void LoadStaticConfig() => StaticConfig = LoadConfig();
    }
}