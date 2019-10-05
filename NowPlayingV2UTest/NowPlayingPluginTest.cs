using System;
using System.IO.Pipes;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Linq;
using System.Reflection;
using NowPlayingCore.NowPlaying;
using Xunit;

namespace NowPlayingV2UTest
{
    public class NowPlayingPluginTest
    {
        [Trait("Category", "PluginTest")]
        [Fact]
        public void GetJsonTest()
        {
            var stream = new NamedPipeServerStream("NowPlayingTunesV2PIPE");
            var memstream = new MemoryStream();
            stream.WaitForConnection();
            int readret = -1;
            while (readret != 0)
            {
                var buffer = new byte[1024];
                readret = stream.Read(buffer, 0, buffer.Length);
                memstream.Write(buffer, 0, readret);
            }

            stream.Close();
            var bary = memstream.ToArray();
            var rawjson = System.Text.Encoding.UTF8.GetString(bary);
            //Console.WriteLine(rawjson);
            dynamic json = JsonConvert.DeserializeObject(rawjson);
            foreach (JProperty kvp in (json as IEnumerable<object>))
            {
                if (kvp.Name != "albumart") Console.WriteLine($"{kvp.Name}:{kvp.Value}");
            }
        }

        [Trait("Category", "PluginTest")]
        [Fact]
        public void TestEventListener()
        {
            var waitHandle = new ManualResetEvent(false);
            PipeListener.MkStaticInstance();
            PipeListener.staticpipelistener.OnMusicPlay += (songinfo) =>
            {
                Assert.NotNull(songinfo.Album);
                Assert.NotNull(songinfo.AlbumArtBase64);
                Assert.NotNull(songinfo.AlbumArtist);
                Assert.NotNull(songinfo.Title);
                Assert.NotNull(songinfo.TrackCount);
                Assert.NotNull(songinfo.Year);
                Assert.NotNull(songinfo.Composer);
                songinfo.GetType().GetProperties().ToList().ForEach(itm =>
                {
                    if (itm.GetValue(songinfo) is string str && itm.Name != "AlbumArtBase64")
                        Console.WriteLine($"{itm.Name} : {str}");
                });
                waitHandle.Set();
            };
            waitHandle.WaitOne(Timeout.Infinite);
            PipeListener.staticpipelistener.StopPipeListener();
        }

        [Trait("Category", "PluginTest")]
        [Fact]
        public void TestImageDecoder()
        {
            var waitHandle = new ManualResetEvent(false);
            PipeListener.MkStaticInstance();
            PipeListener.staticpipelistener.OnMusicPlay += (songinfo) =>
            {
                if (songinfo.IsAlbumArtAvaliable())
                {
                    Console.WriteLine($"Image Width : {songinfo.GetAlbumArt().Width}");
                    Console.WriteLine($"Image Height : {songinfo.GetAlbumArt().Height}");
                }

                Assert.True(songinfo.AlbumArtBase64.Length == 0 || songinfo.IsAlbumArtAvaliable());
                songinfo.GetType().GetProperties().ToList()
                    .ForEach(itm => Console.WriteLine($"{itm.Name} : {itm.GetValue(songinfo)}"));
                waitHandle.Set();
            };
            waitHandle.WaitOne(Timeout.Infinite);
            PipeListener.staticpipelistener.StopPipeListener();
        }
    }
}