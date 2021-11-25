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
using static StudentManagement.ViewModels.StudentCourseRegistryViewModel;

namespace StudentManagement.ViewModels
{
    public class AdminCourseRegistryViewModel : BaseViewModel
    {
        #region classes
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
        /*private ObservableCollection<ObservableCollection<CourseItems>> _courseRegistryItemsAll;
        public ObservableCollection<ObservableCollection<CourseItems>> CourseRegistryItemsAll { get => _courseRegistryItemsAll; set => _courseRegistryItemsAll = value; }
        private ObservableCollection<CourseItems> _courseRegistryItems;
        public ObservableCollection<CourseItems> CourseRegistryItems { get => _courseRegistryItems; set => _courseRegistryItems = value; }
        private ObservableCollection<CourseItems> _courseRegistryItemsDisplay;
        public ObservableCollection<CourseItems> CourseRegistryItemsDisplay { get => _courseRegistryItemsDisplay; set { _courseRegistryItemsDisplay = value; OnPropertyChanged(); } }*/
        private ObservableCollection<ObservableCollection<CourseRegistryItem>> _courseRegistryItemsAll;
        public ObservableCollection<ObservableCollection<CourseRegistryItem>> CourseRegistryItemsAll { get => _courseRegistryItemsAll; set => _courseRegistryItemsAll = value; }
        private ObservableCollection<CourseRegistryItem> _courseRegistryItems;
        public ObservableCollection<CourseRegistryItem> CourseRegistryItems { get => _courseRegistryItems; set => _courseRegistryItems = value; }
        private ObservableCollection<CourseRegistryItem> _courseRegistryItemsDisplay;
        public ObservableCollection<CourseRegistryItem> CourseRegistryItemsDisplay { get => _courseRegistryItemsDisplay; set { _courseRegistryItemsDisplay = value; OnPropertyChanged(); } }
        //
        public ObservableCollection<Models.Semester> Semesters { get => _semesters; set { _semesters = value; OnPropertyChanged(); } }
        private ObservableCollection<Models.Semester> _semesters;

        public Models.Semester SelectedSemester { get => _selectedSemester; 
            set { _selectedSemester = value; OnPropertyChanged(); } }
        private Models.Semester _selectedSemester;
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
                SearchCourseRegistryItemsFunction();
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
        public object _creatNewCourseViewModel;
        #endregion
        #region commands
        public ICommand SwitchSearchButton { get => _switchSearchButton; set => _switchSearchButton = value; }

        private ICommand _switchSearchButton;
        public ICommand SearchCourseRegistryItems { get => _searchCourseRegistryItems; set => _searchCourseRegistryItems = value; }

        private ICommand _searchCourseRegistryItems;
        public ICommand DeleteSelectedItemsCommand { get; set; }
        public ICommand CreateNewCourseCommand { get; set; }

        public ICommand OpenSemesterCommand { get; set; }
        public ICommand PauseSemesterCommand { get; set; }
        public ICommand StopSemesterCommand { get; set; }
        #endregion

