using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardcodet.Wpf.TaskbarNotification;

namespace NowPlayingV2.UI.NotifyIcon
{
    public class NotifyIconManager
    {
        public static NotifyIconManager NotifyIconSingleton = new NotifyIconManager();

        private NPNotifyIcon NPIcon;

        public void InitIcon() => NPIcon = new NPNotifyIcon();
    }
}
