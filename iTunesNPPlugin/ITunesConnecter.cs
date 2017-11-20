using iTunesLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace iTunesNPPlugin
{
    public class ITunesConnecter
    {
        //iTunes instance
        private iTunesApp app;

        private bool isITunesInitialized = false;
        private int lastTrackID;

        public static void CreateITunesInstance()
        {
            new ITunesConnecter();
        }

        public ITunesConnecter()
        {
            isITunesInitialized = true;
            Parallel.Invoke(SongUpdateWatcher, ITunesQuitWatcher);
            Debug.WriteLine("[DEBUG]itunes watcher initialized.");
        }

        private void SongUpdateWatcher()
        {
            while (true)
            {
                //wait
                Thread.Sleep(1000);
                if (!isITunesInitialized) return;
                app = new iTunesApp();
                Debug.WriteLine("[DEBUG]itunes COM initialized.");
                if (app.CurrentTrack == null || app.CurrentTrack.TrackDatabaseID == lastTrackID)
                {
                    Marshal.ReleaseComObject(app);
                    continue;
                }
                //song changed
                OnPlayerPlayingTrackChangedEvent();
                lastTrackID = app.CurrentTrack.TrackDatabaseID;
                Marshal.ReleaseComObject(app);
            }
        }

        private void ITunesQuitWatcher()
        {
            //TODO wait for first itunes initalize.
            while (true)
            {
                if (Win32API.FindWindow("iTunesApp", "iTunes") == IntPtr.Zero ||
                    Win32API.FindWindow("iTunes", "iTunes") == IntPtr.Zero || !isITunesInitialized)
                {
                    Debug.WriteLine("[DEBUG]itunes quit event.");
                    OnQuittingEvent();
                    return;
                }
                Thread.Sleep(1000);
            }
        }

        private void OnQuittingEvent()
        {
            isITunesInitialized = false;
            //wait until itunes exits.
            while (true)
            {
                if (Process.GetProcessesByName("iTunes").Count() == 0)
                {
                    ITunesWatcher.CreateWatcherTask();
                    return;
                }
            }
        }

        private void OnPlayerPlayingTrackChangedEvent()
        {
            var sendmap = new Dictionary<string, string>();
            var track = (iTunesLib.IITFileOrCDTrack) app.CurrentTrack;
            sendmap.Add("title", track.Name);
            sendmap.Add("albumartist", track.AlbumArtist);
            sendmap.Add("artist", track.Artist);
            sendmap.Add("trackcount", track.PlayedCount.ToString());
            sendmap.Add("album", track.Album);
            sendmap.Add("composer", track.Composer);
            if (track.Artwork.Count > 0)
            {
                track.Artwork[0].SaveArtworkToFile(AppDomain.CurrentDomain.BaseDirectory + "/artwork.png");
                var imgbary = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "/artwork.png");
                sendmap.Add("albumart", Convert.ToBase64String(imgbary));
                System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + "/artwork.png");
            }
            var json = new JavaScriptSerializer().Serialize(sendmap.ToDictionary(item => item.Key.ToString(),
                item => item.Value.ToString()));
            Debug.WriteLine(json);
            try
            {
                var bary = Encoding.UTF8.GetBytes(json);
                var pipe = new NamedPipeClientStream("NowPlayingTunesV2PIPE");
                pipe.Connect(1000); //set timeout 1000msec.
                pipe.Write(bary, 0, bary.Count());
                pipe.Close();
            }
            catch
            {
            }
        }
    }
}