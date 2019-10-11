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
using Reactive.Bindings;
using Visibility = Mastonet.Visibility;

namespace NowPlayingV2.UI
{
    /// <summary>
    /// TweetDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class TweetDialog : MetroWindow
    {
        private SongInfo? songCache;

        private ReactiveProperty<Mastonet.Visibility> mastodonVisibility { get; } =
            new ReactiveProperty<Visibility>(Mastonet.Visibility.Public);

        public TweetDialog()
        {
            InitializeComponent();
            AccountListComboBox.ItemsSource = ConfigStore.StaticConfig.accountList;
            MastodonVisibilitySelector.DataContext = mastodonVisibility;
            AccountListComboBox.SelectionChanged += AccountListComboBox_SelectionChanged;
            if (ConfigStore.StaticConfig.accountList.Count > 0) AccountListComboBox.SelectedIndex = 0;
        }

        private void AccountListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccountListComboBox.SelectedItem is MastodonAccount)
            {
                MastodonVisibilitySelector.IsEnabled = true;
            }
            else
            {
                MastodonVisibilitySelector.IsEnabled = false;
            }
        }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var songInfo = PipeListener.StaticPipeListener?.LastPlayedSong;
            songCache = songInfo?.Clone() as SongInfo;
            if (songInfo == null)
            {
                await this.ShowMessageAsync("メッセージ", "現在何も再生されていません。\n(注:アプリケーションが起動する前に再生されていた曲は取得できません)");
                this.Close();
                return;
            }

            var isource = new Func<BitmapSource?>(() =>
            {
                try
                {
                    if (!songInfo.IsAlbumArtAvaliable())
                    {
                        return null;
                    }

                    return GdiUtils.ToImageSource(songInfo.GetAlbumArt()!);
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

        private async void OnTweetClick(object? sender, RoutedEventArgs? e)
        {
            if (songCache == null)
            {
                throw new InvalidOperationException("songCache must not be null!!");
            }

            var loadingView = await this.ShowProgressAsync("Please wait...", "つぶやいています.....");
            loadingView.SetIndeterminate();
            //get current account
            var acc = AccountListComboBox.SelectedItem as AccountContainer;
            var tweetText = TweetTextBox.Text;
            try
            {
                if (acc == null) throw new Exception("アカウントが何も追加されていない状態でツイートすることはできません。");
                if (EnableImageTweetCBox.IsChecked ?? false)
                {
                    if (acc is MastodonAccount account)
                    {
                        await account.UpdateStatus(tweetText, songCache.AlbumArtBase64,
                            mastodonVisibility.Value);
                    }
                    else
                    {
                        await acc.UpdateStatus(tweetText, songCache.AlbumArtBase64);
                    }
                }
                else
                {
                    if (acc is MastodonAccount account)
                    {
                        await account.UpdateStatus(tweetText, mastodonVisibility.Value);
                    }
                    else
                    {
                        await acc.UpdateStatus(tweetText);
                    }
                }

                await loadingView.CloseAsync();
                this.Close();
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("エラー", $"ツイート中にエラーが発生しました。\n{ex}");
                await loadingView.CloseAsync();
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