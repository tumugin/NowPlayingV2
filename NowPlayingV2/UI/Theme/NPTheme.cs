using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NowPlayingV2.UI.Theme
{
    public class NPTheme
    {
        public string Name { get; set; }
        public Uri XamlSource { get; set; }

        public NPTheme(string name, Uri xamlSource)
        {
            this.Name = name;
            this.XamlSource = xamlSource;
        }

        public void ApplyTheme()
        {
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() {Source = this.XamlSource});
        }
    }
}