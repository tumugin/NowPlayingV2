using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NowPlayingV2.UI.Extension
{
    public class TweetCounter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tboxtext = value as string;
            //CRLF is counted as 2 chars(should be counted as 1 char)
            var sinfo = new StringInfo(tboxtext.Replace(Environment.NewLine," "));
            return (140 - sinfo.LengthInTextElements).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
