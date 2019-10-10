using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NowPlayingV2.UI.Controls
{
    public class VisibilityItem
    {
        public string Label { get; set; }
        public string Description { get; set; }
        public Mastonet.Visibility Visibility { get; set; }
        public ControlTemplate Icon { get; set; }

        public VisibilityItem(string label, string description, Mastonet.Visibility visibility, ControlTemplate icon)
        {
            this.Label = label;
            this.Description = description;
            this.Visibility = visibility;
            this.Icon = icon;
        }
    }
}