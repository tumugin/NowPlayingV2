using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Hardcodet.Wpf.TaskbarNotification;

namespace NowPlayingV2.UI.NotifyIcon
{
    public class NotifyIconManager
    {
        public static NotifyIconManager NotifyIconSingleton = new NotifyIconManager();

        private TaskbarIcon NPIcon;

        public void InitIcon()
        {
            NPIcon = Application.Current.FindResource("NPIcon") as TaskbarIcon;
            (LogicalTreeHelper.FindLogicalNode(NPIcon.ContextMenu, "OnOpenSetting") as MenuItem).Click +=
                (sender, e) =>
                {
                    (new UI.MainWindow()).Show();
                };
            (LogicalTreeHelper.FindLogicalNode(NPIcon.ContextMenu, "OnAppExit") as MenuItem).Click +=
                (sender, e) =>
                {
                    Application.Current.Shutdown();
                };
        }
    }
}