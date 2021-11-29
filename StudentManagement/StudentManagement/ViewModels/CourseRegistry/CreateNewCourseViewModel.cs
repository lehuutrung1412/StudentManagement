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
    public class CreateNewCourseViewModel : BaseViewModel
    {
        #region error
        private readonly ErrorBaseViewModel _errorBaseViewModel;
        public bool HasErrors => _errorBaseViewModel.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public IEnumerable GetErrors(string propertyName)
        {
            return _errorBaseViewModel.GetErrors(propertyName);
        }
        #endregion
        #region properties
        private SubjectClass _currentCard;
        public SubjectClass CurrentCard { get => _currentCard; set => _currentCard = value; }
        private Semester _semester;
        private ObservableCollection<CourseItems> _courses;
        public Semester Semester { get => _semester; set => _semester = value; }
        public ObservableCollection<CourseItems> Courses { get => _courses; set => _courses = value; }
        public ObservableCollection<Subject> Subjects { get => _subjects; set => _subjects = value; }
        public ObservableCollection<TrainingForm> TrainingForms { get => _trainingForms; set => _trainingForms = value; }
        public ObservableCollection<string> DayOfWeeks { get => _dayOfWeeks; set { _dayOfWeeks = value; OnPropertyChanged(); } }

        private ObservableCollection<Subject> _subjects;

        private ObservableCollection<TrainingForm> _trainingForms;

        private ObservableCollection<string> _dayOfWeeks;

        private Subject _selectedSubject;
        private TrainingForm _selectedTF;
        private string _period;
        private string _selectedDay;
        private string _maxNumber;
        private string _subjectClassCode;
        private DateTime? _startDate;
        private DateTime? _endDate;

        public Subject SelectedSubject { get => _selectedSubject; set { 
                _selectedSubject = value; 
                OnPropertyChanged(); 
                UpdateSubjectClassCode(); 
                } }
        public TrainingForm SelectedTF { get => _selectedTF; set { _selectedTF = value; OnPropertyChanged(); /*UpdateSubjectClassCode();*/ } }
        public string Period { get => _period; set{ 
                _period = value; 
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(Period))
                {
                    _errorBaseViewModel.AddError(nameof(Period), "Vui lòng nhập tiết!");
                }
                OnPropertyChanged();
            } }
        public string SelectedDay { get => _selectedDay; set{ 
                _selectedDay = value; 
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(Period))
                {
                    _errorBaseViewModel.AddError(nameof(Period), "Vui lòng chọn thứ!");
                }
                OnPropertyChanged();} }
        public string MaxNumber { get => _maxNumber; set{ _maxNumber = value; OnPropertyChanged();} }
        public string SubjectClassCode { get => _subjectClassCode; set{ _subjectClassCode = value; OnPropertyChanged();} }

        public DateTime? StartDate { get => _startDate; set { _startDate = value; OnPropertyChanged(); } }
        public DateTime? EndDate { get => _endDate; set { _endDate = value; OnPropertyChanged(); } }
        #endregion

        #region command
        public ICommand ConfirmCommand { get; set; }
        #endregion
        public CreateNewCourseViewModel(SubjectClass card, Semester semester, ObservableCollection<CourseItems> list)
        {
            CurrentCard = card;
            Semester = semester;
            Courses = list;
            SubjectClassCode = "x.x.x";
            InitCombobox();
            InitCommand();
            _errorBaseViewModel = new ErrorBaseViewModel();
            _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;
        }
        #region methods
        public void InitCombobox()
        {
            Subjects = new ObservableCollection<Subject>(DataProvider.Instance.Database.Subjects);
            TrainingForms = new ObservableCollection<TrainingForm>(DataProvider.Instance.Database.TrainingForms);
            DayOfWeeks = new ObservableCollection<string>() { "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy", "Chủ nhật" };
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
                WeekDay = SelectedDay/*,
                Code = SubjectClassCode,
                MaxOfRegister = MaxNumber,
                NumberStudent = 0,
                TraningForm = SelectedTF*/
            };
            CurrentCard = newCourse;
            Courses.Add(new CourseItems(newCourse, false));
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

            int indexCourse = Courses.Where(course => course.Subject.DisplayName == SelectedSubject.DisplayName).Count() + 1;
            SubjectClassCode += Convert.ToString(indexCourse);
        }
        private void ErrorBaseViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }
        private bool IsValid(string propertyName)
        {
            return !string.IsNullOrEmpty(propertyName) && !string.IsNullOrWhiteSpace(propertyName);
        }
        #endregion

    }
}
