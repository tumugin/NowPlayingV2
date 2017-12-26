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

namespace NowPlayingV2.UI
{
    /// <summary>
    /// VersionInfoUI.xaml の相互作用ロジック
    /// </summary>
    public partial class VersionInfoUI : UserControl
    {
        public VersionInfoUI()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var ver = asm.GetName().Version.ToString();
            var builddate = Matsuri.BuildDate.GetBuildDateTime(asm).ToString("R");
            VersionLabel.Content = $"Version: {ver}\nBuildDate: {builddate}";
        }
    }
}
