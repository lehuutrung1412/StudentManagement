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

        public static readonly DependencyProperty FirstSearchButtonTextProperty =
            DependencyProperty.RegisterAttached("FirstSearchButtonText", typeof(string), typeof(SearchBarOne), new PropertyMetadata("Mã lớp"));
        public static readonly DependencyProperty SecondSearchButtonTextProperty =
            DependencyProperty.RegisterAttached("SecondSearchButtonText", typeof(string), typeof(SearchBarOne), new PropertyMetadata("Giáo viên"));
        
        public static readonly DependencyProperty SearchQueryProperty =
            DependencyProperty.RegisterAttached("SearchQuery", typeof(string), typeof(SearchBarOne), new PropertyMetadata(""));
    }
}
