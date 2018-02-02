using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NowPlayingV2.Matsuri;
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
            //should be 8
            var yabuki_kana = "矢吹可奈";
            Assert.AreEqual(SeaSlug.CountText(yabuki_kana), 8);
            //should be 1
            Assert.AreEqual(SeaSlug.CountText(Environment.NewLine), 1);
            //should be 14
            var tokugawa_matsuri =
                $"徳{Environment.NewLine}川{Environment.NewLine}ま{Environment.NewLine}つ{Environment.NewLine}り";
            Assert.AreEqual(SeaSlug.CountText(tokugawa_matsuri), 10 + 4);
            //should be 224
            var yanyanokuresoudesu =
                "やんやんっ😖🐤遅れそうです😫🌀 たいへんっ⚡駅🚉までだっしゅ！🏃💨 初めて💕のデート💑ごめん🙇で登場？💦やんやんっ🐦😥そんなのだめよ🙅たいへんっ😰電車🚃よいそげ！🙏♥ 不安な気持ち😞がすっぱい⚡😖😖ぶる～べりぃ💜とれいん 💖🐣💚";
            Assert.AreEqual(SeaSlug.CountText(yanyanokuresoudesu), 114 * 2 - 4);
        }
    }
}