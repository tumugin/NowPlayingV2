using Newtonsoft.Json;
using System;
using System.Collections;
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

        //delegate
        public delegate void OnMusicPlayDelegate(SongInfo songInfo);
        public event OnMusicPlayDelegate OnMusicPlay = (songinfo) => { };

        //property
        public SongInfo LastPlayedSong { get; private set; } = null;

        //pipe listener
        private ManualResetEvent stopevent = new ManualResetEvent(false);
        private AutoResetEvent taskstopwait = new AutoResetEvent(false);
        private Task listenertask = null;
        public void startPipeListener()
        {
            //do not start more than 1 task.
            if (!new TaskStatus?[] { TaskStatus.RanToCompletion, TaskStatus.Faulted, TaskStatus.WaitingForChildrenToComplete, null }.Contains(listenertask?.Status)) return;
            listenertask = Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var aresetev = new AutoResetEvent(false);
                        var stream = new NamedPipeServerStream("NowPlayingTunesV2PIPE", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                        stream.BeginWaitForConnection((IAsyncResult ar) =>
                        {
                            if (stopevent.WaitOne(0))
                            {
                                //WaitOne(0) returns true when state is set
                                taskstopwait.Set();
                                return;
                            }
                            stream.EndWaitForConnection(ar);
                            if (!stream.IsConnected) return;
                            var memstream = new MemoryStream();
                            int readret = -1;
                            while (readret != 0)
                            {
                                var buffer = new byte[1024];
                                readret = stream.Read(buffer, 0, buffer.Length);
                                memstream.Write(buffer, 0, readret);
                            }
                            stream.Close();
                            aresetev.Set();
                            var bary = memstream.ToArray();
                            var rawjson = System.Text.Encoding.UTF8.GetString(bary);
                            var sinfo = JsonConvert.DeserializeObject<SongInfo>(rawjson);
                            LastPlayedSong = sinfo;
                            OnMusicPlay(sinfo);
                        }, null);
                        if (WaitHandle.WaitAny(new WaitHandle[] { aresetev, stopevent }) == 1)
                        {
                            stream.Close();
                            return;
                        }
                    }
                    catch { }
                }
            });
        }

        public void stopPipeListener()
        {
            if (listenertask?.Status == TaskStatus.Running)
            {
                stopevent.Set();
                taskstopwait.WaitOne(Timeout.Infinite);
                stopevent.Reset();
            }
        }
    }
}
