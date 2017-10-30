using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hardcodet.Wpf.TaskbarNotification;
using Newtonsoft.Json;
using NowPlayingV2.Core;
using NowPlayingV2.UI.NotifyIcon;

namespace NowPlayingV2.Updater
{
    public class UpdateChecker
    {
        private Config config => ConfigStore.StaticConfig;
        private NotifyIconManager nim;

        public UpdateChecker(NotifyIconManager manager) => nim = manager;

        public async void CheckUpdateAsync()
        {
            if (!config.CheckUpdate) return;
            try
            {
                var vc = await VersionClass.GetUpdaterAsync();
                if (vc.IsUpdateAvaliable()) return;
                nim.NPIcon.ShowBalloonTip(vc.UpdateTitle,vc.UpdateMessage,BalloonIcon.Info);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"[UpdateChecker]Could not check update.(Err={ex.Message})");
            }
        }
    }
}
