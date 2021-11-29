using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
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

        public CourseItems CurrentItem
        {
            get { return _currentItem; }
            set
            {
                _currentItem = value;
                OnPropertyChanged();
            }
        }
        public string SubjectName { get => _subjectName; set { _subjectName = value; OnPropertyChanged(); } }
        public string CourseItemsCode { get => _subjectClassCode; set { _subjectClassCode = value; OnPropertyChanged(); } }
        public string MaxOfRegister { get => _maxOfRegister; set { _maxOfRegister = value; OnPropertyChanged(); }
}
        public string Period { get => _period; set { _period = value; OnPropertyChanged(); } }
        public string WeekDay { get => _weekDay; set { _weekDay = value; OnPropertyChanged(); } }
        public DateTime? StartDate { get => _startDate; set { _startDate = value; OnPropertyChanged(); } }
        public DateTime? EndDate { get => _endDate; set { _endDate = value; OnPropertyChanged(); } }


        private CourseItems _currentItem;

        private string _subjectName;
        private string _subjectClassCode;
        private string _maxOfRegister;
        private string _period;
        private string _weekDay;
        private DateTime? _startDate;
        private DateTime? _endDate;

        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        

        string[] DayOfWeeks = { "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy", "Chủ nhật" };

    public AdminCourseRegistryRightSideBarItemEditViewModel()
        {
            CurrentItem = null;
            SubjectName = "";
            CourseItemsCode = "";
            StartDate = null;
            EndDate = null;
            Period = "";
            MaxOfRegister = "";
            InitCommand();
        }

        public AdminCourseRegistryRightSideBarItemEditViewModel(CourseItems item)
        {
            CurrentItem = item;
            InitProperties();
            InitCommand();

        }

        public void InitProperties()
        {
            SubjectName = CurrentItem.Subject.DisplayName;
            /*CourseItemsCode = CurrentItem.Code;*/
            StartDate = CurrentItem.StartDate;
            EndDate = CurrentItem.EndDate;
            Period = CurrentItem.Period;
            WeekDay = CurrentItem.WeekDay;
            /*MaxOfRegister = CurrentItem.MaxOfRegister;*/
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
            CurrentItem.Subject.DisplayName = SubjectName;
            CurrentItem.StartDate = StartDate;
            CurrentItem.EndDate = EndDate;
            CurrentItem.Period = Period;
            CurrentItem.WeekDay = WeekDay;
            Cancel();
        }

        public void Cancel()
        {
            AdminCourseRegistryRightSideBarViewModel.Instance.RightSideBarItemViewModel = new AdminCourseRegistryRightSideBarItemViewModel(CurrentItem);
        }
    }
}
