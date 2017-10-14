using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NowPlayingV2.Core;
using NowPlayingV2.Matsuri;

namespace NowPlayingV2.UI
{
    /// <summary>
    /// TweetDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class TweetDialog : MetroWindow
    {
        public TweetDialog()
        {
            InitializeComponent();
            AccountListComboBox.ItemsSource = ConfigStore.StaticConfig.accountList;
            if(ConfigStore.StaticConfig.accountList.Count > 0) AccountListComboBox.SelectedIndex = 0;
        }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var songInfo = NowPlaying.PipeListener.staticpipelistener.LastPlayedSong;
            if (songInfo == null)
            {
                await this.ShowMessageAsync("メッセージ", "現在何も再生されていません。\n(注:アプリケーションがする前に再生されていた曲は取得できません)");
                this.Close();
                return;
            }
            var isource = new Func<BitmapSource>(() =>
            {
                try
                {
                    if (!songInfo.IsAlbumArtAvaliable()) return null;
                    return ImageTool.ToImageSource(songInfo.GetAlbumArt());
                }
                catch
                {
                    return null;
                }
            })();
            AlbumArtImage.Source = isource;
        }
    }
}
