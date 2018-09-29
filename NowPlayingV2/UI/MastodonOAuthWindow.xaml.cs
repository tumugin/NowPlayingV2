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
using Mastonet;
using Mastonet.Entities;
using NowPlayingCore.Core;
using NowPlayingV2.Core;

namespace NowPlayingV2.UI
{
    /// <summary>
    /// MastodonOAuthWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MastodonOAuthWindow : MetroWindow
    {
        private AppRegistration registeredApp;
        private AuthenticationClient authClient;

        public MastodonOAuthWindow()
        {
            InitializeComponent();
        }

        private async void OnAppRegistAsync(object sender, RoutedEventArgs e)
        {
            var progdiag = await this.ShowProgressAsync("読み込み中...", "認証の準備をしています。しばらくお待ちください。");
            try
            {
                authClient = new AuthenticationClient(InstanceNameTextBox.Text);
                registeredApp = await authClient.CreateApp("なうぷれTunes", Scope.Read | Scope.Write);
                WindowTab.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("エラー",
                    $"何らかのエラーで認証を開始することが出来ませんでした。インスタンス名をもう一度確認してください。\n\n{ex}");
            }
            finally
            {
                await progdiag.CloseAsync();
            }
        }

        private async void OnOpenBrowserAuthAsync(object sender, RoutedEventArgs e)
        {
            var progdiag = await this.ShowProgressAsync("読み込み中...", "認証の準備をしています。しばらくお待ちください。");
            try
            {
                await Task.Run(() =>
                {
                    var url = authClient.OAuthUrl();
                    System.Diagnostics.Process.Start(url);
                });
                WindowTab.SelectedIndex = 2;
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("エラー",
                    $"何らかのエラーで認証を開始することが出来ませんでした。\n\n{ex}");
            }
            finally
            {
                await progdiag.CloseAsync();
            }
        }

        private async void OnAddAccountClickAsync(object sender, RoutedEventArgs e)
        {
            var progdiag = await this.ShowProgressAsync("認証中", "認証処理をしています。しばらくお待ちください。");
            try
            {
                var tokens = await authClient.ConnectWithCode(PinCodeTextBox.Text);
                await Task.Run(() =>
                {
                    var client = new MastodonClient(registeredApp, tokens);
                    var container = new MastodonAccount(client);
                    Core.ConfigStore.StaticConfig.accountList.Add(container);
                });
                await progdiag.CloseAsync();
                this.Close();
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("エラー",
                    $"何らかのエラーで認証を開始することが出来ませんでした。\n\n{ex}");
                await progdiag.CloseAsync();
            }
        }
    }
}