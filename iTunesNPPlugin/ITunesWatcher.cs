using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iTunesNPPlugin
{
    public class ITunesWatcher
    {
        private static Task watchertask;

        private static void WatcherMain()
        {
            while (true)
            {
                if (Process.GetProcessesByName("iTunes").Count() > 0)
                {
                    //connect to itunes
                    ITunesConnecter.CreateITunesInstance();
                    return;
                }
                Thread.Sleep(1000);
            }
        }

        public static void CreateWatcherTask()
        {
            //do not start more than 1 task.
            if (!new TaskStatus?[] { TaskStatus.RanToCompletion, TaskStatus.Faulted, TaskStatus.WaitingForChildrenToComplete, null }.Contains(watchertask?.Status)) return;
            watchertask = Task.Run(() => WatcherMain());
        }
    }
}
