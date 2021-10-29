using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static StudentManagement.ViewModels.AdminSubjectClassViewModel;

namespace StudentManagement.ViewModels
{
    public class AdminSubjectClassRightSideBarViewModel : BaseViewModel
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


        private object _adminSubjectClassRightSideBarItemViewModel;

        private object _emptyStateRightSideBarViewModel;


        public ICommand ShowCardInfo { get => _showCardInfo; set => _showCardInfo = value; }

        private ICommand _showCardInfo;

        public AdminSubjectClassRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();

            ShowCardInfo = new RelayCommand<UserControl>((p) => { return true; }, (p) => ShowCardInfoByCardDataContext(p));
        }


        public void InitRightSideBarItemViewModel()
        {
            this._adminSubjectClassRightSideBarItemViewModel = new AdminSubjectClassRightSideBarItemViewModel();
            this._emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();

            this.RightSideBarItemViewModel = this._emptyStateRightSideBarViewModel;
        }
        public void ShowCardInfoByCardDataContext(UserControl p)
        {
            CardInfo card = p.DataContext as CardInfo;

            this._adminSubjectClassRightSideBarItemViewModel = new AdminSubjectClassRightSideBarItemViewModel(card);

            this.RightSideBarItemViewModel = this._adminSubjectClassRightSideBarItemViewModel;
        }
    }
}
