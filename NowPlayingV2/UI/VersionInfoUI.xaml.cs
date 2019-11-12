using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NowPlayingV2.Updater;

namespace NowPlayingV2.UI
{
    public partial class VersionInfoUI : UserControl
    {
        public VersionInfoUI()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var ver = asm.GetName().Version?.ToString() ?? "unknown version";
            var buildDate = asm.GetCustomAttribute<BuildDateAttribute>()!.DateTime.ToString("R");
            VersionLabel.Content = $"Version: {ver}\nBuildDate: {buildDate}";
        }

        private async void CheckUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckUpdateButton.Content = "アップデートを確認しています.....";
                var vc = await VersionClass.GetUpdaterAsync();
                if (vc.IsUpdateAvailable())
                {
                    //show updater screen
                    CheckUpdateButton.Content = "\u203Cアップデートが利用可能です";
                    CheckUpdateButton.Background =
                        new SolidColorBrush((Color) ColorConverter.ConvertFromString("#f5ad3b"));
                    //get parent Window
                    var window = Matsuri.SeaSlug.GetAncestorOfType<MetroWindow>((Button) sender);
                    var diagret = await window.ShowMessageAsync($"バージョン{vc.AppVersion}が利用可能です",
                        $"{vc.UpdateMessage}\n\nアップデートページを開きますか？", MessageDialogStyle.AffirmativeAndNegative);
                    if (diagret == MessageDialogResult.Affirmative)
                    {
                        var processStartInfo = new ProcessStartInfo(vc.UpdateNotifyUrl)
                        {
                            UseShellExecute = true,
                            Verb = "open"
                        };
                        Process.Start(processStartInfo);
                    }
                }
                else
                {
                    //change text
                    CheckUpdateButton.Content = "\u2714最新版をご利用です";
                    CheckUpdateButton.Background =
                        new SolidColorBrush((Color) ColorConverter.ConvertFromString("#5abfb7"));
                }
            }
            catch (Exception ex)
            {
                CheckUpdateButton.Content = "\u274Cアップデートを確認できませんでした";
                CheckUpdateButton.Background =
                    new SolidColorBrush((Color) ColorConverter.ConvertFromString("#d7385f"));
                Trace.WriteLine($"[UpdateChecker]Could not check update.(Err={ex.Message})");
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            var processStartInfo = new ProcessStartInfo(e.Uri.AbsoluteUri)
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(processStartInfo);
        }
    }
}