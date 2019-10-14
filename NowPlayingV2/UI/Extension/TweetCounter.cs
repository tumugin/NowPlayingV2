using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using NowPlayingCore.Core;
using NowPlayingV2.Core;

namespace NowPlayingV2.UI.Extension
{
    public class TweetCounter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var account = value[1] as AccountContainer;
            return (account?.MaxTweetLength - account?.CountText(value[0] as string ?? throw new ArgumentException()))?.ToString() ??
                   (280 - TwitterAccount.CountTextStatic(value[0] as string ?? throw new ArgumentException())).ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}