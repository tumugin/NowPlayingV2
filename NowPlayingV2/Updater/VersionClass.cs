using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace NowPlayingV2.Updater
{
    public class VersionClass
    {
        private const string UpdateUrl =
            "https://raw.githubusercontent.com/tumugin/NowPlayingV2/master/NowPlayingV2/Updater/version.json";
        public string AppVersion { get; set; }
        public string UpdateTitle { get; set; }
        public string UpdateMessage { get; set; }
        public string UpdateNotifyUrl { get; set; }

        public bool IsUpdateAvaliable()
        {
            var newversion = new Version(AppVersion);
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var currentver = asm.GetName().Version;
            return newversion > currentver;
        }

        public static Task<VersionClass> GetUpdaterAsync()
        {
            return Task.Run(() =>
            {
                var client = new WebClient();
                var rawjsonbary = client.DownloadData(UpdateUrl);
                var rawjson = Encoding.UTF8.GetString(rawjsonbary);
                return JsonConvert.DeserializeObject<VersionClass>(rawjson);
            });
        }
    }
}