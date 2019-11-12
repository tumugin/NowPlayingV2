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
        public static T? GetAncestorOfType<T>(FrameworkElement child) where T : class
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent != null && !(parent is T))
            {
                return GetAncestorOfType<T>((FrameworkElement) parent);
            }

            return parent as T;
        }
    }
}