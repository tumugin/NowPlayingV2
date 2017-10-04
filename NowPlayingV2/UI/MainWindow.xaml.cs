using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using NowPlayingV2.Core;
using NowPlayingV2.Matsuri;

namespace NowPlayingV2.UI
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BindingOperations.EnableCollectionSynchronization(ConfigStore.StaticConfig.accountList,new object());
            NowPlaying.PipeListener.staticpipelistener.OnMusicPlay += UpdatePlayingSongView;
            AccountListView.DataContext = ConfigStore.StaticConfig.accountList;
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
                    catch { return null; }
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
    }
}
