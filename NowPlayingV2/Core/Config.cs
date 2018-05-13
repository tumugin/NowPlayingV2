using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NowPlayingV2.UI.Theme;

namespace NowPlayingV2.Core
{
    public class Config : ConfigBase
    {
        public ThemeManager Theme { get; set; } = new ThemeManager();
    }
}