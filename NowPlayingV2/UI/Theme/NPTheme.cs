using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NowPlayingV2.UI.Theme
{
    public class NPTheme
    {
        public string Name { get; set; }
        public Uri XamlSource { get; set; }
    }
}