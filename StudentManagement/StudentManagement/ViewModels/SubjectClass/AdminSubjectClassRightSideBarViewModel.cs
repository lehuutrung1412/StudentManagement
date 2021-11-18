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
        #region properties
        private static AdminSubjectClassRightSideBarViewModel s_instance;
        public static AdminSubjectClassRightSideBarViewModel Instance
        {
            get => s_instance ?? (s_instance = new AdminSubjectClassRightSideBarViewModel());

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

        private object _adminSubjectClassRightSideBarItemViewModel;

        private object _emptyStateRightSideBarViewModel;
        #endregion

        #region icommand

        public ICommand ShowCardInfo { get => _showCardInfo; set => _showCardInfo = value; }

        private ICommand _showCardInfo;
        public ICommand EditSubjectCardInfo { get => _editSubjectCardInfo; set => _editSubjectCardInfo = value; }

        private ICommand _editSubjectCardInfo;
        public ICommand DeleteSubjectCardInfo { get => _deleteSubjectCardInfo; set => _deleteSubjectCardInfo = value; }

        private ICommand _deleteSubjectCardInfo;

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

        public void EditSubjectCardByCardFunction(object p)
        {
            SubjectCard card = p as SubjectCard;

            _adminSubjectClassRightSideBarItemViewModel = new AdminSubjectClassRightSideBarItemEditViewModel(card);

            RightSideBarItemViewModel = _adminSubjectClassRightSideBarItemViewModel;
        }

        public void DeleteSubjectCardByCardFunction(object p)
        {
            SubjectCard card = p as SubjectCard;

            SubjectCards.Remove(card);

            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }
        #endregion
    }
}
