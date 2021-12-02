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
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentManagement.ViewModels
{
    public class AdminStudentListViewModel : BaseViewModel
    {


        #region object
        public class Student
        {
            private string _nameStudent;
            private string _idStudent;
            private string _emailStudent;
            private string _gender;
            private string _faculty;
            private string _status;
            private int _stt;
            private string _training;
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

            public string Training
            {
                get => _training;
                set => _training = value;
            }
        }

        public class PieChartElement
        {
            private float _percent;
            private string _title;
            private Brush _colorBrush;

            public float Percent
            {
                get => _percent;
                set => _percent = value;
            }

            public string Title
            {
                get => _title;
                set => _title = value;
            }

            public Brush Brush
            {
                get => _colorBrush;
                set => _colorBrush = value;
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

        private Student _selectedItem;
        public Student SelectedItem
        {
            get => _selectedItem;
            set => _selectedItem = value;
        }

        private bool _isSelectedAll;
        public bool IsSelectedAll
        {
            get => _isSelectedAll;
            set
            {
                _isSelectedAll = value;
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

        private ObservableCollection<Student> _studentClass;
        public ObservableCollection<Student> StudentClass
        {
            get => _studentClass;
            set => _studentClass = value;
        }

        private ObservableCollection<Student> _deleteStudentList;
        public ObservableCollection<Student> DeleteStudentList
        {
            get => _deleteStudentList;
            set => _deleteStudentList = value;
        }

        public ICommand SearchName { get; set; }
        public ICommand DeleteStudent { get; set; }
        public ICommand AddStudent { get; set; }

        private float _pieWidth, _pieHeight, _centerX, _centerY, _radius;

        private void InitParemeter()
        {
            _pieWidth = 300; _pieHeight = 300;
            _centerX = _pieWidth / 2; _centerY = _pieHeight / 2;
            _radius = _pieWidth / 2;
        }


        public AdminStudentListViewModel()
        {
            InitParemeter();

            StudentDatabase = new ObservableCollection<Student>();
            StudentDatabase.Add(new Student { NameStudent = "Nguyễn Tấn Trần Minh Khang", EmailStudent = "example0@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520123", STT = 1 });
            StudentDatabase.Add(new Student { NameStudent = "Ngô Quang Vinh", EmailStudent = "example1@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520124", STT = 2 });
            StudentDatabase.Add(new Student { NameStudent = "Lê Hữu Trung", EmailStudent = "example2@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520125", STT = 3 });
            StudentDatabase.Add(new Student { NameStudent = "Hứa Thanh Tân", EmailStudent = "example3@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520126", STT = 4 });
            StudentDatabase.Add(new Student { NameStudent = "Nguyễn Đỗ Mạnh Cường", EmailStudent = "example4@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520127", STT = 5 });
            StudentDatabase.Add(new Student { NameStudent = "Nguyễn Đình Bình An", EmailStudent = "example5@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520128", STT = 6 });
            StudentDatabase.Add(new Student { NameStudent = "Nguyễn Minh Huy", EmailStudent = "example6@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520129", STT = 6 });
            StudentDatabase.Add(new Student { NameStudent = "Nguyễn Minh Huy Cầu Vòng", EmailStudent = "example7@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520130", STT = 6 });
            StudentDatabase.Add(new Student { NameStudent = "Nguyễn Minh RainbowShine", EmailStudent = "example8@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520131", STT = 6 });
            StudentDatabase.Add(new Student { NameStudent = "Nguyễn Minh UIT.Leader", EmailStudent = "example9@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520132", STT = 6 });

            StudentClass = new ObservableCollection<Student>();
            StudentClass.Add(new Student { NameStudent = "Nguyễn Tấn Trần Minh Khang", EmailStudent = "example0@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520123", IsSelected = false });
            StudentClass.Add(new Student { NameStudent = "Ngô Quang Vinh", EmailStudent = "example1@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520124", IsSelected = false });
            StudentClass.Add(new Student { NameStudent = "Lê Hữu Trung", EmailStudent = "example2@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520125", IsSelected = false });
            StudentClass.Add(new Student { NameStudent = "Hứa Thanh Tân", EmailStudent = "example3@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520126", IsSelected = false });
            StudentClass.Add(new Student { NameStudent = "Nguyễn Đỗ Mạnh Cường", EmailStudent = "example4@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520127", IsSelected = false });
            StudentClass.Add(new Student { NameStudent = "Nguyễn Đình Bình An", EmailStudent = "example5@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520128", IsSelected = false });


            FindNameData = new ObservableCollection<Student>(StudentClass);

            SearchName = new RelayCommand<object>((p) => true, (p) => SearchNameFunction());
            DeleteStudent = new RelayCommand<object>((p) => true, (p) => DeleteStudentFunction());
            AddStudent = new RelayCommand<object>((p) => true, (p) => AddStudentFunction());
        }

        void SearchNameFunction()
        {
            if (SearchQuery == null)
            {
                SearchQuery = "";
            }

            int stt = 0;
            FindNameData.Clear();
            foreach (var item in StudentClass)
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

        bool checkStudentExistInClass()
        {
            foreach (var item in StudentClass)
                if (item.IDStudent == SearchQuery)
                    return false;
            return true;
        } 

        private Student FindSearchQueryInDatabase()
        {
            foreach (var item in StudentDatabase)
                if (item.IDStudent == SearchQuery)
                    return item;
            return null;
        }
            
        void AddStudentFunction()
        {
            if (!checkStudentExistInClass())
            {
                System.Windows.MessageBox.Show("Sinh viên " + SearchQuery + " đã tồn tại ở lớp học!!!");
                return;
            }
            
            Student findStudent = FindSearchQueryInDatabase();

            if (findStudent == null)
            {
                System.Windows.MessageBox.Show("Mã số sinh viên " + SearchQuery + " không hợp lệ !!!");
                return;
            }

            StudentClass.Add(findStudent);
            System.Windows.MessageBox.Show("Sinh viên " + SearchQuery + " đã được thêm vào lớp học!!!");

            SearchQuery = "";

        }

        void DeleteSingleStudent()
        {

            DeleteStudentList = new ObservableCollection<Student>();

            foreach (var item in StudentClass)
            {
                if (VietnameseStringNormalizer.Instance.Normalize(item.NameStudent)
                    .Contains(VietnameseStringNormalizer.Instance.Normalize(SearchQuery))
                    || item.IDStudent.Contains(SearchQuery)
                )
                {
                    DeleteStudentList.Add(item);
                }
            }

            if (DeleteStudentList.Count() == 0)
            {
                System.Windows.MessageBox.Show("Sinh viên " + SearchQuery + " không tồn tại trong lớp học!!!");
                return;
            }

            if (DeleteStudentList.Count() > 1)
            {
                System.Windows.MessageBox.Show("Có quá nhiều sinh viên " + SearchQuery + " trong lớp học!!!");
                return;
            }

            foreach (var item in DeleteStudentList)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Bạn có chắc chắn xóa sinh viên " + SearchQuery + " ?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                    StudentClass.Remove(item);
                else
                    return;
            }

            SearchNameFunction();
        }

        void DeleteStudentFunction()
        {
            DeleteStudentList = new ObservableCollection<Student>();
            foreach (var item in FindNameData)
                if (item.IsSelected)
                {
                    item.IsSelected = false;
                    DeleteStudentList.Add(item);
                }

            if (DeleteStudentList.Count() == 0 && SearchQuery == "")
            {
                System.Windows.MessageBox.Show("Vui lòng chọn sinh viên hợp lệ!!!");
                return;
            }

            if (DeleteStudentList.Count() == 0 && SearchQuery != "")
            {
                DeleteSingleStudent();
                return;
            }

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Bạn có chắc chắn xóa sinh viên?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                foreach (var item in DeleteStudentList)
                    StudentClass.Remove(item);
            }
            else return;

            SearchNameFunction();
        }
    }
}
