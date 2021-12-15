using StudentManagement.Commands;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static StudentManagement.ViewModels.AdminFacultyTrainingFormViewModel;

namespace StudentManagement.ViewModels
{
    public class AdminFacultyTrainingFormRightSideBarViewModel : BaseViewModel
    {
        #region properties
        private static AdminFacultyTrainingFormRightSideBarViewModel s_instance;
        public static AdminFacultyTrainingFormRightSideBarViewModel Instance
        {
            get => s_instance ?? (s_instance = new AdminFacultyTrainingFormRightSideBarViewModel());

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

        private object _adminFacultyRightSideBarItemViewModel;

        private object _adminTrainingFormRightSideBarItemViewModel;

        private object _emptyStateRightSideBarViewModel;

        private FacultyCard _selectedFaculty;
        public FacultyCard SelectedFaculty
        {
            get => _selectedFaculty; set
            {
                _selectedFaculty = value;
                OnPropertyChanged();
                if (_selectedFaculty != null)
                {
                    _adminFacultyRightSideBarItemViewModel = new AdminFacultyRightSideBarItemViewModel(_selectedFaculty);
                    RightSideBarItemViewModel = _adminFacultyRightSideBarItemViewModel;
                }

            }
        }

        #endregion

        #region icommands
        public ICommand ShowFacultyCardInfo { get => _showFacultyCardInfo; set => _showFacultyCardInfo = value; }

        private ICommand _showFacultyCardInfo;

        public ICommand EditTrainingFormCardInfo { get => _editTrainingFormCardInfo; set => _editTrainingFormCardInfo = value; }

        private ICommand _editTrainingFormCardInfo;
        public ICommand DeleteTrainingFormCardInfo { get => _deleteTrainingFormCardInfo; set => _deleteTrainingFormCardInfo = value; }

        private ICommand _deleteTrainingFormCardInfo;

        public ICommand CreateTrainingFormCardInfo { get => _createTrainingFormCardInfo; set => _createTrainingFormCardInfo = value; }

        private ICommand _createTrainingFormCardInfo;

        public ICommand ShowTrainingFormCardInfo { get => _showTrainingFormCardInfo; set => _showTrainingFormCardInfo = value; }

        private ICommand _showTrainingFormCardInfo;

        public ICommand EditFacultyCardInfo { get => _editFacultyCardInfo; set => _editFacultyCardInfo = value; }

        private ICommand _editFacultyCardInfo;

        public ICommand DeleteFacultyCardInfo { get => _deleteFacultyCardInfo; set => _deleteFacultyCardInfo = value; }

        private ICommand _deleteFacultyCardInfo;
        public ICommand CreateFacultyCardInfo { get => _createFacultyCardInfo; set => _createFacultyCardInfo = value; }

        private ICommand _createFacultyCardInfo;
        #endregion

        public AdminFacultyTrainingFormRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();
            InitRightSideBarCommand();
            Instance = this;
        }

        #region methods
        public void InitRightSideBarItemViewModel()
        {
            _adminFacultyRightSideBarItemViewModel = new AdminFacultyRightSideBarItemViewModel();
            _adminTrainingFormRightSideBarItemViewModel = new AdminTrainingFormRightSideBarItemViewModel();
            _emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();

            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }

