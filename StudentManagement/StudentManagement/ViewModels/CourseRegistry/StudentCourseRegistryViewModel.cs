using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Services;
using StudentManagement.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class StudentCourseRegistryViewModel : BaseViewModel
    {

        #region Properties
        private int _totalCredit;
        public int TotalCredit
        {
            get => _totalCredit; set
            {
                _totalCredit = value;
                OnPropertyChanged();
            }
        }

        private Semester _currentSemester;
        public Semester CurrentSemester { get => _currentSemester; set => _currentSemester = value; }

        private Student _currentStudent;
        public Student CurrentStudent { get => _currentStudent; set => _currentStudent = value; }

        private bool _isAllItemsSelected1 = false;
        public bool IsAllItemsSelected1
        {
            get => _isAllItemsSelected1;
            set
            {
                _isAllItemsSelected1 = value;
                OnPropertyChanged();
                CourseRegistryItems1.Select(c => { c.IsSelected = value; return c; }).ToList();
            }
        }
        private bool _isAllItemsSelected2 = false;
        public bool IsAllItemsSelected2
        {
            get => _isAllItemsSelected2;
            set
            {
                _isAllItemsSelected2 = value;
                OnPropertyChanged();
                CourseRegistryItems2.Select(c => { c.IsSelected = value; return c; }).ToList();
            }
        }
        private ObservableCollection<CourseItem> courseRegistryItems1;
        public ObservableCollection<CourseItem> CourseRegistryItems1 { get => courseRegistryItems1; set => courseRegistryItems1 = value; }
        private ObservableCollection<CourseItem> courseRegistryItems2;
        public ObservableCollection<CourseItem> CourseRegistryItems2 { get => courseRegistryItems2; set => courseRegistryItems2 = value; }
        private ObservableCollection<CourseItem> courseRegistryItems2Display;
        public ObservableCollection<CourseItem> CourseRegistryItems2Display { get => courseRegistryItems2Display; set { courseRegistryItems2Display = value; OnPropertyChanged(); } }
        private string _searchQuery = "";
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
                Search();
            }
        }
        private bool _isFirstSearchButtonEnabled;
        public bool IsFirstSearchButtonEnabled { get => _isFirstSearchButtonEnabled; set { _isFirstSearchButtonEnabled = value; OnPropertyChanged(); } }
        public VietnameseStringNormalizer vietnameseStringNormalizer = VietnameseStringNormalizer.Instance;

        private ObservableCollection<ScheduleItem> _scheduleItemsRegistered;
        public ObservableCollection<ScheduleItem> ScheduleItemsRegistered { get => _scheduleItemsRegistered; set { _scheduleItemsRegistered = value; OnPropertyChanged(); } }
        #endregion
        #region Command
        public ICommand RegisterCommand { get => _registerCommand; set => _registerCommand = value; }
        private ICommand _registerCommand;
        public ICommand UnregisterCommand { get => _unregisterCommand; set => _unregisterCommand = value; }

        private ICommand _unregisterCommand;
        private ICommand _searchCommand;
        public ICommand SearchCommand { get => _searchCommand; set => _searchCommand = value; }
        private ICommand _switchSearchButtonCommand;
        public ICommand SwitchSearchButtonCommand { get => _switchSearchButtonCommand; set => _switchSearchButtonCommand = value; }
        

        #endregion
        public StudentCourseRegistryViewModel()
        {
            CurrentSemester = SemesterServices.Instance.GetLastOpenningRegisterSemester();
            CurrentStudent = StudentServices.Instance.GetFirstStudent();
            if (SubjectClassServices.Instance.LoadSubjectClassList().Count() == 0)
            {
                CourseRegistryItems1 = new ObservableCollection<CourseItem>();
                CourseRegistryItems2 = new ObservableCollection<CourseItem>();
            }
            else
            {
                CourseRegistryItems1 = CourseItem.ConvertToListCourseItem(CourseRegisterServices.Instance.LoadCourseRegisteredListBySemesterIdAndStudentId(CurrentSemester.Id, CurrentStudent.Id));
                CourseRegistryItems2 = CourseItem.ConvertToListCourseItem(CourseRegisterServices.Instance.LoadCourseUnregisteredListBySemesterIdAndStudentId(CurrentSemester.Id, CurrentStudent.Id));
            }
            CourseRegistryItems2Display = CourseRegistryItems2;
            TotalCredit = CourseRegistryItems1.Sum(x => Convert.ToInt32(x.Subject.Credit));
            InitCommand();
            InitScheduleItems();
        }
        #region Methods
        public void InitCommand()
        {
            RegisterCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => RegisterSelectedCourses());
            UnregisterCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => UnregisterSelectedCourses());
            SearchCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => Search());
            SwitchSearchButtonCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => SwitchSearchButton());
        }

        public void InitScheduleItems()
        {
            ScheduleItemsRegistered = new ObservableCollection<ScheduleItem>();
            if (SubjectClassServices.Instance.LoadSubjectClassList().Count() == 0)
                return;
            foreach (SubjectClass item in CourseRegisterServices.Instance.LoadCourseRegisteredListBySemesterIdAndStudentId(CurrentSemester.Id, CurrentStudent.Id))
            {
                ScheduleItem temp = new ScheduleItem(item);
                ScheduleItemsRegistered.Add(temp);
            }
        }
        public void RegisterSelectedCourses()
        {
            var SelectedItems = CourseRegistryItems2.Where(x => x.IsSelected == true).ToList();
            foreach (CourseItem item in SelectedItems)
            {
                item.IsSelected = false;
                CourseRegistryItems1.Add(item);
                CourseRegisterServices.Instance.StudentRegisterSubjectClassToDatabase(CurrentSemester.Id, CurrentStudent.Id, item.ConvertToSubjectClass());
                CourseRegistryItems2.Remove(item);
            }
            Search();
        }
        public void UnregisterSelectedCourses()
        {
            var SelectedItems = CourseRegistryItems1.Where(x => x.IsSelected == true).ToList();
            foreach (CourseItem item in SelectedItems)
            {
                item.IsSelected = false;
                CourseRegistryItems2.Add(item);
                CourseRegistryItems1.Remove(item);
                CourseRegisterServices.Instance.StudentUnregisterSubjectClassToDatabase(CurrentSemester.Id, CurrentStudent.Id, item.ConvertToSubjectClass());
            }
        }
        public void Search()
        {
            if (SearchQuery == "" || SearchQuery == null)
            {
                CourseRegistryItems2Display = CourseRegistryItems2;
                return;
            }
            if (!IsFirstSearchButtonEnabled)
            {
                var tmp = CourseRegistryItems2.Where(x => x.Code.ToLower().Contains(SearchQuery.ToLower())).ToList();
                CourseRegistryItems2Display = new ObservableCollection<CourseItem>(tmp);
            }
            else
            {
                var tmp = CourseRegistryItems2.Where(x => vietnameseStringNormalizer.Normalize(x.Subject.DisplayName).ToLower().Contains(vietnameseStringNormalizer.Normalize(SearchQuery.ToLower()))).ToList();
                CourseRegistryItems2Display = new ObservableCollection<CourseItem>(tmp);
            }
        }
        public void SwitchSearchButton()
        {
            IsFirstSearchButtonEnabled = !IsFirstSearchButtonEnabled;
        }
        /*public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }*/
        #endregion
    }
}
