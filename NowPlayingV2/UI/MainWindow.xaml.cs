using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls.Dialogs;
using NowPlayingV2.Core;
using NowPlayingV2.Matsuri;
using NowPlayingV2.UI.View;

namespace NowPlayingV2.UI
{
    public partial class MainWindow
    {
        private static MainWindow windowinstance;

        public static void OpenSigletonWindow()
        {
            if (windowinstance == null)
            {
                (new MainWindow()).Show();
            }
            else
            {
                windowinstance.Activate();
            }
        }
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            windowinstance = this;
            BindingOperations.EnableCollectionSynchronization(ConfigStore.StaticConfig.accountList, new object());
            NowPlaying.PipeListener.staticpipelistener.OnMusicPlay += UpdatePlayingSongView;
        }

        private void UpdatePlayingSongView(NowPlaying.SongInfo songInfo)
        {
            Dispatcher.Invoke(() =>
            {
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
                SongImage.Source = isource;
                SongTitleLabel.Content = songInfo.Title;
                SongArtistLabel.Content = songInfo.Artist;
                SongAlbumLabel.Content = songInfo.Album;
            });
        }

        private void OnAddAccountClick(object sender, RoutedEventArgs e)
        {
            (new UI.OAuthWindow()).ShowDialog();
        }

        private async void OnUpdateAccountClick(object sender, RoutedEventArgs e)
        {
            var res = await this.ShowMessageAsync("アカウント情報を更新しますか？", "アカウントの名前やIDの情報を再取得しますか？",
                MessageDialogStyle.AffirmativeAndNegative);
            if (res != MessageDialogResult.Affirmative) return;
            var waitdiag = await this.ShowProgressAsync("アカウント情報を取得中...", "アカウント情報を取得しています。しばらくお待ちください。");
            try
            {
                await AccountManager.UpdateAccountAsync(ConfigStore.StaticConfig.accountList);
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("エラー", $"正常にアカウント情報を更新出来ませんでした。\n\n{ex.Message}\n{ex.StackTrace}",
                    MessageDialogStyle.AffirmativeAndNegative);
            }
            finally
            {
                await waitdiag.CloseAsync();
            }
            ConfigStore.StaticConfig.accountList.ToList().ForEach(itm => itm.ReDrawAllProperty());
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            NowPlaying.PipeListener.staticpipelistener.OnMusicPlay -= UpdatePlayingSongView;
            windowinstance = null;
        }
    }
}