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
using Reactive.Bindings;

namespace NowPlayingV2.UI.Controls
{
    public partial class MastodonVisibilitySelector : UserControl
    {
        private readonly MastodonVisibilitySelectorVM mastodonVisibilitySelectorVm = new MastodonVisibilitySelectorVM();

        public Mastonet.Visibility? MastodonVisibility
        {
            get => (Mastonet.Visibility?) GetValue(MastodonVisibilityProperty);
            set => SetValue(MastodonVisibilityProperty, value);
        }

        public static readonly DependencyProperty MastodonVisibilityProperty =
            DependencyProperty.Register(nameof(MastodonVisibility), typeof(Mastonet.Visibility?),
                typeof(MastodonVisibilitySelector),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, MastodonVisibility_PropertyChanged));

        static void MastodonVisibility_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = (MastodonVisibilitySelector) obj;
            control.VisibilityComboBox.SelectedItem =
                control.mastodonVisibilitySelectorVm.VisibilityItems.First(i =>
                    i.Visibility == control.MastodonVisibility);
        }

        public MastodonVisibilitySelector()
        {
            InitializeComponent();
            VisibilityComboBox.DataContext = mastodonVisibilitySelectorVm;
            VisibilityComboBox.SelectionChanged += (sender, args) =>
            {
                MastodonVisibility = ((VisibilityItem) VisibilityComboBox.SelectedItem).Visibility;
            };
        }
    }
}