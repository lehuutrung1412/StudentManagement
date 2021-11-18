using StudentManagement.Commands;
using StudentManagement.Objects;
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
    #region properties
    public class AdminFacultyTrainingFormRightSideBarViewModel : BaseViewModel
    {
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

        public ICommand ShowTrainingFormCardInfo { get => _showTrainingFormCardInfo; set => _showTrainingFormCardInfo = value; }

        private ICommand _showTrainingFormCardInfo;

        public ICommand EditFacultyCardInfo { get => _editFacultyCardInfo; set => _editFacultyCardInfo = value; }

        private ICommand _editFacultyCardInfo;

        public ICommand DeleteFacultyCardInfo { get => _deleteFacultyCardInfo; set => _deleteFacultyCardInfo = value; }

        private ICommand _deleteFacultyCardInfo;

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
        }

        public void ShowFacultyCardByCardDataContext(UserControl p)
        {
            FacultyCard card = p.DataContext as FacultyCard;

            _adminFacultyRightSideBarItemViewModel = new AdminFacultyRightSideBarItemViewModel(card);

            RightSideBarItemViewModel = _adminFacultyRightSideBarItemViewModel;
        }

        public void ShowTrainingFormCardByCardDataContext(UserControl p)
        {
            TrainingFormCard card = p.DataContext as TrainingFormCard;

            _adminTrainingFormRightSideBarItemViewModel = new AdminTrainingFormRightSideBarItemViewModel(card);

            RightSideBarItemViewModel = _adminTrainingFormRightSideBarItemViewModel;
        }

        public void EditTrainingFormCardByCardFunction(object p)
        {
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

        public void DeleteFacultyCardByCardFunction(object p)
        {
            FacultyCard card = p as FacultyCard;

            FacultyCards.Remove(card);
            StoredFacultyCards.Remove(card);

            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }
        public void DeleteTrainingFormCardByCardFunction(object p)
        {
            TrainingFormCard card = p as TrainingFormCard;

            TrainingFormCards.Remove(card);

            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }
        #endregion
    }
}
