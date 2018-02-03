using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NowPlayingV2.UI.Extension
{
    public class HotchPotchConverter : IMultiValueConverter
    {
        public IMultiValueConverter MultiConverter { get; set; }
        public IValueConverter SingleConverter { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var val1 = System.Convert.ToDouble(MultiConverter.Convert(values, null, null, culture));
            return SingleConverter.Convert(val1, targetType, parameter, culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}