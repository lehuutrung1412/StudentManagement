using ExcelDataReader;
using StudentManagement.Commands;
using StudentManagement.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using Student = StudentManagement.ViewModels.AdminStudentListViewModel.Student;

namespace StudentManagement.ViewModels
{
    public class CampusStudentListViewModel : BaseViewModel
    {
        private static CampusStudentListViewModel s_instance;
        public static CampusStudentListViewModel Instance
        {
            get => s_instance ?? (s_instance = new CampusStudentListViewModel());

            private set => s_instance = value;
        }

        private ObservableCollection<Student> _studentDatabase;
        public ObservableCollection<Student> StudentDatabase
        {
            get => _studentDatabase;
            set => _studentDatabase = value;
        }

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

        public ICommand SearchName { get; set; }
        public ICommand AddStudent { get; set; }
        public ICommand AddStudentList { get; set; }

        private ObservableCollection<Score> _studentScore;
        public ObservableCollection<Score> StudentScore
        {
            get => _studentScore;
            set => _studentScore = value;
        }

        public CampusStudentListViewModel()
        {
            StudentDatabase = new ObservableCollection<Student>();
            Instance = this;

            //StudentDatabase.Add(new Student { Training = "Đại trà", NameStudent = "Nguyễn Tấn Trần Minh Khang", EmailStudent = "example0@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", IDStudent = "19520123", STT = 1});
            StudentDatabase.Add(new Student { TrainingForm = "Tài năng", DisplayName = "Ngô Quang Vinh", Email = "example1@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520124", Number = 2 });
            StudentDatabase.Add(new Student { TrainingForm = "Tài năng", DisplayName = "Lê Hữu Trung", Email = "example2@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520125", Number = 3 });
            StudentDatabase.Add(new Student { TrainingForm = "Tài năng", DisplayName = "Hứa Thanh Tân", Email = "example3@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520126", Number = 4 });
            StudentDatabase.Add(new Student { TrainingForm = "Tài năng", DisplayName = "Nguyễn Đỗ Mạnh Cường", Email = "example4@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520127", Number = 5 });
            StudentDatabase.Add(new Student { TrainingForm = "Tài năng", DisplayName = "Nguyễn Đình Bình An", Email = "example5@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520128", Number = 6 });

            FindNameData = new ObservableCollection<Student>(StudentDatabase);

            SearchName = new RelayCommand<object>((p) => true, (p) => SearchNameFunction());
            AddStudent = new RelayCommand<object>((p) => true, (p) => AddStudentFunction());
            AddStudentList = new RelayCommand<object>((p) => true, (p) => AddStudentListFunction());
        }

        public void SearchNameFunction()
        {
            if (SearchQuery == null)
            {
                SearchQuery = "";
            }

            int stt = 0;
            FindNameData.Clear();
            foreach (var item in StudentDatabase)
            {
                if (VietnameseStringNormalizer.Instance.Normalize(item.DisplayName)
                    .Contains(VietnameseStringNormalizer.Instance.Normalize(SearchQuery))
                    || item.Username.Contains(SearchQuery)
                )
                {
                    item.Number = stt + 1;
                    stt += 1;
                    FindNameData.Add(item);
                }
            }
        }

        void AddStudentFunction()
        {
            Student CurrentStudent = new Student();
            CampusStudentListRightSideBarViewModel studentListRightSideBarViewModel = CampusStudentListRightSideBarViewModel.Instance;
            studentListRightSideBarViewModel.RightSideBarItemViewModel = new AddStudentListViewModel(CurrentStudent);

            SearchNameFunction();
        }

        DataTableCollection dataSheets;
        void AddStudentListFunction()
        {
            using (OpenFileDialog op = new OpenFileDialog() { Filter = "Excel|*.xls;*.xlsx;" })
            {
                if (op.ShowDialog() == DialogResult.OK)
                {
                    using (var stream = File.Open(op.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                            });
                            dataSheets = result.Tables;
                        }
                    }
                    DataTable data = dataSheets[0];

                    foreach (DataRow student in data.Rows)
                    {
                        var item = new Student
                        {
                            Username = Convert.ToString(student[0]),
                            DisplayName = Convert.ToString(student[1]),
                            Faculty = Convert.ToString(student[2]),
                            TrainingForm = Convert.ToString(student[3])
                        };

                        StudentDatabase.Add(item);

                    }
                }
            }
            MyMessageBox.Show("Thêm thành công");
            SearchNameFunction();
        }
    }
}
