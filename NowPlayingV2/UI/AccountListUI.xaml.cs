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
            //get parent Window
            var window = Matsuri.SeaSlug.GetAncestorOfType<MetroWindow>(sender as Button);
            var diagret = await window.ShowMessageAsync("確認", "本当にアカウントを削除しますか？", MessageDialogStyle.AffirmativeAndNegative);
            if (diagret == MessageDialogResult.Negative) return;
            //get parent item
            var parentitem = Matsuri.SeaSlug.GetAncestorOfType<ListViewItem>(sender as Button);
            //get list view
            var parentlistview = Matsuri.SeaSlug.GetAncestorOfType<ListView>(sender as Button);
            var list = (parentlistview.DataContext as AccountListViewModel).StarryMelody;
            list.Remove(parentitem.DataContext as AccountContainer);
        }
    }
}
