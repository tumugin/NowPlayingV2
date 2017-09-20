using iTunesLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace iTunesNPPlugin
{
    public class ITunesConnecter
    {
        private static iTunesApp app;

        public static void CreateITunesInstance()
        {
            app = new iTunesApp();
            app.OnPlayerPlayingTrackChangedEvent += OnPlayerPlayingTrackChangedEvent;
            app.OnQuittingEvent += OnQuittingEvent;
        }

        private static void OnQuittingEvent()
        {
            app.OnPlayerPlayingTrackChangedEvent -= OnPlayerPlayingTrackChangedEvent;
            app.OnQuittingEvent -= OnQuittingEvent;
            Marshal.ReleaseComObject(app);
            app = null;
            Task.Run(() =>
            {
                //wait until itunes exits.
                while (true)
                {
                    if (Process.GetProcessesByName("iTunes").Count() == 0)
                    {
                        ITunesWatcher.CreateWatcherTask();
                        return;
                    }
                }
            });
        }

        private static void OnPlayerPlayingTrackChangedEvent(object iTrack)
        {
            //it's running on iTunes UI Thread so use Task!
            Task.Run(() =>
            {
                var sendmap = new Dictionary<string, string>();
                var track = (iTunesLib.IITFileOrCDTrack)app.CurrentTrack;
                sendmap.Add("title", track.Name);
                sendmap.Add("albumartist", track.AlbumArtist);
                sendmap.Add("artist", track.Artist);
                sendmap.Add("trackcount", track.PlayedCount.ToString());
                sendmap.Add("album", track.Album);
                sendmap.Add("composer", track.Composer);
                if(track.Artwork.Count > 0)
                {
                    track.Artwork[0].SaveArtworkToFile(AppDomain.CurrentDomain.BaseDirectory + "/artwork.png");
                    var imgbary = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "/artwork.png");
                    sendmap.Add("albumart", Convert.ToBase64String(imgbary));
                    System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + "/artwork.png");
                }
                var json = new JavaScriptSerializer().Serialize(sendmap.ToDictionary(item => item.Key.ToString(), item => item.Value.ToString()));
                Debug.WriteLine(json);
                try
                {
                    var bary = Encoding.UTF8.GetBytes(json);
                    var pipe = new NamedPipeClientStream("NowPlayingTunesV2PIPE");
                    pipe.Connect(1000); //set timeout 1000msec.
                    pipe.Write(bary, 0, bary.Count());
                    pipe.Close();
                }
                catch { }
            });
        }
    }
}
