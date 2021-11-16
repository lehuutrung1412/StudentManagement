using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static StudentManagement.ViewModels.SubjectClass.AdminSubjectClassViewModel;

namespace StudentManagement.ViewModels.SubjectClass
{
    public class AdminSubjectClassRightSideBarViewModel : BaseViewModel
    {
        #region properties
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
        #endregion

        #region icommand

        public ICommand ShowCardInfo { get => _showCardInfo; set => _showCardInfo = value; }

        private ICommand _showCardInfo;

        #endregion

        public AdminSubjectClassRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();

            ShowCardInfo = new RelayCommand<UserControl>((p) => { return true; }, (p) => ShowCardInfoByCardDataContext(p));
        }

        #region methods
        public void InitRightSideBarItemViewModel()
        {
            _adminSubjectClassRightSideBarItemViewModel = new AdminSubjectClassRightSideBarItemViewModel();
            _emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();

            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }
        public void ShowCardInfoByCardDataContext(UserControl p)
        {
            SubjectCard card = p.DataContext as SubjectCard;

            _adminSubjectClassRightSideBarItemViewModel = new AdminSubjectClassRightSideBarItemViewModel(card);

            RightSideBarItemViewModel = _adminSubjectClassRightSideBarItemViewModel;
        }
        #endregion
    }
}
