using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NowPlayingCore.NowPlaying;
using NowPlayingV2.Core;
using NowPlayingV2.UI.NotifyIcon;
using NowPlayingV2.Updater;

namespace NowPlayingV2
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //Start Pipe Listener
            PipeListener.MkStaticInstance();
            //Load config
            if (Core.ConfigStore.ConfigExists())
            {
                Core.ConfigStore.LoadStaticConfig();
                Core.ConfigStore.StaticConfig.Theme.CurrentTheme.ApplyTheme();
            }
            else
            {
                Core.ConfigStore.StaticConfig.Theme.CurrentTheme.ApplyTheme();
                UI.MainWindow.OpenSigletonWindow();
            }
            //Start Auto Tweet
            AutoTweet.AutoTweetSingleton.InitListner(PipeListener.StaticPipeListener, ConfigStore.StaticConfig);
            //Init Notify Icon
            NotifyIconManager.NotifyIconSingleton.InitIcon();
            //Check update
            new UpdateChecker(NotifyIconManager.NotifyIconSingleton).CheckUpdateAsync();
            //Start iTunes Plugin
            Plugin.ITunesPlugin.Start();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            //Save Config
            Core.ConfigStore.SaveConfig(Core.ConfigStore.StaticConfig);
            //Stop Pipe Listener
            PipeListener.StaticPipeListener?.StopPipeListener();
            //Stop all tweet job
            AutoTweet.AutoTweetSingleton.StopAllTask();
            //Delete Icon
            NotifyIconManager.NotifyIconSingleton.DeleteIcon();
            //Stop iTunes Plugin
            Plugin.ITunesPlugin.Exit();
        }
    }
}
