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
using static StudentManagement.ViewModels.AdminCourseRegistryViewModel;
using static StudentManagement.ViewModels.StudentCourseRegistryViewModel;

namespace StudentManagement.ViewModels
{
    public class CreateNewCourseViewModel : BaseViewModel, INotifyDataErrorInfo
    {
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
        #region properties
        private SubjectClass _currentCard;
        public SubjectClass CurrentCard { get => _currentCard; set => _currentCard = value; }
        private Semester _semester;
        private ObservableCollection<CourseItem> _courses;
        public Semester Semester { get => _semester; set => _semester = value; }
        public ObservableCollection<CourseItem> Courses { get => _courses; set => _courses = value; }
        public ObservableCollection<Subject> Subjects { get => _subjects; set => _subjects = value; }
        public ObservableCollection<TrainingForm> TrainingForms { get => _trainingForms; set => _trainingForms = value; }
        public ObservableCollection<string> DayOfWeeks { get => _dayOfWeeks; set { _dayOfWeeks = value; OnPropertyChanged(); } }

        public ObservableCollection<Teacher> Teachers { get => _teachers; set { _teachers = value; OnPropertyChanged(); } }

        private ObservableCollection<Subject> _subjects;

        private ObservableCollection<TrainingForm> _trainingForms;

        private ObservableCollection<string> _dayOfWeeks;

        private ObservableCollection<Teacher> _teachers;

        private Subject _selectedSubject;
        private TrainingForm _selectedTF;
        private string _period;
        private string _selectedDay;
        private string _maxNumber;
        private string _subjectClassCode;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private Teacher _selectedTeacher;

        public Subject SelectedSubject { get => _selectedSubject; set { 
                _selectedSubject = value; 
                OnPropertyChanged(); 
                UpdateSubjectClassCode(); 
                } }
        public TrainingForm SelectedTF { get => _selectedTF; set { _selectedTF = value; OnPropertyChanged(); /*UpdateSubjectClassCode();*/ } }
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
        public string SelectedDay { get => _selectedDay; set{ 
                _selectedDay = value; 
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(SelectedDay))
                {
                    _errorBaseViewModel.AddError(nameof(Period), "Vui lòng chọn thứ!");
                }
                OnPropertyChanged();} }
        public string MaxNumber
        {
            get => _maxNumber;
            set
            {
                _maxNumber = value;

                // Validation
                _errorBaseViewModel.ClearErrors();

                if (string.IsNullOrWhiteSpace(MaxNumber))
                {
                    _errorBaseViewModel.AddError(nameof(MaxNumber), "Vui lòng nhập sĩ số tối đa!");
                }
                int tempTryParse;
                if (!int.TryParse(MaxNumber, out tempTryParse) || tempTryParse<0)
                {
                    _errorBaseViewModel.AddError(nameof(MaxNumber), "Giá trị phải là số nguyên dương!");
                }
                OnPropertyChanged();
            }
        }
        public string SubjectClassCode { get => _subjectClassCode; set{ _subjectClassCode = value; OnPropertyChanged();} }

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

        private bool _isDoneVisible;
        public bool IsDoneVisible { get => _isDoneVisible; set { _isDoneVisible = value; OnPropertyChanged(); } }
        #endregion

        #region command
        public ICommand ConfirmCommand { get; set; }
        #endregion
        public CreateNewCourseViewModel(SubjectClass card, Semester semester, ObservableCollection<CourseItem> list)
        {
            _errorBaseViewModel = new ErrorBaseViewModel();
            _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;

            CurrentCard = card;
            Semester = semester;
            Courses = list;
            SubjectClassCode = "x.x.x";
            InitCombobox();
            InitCommand();
        }
        #region methods
        public void InitCombobox()
        {
            Subjects = new ObservableCollection<Subject>(DataProvider.Instance.Database.Subjects);
            TrainingForms = new ObservableCollection<TrainingForm>(DataProvider.Instance.Database.TrainingForms);
            DayOfWeeks = new ObservableCollection<string>() { "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy", "Chủ nhật" };
            Teachers = new ObservableCollection<Teacher>(DataProvider.Instance.Database.Teachers);
        }

        public void InitCommand()
        {
            ConfirmCommand = new RelayCommand<object>((p) => { return true; }, (p) => Confirm());
        }

        public void Confirm()
        {
            var newCourse = new SubjectClass()
            {
                Id = Guid.NewGuid(),
                Subject = SelectedSubject,
                StartDate = StartDate,
                EndDate = EndDate,
                Semester = Semester,
                Period = Period,
                WeekDay = DayOfWeeks.IndexOf(SelectedDay),
                Code = SubjectClassCode,
                MaxNumberOfStudents = Convert.ToInt32(MaxNumber),
                NumberOfStudents = 0,
                TrainingForm = SelectedTF,
                DatabaseImageTable = DatabaseImageTableServices.Instance.GetFirstDatabaseImageTable(),
                Teachers = new ObservableCollection<Teacher>() { SelectedTeacher }
            };
            SubjectClassServices.Instance.GenerateDefaultCommponentScore(newCourse);
            CurrentCard = newCourse;
            IsDoneVisible = SubjectClassServices.Instance.SaveSubjectClassToDatabase(newCourse);
            
            Courses.Add(new CourseItem(newCourse, false));
            UpdateSubjectClassCode();
        }
        public void UpdateSubjectClassCode()
        {
            SubjectClassCode = "";
            if (SelectedSubject != null)
                SubjectClassCode += SelectedSubject.Code;

            string codeSemester = ".";
            codeSemester += (char)(Convert.ToInt32(Semester.Batch.Split('-')[0]) - 2010 + 65);
            var listSemester = SemesterServices.Instance.LoadListSemestersByBatch(Semester.Batch);
            int indexSemester = listSemester.IndexOf(Semester) + 1;
            codeSemester += Convert.ToString(indexSemester);
            SubjectClassCode += codeSemester;

            int indexCourse = (Courses == null)? 1 : Courses.Where(course => course.Subject.DisplayName == SelectedSubject.DisplayName).Count() + 1;
            SubjectClassCode += Convert.ToString(indexCourse);
        }
        private bool IsValid(string propertyName)
        {
            return !string.IsNullOrEmpty(propertyName) && !string.IsNullOrWhiteSpace(propertyName);
        }
        #endregion

    }
}
