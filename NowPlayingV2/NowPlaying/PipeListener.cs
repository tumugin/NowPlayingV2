using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NowPlayingV2.NowPlaying
{
    public class PipeListener
    {
        //for static instance
        public static PipeListener staticpipelistener;

        public static void MkStaticInstance()
        {
            if (staticpipelistener == null)
            {
                staticpipelistener = new PipeListener();
                staticpipelistener.startPipeListener();
            }
        }

        //set values to SongInfo
        private class MKSongInfo : SongInfo
        {
            public void SetProp(string propname, object value)
            {
                var prop = this.GetType().GetProperty(propname);
                prop.SetValue(this, value);
            }
        }

        //delegate
        public delegate void OnMusicPlayDelegate(SongInfo songInfo);
        public event OnMusicPlayDelegate OnMusicPlay = (songinfo) => { };

        //pipe listener
        private AutoResetEvent resetevent = new AutoResetEvent(false);
        private void startPipeListener()
        {
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        var aresetev = new AutoResetEvent(false);
                        var stream = new NamedPipeServerStream("NowPlayingTunesV2PIPE",PipeDirection.InOut,1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                        stream.BeginWaitForConnection((IAsyncResult ar) =>
                        {
                            //TODO: stopPipeListener()呼び出し時にここで絶対に例外が発生するので発生しないコードにする
                            stream.EndWaitForConnection(ar);
                            if (!stream.IsConnected) return;
                            var memstream = new MemoryStream();
                            int readret = -1;
                            while (readret != 0)
                            {
                                var buffer = new byte[1024];
                                readret = stream.Read(buffer, 0, buffer.Length);
                                memstream.Write(buffer, 0, buffer.Length);
                            }
                            stream.Close();
                            aresetev.Set();
                            var bary = memstream.ToArray();
                            var rawjson = System.Text.Encoding.UTF8.GetString(bary);
                            dynamic json = JsonConvert.DeserializeObject(rawjson);
                            MKSongInfo sinfo = new MKSongInfo();
                            sinfo.SetProp(nameof(sinfo.Album), json["album"].Value);
                            sinfo.SetProp(nameof(sinfo.AlbumArtBase64), json["albumart"].Value);
                            sinfo.SetProp(nameof(sinfo.AlbumArtist), json["albumartist"].Value);
                            sinfo.SetProp(nameof(sinfo.Title), json["title"].Value);
                            sinfo.SetProp(nameof(sinfo.TrackCount), json["trackcount"].Value);
                            sinfo.SetProp(nameof(sinfo.Artist), json["artist"].Value);
                            OnMusicPlay(sinfo);
                        }, null);
                        if (WaitHandle.WaitAny(new WaitHandle[] { aresetev, resetevent }) == 1)
                        {
                            stream.Close();
                            return;
                        }
                    }
                    catch { }
                }
            }).Start();
        }

        public void stopPipeListener()
        {
            resetevent.Set();
        }
    }
}
