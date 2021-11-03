using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using static StudentManagement.ViewModels.StudentCourseRegistryViewModel;

namespace StudentManagement.Components
{
    /// <summary>
    /// Interaction logic for SubjectRegistryDataGrid.xaml
    /// </summary>
    public partial class SubjectRegistryDataGrid : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public SubjectRegistryDataGrid()
        {
            InitializeComponent();
        }


        public ObservableCollection<CourseRegistryItem> Data
        {
            get { return (ObservableCollection<CourseRegistryItem>)GetValue(DataProperty); }
            set { SetValue(DataProperty, value);}
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(ObservableCollection<CourseRegistryItem>), typeof(SubjectRegistryDataGrid), new PropertyMetadata(null, OnDataChangeCallBack));
        private static void OnDataChangeCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SubjectRegistryDataGrid c = sender as SubjectRegistryDataGrid;
            if (c != null)
            {
                c.OnPropertyChanged("Data");
            }
        }


        public bool IsAllItemsSelected
        {
            get { return (bool)GetValue(IsAllItemsSelectedProperty); }
            set { SetValue(IsAllItemsSelectedProperty, value); OnPropertyChanged(); Data.Select(c => { c.IsSelected = value; return c; }).ToList(); }
        }

        // Using a DependencyProperty as the backing store for IsAllItemsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAllItemsSelectedProperty =
            DependencyProperty.Register("IsAllItemsSelected", typeof(bool), typeof(SubjectRegistryDataGrid), new PropertyMetadata(false));
/*        private static void OnAllSelectedChangeCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SubjectRegistryDataGrid c = sender as SubjectRegistryDataGrid;
            if (c != null)
            {
                c.OnPropertyChanged("IsAllItemsSelected");
            }
        }*/




    }
}
