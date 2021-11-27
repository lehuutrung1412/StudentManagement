using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class AdminCourseRegistryRightSideBarItemEditViewModel : BaseViewModel
    {
        // currentCard just for binding to view, actualcard is real card

        public TempSubjectClass CurrentItem
        {
            get { return _currentItem; }
            set
            {
                _currentItem = value;
                OnPropertyChanged();
            }
        }
        public string SubjectName { get => _subjectName; set { _subjectName = value; OnPropertyChanged(); } }
        public string SubjectClassCode { get => _subjectClassCode; set { _subjectClassCode = value; OnPropertyChanged(); } }
        public string Limit { get => _limit; set { _limit = value; OnPropertyChanged(); } }
        public string TKB { get => _tKB; set { _tKB = value; OnPropertyChanged(); } }


        private TempSubjectClass _currentItem;

        private string _subjectName;
        private string _subjectClassCode;
        private string _limit;
        private string _tKB;
        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        

        public AdminCourseRegistryRightSideBarItemEditViewModel()
        {
            CurrentItem = null;
            SubjectName = "";
            SubjectClassCode = "";
            TKB = "";
            InitCommand();
        }

        public AdminCourseRegistryRightSideBarItemEditViewModel(TempSubjectClass item)
        {
            CurrentItem = item;
            InitProperties();
            InitCommand();

        }

        public void InitProperties()
        {
            SubjectName = CurrentItem.SubjectName;
            SubjectClassCode = CurrentItem.IdSubjectClass;
            TKB = CurrentItem.TKB;
        }

        public void InitCommand()
        {
            ConfirmCommand = new RelayCommand<object>(
                (p) =>
                {
                    return true;
                },
                (p) =>
                {
                    Confirm();
                });
            CancelCommand = new RelayCommand<object>(
                (p) =>
                {
                    return true;
                },
                (p) =>
                {
                    Cancel();
                });
        }

        public void Confirm()
        {
            CurrentItem.TKB = TKB;
            Cancel();
        }

        public void Cancel()
        {
            AdminCourseRegistryRightSideBarViewModel.Instance.RightSideBarItemViewModel = new AdminCourseRegistryRightSideBarItemViewModel(CurrentItem);
        }
    }
}
