using ExcelDataReader;
using StudentManagement.Commands;
using StudentManagement.Objects;
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
using StudentManagement.Services;
using StudentManagement.Models;

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


        private ObservableCollection<UserCard> _userDatabase;
        public ObservableCollection<UserCard> UserDatabase
        {
            get => _userDatabase;
            set
            {
                _userDatabase = value;
              
            }
        }

        public string SearchQuery { get => _searchQuery; set { _searchQuery = value; SearchNameFunction(); OnPropertyChanged(); } }
        private string _searchQuery;

        private ObservableCollection<UserCard> _findNameData;
        public ObservableCollection<UserCard> FindNameData
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


        public CampusStudentListViewModel()
        {
           
            Instance = this;

            UserDatabase = new ObservableCollection<UserCard>();

            var teacherList = TeacherServices.Instance.LoadTeacherList();
            var studentList = StudentServices.Instance.LoadStudentList();
            var adminList = AdminServices.Instance.LoadAdminList();

            foreach (var item in studentList)
                UserDatabase.Add(new UserCard(item));

            foreach (var item in teacherList)
                UserDatabase.Add(new UserCard(item));

            foreach (var item in adminList)
                UserDatabase.Add(new UserCard(item));

            FindNameData = new ObservableCollection<UserCard>();

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

            FindNameData.Clear();
            foreach (var item in UserDatabase)
            {
                if (VietnameseStringNormalizer.Instance.Normalize(item.DisplayName)
                    .Contains(VietnameseStringNormalizer.Instance.Normalize(SearchQuery))
                    
                )
                {
                   
                    FindNameData.Add(item);
                }
            }
        }

        void AddStudentFunction()
        {
            CampusStudentListRightSideBarViewModel studentListRightSideBarViewModel = CampusStudentListRightSideBarViewModel.Instance;
            studentListRightSideBarViewModel.RightSideBarItemViewModel = new AddStudentListViewModel();

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
                        

                    }
                }
            }
            MyMessageBox.Show("Thêm thành công");
            SearchNameFunction();
        }
    }
}
