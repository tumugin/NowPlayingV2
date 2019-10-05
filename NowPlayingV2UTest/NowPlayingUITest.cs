using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NowPlayingCore.Core;
using NowPlayingV2.Core;
using NowPlayingV2.Matsuri;
using NowPlayingV2.UI.Extension;
using Xunit;

namespace NowPlayingV2UTest
{
    public class NowPlayingUITest
    {
        [Fact]
        public void TestTweetCounter()
        {
            var tweetcounter = new TweetCounter();
            //should be 8
            var yabuki_kana = "矢吹可奈";
            Assert.Equal(8, TwitterAccount.CountTextStatic(yabuki_kana));
            //should be 1
            Assert.Equal(1, TwitterAccount.CountTextStatic(Environment.NewLine));
            //should be 14
            var tokugawa_matsuri =
                $"徳{Environment.NewLine}川{Environment.NewLine}ま{Environment.NewLine}つ{Environment.NewLine}り";
            Assert.Equal(10 + 4, TwitterAccount.CountTextStatic(tokugawa_matsuri));
            //should be 224
            var yanyanokuresoudesu =
                "やんやんっ😖🐤遅れそうです😫🌀 たいへんっ⚡駅🚉までだっしゅ！🏃💨 初めて💕のデート💑ごめん🙇で登場？💦やんやんっ🐦😥そんなのだめよ🙅たいへんっ😰電車🚃よいそげ！🙏♥ 不安な気持ち😞がすっぱい⚡😖😖ぶる～べりぃ💜とれいん 💖🐣💚";
            Assert.Equal(114 * 2 - 4, TwitterAccount.CountTextStatic(yanyanokuresoudesu));
        }
    }
}