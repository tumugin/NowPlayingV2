using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Mastonet;

namespace NowPlayingV2.UI.Controls
{
    public class MastodonVisibilitySelectorVM
    {
        public VisibilityItem[] VisibilityItems { get; set; } = new[]
        {
            new VisibilityItem()
            {
                Description = "公開TLに投稿する", Label = "公開", Visibility = Mastonet.Visibility.Public,
                Icon = (ControlTemplate) Application.Current.Resources["Globe"]
            },
            new VisibilityItem()
            {
                Description = "公開タイムラインに表示しない", Label = "未収載", Visibility = Mastonet.Visibility.Unlisted,
                Icon = (ControlTemplate) Application.Current.Resources["Unlocked"]
            },
            new VisibilityItem()
            {
                Description = "フォロワーにだけ公開", Label = "フォロワー限定", Visibility = Mastonet.Visibility.Private,
                Icon = (ControlTemplate) Application.Current.Resources["Lock"]
            },
        };
    }
}