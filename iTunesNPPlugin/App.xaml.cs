using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace iTunesNPPlugin
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var isDebugMode = false;
#if DEBUG
            isDebugMode = true;
            if (Debugger.IsAttached) isDebugMode = false;
#endif
            if (isDebugMode)
            {
                Win32API.AllocConsole();
                Console.WriteLine("[DEBUG]Debug mode enabled.");
                Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            }
            Debug.WriteLine("[DEBUG]Application Start.");
            ITunesWatcher.CreateWatcherTask();
            base.OnStartup(e);
        }
    }
}