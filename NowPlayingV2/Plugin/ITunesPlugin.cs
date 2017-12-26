using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NowPlayingV2.Core;

namespace NowPlayingV2.Plugin
{
    public class ITunesPlugin
    {
        public static void Start()
        {
            //check setting
            if (!ConfigStore.StaticConfig.EnableITunesPlugin) return;
            //check if process exists and kill
            Process.GetProcessesByName("iTunesNPPlugin").ToList().ForEach(i => i.Kill());
            //start process
            Process.Start($"{AppDomain.CurrentDomain.BaseDirectory}/iTunesNPPlugin.exe");
        }

        public static void Exit()
        {
            //check setting
            if (!ConfigStore.StaticConfig.EnableITunesPlugin) return;
            //kill process
            Process.GetProcessesByName("iTunesNPPlugin").ToList().ForEach(i => i.Kill());
        }
    }
}
