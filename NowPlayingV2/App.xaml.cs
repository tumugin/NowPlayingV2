using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NowPlayingV2.UI.NotifyIcon;

namespace NowPlayingV2
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //Init Notify Icon
            NotifyIconManager.NotifyIconSingleton.InitIcon();
            //Start Pipe Listener
            NowPlaying.PipeListener.MkStaticInstance();
            //Start Auto Tweet
            NowPlaying.AutoTweet.Autotweetsigleton.InitListner(NowPlaying.PipeListener.staticpipelistener);
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
        }
        protected override void OnExit(ExitEventArgs e)
        {
            //Save Config
            Core.ConfigStore.SaveConfig(Core.ConfigStore.StaticConfig);
            //Stop Pipe Listener
            NowPlaying.PipeListener.staticpipelistener?.StopPipeListener();
            //Stop all tweet job
            NowPlaying.AutoTweet.Autotweetsigleton.StopAllTask();
            //Delete Icon
            NotifyIconManager.NotifyIconSingleton.DeleteIcon();
        }
    }
}