        public AdminCourseRegistryViewModel()
        {
            /*Thiếu lấy dữ liệu từ model cho semester và SubjectClasses*/
            Semesters = new ObservableCollection<Models.Semester>()
            {
                new Semester(){Batch = "2019-2020", DisplayName = "Học kỳ 2", CourseRegisterStatus = 2},
                new Semester(){Batch = "2020-2021", DisplayName = "Học kỳ 1", CourseRegisterStatus = 1},
                new Semester(){Batch = "2020-2021", DisplayName = "Học kỳ 2", CourseRegisterStatus = 0}
            };
            /*CourseRegistryItemsAll = new ObservableCollection<ObservableCollection<CourseItems>>();*/
            CourseRegistryItemsAll = new ObservableCollection<ObservableCollection<CourseRegistryItem>>();
            for (int i = 0; i < Semesters.Count; i++)
            {
                /*Semester semester = Semesters[i];*/
                /*var subjectClasses1Semester = SubjectClasses.Where(x => x.Semester == semester).ToList();*/
                /*var courseItems1Semester = new ObservableCollection<CourseItems>();*/
                /*foreach (Models.SubjectClass a in subjectClasses1Semester)
                {
                    courseItems1Semester.Add(new CourseItems(a, false));
                }*/
                /*var courseItems1Semester = new ObservableCollection<CourseItems>();*/
                var courseItems1Semester = new ObservableCollection<CourseRegistryItem>()
                {
                    new CourseRegistryItem(false, "IT008.L21.KHTN " + i, "Lập trình trực quan " + i, 4, 50, 30),
                    new CourseRegistryItem(false, "IT009.L21.KHCL " + i, "Không biết " + i, 2, 30, 30),
                    new CourseRegistryItem(false, "ENG02.L21 " + i, "Anh văn 2 " + i, 4, 30, 28)
                };
                CourseRegistryItemsAll.Add(courseItems1Semester);
            }
            SelectedSemester = Semesters.Last();
            InitCommand();
            
        }

        public void InitCommand()
        {
            SwitchSearchButton = new RelayCommand<UserControl>((p) => { return true; }, (p) => SwitchSearchButtonFunction(p));
            SearchCourseRegistryItems = new RelayCommand<object>((p) => { return true; }, (p) => SearchCourseRegistryItemsFunction());
            DeleteSelectedItemsCommand = new RelayCommand<UserControl>(
                (p) =>
                {
                    return CourseRegistryItemsDisplay.Where(x => x.IsSelected == true).Count() > 0;
                },
                (p) =>
                {
                    DeleteSelectedItems();
                });
            CreateNewCourseCommand = new RelayCommand<object>((p) => {
                if (SelectedSemester == null)
                    return true;
                if (SelectedSemester.CourseRegisterStatus > 0)
                {
                    return false;
                }
                return true;
            }, (p) => CreateNewCourse());
            OpenSemesterCommand = new RelayCommand<object>((p) => true, (p) => SelectedSemester.CourseRegisterStatus = 1);
            PauseSemesterCommand = new RelayCommand<object>((p) => true, (p) => SelectedSemester.CourseRegisterStatus = 0);
            StopSemesterCommand = new RelayCommand<object>((p) => true, (p) => SelectedSemester.CourseRegisterStatus = 2);
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

        public void SearchCourseRegistryItemsFunction()
        {
            if (SearchQuery == "" || SearchQuery == null)
            {
                CourseRegistryItemsDisplay = CourseRegistryItems;
                return;
            }
            if (!IsFirstSearchButtonEnabled)
            {
                var tmp = CourseRegistryItems.Where(x => x.IdSubjectClass.ToLower().Contains(SearchQuery.ToLower())).ToList();
                CourseRegistryItemsDisplay = new ObservableCollection<CourseRegistryItem>(tmp);
            }
            else
            {
                var tmp = CourseRegistryItems.Where(x => vietnameseStringNormalizer.Normalize(x.SubjectName).ToLower().Contains(vietnameseStringNormalizer.Normalize(SearchQuery.ToLower()))).ToList();
                CourseRegistryItemsDisplay = new ObservableCollection<CourseRegistryItem>(tmp);
            }
        }
        public void DeleteSelectedItems()
        {
            var SelectedItems = CourseRegistryItems.Where(x => x.IsSelected == true).ToList();
            foreach (CourseRegistryItem item in SelectedItems)
            {
                item.IsSelected = false;
                CourseRegistryItems.Remove(item);
            }
            SearchCourseRegistryItemsFunction();
        }
        public void CreateNewCourse()
        {
            CourseRegistryItem newCard = new CourseRegistryItem(false, "", "", 0, 0, 0);
            _creatNewCourseViewModel = new CreateNewCourseViewModel(newCard, SelectedSemester, CourseRegistryItems);
            this.DialogItemViewModel = this._creatNewCourseViewModel;
        }
    }
}
