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
using NowPlayingCore.Core;
using NowPlayingCore.Matsuri;
using NowPlayingCore.NowPlaying;
using NowPlayingCore.Tsumugi;
using NowPlayingV2.Core;
using NowPlayingV2.Matsuri;

namespace NowPlayingV2.UI
{
    /// <summary>
    /// TweetDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class TweetDialog : MetroWindow
    {
        private SongInfo _songcache;

        public TweetDialog()
        {
            InitializeComponent();
            AccountListComboBox.ItemsSource = ConfigStore.StaticConfig.accountList;
            if (ConfigStore.StaticConfig.accountList.Count > 0) AccountListComboBox.SelectedIndex = 0;
        }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var songInfo = PipeListener.staticpipelistener.LastPlayedSong;
            _songcache = songInfo?.Clone() as SongInfo;
            if (songInfo == null)
            {
                await this.ShowMessageAsync("メッセージ", "現在何も再生されていません。\n(注:アプリケーションが起動する前に再生されていた曲は取得できません)");
                this.Close();
                return;
            }
            var isource = new Func<BitmapSource>(() =>
            {
                try
                {
                    if (!songInfo.IsAlbumArtAvaliable()) return null;
                    return GdiUtils.ToImageSource(songInfo.GetAlbumArt());
                }
                catch
                {
                    return null;
                }
            })();
            AlbumArtImage.Source = isource;
            TweetTextBox.Text = TweetConverter.SongInfoToString(ConfigStore.StaticConfig.TweetFormat, songInfo);
            TweetTextBox.Focus();
        }

        private async void OnTweetClick(object sender, RoutedEventArgs e)
        {
            var loadingview = await this.ShowProgressAsync("Please wait...", "つぶやいています.....");
            loadingview.SetIndeterminate();
            //get current account
            var acc = AccountListComboBox.SelectedItem as AccountContainer;
            var tweetext = TweetTextBox.Text;
            try
            {
                if(acc == null) throw new Exception("アカウントが何も追加されていない状態でツイートすることはできません。");
                if (EnableImageTweetCBox.IsChecked.Value)
                {
                    await Task.Run(() => acc.UpdateStatus(tweetext,_songcache.AlbumArtBase64));
                }
                else
                {
                    await Task.Run(() => acc.UpdateStatus(tweetext));
                }
                await loadingview.CloseAsync();
                this.Close();
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("エラー", $"ツイート中にエラーが発生しました。\n{ex}");
                await loadingview.CloseAsync();
            }
        }

        private void TweetTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //detect Ctrl+Enter
            if (e.Key.Equals(Key.Return) && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                e.Handled = true;
                OnTweetClick(null, null);
            }
        }
    }
}