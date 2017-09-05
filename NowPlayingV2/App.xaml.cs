using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NowPlayingV2
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //Start Pipe Listener
            NowPlaying.PipeListener.MkStaticInstance();
            //Load config
            if (Core.ConfigStore.ConfigExists())
            {

            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }
        protected override void OnExit(ExitEventArgs e)
        {
            //Stop Pipe Listener
            NowPlaying.PipeListener.staticpipelistener?.stopPipeListener();
        }
    }
}
