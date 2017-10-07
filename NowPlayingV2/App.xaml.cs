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
            //Load config
            if (Core.ConfigStore.ConfigExists())
            {

            }
            else
            {
                UI.MainWindow mainWindow = new UI.MainWindow();
                mainWindow.Show();
            }
        }
        protected override void OnExit(ExitEventArgs e)
        {
            //Stop Pipe Listener
            NowPlaying.PipeListener.staticpipelistener?.StopPipeListener();
        }
    }
}
