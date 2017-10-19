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
        public static ReadOnlyCollection<NPTheme> Themes = Array.AsReadOnly(new NPTheme[]
        {
            new NPTheme()
            {
                Name = "dark",
                XamlSource = new Uri("dark/dark.xaml", UriKind.Relative)
            },
            new NPTheme()
            {
                Name = "vivid_rabbit",
                XamlSource = new Uri("vivid_rabbit/vivid_rabbit.xaml", UriKind.Relative)
            }
        });

        public NPTheme CurrentTheme { get; set; } = Themes.First();

        public ThemeManager()
        {
        }

        protected ThemeManager(SerializationInfo info, StreamingContext context)
        {
            var theme = Themes.First(i => i.Name == info.GetString("ThemeName"));
            if (theme != null) CurrentTheme = theme;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ThemeName", CurrentTheme.Name);
        }
    }
}