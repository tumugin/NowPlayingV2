using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NowPlayingV2.Core;
using NowPlayingV2.UI.NotifyIcon;

namespace NowPlayingV2.Updater
{
    public class UpdateChecker
    {
        private Config config => ConfigStore.StaticConfig;
        private NotifyIconManager nim;
        private const string UpdateUrl =
            "https://raw.githubusercontent.com/tumugin/NowPlayingV2/master/NowPlayingV2/Updater/version.json";

        public UpdateChecker(NotifyIconManager manager) => nim = manager;

        public void CheckUpdate()
        {
            if (!config.CheckUpdate) return;
            try
            {
                var client = new WebClient();
                Task.Run(() =>
                {
                    var rawjson = client.DownloadString(UpdateUrl);
                    
                });
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
