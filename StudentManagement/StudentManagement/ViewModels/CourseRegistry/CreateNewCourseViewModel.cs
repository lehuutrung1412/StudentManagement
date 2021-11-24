using StudentManagement.Commands;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private CourseRegistryItem _currentCard;
        public CourseRegistryItem CurrentCard { get => _currentCard; set => _currentCard = value; }
        private TempSemester _semester;
        private ObservableCollection<CourseRegistryItem> _courses;
        public TempSemester Semester { get => _semester; set => _semester = value; }
        public ObservableCollection<CourseRegistryItem> Courses { get => _courses; set => _courses = value; }
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
        private bool _isPracticed;
        private string _subjectClassCode;
        public Subject SelectedSubject { get => _selectedSubject; set { _selectedSubject = value; OnPropertyChanged(); UpdateSubjectClassCode(); } }
        public TrainingForm SelectedTF { get => _selectedTF; set { _selectedTF = value; OnPropertyChanged(); UpdateSubjectClassCode(); } }
        public string Period { get => _period; set{ _period = value; OnPropertyChanged();} }
        public string SelectedDay { get => _selectedDay; set{ _selectedDay = value; OnPropertyChanged();} }
        public string MaxNumber { get => _maxNumber; set{ _maxNumber = value; OnPropertyChanged();} }
        public bool IsPracticed { get => _isPracticed; set{ _isPracticed = value; OnPropertyChanged(); UpdateSubjectClassCode(); } }
        public string SubjectClassCode { get => _subjectClassCode; set{ _subjectClassCode = value; OnPropertyChanged();} }

        public ICommand ConfirmCommand { get; set; }

        public CreateNewCourseViewModel(CourseRegistryItem card, TempSemester semester, ObservableCollection<CourseRegistryItem> list)
        {
            CurrentCard = card;
            Semester = semester;
            Courses = list;
            SubjectClassCode = "x.x.x";
            InitCombobox();
            InitCommand();

        }

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

        }
        public void UpdateSubjectClassCode()
        {
            SubjectClassCode = "";
            SubjectClassCode += SelectedSubject.Code;
            string codeTF = "";
            string codeSemester = ".";
            /*string codePractice = Courses.Where(x=>x.)*/
            codeSemester += (char)(Convert.ToInt32(Semester.Batch.Split('-')[0]) - 2010 + 65);
            codeTF += "KHTN";
            SubjectClassCode += codeSemester;
            SubjectClassCode += codeTF;

        }

        
    }
}
