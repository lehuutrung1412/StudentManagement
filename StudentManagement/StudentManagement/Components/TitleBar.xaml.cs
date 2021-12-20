using StudentManagement.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace StudentManagement.Components
{
    /// <summary>
    /// Interaction logic for TitleBar.xaml
    /// </summary>
    public partial class TitleBar : UserControl
    {
        public TitleBar()
        {
            InitializeComponent();
            DataContext = new TitleBarViewModel();
        }

        public Visibility ButtonVisibility
        {
            get => (Visibility)GetValue(ButtonVisibilityProperty);
            set => SetValue(ButtonVisibilityProperty, value);
        }

        public static readonly DependencyProperty ButtonVisibilityProperty =
            DependencyProperty.RegisterAttached("ButtonVisibility", typeof(Visibility), typeof(TitleBar), new PropertyMetadata(Visibility.Visible));
    }
}
