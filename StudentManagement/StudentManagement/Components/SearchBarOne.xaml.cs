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

namespace StudentManagement.Components
{
    /// <summary>
    /// Interaction logic for SearchBarOne.xaml
    /// </summary>
    public partial class SearchBarOne : UserControl
    {
        public SearchBarOne()
        {
            InitializeComponent();
        }

        public string FirstSearchButtonText
        {
            get => (string)GetValue(FirstSearchButtonTextProperty);
            set => SetValue(FirstSearchButtonTextProperty, value);
        }
        public string SecondSearchButtonText
        {
            get => (string)GetValue(SecondSearchButtonTextProperty);
            set => SetValue(SecondSearchButtonTextProperty, value);
        }

        public string SearchQuery
        {
            get => (string)GetValue(SearchQueryProperty);
            set => SetValue(SearchQueryProperty, value);
        }

        public ICommand SearchCommand
        {
            get => (ICommand)GetValue(SearchCommandProperty);
            set => SetValue(SearchCommandProperty, value);
        }

        public static readonly DependencyProperty FirstSearchButtonTextProperty =
            DependencyProperty.RegisterAttached("FirstSearchButtonText", typeof(string), typeof(SearchBarOne), new PropertyMetadata("Mã lớp"));
        public static readonly DependencyProperty SecondSearchButtonTextProperty =
            DependencyProperty.RegisterAttached("SecondSearchButtonText", typeof(string), typeof(SearchBarOne), new PropertyMetadata("Giáo viên"));

        public static readonly DependencyProperty SearchQueryProperty =
            DependencyProperty.RegisterAttached("SearchQuery", typeof(string), typeof(SearchBarOne), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty SearchCommandProperty =
             DependencyProperty.RegisterAttached("SearchCommand", typeof(ICommand), typeof(SearchBarOne));



        public bool FirstSearchButtonVisibility
        {
            get { return (bool)GetValue(FirstSearchButtonVisibilityProperty); }
            set { SetValue(FirstSearchButtonVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FirstSearchButtonVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstSearchButtonVisibilityProperty =
            DependencyProperty.Register("FirstSearchButtonVisibility", typeof(bool), typeof(SearchBarOne), new PropertyMetadata(true));



        public bool SecondSearchButtonVisibility
        {
            get { return (bool)GetValue(SecondSearchButtonVisibilityProperty); }
            set { SetValue(SecondSearchButtonVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SecondSearchButtonVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondSearchButtonVisibilityProperty =
            DependencyProperty.Register("SecondSearchButtonVisibility", typeof(bool), typeof(SearchBarOne), new PropertyMetadata(true));



        public ICommand SwitchSearchButton
        {
            get { return (ICommand)GetValue(SwitchSearchButtonProperty); }
            set { SetValue(SwitchSearchButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SwitchSearchButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SwitchSearchButtonProperty =
            DependencyProperty.Register("SwitchSearchButton", typeof(ICommand), typeof(SearchBarOne));


        public bool IsFirstSearchButtonEnabled
        {
            get { return (bool)GetValue(IsFirstSearchButtonEnabledProperty); }
            set { SetValue(IsFirstSearchButtonEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFirstSearchButtonEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFirstSearchButtonEnabledProperty =
            DependencyProperty.Register("IsFirstSearchButtonEnabled", typeof(bool), typeof(SearchBarOne));
    }
}
