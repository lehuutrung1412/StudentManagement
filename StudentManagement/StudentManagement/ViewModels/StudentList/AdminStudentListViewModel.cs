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

        public class Score
        {
            private string _quaTrinh;
            private string _thucHanh;
            private string _giuaKi;
            private string _cuoiKi;
            private string _diemTB;
            private string _idSubject;
            private string _idStudent;

            public string QuaTrinh
            {
                get => _quaTrinh;
                set => _quaTrinh = value;
            }


            public string ThucHanh
            {
                get => _thucHanh;
                set => _thucHanh = value;
            }

            public string GiuaKi
            {
                get => _giuaKi;
                set => _giuaKi = value;
            }

            public string CuoiKi
            {
                get => _cuoiKi;
                set => _cuoiKi = value;
            }

            public string DiemTB
            {
                get => _diemTB;
                set => _diemTB = value;
            }

            public string IDSubject
            {
                get => _idSubject;
                set => _idSubject = value;
            }

            public string IDStudent
            {
                get => _idStudent;
                set => _idStudent = value;
            }

        }

        #endregion

        public string SearchQuery { get => _searchQuery; set { _searchQuery = value; OnPropertyChanged(); } }
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
            StudentDatabase.Add(new Student { NameStudent = "Nguyễn Tấn Trần Minh Khang", EmailStudent = "example0@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520123"});
            StudentDatabase.Add(new Student { NameStudent = "Ngô Quang Vinh", EmailStudent = "example1@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520124" });
            StudentDatabase.Add(new Student { NameStudent = "Lê Hữu Trung", EmailStudent = "example2@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520125" });
            StudentDatabase.Add(new Student { NameStudent = "Hứa Thanh Tân", EmailStudent = "example3@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520126" });
            StudentDatabase.Add(new Student { NameStudent = "Nguyễn Đỗ Mạnh Cường", EmailStudent = "example4@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520127" });
            StudentDatabase.Add(new Student { NameStudent = "Nguyễn Đình Bình An", EmailStudent = "example5@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520128" });

            StudentScore = new ObservableCollection<Score>();
            StudentScore.Add(new Score { CuoiKi = "10", GiuaKi = "10", QuaTrinh = "10", ThucHanh = "10", DiemTB = "10", IDStudent = "19520123"});
            StudentScore.Add(new Score { CuoiKi = "10", GiuaKi = "10", QuaTrinh = "10", ThucHanh = "10", DiemTB = "10", IDStudent = "19520124" });
            StudentScore.Add(new Score { CuoiKi = "10", GiuaKi = "10", QuaTrinh = "10", ThucHanh = "10", DiemTB = "10", IDStudent = "19520125" });
            StudentScore.Add(new Score { CuoiKi = "10", GiuaKi = "10", QuaTrinh = "10", ThucHanh = "10", DiemTB = "10", IDStudent = "19520126" });
            StudentScore.Add(new Score { CuoiKi = "10", GiuaKi = "10", QuaTrinh = "10", ThucHanh = "10", DiemTB = "10", IDStudent = "19520127" });
            StudentScore.Add(new Score { CuoiKi = "10", GiuaKi = "10", QuaTrinh = "10", ThucHanh = "10", DiemTB = "10", IDStudent = "19520128" });

            SearchName = new RelayCommand<object>((p) => true, (p) => SearchNameFunction());
        }

        void SearchNameFunction()
        {
            if (SearchQuery == null)
            {
                SearchQuery = "";
            }

            int stt = 0;
            FindNameData = new ObservableCollection<Student>();
            foreach (var item in StudentDatabase)
            {
                if (VietnameseStringNormalizer.Instance.Normalize(item.NameStudent)
                    .Contains(VietnameseStringNormalizer.Instance.Normalize(SearchQuery))
                    || (SearchQuery == "")
                )
                {
                    item.STT = stt + 1;
                    stt += 1;
                    FindNameData.Add(item);
                }
            }

            if (stt > 0) return;
            foreach (var item in StudentDatabase)
            {
                if (item.IDStudent.Contains(SearchQuery))
                {
                    item.STT = stt + 1;
                    stt += 1;
                    FindNameData.Add(item);
                }
            }

            if (stt > 0) return;
            foreach (var item in StudentDatabase)
            {
                if (item.EmailStudent.Contains(SearchQuery))
                {
                    item.STT = stt + 1;
                    stt += 1;
                    FindNameData.Add(item);
                }
            }

        }

    }
}
