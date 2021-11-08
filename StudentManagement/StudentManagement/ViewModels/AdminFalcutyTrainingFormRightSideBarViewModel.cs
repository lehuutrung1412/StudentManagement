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
    public class AdminFalcutyTrainingFormRightSideBarViewModel : BaseViewModel
    {
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
                    this._adminFalcutyRightSideBarItemViewModel = new AdminFalcutyRightSideBarItemViewModel(_selectedFalcuty);
                    this.RightSideBarItemViewModel = this._adminFalcutyRightSideBarItemViewModel;
                }

            }
        }

        public ICommand ShowFalcutyCardInfo { get => _showFalcutyCardInfo; set => _showFalcutyCardInfo = value; }

        private ICommand _showFalcutyCardInfo;

        public ICommand ShowTrainingFormCardInfo { get => _showTrainingFormCardInfo; set => _showTrainingFormCardInfo = value; }

        private ICommand _showTrainingFormCardInfo;

        public AdminFalcutyTrainingFormRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();
            InitRightSideBarCommand();
        }

        public void InitRightSideBarItemViewModel()
        {
            this._adminFalcutyRightSideBarItemViewModel = new AdminFalcutyRightSideBarItemViewModel();
            this._adminTrainingFormRightSideBarItemViewModel = new AdminTrainingFormRightSideBarItemViewModel();
            this._emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();

            this.RightSideBarItemViewModel = this._emptyStateRightSideBarViewModel;
        }

        public void InitRightSideBarCommand()
        {
            ShowFalcutyCardInfo = new RelayCommand<UserControl>((p) => { return true; }, (p) => ShowFalcutyCardByCardDataContext(p));
            ShowTrainingFormCardInfo = new RelayCommand<UserControl>((p) => { return true; }, (p) => ShowTrainingFormCardByCardDataContext(p));
        }

        public void ShowFalcutyCardByCardDataContext(UserControl p)
        {
            FalcutyCard card = p.DataContext as FalcutyCard;

            this._adminFalcutyRightSideBarItemViewModel = new AdminFalcutyRightSideBarItemViewModel(card);

            this.RightSideBarItemViewModel = this._adminFalcutyRightSideBarItemViewModel;
        }

        public void ShowTrainingFormCardByCardDataContext(UserControl p)
        {
            TrainingFormCard card = p.DataContext as TrainingFormCard;

            this._adminTrainingFormRightSideBarItemViewModel = new AdminTrainingFormRightSideBarItemViewModel(card);

            this.RightSideBarItemViewModel = this._adminTrainingFormRightSideBarItemViewModel;
        }
    }
}
