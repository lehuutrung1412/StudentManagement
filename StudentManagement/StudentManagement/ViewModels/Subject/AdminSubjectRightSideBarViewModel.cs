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

namespace StudentManagement.ViewModels
{
    public class AdminSubjectRightSideBarViewModel : BaseViewModel
    {
        #region properties
        private static AdminSubjectRightSideBarViewModel s_instance;
        public static AdminSubjectRightSideBarViewModel Instance
        {
            get => s_instance ?? (s_instance = new AdminSubjectRightSideBarViewModel());

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

        private object _adminSubjectRightSideBarItemViewModel;

        private object _emptyStateRightSideBarViewModel;

        private SubjectCard _selectedSubject;
        public SubjectCard SelectedSubject
        {
            get => _selectedSubject; set
            {
                _selectedSubject = value;
                OnPropertyChanged();
                if (_selectedSubject != null)
                {
                    _adminSubjectRightSideBarItemViewModel = new AdminSubjectRightSideBarItemViewModel(_selectedSubject);
                    RightSideBarItemViewModel = _adminSubjectRightSideBarItemViewModel;
                }

            }
        }

        #endregion

        #region icommand
        public ICommand ShowSubjectCardInfo { get => _showSubjectCardInfo; set => _showSubjectCardInfo = value; }

        private ICommand _showSubjectCardInfo;

        public ICommand EditSubjectCardInfo { get => _editSubjectCardInfo; set => _editSubjectCardInfo = value; }

        private ICommand _editSubjectCardInfo;

        public ICommand DeleteSubjectCardInfo { get => _deleteSubjectCardInfo; set => _deleteSubjectCardInfo = value; }

        private ICommand _deleteSubjectCardInfo;
        public ICommand CreateSubjectCardInfo { get => _createSubjectCardInfo; set => _createSubjectCardInfo = value; }

        private ICommand _createSubjectCardInfo;
        #endregion

        public AdminSubjectRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();
            InitRightSideBarCommand();
            Instance = this;
        }


        #region methods
        public void InitRightSideBarItemViewModel()
        {
            _adminSubjectRightSideBarItemViewModel = new AdminSubjectRightSideBarItemViewModel();
            _emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();
            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }

        public void InitRightSideBarCommand()
        {
            ShowSubjectCardInfo = new RelayCommand<UserControl>((p) => { return true; }, (p) => ShowSubjectCardByCardDataContext(p));
            EditSubjectCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => EditSubjectCardByCardFunction(p));
            DeleteSubjectCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => DeleteSubjectCardByCardFunction(p));
            CreateSubjectCardInfo = new RelayCommand<object>((p) => { return true; }, (p) => CreateSubjectCardByCardFunction());
        }

        public void ShowSubjectCardByCardDataContext(UserControl p)
        {
            SubjectCard card = p.DataContext as SubjectCard;

            _adminSubjectRightSideBarItemViewModel = new AdminSubjectRightSideBarItemViewModel(card);

            RightSideBarItemViewModel = _adminSubjectRightSideBarItemViewModel;
        }


        public void EditSubjectCardByCardFunction(object p)
        {
            SubjectCard card = p as SubjectCard;

            _adminSubjectRightSideBarItemViewModel = new AdminSubjectRightSideBarItemEditViewModel(card);

            RightSideBarItemViewModel = _adminSubjectRightSideBarItemViewModel;
        }

        public void CreateSubjectCardByCardFunction()
        {
            SubjectCard card = new SubjectCard();

            _adminSubjectRightSideBarItemViewModel = new AdminSubjectRightSideBarItemEditViewModel(card, isCreatedNew: true);

            RightSideBarItemViewModel = _adminSubjectRightSideBarItemViewModel;
        }


        public void DeleteSubjectCardByCardFunction(object p)
        {
            SubjectCard card = p as SubjectCard;

            if (MyMessageBox.Show($"Bạn thực sự muốn xóa môn học {card.DisplayName}({card?.Code})? Xóa môn học sẽ không xóa các lớp học và điểm thành phần của sinh viên!!!", "Thông báo", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
            {
                bool success = SubjectServices.Instance.RemoveSubjectCardFromDatabase(card);

                if (success)
                {
                    AdminSubjectViewModel.SubjectCards.Remove(card);
                    AdminSubjectViewModel.StoredSubjectCards.Remove(card);
                    MyMessageBox.Show($"Xóa môn học {card.DisplayName}({card?.Code}) thành công");
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
