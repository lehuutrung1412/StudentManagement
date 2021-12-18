using StudentManagement.Commands;
using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static StudentManagement.ViewModels.AdminStudentListViewModel;
using StudentManagement.Services;
using StudentManagement.ViewModels.UserInfo;
using static StudentManagement.Services.LoginServices;

namespace StudentManagement.ViewModels
{
    public class CampusStudentListRightSideBarViewModel : BaseViewModel
    {
        private static CampusStudentListRightSideBarViewModel s_instance;
        public static CampusStudentListRightSideBarViewModel Instance
        {
            get => s_instance ?? (s_instance = new CampusStudentListRightSideBarViewModel());

            private set => s_instance = value;
        }

        private object _rightSideBarItemViewModel;

        public object RightSideBarItemViewModel
        {
            get { return _rightSideBarItemViewModel; }
            set
            {
                _rightSideBarItemViewModel = value;
                OnPropertyChanged();
            }
        }

        private UserCard _selectedItem;
        public UserCard SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<InfoItemViewModel> _infoSource;
        public ObservableCollection<InfoItemViewModel> InfoSource { get => _infoSource; set => _infoSource = value; }

        private object _campusStudentListRightSideBarItemViewModel;

        private object _emptyStateRightSideBarViewModel;

        

        public ICommand EditStudentInfo { get => _editStudentInfo; set => _editStudentInfo = value; }

        private ICommand _editStudentInfo;

        private ICommand _showStudentCardInfo;

        public ICommand ShowStudentCardInfo { get => _showStudentCardInfo; set => _showStudentCardInfo = value; }

        public void InitRightSideBarItemViewModel()
        {
            _campusStudentListRightSideBarItemViewModel = new CampusStudentListRightSideBarItemViewModel();
            _emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();
            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }

        public void InitRightSideBarCommand()
        {
            EditStudentInfo = new RelayCommand<object>((p) => { return true; }, (p) => EditStudentInfoFunction(p));
            ShowStudentCardInfo = new RelayCommand<UserControl>((p) => { return true; }, (p) => ShowStudentCardInfoFunction(p));
        }

        void EditStudentInfoFunction(object p)
        {
            UserCard currentStudent = p as UserCard;
            _campusStudentListRightSideBarItemViewModel = new CampusStudentListRightSideBarItemEditViewModel(SelectedItem);
            RightSideBarItemViewModel = _campusStudentListRightSideBarItemViewModel;
        }

        public CampusStudentListRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();
            InitRightSideBarCommand();
            InfoSource = new ObservableCollection<InfoItemViewModel>();

            Instance = this;
            LoginServices.UpdateCurrentUser += FreeRightSideBar;
        }

        public void LoadInfoSource(Guid IdUser)
        {
            try
            {
                var user = UserServices.Instance.GetUserById(IdUser);
                InfoSource = new ObservableCollection<InfoItemViewModel>();
                InfoSource.Add(new InfoItemViewModel(new InfoItem(Guid.NewGuid(), "Họ và tên", 0, null, user.DisplayName, false)));
                InfoSource.Add(new InfoItemViewModel(new InfoItem(Guid.NewGuid(), "Địa chỉ email", 0, null, user.Email, false)));

                switch (user.UserRole.Role)
                {
                    case "Sinh viên":
                        {
                            var student = StudentServices.Instance.GetStudentbyUser(user);

                            InfoSource.Add(new InfoItemViewModel(new InfoItem(Guid.NewGuid(), "Khoa", 2, FacultyServices.Instance.LoadListFaculty(), student.Faculty.DisplayName, true)));
                            InfoSource.Add(new InfoItemViewModel(new InfoItem(Guid.NewGuid(), "Hệ đào tạo", 2, TrainingFormServices.Instance.LoadListTrainingForm(), student.TrainingForm.DisplayName, true)));
                            break;
                        }
                    case "Giáo viên":
                        {

                            var lecture = TeacherServices.Instance.GetTeacherbyUser(user);
                            InfoSource.Add(new InfoItemViewModel(new InfoItem(Guid.NewGuid(), "Khoa", 2, FacultyServices.Instance.LoadListFaculty(), lecture.Faculty.DisplayName, false)));
                            break;
                        }
                    case "Admin":
                        {
                            foreach (var infoItem in InfoSource)
                                infoItem.CurrendInfoItem.IsEnable = true;
                            break;
                        }
                }
                var listInfoItem = InfoItemServices.Instance.GetInfoSourceByUserId(IdUser);
                foreach (var infoItem in listInfoItem)
                {
                    InfoSource.Add(new InfoItemViewModel(infoItem));
                }
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra");
                
            }
        }

        void ShowStudentCardInfoFunction(UserControl p)
        {
            UserCard currentStudent = p.DataContext as UserCard;
            SelectedItem = currentStudent;
            ShowStudentCardInfoDetail(currentStudent);
        }

        public void ShowStudentCardInfoDetail(UserCard currentStudent)
        {
            LoadInfoSource((Guid)currentStudent.ID);
            _campusStudentListRightSideBarItemViewModel = new CampusStudentListRightSideBarItemViewModel(InfoSource);
            RightSideBarItemViewModel = _campusStudentListRightSideBarItemViewModel;
        }

        private void FreeRightSideBar(object sender, LoginEvent e)
        {
            _rightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }

    }
}
