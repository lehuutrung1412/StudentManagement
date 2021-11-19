using StudentManagement.Commands;
using StudentManagement.Models;
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
    public class AdminCourseRegistryViewModel : BaseViewModel
    {
        #region classes
        public class TempSemester : Models.Semester
        {
            public TempSemester(string batch, string displayName, int status)
            {
                this.Batch = batch;
                this.DisplayName = displayName;
                this.CourseRegisterStatus = status;
            }
        }
        public class CourseItems : Models.SubjectClass
        {
            private bool _isSelected;
            public bool IsSelected
            {
                get => _isSelected;
                set { _isSelected = value; OnPropertyChanged(); }
            }
            public CourseItems(Models.SubjectClass a, bool isSelected)
            {
                this.Id = a.Id;
                this.Teachers = a.Teachers;
                this.Semester = a.Semester;
                this.Subject = a.Subject;
                this.StartDate = a.StartDate;
                this.EndDate = a.EndDate;
                this.Period = a.Period;
                this.WeekDay = a.WeekDay;
                this.IsSelected = false;
            }
        }
        #endregion
        #region properties
        private bool _isAllItemsSelected = false;
        public bool IsAllItemsSelected
        {
            get => _isAllItemsSelected;
            set
            {
                _isAllItemsSelected = value;
                OnPropertyChanged();
                CourseRegistryItemsDisplay.Select(c => { c.IsSelected = value; return c; }).ToList();
            }
        }
        private ObservableCollection<Models.SubjectClass> _subjectClasses;
        public ObservableCollection<Models.SubjectClass> SubjectClasses { get => _subjectClasses; set => _subjectClasses = value; }
        private ObservableCollection<ObservableCollection<CourseItems>> _courseRegistryItemsAll;
        public ObservableCollection<ObservableCollection<CourseItems>> CourseRegistryItemsAll { get => _courseRegistryItemsAll; set => _courseRegistryItemsAll = value; }
        private ObservableCollection<CourseItems> _courseRegistryItems;
        public ObservableCollection<CourseItems> CourseRegistryItems { get => _courseRegistryItems; set => _courseRegistryItems = value; }
        private ObservableCollection<CourseItems> _courseRegistryItemsDisplay;
        public ObservableCollection<CourseItems> CourseRegistryItemsDisplay { get => _courseRegistryItemsDisplay; set { _courseRegistryItemsDisplay = value; OnPropertyChanged(); } }
        public ObservableCollection<TempSemester> Semesters { get => _semesters; set { _semesters = value; OnPropertyChanged(); } }
        private ObservableCollection<TempSemester> _semesters;

        public TempSemester SelectedSemester { get => _selectedSemester; set { _selectedSemester = value; OnPropertyChanged(); } }
        private TempSemester _selectedSemester;
        public int SelectedSemesterIndex { get => _selectedSemesterIndex; set { _selectedSemesterIndex = value; OnPropertyChanged(); SelectData(); } }
        private int _selectedSemesterIndex;

        public VietnameseStringNormalizer vietnameseStringNormalizer = VietnameseStringNormalizer.Instance;
        public bool IsFirstSearchButtonEnabled
        {
            get { return _isFirstSearchButtonEnabled; }
            set
            {
                _isFirstSearchButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _isFirstSearchButtonEnabled = false;

        private string _searchQuery = "";
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
            }
        }
        private object _dialogItemViewModel;
        public object DialogItemViewModel
        {
            get { return _dialogItemViewModel; }
            set
            {
                _dialogItemViewModel = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region commands
        public ICommand SwitchSearchButton { get => _switchSearchButton; set => _switchSearchButton = value; }

        private ICommand _switchSearchButton;
        public ICommand SearchCourseRegistryItems { get => _searchCourseRegistryItems; set => _searchCourseRegistryItems = value; }

        private ICommand _searchCourseRegistryItems;
        #endregion
        public AdminCourseRegistryViewModel()
        {
            /*Thiếu lấy dữ liệu từ model cho semester và SubjectClasses*/
            Semesters = new ObservableCollection<TempSemester>()
            {
                new TempSemester("2019-2020", "HK2", 1),
                new TempSemester("2020-2021", "HK1", 1),
                new TempSemester("2020-2021", "HK2", 0)
            };
            CourseRegistryItemsAll = new ObservableCollection<ObservableCollection<CourseItems>>();
            for (int i = 0; i < Semesters.Count; i++)
            {
                /*Semester semester = Semesters[i];*/
                /*var subjectClasses1Semester = SubjectClasses.Where(x => x.Semester == semester).ToList();*/
                /*var courseItems1Semester = new ObservableCollection<CourseItems>();*/
                /*foreach (Models.SubjectClass a in subjectClasses1Semester)
                {
                    courseItems1Semester.Add(new CourseItems(a, false));
                }*/
                var courseItems1Semester = new ObservableCollection<CourseItems>();
                CourseRegistryItemsAll.Add(courseItems1Semester);
            }
            SelectedSemester = Semesters.Last();

            SwitchSearchButton = new RelayCommand<UserControl>((p) => { return true; }, (p) => SwitchSearchButtonFunction(p));
            SearchCourseRegistryItems = new RelayCommand<object>((p) => { return true; }, (p) => SearchCourseRegistryItemsFunction(p));
        }

        public void SelectData()
        {
            CourseRegistryItems = CourseRegistryItemsAll[SelectedSemesterIndex];
            CourseRegistryItemsDisplay = CourseRegistryItems;
        }
        public void SwitchSearchButtonFunction(UserControl p)
        {
            IsFirstSearchButtonEnabled = !IsFirstSearchButtonEnabled;
        }

        public void SearchCourseRegistryItemsFunction(object p)
        {
            /*var tmp = StoredSubjectCards.Where(x => !IsFirstSearchButtonEnabled ?
                                                    vietnameseStringNormalizer.Normalize(x.TenMon).Contains(vietnameseStringNormalizer.Normalize(SearchQuery))
                                                    : vietnameseStringNormalizer.Normalize(x.GiaoVien).Contains(vietnameseStringNormalizer.Normalize(SearchQuery)));
            SubjectCards.Clear();
            foreach (SubjectCard card in tmp)
                SubjectCards.Add(card);*/
        }
    }
}
