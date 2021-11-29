using StudentManagement.Commands;
using StudentManagement.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class AdminStudentListViewModel : BaseViewModel
    {


        #region
        public class Student
        {
            private string _nameStudent;
            private string _idStudent;
            private string _emailStudent;
            private string _gender;
            private string _faculty;
            private string _status;
            private int _stt;
            private bool _isSelected;

            public bool IsSelected
            {
                get => _isSelected;
                set => _isSelected = value;
            }

            public string NameStudent
            {
                get => _nameStudent;
                set => _nameStudent = value;
            }

            public string IDStudent
            {
                get => _idStudent;
                set => _idStudent = value;
            }

            public string EmailStudent
            {
                get => _emailStudent;
                set => _emailStudent = value;
            }

            public string Gender
            {
                get => _gender;
                set => _gender = value;
            }

            public string Faculty
            {
                get => _faculty;
                set => _faculty = value;
            }

            public string Status
            {
                get => _status;
                set => _status = value;
            }

            public int STT
            {
                get => _stt;
                set => _stt = value;
            }
        }

       

        #endregion

        public string SearchQuery { get => _searchQuery; set { _searchQuery = value; SearchNameFunction(); OnPropertyChanged(); } }
        private string _searchQuery;

        private ObservableCollection<Student> _findNameData;
        public ObservableCollection<Student> FindNameData
        {
            get => _findNameData;
            set
            {
                _findNameData = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Score> _studentScore;
        public ObservableCollection<Score> StudentScore
        {
            get => _studentScore;
            set => _studentScore = value;
        }

        private ObservableCollection<Student> _studentDatabase;
        public ObservableCollection<Student> StudentDatabase
        {
            get => _studentDatabase;
            set => _studentDatabase = value;
        }
        public ICommand SearchName { get; set; }

        public AdminStudentListViewModel()
        {
            StudentDatabase = new ObservableCollection<Student>();
            StudentDatabase.Add(new Student { NameStudent = "Nguyễn Tấn Trần Minh Khang", EmailStudent = "example0@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520123", STT = 1});
            StudentDatabase.Add(new Student { NameStudent = "Ngô Quang Vinh", EmailStudent = "example1@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520124", STT = 2});
            StudentDatabase.Add(new Student { NameStudent = "Lê Hữu Trung", EmailStudent = "example2@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520125", STT = 3 });
            StudentDatabase.Add(new Student { NameStudent = "Hứa Thanh Tân", EmailStudent = "example3@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520126", STT = 4 });
            StudentDatabase.Add(new Student { NameStudent = "Nguyễn Đỗ Mạnh Cường", EmailStudent = "example4@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520127", STT = 5 });
            StudentDatabase.Add(new Student { NameStudent = "Nguyễn Đình Bình An", EmailStudent = "example5@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520128", STT = 6 });


            FindNameData = new ObservableCollection<Student>(StudentDatabase);
            
            SearchName = new RelayCommand<object>((p) => true, (p) => SearchNameFunction());

        }

        void SearchNameFunction()
        {
            if (SearchQuery == null)
            {
                SearchQuery = "";
            }

            int stt = 0;
            FindNameData.Clear();
            foreach (var item in StudentDatabase)
            {
                if (VietnameseStringNormalizer.Instance.Normalize(item.NameStudent)
                    .Contains(VietnameseStringNormalizer.Instance.Normalize(SearchQuery))
                    || item.IDStudent.Contains(SearchQuery)
                )
                {
                    item.STT = stt + 1;
                    stt += 1;
                    FindNameData.Add(item);
                }
            }
        }
    }
}
