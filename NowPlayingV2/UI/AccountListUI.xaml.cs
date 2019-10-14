using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NowPlayingCore.Core;
using NowPlayingV2.Core;
using NowPlayingV2.UI.View;

namespace NowPlayingV2.UI
{
    /// <summary>
    /// AccountListUI.xaml の相互作用ロジック
    /// </summary>
    public partial class AccountListUI : GridView
    {
        public AccountListUI()
        {
            InitializeComponent();
        }

        private async void AccDelButtonClick(object sender, RoutedEventArgs e)
        {
            var senderButton = (Button) sender;
            //get parent Window
            var window = Matsuri.SeaSlug.GetAncestorOfType<MetroWindow>(senderButton)!;
            var diagRet =
                await window.ShowMessageAsync("確認", "本当にアカウントを削除しますか？", MessageDialogStyle.AffirmativeAndNegative);
            if (diagRet == MessageDialogResult.Negative) return;
            //get parent item
            var parentItem = Matsuri.SeaSlug.GetAncestorOfType<ListViewItem>(senderButton)!;
            //get list view
            var parentListView = Matsuri.SeaSlug.GetAncestorOfType<ListView>(senderButton)!;
            var list = ((AccountListViewModel) parentListView.DataContext).StarryMelody;
            list.Remove((AccountContainer) parentItem.DataContext);
        }

        private void AccEnableButtonClick(object sender, RoutedEventArgs e)
        {
            var senderButton = (Button) sender;
            //get parent item
            var parentItem = Matsuri.SeaSlug.GetAncestorOfType<ListViewItem>(senderButton)!;
            //update
            var item = (AccountContainer) parentItem.DataContext;
            item.Enabled = !item.Enabled;
            //refresh view
            item.ReDrawAllProperty();
        }
    }
}