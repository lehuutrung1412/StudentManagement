using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class AdminCourseRegistryRightSideBarItemEditViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        // currentCard just for binding to view, actualcard is real card
        #region Validation
        private readonly ErrorBaseViewModel _errorBaseViewModel;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public bool CanConfirm => !HasErrors;
        public bool HasErrors => _errorBaseViewModel.HasErrors;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorBaseViewModel.GetErrors(propertyName);
        }

        private void ErrorBaseViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanConfirm));
        }
        #endregion Validation
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
        public string SubjectClassCode 
        { 
            get => _subjectClassCode; 
            set 
            { 
                _subjectClassCode = value;

                // Validation
                _errorBaseViewModel.ClearErrors();

                if (string.IsNullOrWhiteSpace(SubjectClassCode))
                {
                    _errorBaseViewModel.AddError(nameof(SubjectClassCode), "Vui lòng nhập mã lớp môn học!");
                }
                else if (!SubjectClassCode.Contains(CurrentItem.Subject.Code))
                {
                    _errorBaseViewModel.AddError(nameof(SubjectClassCode), "Mã lớp môn học phải chứa mã môn học");
                }
                OnPropertyChanged(); 
            } 
        }
        public string MaxOfRegister
        {
            get => _maxOfRegister;
            set
            {
                _maxOfRegister = value;

                // Validation
                _errorBaseViewModel.ClearErrors();

                if (string.IsNullOrWhiteSpace(MaxOfRegister))
                {
                    _errorBaseViewModel.AddError(nameof(MaxOfRegister), "Vui lòng nhập sĩ số tối đa!");
                }
                int tempTryParse;
                if (!int.TryParse(MaxOfRegister, out tempTryParse) || tempTryParse < 0)
                {
                    _errorBaseViewModel.AddError(nameof(MaxOfRegister), "Giá trị phải là số nguyên dương!");
                }
                OnPropertyChanged();
            }
        }
        public string Period
        {
            get => _period;
            set
            {
                _period = value;

                // Validation
                _errorBaseViewModel.ClearErrors();

                if (string.IsNullOrWhiteSpace(Period))
                {
                    _errorBaseViewModel.AddError(nameof(Period), "Vui lòng nhập tiết học!");
                }

                if (!SubjectClassServices.Instance.IsValidPeriod(Period))
                {
                    _errorBaseViewModel.AddError(nameof(Period), "Tiết học không hợp lệ!");
                }
                OnPropertyChanged();
            }
        }
        public string WeekDay
        {
            get => _weekDay;
            set
            {
                _weekDay = value;

                //Validation
                _errorBaseViewModel.ClearErrors();

                if (string.IsNullOrWhiteSpace(WeekDay))
                {
                    _errorBaseViewModel.AddError(nameof(WeekDay), "Vui lòng chọn thứ trong tuần");
                }
                OnPropertyChanged();
            }
        }
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;

                //Validation
                _errorBaseViewModel.ClearErrors();
                _errorBaseViewModel.ClearErrors(nameof(EndDate));

                if (!StartDate.HasValue)
                {
                    _errorBaseViewModel.AddError(nameof(StartDate), "Vui lòng chọn ngày bắt đầu!");
                }
                if (StartDate > EndDate)
                {
                    _errorBaseViewModel.AddError(nameof(StartDate), "Ngày bắt đầu không được trễ hơn ngày kết thúc");
                }
                OnPropertyChanged();
            }
        }
        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;

                //Validation
                _errorBaseViewModel.ClearErrors();
                _errorBaseViewModel.ClearErrors(nameof(StartDate));

                if (!EndDate.HasValue)
                {
                    _errorBaseViewModel.AddError(nameof(EndDate), "Vui lòng chọn ngày kết thúc!");
                }
                if (StartDate > EndDate)
                {
                    _errorBaseViewModel.AddError(nameof(EndDate), "Ngày kết thúc không được sớm hơn ngày bắt đầu");
                }
                OnPropertyChanged();
            }
        }
        public Teacher SelectedTeacher
        {
            get => _selectedTeacher;
            set
            {
                _selectedTeacher = value;

                //Validaton
                _errorBaseViewModel.ClearErrors();
                if (SelectedTeacher == null)
                    _errorBaseViewModel.AddError(nameof(SelectedTeacher), "Vui lòng chọn giáo viên");
                OnPropertyChanged();
            }
        }

        private CourseItem _currentItem;

        private string _subjectName;
        private string _subjectClassCode;
        private string _maxOfRegister;
        private string _period;
        private string _weekDay;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private Teacher _selectedTeacher;

        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ObservableCollection<string> DayOfWeeks { get => _dayOfWeeks; set { _dayOfWeeks = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _dayOfWeeks;

        public ObservableCollection<Teacher> Teachers { get => _teachers; set { _teachers = value; OnPropertyChanged(); } }
        private ObservableCollection<Teacher> _teachers;

        public AdminCourseRegistryRightSideBarItemEditViewModel()
        {
            _errorBaseViewModel = new ErrorBaseViewModel();
            _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;

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
            _errorBaseViewModel = new ErrorBaseViewModel();
            _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;

            CurrentItem = item;
            InitProperties();
            InitCommand();

        }

        public void InitProperties()
        {
            DayOfWeeks = new ObservableCollection<string>() { "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy", "Chủ nhật" };
            Teachers = new ObservableCollection<Teacher>(TeacherServices.Instance.LoadTeacherList());
            SubjectName = CurrentItem.Subject.DisplayName;
            SubjectClassCode = CurrentItem.Code;
            StartDate = CurrentItem.StartDate;
            EndDate = CurrentItem.EndDate;
            Period = CurrentItem.Period;
            WeekDay = DayOfWeeks[(int)CurrentItem.WeekDay];
            MaxOfRegister = Convert.ToString(CurrentItem.MaxNumberOfStudents);
            SelectedTeacher = CurrentItem.Teachers.FirstOrDefault();
        }

        public void InitCommand()
        {
            ConfirmCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (_errorBaseViewModel.HasErrors)
                        return false;
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
            CurrentItem.Code = SubjectClassCode;
            CurrentItem.StartDate = StartDate;
            CurrentItem.EndDate = EndDate;
            CurrentItem.Period = Period;
            CurrentItem.WeekDay = DayOfWeeks.IndexOf(WeekDay);
            CurrentItem.MaxNumberOfStudents = Convert.ToInt32(MaxOfRegister);
            //CurrentItem.Teachers = new ObservableCollection<Teacher>() { SelectedTeacher };

            var temp = SubjectClassServices.Instance.FindSubjectClassBySubjectClassId(CurrentItem.Id);

            if (temp != null)
            {
                CurrentItem.Teachers = temp.Teachers;
            }
            try
            {
                CurrentItem.Teachers.Clear();
                CurrentItem.Teachers.Add(SelectedTeacher);
                CurrentItem.MainTeacher = SelectedTeacher;

                SubjectClass tempSubjectClass = CurrentItem.ConvertToSubjectClass();
                if (!SubjectClassServices.Instance.SaveSubjectClassToDatabase(tempSubjectClass))
                    throw new Exception();
                else
                    MyMessageBox.Show("Chỉnh sửa thành công", "Thành công");
            }
            catch
            {
                MyMessageBox.Show("Chỉnh sửa thất bại", "Lỗi");
            }
            
            Cancel();
        }

        public void Cancel()
        {
            AdminCourseRegistryRightSideBarViewModel.Instance.RightSideBarItemViewModel = new AdminCourseRegistryRightSideBarItemViewModel(CurrentItem);
        }
    }
}
