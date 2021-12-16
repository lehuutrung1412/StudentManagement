using StudentManagement.Models;
using StudentManagement.Services;
using StudentManagement.ViewModels;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace StudentManagement.Objects
{
    public class SubjectClassCard : BaseObjectWithBaseViewModel, IBaseCard, INotifyDataErrorInfo
    {
        // define validation rule
        private readonly ErrorBaseViewModel _errorBaseViewModel = new ErrorBaseViewModel();

        public bool HasErrors
        {
            get => _errorBaseViewModel.HasErrors;
            set { }
        }

        private bool IsValid(string propertyName)
        {
            return !string.IsNullOrEmpty(propertyName) && !string.IsNullOrWhiteSpace(propertyName);
        }

        private void ErrorBaseViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            //OnPropertyChanged(nameof(CanLogin));
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorBaseViewModel.GetErrors(propertyName);
        }

        private int? _numberOfStudents = 0;
        private Guid _id;
        private string _code;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private string _period;
        private int? _maxNumberOfStudents;
        private string _image;
        private Teacher _selectedTeacher = null;
        private Subject _selectedSubject = null;
        private TrainingForm _selectedTrainingForm;
        private Semester _selectedSemester;
        private string _selectedDay;

        private ObservableCollection<string> _dayOfWeeks;
        private ObservableCollection<Subject> _subjects;
        private ObservableCollection<TrainingForm> _trainingForms;
        private ObservableCollection<Semester> _semesters;
        private ObservableCollection<Teacher> _teachers;


        public SubjectClassCard()
        {
            Id = Guid.NewGuid();
            _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;
            InitCardData();
        }

        public void InitCardData()
        {
            Subjects = new ObservableCollection<Subject>(SubjectServices.Instance.LoadSubjectList().Where(el => el.IsDeleted != true));
            TrainingForms = new ObservableCollection<TrainingForm>(DataProvider.Instance.Database.TrainingForms.Where(el => el.IsDeleted != true));
            Semesters = new ObservableCollection<Semester>(DataProvider.Instance.Database.Semesters);
            Teachers = new ObservableCollection<Teacher>(DataProvider.Instance.Database.Teachers);
            DayOfWeeks = new ObservableCollection<string>() { "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy", "Chủ nhật" };
        }

        public int? NumberOfStudents
        {
            get => _numberOfStudents;
            set
            {
                _numberOfStudents = value;
            }
        }

        public string Code
        {
            get => _code; set
            {
                _code = value;

                // Validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(Code))
                {
                    _errorBaseViewModel.AddError(nameof(Code), "Vui lòng nhập mã môn học!");
                }
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

        public int? MaxNumberOfStudents
        {
            get => _maxNumberOfStudents;
            set
            {
                _maxNumberOfStudents = value;

                // Validation
                _errorBaseViewModel.ClearErrors();

                if (string.IsNullOrWhiteSpace(MaxNumberOfStudents.ToString()))
                {
                    _errorBaseViewModel.AddError(nameof(MaxNumberOfStudents), "Vui lòng nhập sĩ số tối đa!");
                }
                int tempTryParse;
                if (!int.TryParse(MaxNumberOfStudents.ToString(), out tempTryParse) || tempTryParse < 0)
                {
                    _errorBaseViewModel.AddError(nameof(MaxNumberOfStudents), "Giá trị phải là số nguyên dương!");
                }
                OnPropertyChanged();
            }
        }

        public Guid Id { get => _id; set => _id = value; }
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

        public Subject SelectedSubject
        {
            get => _selectedSubject; set
            {
                _selectedSubject = value;

                // validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(SelectedSubject?.DisplayName))
                {
                    _errorBaseViewModel.AddError(nameof(SelectedSubject), "Vui lòng chọn môn học!");
                }
                OnPropertyChanged();

                OnPropertyChanged();
            }
        }
        public TrainingForm SelectedTrainingForm
        {
            get => _selectedTrainingForm;
            set
            {
                _selectedTrainingForm = value;

                // validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(SelectedTrainingForm?.DisplayName))
                {
                    _errorBaseViewModel.AddError(nameof(SelectedTrainingForm), "Vui lòng chọn hệ đào tạo!");
                }
                OnPropertyChanged();
            }
        }
        public Semester SelectedSemester
        {
            get => _selectedSemester;
            set
            {
                _selectedSemester = value;

                // validation

                _errorBaseViewModel.ClearErrors();
                if (!IsValid(SelectedSemester?.DisplayName))
                {
                    _errorBaseViewModel.AddError(nameof(SelectedSemester), "Vui lòng chọn học kỳ!");
                }
                OnPropertyChanged();
            }
        }
        public string SelectedDay
        {
            get => _selectedDay; set
            {
                _selectedDay = value;

                // validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(SelectedDay))
                {
                    _errorBaseViewModel.AddError(nameof(SelectedDay), "Vui lòng chọn thứ!");
                }
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Subject> Subjects { get => _subjects; set => _subjects = value; }
        public ObservableCollection<TrainingForm> TrainingForms { get => _trainingForms; set => _trainingForms = value; }
        public ObservableCollection<string> DayOfWeeks { get => _dayOfWeeks; set => _dayOfWeeks = value; }
        public ObservableCollection<Semester> Semesters { get => _semesters; set => _semesters = value; }
        public ObservableCollection<Teacher> Teachers { get => _teachers; set => _teachers = value; }
        public string Image { get => _image; set { _image = value; OnPropertyChanged(); } }
    }
}
