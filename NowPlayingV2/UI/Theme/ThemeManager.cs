using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NowPlayingV2.UI.Theme
{
    [Serializable]
    public class ThemeManager : ISerializable
    {
        public static ReadOnlyCollection<NPTheme> Themes { get; set; } = Array.AsReadOnly(new NPTheme[]
        {
            new NPTheme("dark", new Uri("pack://application:,,,/UI/Theme/dark/dark.xaml", UriKind.Absolute)),
            new NPTheme("vivid_rabbit",
                new Uri("pack://application:,,,/UI/Theme/vivid_rabbit/vivid_rabbit.xaml", UriKind.Absolute))
        });

        public NPTheme CurrentTheme { get; set; } = Themes.First();

        public ThemeManager()
        {
        }

        protected ThemeManager(SerializationInfo info, StreamingContext context)
        {
            var theme = Themes.First(i => i.Name == info.GetString("ThemeName"));
            if (theme != null)
            {
                CurrentTheme = theme;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ThemeName", CurrentTheme.Name);
        }
    }
}