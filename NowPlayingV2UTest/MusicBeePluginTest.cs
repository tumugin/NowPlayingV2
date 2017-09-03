using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Pipes;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace NowPlayingV2UTest
{
    [TestClass]
    public class MusicBeePluginTest
    {
        [TestMethod]
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
                memstream.Write(buffer, 0, buffer.Length);
            }
            stream.Close();
            var bary = memstream.ToArray();
            var rawjson = System.Text.Encoding.UTF8.GetString(bary);
            //Console.WriteLine(rawjson);
            dynamic json = JsonConvert.DeserializeObject(rawjson);
            foreach(JProperty kvp in (json as IEnumerable<object>))
            {
                Console.WriteLine($"{kvp.Name}:{kvp.Value}");
            }
        }
    }
}
