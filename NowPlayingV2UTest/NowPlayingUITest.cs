using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NowPlayingV2.UI.Extension;

namespace NowPlayingV2UTest
{
    [TestClass]
    public class NowPlayingUITest
    {
        [TestMethod]
        public void TestTweetCounter()
        {
            var tweetcounter = new TweetCounter();
            //should be 4
            var yabuki_kana = "矢吹可奈";
            Assert.AreEqual(tweetcounter.Convert(yabuki_kana, null, null, null) as string, (140 - 4).ToString());
            //should be 1
            Assert.AreEqual(tweetcounter.Convert(Environment.NewLine,null,null,null) as string,(140 - 1).ToString());
            //should be 9
            var tokugawa_matsuri =
                $"徳{Environment.NewLine}川{Environment.NewLine}ま{Environment.NewLine}つ{Environment.NewLine}り";
            Assert.AreEqual(tweetcounter.Convert(tokugawa_matsuri, null, null, null) as string, (140 - 9).ToString());
            //should be 114
            var yanyanokuresoudesu =
                "やんやんっ😖🐤遅れそうです😫🌀 たいへんっ⚡駅🚉までだっしゅ！🏃💨 初めて💕のデート💑ごめん🙇で登場？💦やんやんっ🐦😥そんなのだめよ🙅たいへんっ😰電車🚃よいそげ！🙏♥ 不安な気持ち😞がすっぱい⚡😖😖ぶる～べりぃ💜とれいん 💖🐣💚";
            Assert.AreEqual(tweetcounter.Convert(yanyanokuresoudesu, null, null, null) as string, (140 - 114).ToString());
        }
    }
}