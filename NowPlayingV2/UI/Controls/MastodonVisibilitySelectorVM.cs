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
            new VisibilityItem(
                description: "公開TLに投稿する", label: "公開", visibility: Mastonet.Visibility.Public,
                icon: (ControlTemplate) Application.Current.Resources["Globe"]
            ),
            new VisibilityItem(
                description: "公開タイムラインに表示しない", label: "未収載", visibility: Mastonet.Visibility.Unlisted,
                icon: (ControlTemplate) Application.Current.Resources["Unlocked"]
            ),
            new VisibilityItem(
                description: "フォロワーにだけ公開", label: "フォロワー限定", visibility: Mastonet.Visibility.Private,
                icon: (ControlTemplate) Application.Current.Resources["Lock"]
            ),
        };
    }
}