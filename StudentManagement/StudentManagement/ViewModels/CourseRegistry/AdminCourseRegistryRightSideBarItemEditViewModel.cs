using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class AdminCourseRegistryRightSideBarItemEditViewModel : BaseViewModel
    {
        // currentCard just for binding to view, actualcard is real card

        public CourseItem CurrentItem
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
        public string MaxOfRegister { get => _maxOfRegister; set { _maxOfRegister = value; OnPropertyChanged(); }
}
        public string Period { get => _period; set { _period = value; OnPropertyChanged(); } }
        public string WeekDay { get => _weekDay; set { _weekDay = value; OnPropertyChanged(); } }
        public DateTime? StartDate { get => _startDate; set { _startDate = value; OnPropertyChanged(); } }
        public DateTime? EndDate { get => _endDate; set { _endDate = value; OnPropertyChanged(); } }


        private CourseItem _currentItem;

        private string _subjectName;
        private string _subjectClassCode;
        private string _maxOfRegister;
        private string _period;
        private string _weekDay;
        private DateTime? _startDate;
        private DateTime? _endDate;

        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ObservableCollection<string> DayOfWeeks { get => _dayOfWeeks; set { _dayOfWeeks = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _dayOfWeeks;
        
        public AdminCourseRegistryRightSideBarItemEditViewModel()
        {
            CurrentItem = null;
            SubjectName = "";
            SubjectClassCode = "";
            StartDate = null;
            EndDate = null;
            Period = "";
            MaxOfRegister = "";
            InitCommand();
        }

        public AdminCourseRegistryRightSideBarItemEditViewModel(CourseItem item)
        {
            CurrentItem = item;
            InitProperties();
            InitCommand();

        }

        public void InitProperties()
        {
            SubjectName = CurrentItem.Subject.DisplayName;
            SubjectClassCode = CurrentItem.Code;
            StartDate = CurrentItem.StartDate;
            EndDate = CurrentItem.EndDate;
            Period = CurrentItem.Period;
            WeekDay = CurrentItem.WeekDay;
            DayOfWeeks = new ObservableCollection<string>() { "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy", "Chủ nhật" };
            MaxOfRegister = Convert.ToString(CurrentItem.MaxNumberOfStudents);
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
            CurrentItem.StartDate = StartDate;
            CurrentItem.EndDate = EndDate;
            CurrentItem.Period = Period;
            CurrentItem.WeekDay = WeekDay;
            CurrentItem.MaxNumberOfStudents = Convert.ToInt32(MaxOfRegister);
            SubjectClass tempSubjectClass = CurrentItem.ConvertToSubjectClass();
            SubjectClassServices.Instance.SaveSubjectClassToDatabase(tempSubjectClass);
            Cancel();
        }

        public void Cancel()
        {
            AdminCourseRegistryRightSideBarViewModel.Instance.RightSideBarItemViewModel = new AdminCourseRegistryRightSideBarItemViewModel(CurrentItem);
        }
    }
}
