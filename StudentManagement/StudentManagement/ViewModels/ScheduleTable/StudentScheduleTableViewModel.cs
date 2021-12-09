using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudentManagement.ViewModels.StudentCourseRegistryRightSideBarViewModel;

namespace StudentManagement.ViewModels
{
    public class StudentScheduleTableViewModel : BaseViewModel
    {
        #region properties
        private ObservableCollection<SubjectClass> _subjectClasses;
        public ObservableCollection<SubjectClass> SubjectClasses { get => _subjectClasses; set => _subjectClasses = value; }
        private ObservableCollection<ScheduleItem> _scheduleItems;
        public ObservableCollection<ScheduleItem> ScheduleItems { get => _scheduleItems; set { _scheduleItems = value; OnPropertyChanged(); } }
        public ObservableCollection<Semester> Semesters { get => _semesters; set => _semesters = value; }

        private ObservableCollection<Semester> _semesters;
        private Semester _selectedSemester;
        public Semester SelectedSemester
        {
            get => _selectedSemester;
            set
            {
                _selectedSemester = value;
                OnPropertyChanged();
                UpdateScheduleItems();
            }
        }
        private Student _currentStudent;
        public Student CurrentStudent
        {
            get => _currentStudent;
            set
            {
                _currentStudent = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private static StudentScheduleTableViewModel s_instance;
        public static StudentScheduleTableViewModel Instance
        {
            get => s_instance ?? (s_instance = new StudentScheduleTableViewModel());

            private set => s_instance = value;
        }
        public StudentScheduleTableViewModel()
        {
            Instance = this;
            CurrentStudent = StudentServices.Instance.GetFirstStudent();
            UpdateData();
        }
        public void UpdateScheduleItems()
        {
            if (SelectedSemester == null)
                return;
            foreach (SubjectClass item in CourseRegisterServices.Instance.LoadCourseRegisteredListBySemesterIdAndStudentId(SelectedSemester.Id, CurrentStudent.Id))
            {
                ScheduleItem temp = new ScheduleItem(item);
                ScheduleItems.Add(temp);
            }
        }

        public void UpdateData()
        {
            Semesters = SemesterServices.Instance.LoadListSemestersByStudentIdAndSemesterStatuses(CurrentStudent.Id, new bool[]{false, false, true });
            if (Semesters.Count == 0)
            {
                ScheduleItems = new ObservableCollection<ScheduleItem>();
                SelectedSemester = null;
                return;
            }
            ScheduleItems = new ObservableCollection<ScheduleItem>();

            SelectedSemester = Semesters.LastOrDefault();
            UpdateScheduleItems();
        }
    }
}
