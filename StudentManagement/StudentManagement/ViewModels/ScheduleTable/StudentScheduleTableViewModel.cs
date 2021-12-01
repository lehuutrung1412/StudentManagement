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
        public StudentScheduleTableViewModel()
        {
            CurrentStudent = StudentServices.Instance.GetFirstStudent();
            Semesters = SemesterServices.Instance.LoadListSemestersByStudentId(CurrentStudent.Id);
            if (Semesters.Count == 0)
            {
                SelectedSemester = null;
                ScheduleItems = new ObservableCollection<ScheduleItem>();
                return;
            }
            SelectedSemester = Semesters.Last();
            ScheduleItems = new ObservableCollection<ScheduleItem>();
            UpdateScheduleItems();
        }
        public void UpdateScheduleItems()
        {
            if (SelectedSemester == null)
                return;

            ScheduleItems = new ObservableCollection<ScheduleItem>();
            foreach (SubjectClass item in CourseRegisterServices.Instance.LoadCourseRegisteredListBySemesterIdAndStudentId(SelectedSemester.Id, CurrentStudent.Id))
            {
                ScheduleItem temp = new ScheduleItem(item);
                ScheduleItems.Add(temp);
            }
        }
    }
}
