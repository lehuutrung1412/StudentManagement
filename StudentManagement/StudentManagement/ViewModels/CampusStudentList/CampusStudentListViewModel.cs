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
using System.Data.Entity;

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

            FindNameData = new ObservableCollection<UserCard>();

            SearchName = new RelayCommand<object>((p) => true, (p) => SearchNameFunction());
            AddStudent = new RelayCommand<object>((p) => true, (p) => AddStudentFunction());
            AddStudentList = new RelayCommand<object>((p) => true, (p) => AddStudentListFunction());

            try
            {
                List<Teacher> teacherList = TeacherServices.Instance.LoadTeacherList().ToList();
                if (teacherList == null)
                    teacherList =  new List<Teacher>();

                List<Student> studentList = StudentServices.Instance.LoadStudentList().ToList();
                if (studentList == null)
                    studentList = new List<Student>();

                List<Admin> adminList = AdminServices.Instance.LoadAdminList().ToList();
                if (adminList == null)
                    adminList = new List<Admin>();

                foreach (var item in studentList.ToList())
                    UserDatabase.Add(new UserCard(item));

                foreach (var item in teacherList.ToList())
                    UserDatabase.Add(new UserCard(item));

                foreach (var item in adminList.ToList())
                    UserDatabase.Add(new UserCard(item));
               
            }
            catch (Exception e)
            {
                MyMessageBox.Show($"Đã có lỗi xảy ra {e}");
            }

            SearchNameFunction();
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
                    .Contains(VietnameseStringNormalizer.Instance.Normalize(SearchQuery)))
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
            try
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

                        foreach (DataRow item in data.Rows)
                        {
                            var username = item[1].ToString();
                            var findUsername = DataProvider.Instance.Database.Users.Where(x => x.Username == username).FirstOrDefault();
                            if (findUsername != null)
                            {
                                MyMessageBox.Show("Đã thêm trùng user, thêm thất bại");
                                return;
                            }

                        }

                        foreach (DataRow student in data.Rows)
                        {
                            User NewUser = new User();
                            string role = student[0].ToString();

                            NewUser.Id = Guid.NewGuid();
                            NewUser.Username = student[1].ToString();
                            NewUser.Password = SHA256Cryptography.Instance.EncryptString(student[6].ToString());
                            NewUser.DisplayName = student[2].ToString();
                            NewUser.Email = student[3].ToString();
                            NewUser.UserRole = DataProvider.Instance.Database.UserRoles.Where(x => x.Role == role).FirstOrDefault();
                            NewUser.IdUserRole = NewUser.UserRole.Id;

                            UserServices.Instance.SaveUserToDatabase(NewUser);


                            if (role == "Sinh viên")
                            {
                                Student newStudent = new Student();
                                newStudent.IdUsers = NewUser.Id;
                                newStudent.Id = Guid.NewGuid();
                                string temp = student[4].ToString();
                                newStudent.Faculty = DataProvider.Instance.Database.Faculties.Where(x => x.DisplayName == temp).FirstOrDefault();
                                temp = student[5].ToString();
                                newStudent.TrainingForm = DataProvider.Instance.Database.TrainingForms.Where(x => x.DisplayName == temp).FirstOrDefault();
                                newStudent.IdFaculty = newStudent.Faculty.Id;
                                newStudent.IdTrainingForm = newStudent.TrainingForm.Id;
                                newStudent.User = NewUser;

                                StudentServices.Instance.SaveStudentToDatabase(newStudent);
                                UserDatabase.Add(new UserCard(newStudent));

                            }

                            if (role == "Giáo viên")
                            {
                                Teacher newTeacher = new Teacher();
                                newTeacher.IdUsers = NewUser.Id;
                                newTeacher.Id = Guid.NewGuid();
                                string temp = student[4].ToString();
                                newTeacher.Faculty = DataProvider.Instance.Database.Faculties.Where(x => x.DisplayName == temp).FirstOrDefault();
                                newTeacher.IdFaculty = newTeacher.Faculty.Id;
                                newTeacher.User = NewUser;

                                TeacherServices.Instance.SaveTeacherToDatabase(newTeacher);
                                UserDatabase.Add(new UserCard(newTeacher));
                            }

                            List<UserRole_UserInfo> db = DataProvider.Instance.Database.UserRole_UserInfo.Where(infoItem => infoItem.UserRole.Role == role).ToList();
                            foreach (var item in db)
                            {
                                if (item.InfoName != "Hệ đào tạo" && item.InfoName != "Khoa" && item.InfoName != "Họ và tên" && item.InfoName != "Địa chỉ email")
                                {
                                    User_UserRole_UserInfo newInfo = new User_UserRole_UserInfo();
                                    newInfo.Id = Guid.NewGuid();
                                    newInfo.IdUser = NewUser.Id;
                                    newInfo.IdUserRole_Info = item.Id;
                                    newInfo.UserRole_UserInfo = item;
                                    newInfo.User = NewUser;
                                    newInfo.Content = null;
                                    UserUserRoleUserInfoServices.Instance.SaveUserInfoToDatabase(newInfo);
                                }
                            }

                        }

                        MyMessageBox.Show("Thêm thành công");
                        SearchNameFunction();

                        return;
                    }
                }

            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra");
                return;
            }

        MyMessageBox.Show("Thêm thất bại");
            SearchNameFunction();
        }
    }
}
