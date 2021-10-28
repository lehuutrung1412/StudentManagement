using Microsoft.Win32;
using StudentManagement.Commands;
using StudentManagement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    class UserInfoStudentViewModel: BaseViewModel
    {
        public ICommand ClickImageCommand { get; set; }
        public ICommand ClickChangeImageCommand { get; set; }
        private string _visibility = "";
        public string Visibility 
        { 
            get => _visibility;
            set 
            { 
                _visibility = value;
                OnPropertyChanged();
            }
        }
        private string _source = "";
        public string Source
        {
            get => _source;
            set
            {
                _source = value;
                OnPropertyChanged();
            }
        }
        public UserInfoStudentViewModel()
        {
            UserInfoStudent userInfoStudent = new UserInfoStudent();
            Visibility = "Collapsed";
            Source = @"C:\Users\DELL\Pictures\IMG_2959.JPG.jpg";
            ClickImageCommand = new RelayCommand<object>(
            (p) => { return true; },
            (p) =>
            {
                if (!Visibility.Equals("Collapsed"))
                    Visibility = "Collapsed";
                else
                    Visibility = "Visible";
            });
            ClickChangeImageCommand = new RelayCommand<object>(
            (p) => { return true; },
            (p) =>
            {
                OpenFileDialog op = new OpenFileDialog
                {
                    Title = "Select a picture",
                    Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png"
                };
                if (op.ShowDialog() == true)
                {
                    Source = op.FileName;
                    Visibility = "Collapsed";
                }
            });

        }


    }
}
