﻿using Microsoft.Win32;
using StudentManagement.Commands;
using StudentManagement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModels.UserInfo
{
    public class UserInfoStudentViewModel : BaseViewModel
    {
        public ICommand ClickImageCommand { get; set; }
        public ICommand ClickChangeImageCommand { get; set; }
        private string _visibility;
        public string Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                OnPropertyChanged();
            }
        }
        private string _image;
        public string Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }
        public UserInfoStudentViewModel()
        {
            UserInfoStudent userInfoStudent = new UserInfoStudent();
            Visibility = "Collapsed";
            Image = "https://picsum.photos/200";
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
                    Image = op.FileName;
                    Visibility = "Collapsed";
                }
            });

        }


    }
}
