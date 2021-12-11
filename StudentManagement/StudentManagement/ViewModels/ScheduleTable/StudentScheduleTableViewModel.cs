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
        private Teacher _currentTeacher;
        public Teacher CurrentTeacher
        {
            get => _currentTeacher;
            set
            {
                _currentTeacher = value;
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
            LoginServices.UpdateCurrentUser += UpdateCurrentUser;
        }

        private void UpdateCurrentUser(object sender, LoginServices.LoginEvent e)
        {
            UpdateData();
        }

        public void UpdateScheduleItems()
        {
            if (SelectedSemester == null)
                return;
            switch (LoginServices.CurrentUser.UserRole.Role)
            {
                case "Sinh viên":
                    foreach (SubjectClass item in CourseRegisterServices.Instance.LoadCourseRegisteredListBySemesterIdAndStudentId(SelectedSemester.Id, CurrentStudent.Id))
                    {
                        ScheduleItem temp = new ScheduleItem(item);
                        ScheduleItems.Add(temp);
                    }
                    break;
                case "Giáo viên":
                    List<SubjectClass> listSubjectClass = SubjectClassServices.Instance.LoadSubjectClassListBySemesterId(SelectedSemester.Id).ToList();
                    listSubjectClass = new List<SubjectClass>(listSubjectClass.Where(subjectClass => subjectClass.Teachers.Contains(CurrentTeacher)));
                    foreach (SubjectClass item in listSubjectClass)
                    {
                        ScheduleItem temp = new ScheduleItem(item);
                        ScheduleItems.Add(temp);
                    }
                    break;
            }
        }

        public ObservableCollection<Semester> LoadListSemester()
        {
            User currentUser = LoginServices.CurrentUser;
            switch(currentUser.UserRole.Role)
            {
                case "Sinh viên":
                    CurrentStudent = StudentServices.Instance.FindStudentByUserId(currentUser.Id);
                    return SemesterServices.Instance.LoadListSemestersByStudentIdAndSemesterStatuses(CurrentStudent.Id, new bool[] { false, false, true });
                    break;
                case "Giáo viên":
                    CurrentTeacher = TeacherServices.Instance.GetTeacherbyUser(currentUser);
                    return SemesterServices.Instance.LoadListSemestersByTeacherAndSemesterStatuses(CurrentTeacher, new bool[] { false, false, true });
                    break;
                default:
                    return new ObservableCollection<Semester>();
            }
        }

        public void UpdateData()
        {
            Semesters = LoadListSemester();
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
