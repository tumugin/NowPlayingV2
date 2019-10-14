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

namespace NowPlayingCore.NowPlaying
{
    public class PipeListener
    {
        //for static instance
        public static PipeListener? StaticPipeListener;

        public static void MkStaticInstance()
        {
            if (StaticPipeListener == null)
            {
                StaticPipeListener = new PipeListener();
                StaticPipeListener.StartPipeListener();
            }
        }

        //delegate
        public delegate void OnMusicPlayDelegate(SongInfo songInfo);
        public event OnMusicPlayDelegate OnMusicPlay = (songInfo) => { };

        //property
        public SongInfo? LastPlayedSong { get; private set; } = null;

        //pipe listener
        private readonly ManualResetEvent stopEvent = new ManualResetEvent(false);
        private readonly AutoResetEvent streamStopWait = new AutoResetEvent(false);
        private Task? listenerTask = null;
        public void StartPipeListener()
        {
            //do not start more than 1 task.
            if (!new TaskStatus?[] { TaskStatus.RanToCompletion, TaskStatus.Faulted, TaskStatus.WaitingForChildrenToComplete, null }.Contains(listenerTask?.Status)) return;
            listenerTask = Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var resetStreamEvent = new AutoResetEvent(false);
                        var stream = new NamedPipeServerStream("NowPlayingTunesV2PIPE", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                        stream.BeginWaitForConnection(ar =>
                        {
                            //WaitOne(0) returns true when state is set
                            if (stopEvent.WaitOne(0))
                            {
                                //Called when stopping task.
                                streamStopWait.Set();
                                return;
                            }
                            //Must do this to connect to client.
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
                            resetStreamEvent.Set();
                            var bary = memstream.ToArray();
                            var rawjson = System.Text.Encoding.UTF8.GetString(bary);
                            var sinfo = JsonConvert.DeserializeObject<SongInfo>(rawjson);
                            LastPlayedSong = sinfo;
                            OnMusicPlay(sinfo);
                        }, null);

                        //Wait for stop event
                        //WaitAny will return index of WaitHandle array.(Can distingulish which ResetEvent was set.)
                        if (WaitHandle.WaitAny(new WaitHandle[] { resetStreamEvent, stopEvent }) == 1)
                        {
                            //When stopEvent is called, close stream and escape listnertask.
                            stream.Close();
                            return;
                        }
                        //do nothing when resetStreamEvent is set.(Just keep looping)
                    }
                    catch { }
                }
            });
        }

        public void StopPipeListener()
        {
            if (listenerTask?.Status == TaskStatus.Running)
            {
                stopEvent.Set();
                //Wait until stream disposes successfully
                streamStopWait.WaitOne(Timeout.Infinite);
                stopEvent.Reset();
            }
        }
    }
}