        public void InitRightSideBarCommand()
        {
            ShowFacultyCardInfo = new RelayCommand<UserControl>((p) => { return true; }, (p) => ShowFacultyCardByCardDataContext(p));
            ShowTrainingFormCardInfo = new RelayCommand<UserControl>((p) => { return true; }, (p) => ShowTrainingFormCardByCardDataContext(p));
            EditTrainingFormCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => EditTrainingFormCardByCardFunction(p));
            EditFacultyCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => EditFacultyCardByCardFunction(p));
            DeleteFacultyCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => DeleteFacultyCardByCardFunction(p));
            DeleteTrainingFormCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => DeleteTrainingFormCardByCardFunction(p));
            CreateFacultyCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => CreateFacultyCardByCardFunction());
            CreateTrainingFormCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => CreateTrainingFormCardByCardFunction());
        }

        public void LostFocusFaculty()
        {
            // auto lost focus faculty when click trainingform
            SelectedFaculty = null;
        }

        public void ShowFacultyCardByCardDataContext(UserControl p)
        {
            FacultyCard card = p.DataContext as FacultyCard;

            _adminFacultyRightSideBarItemViewModel = new AdminFacultyRightSideBarItemViewModel(card);

            RightSideBarItemViewModel = _adminFacultyRightSideBarItemViewModel;
        }

        public void ShowTrainingFormCardByCardDataContext(UserControl p)
        {
            LostFocusFaculty();

            TrainingFormCard card = p.DataContext as TrainingFormCard;

            _adminTrainingFormRightSideBarItemViewModel = new AdminTrainingFormRightSideBarItemViewModel(card);

            RightSideBarItemViewModel = _adminTrainingFormRightSideBarItemViewModel;
        }

        public void EditTrainingFormCardByCardFunction(object p)
        {
            LostFocusFaculty();

            TrainingFormCard card = p as TrainingFormCard;

            _adminTrainingFormRightSideBarItemViewModel = new AdminTrainingFormRightSideBarItemEditViewModel(card);

            RightSideBarItemViewModel = _adminTrainingFormRightSideBarItemViewModel;
        }

        public void EditFacultyCardByCardFunction(object p)
        {
            FacultyCard card = p as FacultyCard;

            _adminFacultyRightSideBarItemViewModel = new AdminFacultyRightSideBarItemEditViewModel(card);

            RightSideBarItemViewModel = _adminFacultyRightSideBarItemViewModel;
        }

        public void CreateFacultyCardByCardFunction()
        {
            FacultyCard card = new FacultyCard();

            _adminFacultyRightSideBarItemViewModel = new AdminFacultyRightSideBarItemEditViewModel(card, isCreatedNew: true);

            RightSideBarItemViewModel = _adminFacultyRightSideBarItemViewModel;
        }

        public void CreateTrainingFormCardByCardFunction()
        {
            TrainingFormCard card = new TrainingFormCard();

            _adminFacultyRightSideBarItemViewModel = new AdminTrainingFormRightSideBarItemEditViewModel(card, isCreatedNew: true);

            RightSideBarItemViewModel = _adminFacultyRightSideBarItemViewModel;
        }

        public void DeleteFacultyCardByCardFunction(object p)
        {
            FacultyCard card = p as FacultyCard;

            if (MyMessageBox.Show($"Bạn thực sự muốn xóa khoa {card?.DisplayName}? Xóa khoa vẫn giữ lại toàn bộ dữ liệu liên kết với khoa trước đó!!!", "Thông báo", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
            {
                bool success = FacultyServices.Instance.RemoveFacultyCardFromDatabase(card);

                if (success)
                {
                    FacultyCards.Remove(card);
                    StoredFacultyCards.Remove(card);
                    CurrentFacultyCards.Remove(card);
                    AdminFacultyTrainingFormViewModel.Instance.LoadFacultyByPageView();
                    MyMessageBox.Show($"Xóa khoa {card.DisplayName} thành công");
                }
                else
                {
                    MyMessageBox.Show("Có lỗi kết nối đến cơ sở dữ liệu, vui lòng thử lại sau");
                }

                RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
            }
        }
        public void DeleteTrainingFormCardByCardFunction(object p)
        {
            LostFocusFaculty();

            TrainingFormCard card = p as TrainingFormCard;

            if (MyMessageBox.Show($"Bạn thực sự muốn xóa hệ đào tạo {card?.DisplayName}? Dữ liệu về các khoa và sinh viên có hệ đào tạo này vẫn được giữ nguyên!!!", "Thông báo", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
            {
                bool success = TrainingFormServices.Instance.RemoveTrainingFormCardFromDatabase(card);

                if (success)
                {
                    TrainingFormCards.Remove(card);
                    MyMessageBox.Show($"Xóa hệ đào tạo {card.DisplayName} thành công");
                }
                else
                {
                    MyMessageBox.Show("Có lỗi kết nối đến cơ sở dữ liệu, vui lòng thử lại sau");
                }

                RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
            }

        }
        #endregion
    }
}
