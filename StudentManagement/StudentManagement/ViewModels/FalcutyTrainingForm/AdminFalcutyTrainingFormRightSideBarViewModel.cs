using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static StudentManagement.ViewModels.AdminFalcutyTrainingFormViewModel;

namespace StudentManagement.ViewModels
{
    #region properties
    public class AdminFalcutyTrainingFormRightSideBarViewModel : BaseViewModel
    {
        private static AdminFalcutyTrainingFormRightSideBarViewModel s_instance;
        public static AdminFalcutyTrainingFormRightSideBarViewModel Instance
        {
            get => s_instance ?? (s_instance = new AdminFalcutyTrainingFormRightSideBarViewModel());

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

        private object _adminFalcutyRightSideBarItemViewModel;

        private object _adminTrainingFormRightSideBarItemViewModel;

        private object _emptyStateRightSideBarViewModel;

        private FalcutyCard _selectedFalcuty;
        public FalcutyCard SelectedFalcuty
        {
            get => _selectedFalcuty; set
            {
                _selectedFalcuty = value;
                OnPropertyChanged();
                if (_selectedFalcuty != null)
                {
                    _adminFalcutyRightSideBarItemViewModel = new AdminFalcutyRightSideBarItemViewModel(_selectedFalcuty);
                    RightSideBarItemViewModel = _adminFalcutyRightSideBarItemViewModel;
                }

            }
        }

        #endregion

        #region icommands
        public ICommand ShowFalcutyCardInfo { get => _showFalcutyCardInfo; set => _showFalcutyCardInfo = value; }

        private ICommand _showFalcutyCardInfo;

        public ICommand EditTrainingFormCardInfo { get => _editTrainingFormCardInfo; set => _editTrainingFormCardInfo = value; }

        private ICommand _editTrainingFormCardInfo;
        public ICommand DeleteTrainingFormCardInfo { get => _deleteTrainingFormCardInfo; set => _deleteTrainingFormCardInfo = value; }

        private ICommand _deleteTrainingFormCardInfo;

        public ICommand ShowTrainingFormCardInfo { get => _showTrainingFormCardInfo; set => _showTrainingFormCardInfo = value; }

        private ICommand _showTrainingFormCardInfo;

        public ICommand EditFalcutyCardInfo { get => _editFalcutyCardInfo; set => _editFalcutyCardInfo = value; }

        private ICommand _editFalcutyCardInfo;

        public ICommand DeleteFalcutyCardInfo { get => _deleteFalcutyCardInfo; set => _deleteFalcutyCardInfo = value; }

        private ICommand _deleteFalcutyCardInfo;

        #endregion

        public AdminFalcutyTrainingFormRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();
            InitRightSideBarCommand();
            Instance = this;
        }

        #region methods
        public void InitRightSideBarItemViewModel()
        {
            _adminFalcutyRightSideBarItemViewModel = new AdminFalcutyRightSideBarItemViewModel();
            _adminTrainingFormRightSideBarItemViewModel = new AdminTrainingFormRightSideBarItemViewModel();
            _emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();

            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }

        public void InitRightSideBarCommand()
        {
            ShowFalcutyCardInfo = new RelayCommand<UserControl>((p) => { return true; }, (p) => ShowFalcutyCardByCardDataContext(p));
            ShowTrainingFormCardInfo = new RelayCommand<UserControl>((p) => { return true; }, (p) => ShowTrainingFormCardByCardDataContext(p));
            EditTrainingFormCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => EditTrainingFormCardByCardFunction(p));
            EditFalcutyCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => EditFalcutyCardByCardFunction(p));
            DeleteFalcutyCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => DeleteFalcutyCardByCardFunction(p));
            DeleteTrainingFormCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => DeleteTrainingFormCardByCardFunction(p));
        }

        public void ShowFalcutyCardByCardDataContext(UserControl p)
        {
            FalcutyCard card = p.DataContext as FalcutyCard;

            _adminFalcutyRightSideBarItemViewModel = new AdminFalcutyRightSideBarItemViewModel(card);

            RightSideBarItemViewModel = _adminFalcutyRightSideBarItemViewModel;
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

        public void EditFalcutyCardByCardFunction(object p)
        {
            FalcutyCard card = p as FalcutyCard;

            _adminFalcutyRightSideBarItemViewModel = new AdminFalcutyRightSideBarItemEditViewModel(card);

            RightSideBarItemViewModel = _adminFalcutyRightSideBarItemViewModel;
        }

        public void DeleteFalcutyCardByCardFunction(object p)
        {
            FalcutyCard card = p as FalcutyCard;

            FalcutyCards.Remove(card);
            StoredFalcutyCards.Remove(card);

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
