﻿using System;
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
    /// Interaction logic for FileManager.xaml
    /// </summary>
    public partial class FileManager : UserControl
    {
        public FileManager()
        {
            InitializeComponent();
        }

        private void FileGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta / 3);
        }
    }
}
