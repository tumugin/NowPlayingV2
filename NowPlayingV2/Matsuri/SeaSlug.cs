using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NowPlayingV2.Matsuri
{
    public class SeaSlug
    {
        public static T GetAncestorOfType<T>(FrameworkElement child) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent != null && !(parent is T))
                return (T)GetAncestorOfType<T>((FrameworkElement)parent);
            return (T)parent;
        }

        public static int CountText(string text)
        {
            //CRLF is counted as 2 chars(should be counted as 1 char)
            var repchar = text.Replace(Environment.NewLine, " ");
            var sinfo = new StringInfo(repchar);
            //count all chars as 2 chars
            var scount = sinfo.LengthInTextElements * 2;
            //count only Hankaku chars
            var regexcount = Regex.Matches(repchar, "[ -~｡-ﾟ]").Count;
            //minus Hankaku chars(Because all char are counted as 2 chars on the code above)
            scount -= regexcount;
            return scount;
        }
    }
}
