using CoreTweet;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
using static CoreTweet.OAuth;

namespace NowPlayingV2.UI
{
    /// <summary>
    /// OAuthWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class OAuthWindow : MetroWindow
    {
        private OAuthSession session;

        public OAuthWindow()
        {
            InitializeComponent();
        }

        private async void OnOpenBrowserAuthAsync(object sender, RoutedEventArgs e)
        {
            var progdiag = await this.ShowProgressAsync("読み込み中...", "認証の準備をしています。しばらくお待ちください。");
            try
            {
                await Task.Run(() =>
                {
                    session = OAuth.Authorize(Tsumugi.APIKey.CONSUMER_KEY, Tsumugi.APIKey.CONSUMER_SECRET);
                    System.Diagnostics.Process.Start(session.AuthorizeUri.AbsoluteUri);
                });
                WindowTab.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("エラー",
                    "何らかのエラーで認証を開始することが出来ませんでした。\n\n" + ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                await progdiag.CloseAsync();
            }
        }

        private async void OnAddAccountClickAsync(object sender, RoutedEventArgs e)
        {
            var progdiag = await this.ShowProgressAsync("認証中", "認証処理をしています。しばらくお待ちください。");
            var pincode = PinCodeTextBox.Text;
            try
            {
                await Task.Run(() =>
                {
                    var token = session.GetTokens(pincode);
                    var container = new Core.AccountContainer(token);
                    Core.ConfigStore.StaticConfig.accountList.Add(container);
                });
                await progdiag.CloseAsync();
                this.Close();
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("エラー",
                    $"正常に認証できませんでした。PINコードが間違っている可能性があります\n\n{ex.Message}\n{ex.StackTrace}");
                WindowTab.SelectedIndex = 0;
                PinCodeTextBox.Text = "";
                await progdiag.CloseAsync();
            }
        }

        private void TabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (session == null && WindowTab.SelectedIndex == 1)
            {
                ((TabControl) sender).SelectedItem = e.RemovedItems[0];
            }
        }
    }
}