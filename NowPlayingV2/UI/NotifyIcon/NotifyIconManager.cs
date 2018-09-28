using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Hardcodet.Wpf.TaskbarNotification;
using NowPlayingCore.NowPlaying;
using NowPlayingV2.Core;

namespace NowPlayingV2.UI.NotifyIcon
{
    public class NotifyIconManager
    {
        public static NotifyIconManager NotifyIconSingleton = new NotifyIconManager();

        public TaskbarIcon NPIcon;

        public void InitIcon()
        {
            NPIcon = Application.Current.FindResource("NPIcon") as TaskbarIcon;
            (LogicalTreeHelper.FindLogicalNode(NPIcon.ContextMenu, "OnOpenSetting") as MenuItem).Click +=
                (sender, e) => { UI.MainWindow.OpenSigletonWindow(); };
            (LogicalTreeHelper.FindLogicalNode(NPIcon.ContextMenu, "OnAppExit") as MenuItem).Click +=
                (sender, e) => { Application.Current.Shutdown(); };
            (LogicalTreeHelper.FindLogicalNode(NPIcon.ContextMenu, "OnTweetDialog") as MenuItem).Click +=
                (sender, e) => { (new TweetDialog()).Show(); };
            (LogicalTreeHelper.FindLogicalNode(NPIcon.ContextMenu, "IsAutoTweetEnabledMenuItem") as MenuItem)
                .DataContext = Core.ConfigStore.StaticConfig;
            (LogicalTreeHelper.FindLogicalNode(NPIcon.ContextMenu, "OnTweetNow") as MenuItem).Click +=
                async (sender, e) =>
                {
                    try
                    {
                        await ManualTweet.RunManualTweet(ConfigStore.StaticConfig);
                        var song = PipeListener.staticpipelistener.LastPlayedSong;
                        NPIcon.ShowBalloonTip("投稿完了", $"{song.Title}\n{song.Artist}\n{song.Album}", BalloonIcon.Info);
                    }
                    catch (Exception exception)
                    {
                        NPIcon.ShowBalloonTip("エラー", exception.Message, BalloonIcon.Info);
                    }
                };
        }

        public void DeleteIcon() => NPIcon.Dispose();
    }
}