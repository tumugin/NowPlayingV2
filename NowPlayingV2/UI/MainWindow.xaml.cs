using MahApps.Metro.Controls;
using NowPlayingV2.Matsuri;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NowPlayingV2
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
