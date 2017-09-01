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
            //Load config
            if (Core.ConfigStore.configExists())
            {

            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }

        }
    }
}
