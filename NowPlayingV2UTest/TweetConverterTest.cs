using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NowPlayingV2.Core;
using NowPlayingV2.NowPlaying;
using NowPlayingV2.Tsumugi;

namespace NowPlayingV2UTest
{
    [TestClass]
    public class TweetConverterTest
    {
        [TestMethod]
        public void TestAutoShortenTweet()
        {
            //setup test instance
            var dummysong = new SongInfo();
            var constructor_twitter = typeof(TwitterAccount).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null, new Type[0], null);
            var twitteraccount = (TwitterAccount) constructor_twitter.Invoke(null);
            var constructor_mastodon = typeof(MastodonAccount).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null, new Type[0], null);
            var mastodonaccount = (MastodonAccount) constructor_mastodon.Invoke(null);
            //280 chars text
            var testtext280 = "";
            140.Times(i => { testtext280 += (new[] {"ほ", "げ"}).OrderBy((k => Guid.NewGuid())).ToArray().First(); });
            //288 chars text
            var testtext288 = testtext280 + "ほげほげ";
            //500 chars text
            var testtext500 = "";
            500.Times(i => { testtext500 += (new[] {"ほ", "げ"}).OrderBy((k => Guid.NewGuid())).ToArray().First(); });
            var testtext765 = "";
            765.Times(i =>
            {
                testtext765 += "白石紬はぽんこつかわいい".ToCharArray().Select(c => c.ToString()).ToArray()
                    .OrderBy((k => Guid.NewGuid())).ToArray().First();
            });
            //Test Twitter counter
            Assert.IsTrue(twitteraccount.CountText(testtext280) == 280);
            var testconvtext1 = TweetConverter.SongInfoToString(testtext280, dummysong, true, twitteraccount);
            Assert.IsTrue(testconvtext1 == testtext280);
            var testconvtext2 = TweetConverter.SongInfoToString(testtext288, dummysong, true, twitteraccount);
            Assert.IsTrue(twitteraccount.CountText(testconvtext2) == 279);
            //Test Mastodon counter
            Assert.IsTrue(mastodonaccount.CountText(testtext500) == 500);
            var testconvtext3 = TweetConverter.SongInfoToString(testtext500, dummysong, true, mastodonaccount);
            Assert.IsTrue(mastodonaccount.CountText(testconvtext3) == 500);
            var testconvtext4 = TweetConverter.SongInfoToString(testtext765, dummysong, true, mastodonaccount);
            Assert.IsTrue(mastodonaccount.CountText(testconvtext4) == 500);
        }
    }
}