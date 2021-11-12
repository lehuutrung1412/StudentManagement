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
    /// Interaction logic for EmptyStateRightSideBar.xaml
    /// </summary>
    public partial class EmptyStateRightSideBar : UserControl
    {
        public EmptyStateRightSideBar()
        {
            InitializeComponent();
        }

        public string Guideline
        {
            get => (string)GetValue(GuidelineProperty);
            set => SetValue(GuidelineProperty, value);
        }

        public static readonly DependencyProperty GuidelineProperty =
           DependencyProperty.RegisterAttached("Guideline", typeof(string), typeof(EmptyStateRightSideBar), new PropertyMetadata("Vui lòng chọn xem thông tin để hiển thị"));

    }
}
