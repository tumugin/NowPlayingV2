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

        public string AppVersion { get; set; } = default!;
        public string UpdateTitle { get; set; } = default!;
        public string UpdateMessage { get; set; } = default!;
        public string UpdateNotifyUrl { get; set; } = default!;

        public bool IsUpdateAvailable()
        {
            var newVersion = new Version(AppVersion);
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var currentVersion = asm.GetName().Version;
            return newVersion > currentVersion;
        }

        public static Task<VersionClass> GetUpdaterAsync()
        {
            return Task.Run(() =>
            {
                var client = new WebClient();
                var rawJsonBary = client.DownloadData(UpdateUrl);
                var rawJson = Encoding.UTF8.GetString(rawJsonBary);
                return JsonConvert.DeserializeObject<VersionClass>(rawJson);
            });
        }
    }
}