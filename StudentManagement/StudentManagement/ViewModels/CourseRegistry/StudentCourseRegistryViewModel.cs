using StudentManagement.Commands;
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
        #region Class
        public class CourseRegistryItem : BaseViewModel
        {
            private bool _isSelected;
            private string _idSubjectClass;
            private string _subjectName;
            private int _credit;
            private int _limitStudentCount;
            private int _registeredCount;

            public CourseRegistryItem(bool isSelected, string idSubjectClass, string subjectName, int credit, int limitStudentCount, int registeredCount)
            {
                IsSelected = isSelected;
                IdSubjectClass = idSubjectClass;
                SubjectName = subjectName;
                Credit = credit;
                LimitStudentCount = limitStudentCount;
                RegisteredCount = registeredCount;
            }

            public bool IsSelected
            {
                get => _isSelected;
                set { _isSelected = value; OnPropertyChanged(); }
            }
            public string IdSubjectClass { get => _idSubjectClass; set => _idSubjectClass = value; }
            public string SubjectName { get => _subjectName; set => _subjectName = value; }
            public int Credit { get => _credit; set => _credit = value; }
            public int LimitStudentCount { get => _limitStudentCount; set => _limitStudentCount = value; }
            public int RegisteredCount { get => _registeredCount; set => _registeredCount = value; }
        }
        #endregion
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
        private ObservableCollection<CourseRegistryItem> courseRegistryItems1;
        public ObservableCollection<CourseRegistryItem> CourseRegistryItems1 { get => courseRegistryItems1; set => courseRegistryItems1 = value; }
        private ObservableCollection<CourseRegistryItem> courseRegistryItems2;
        public ObservableCollection<CourseRegistryItem> CourseRegistryItems2 { get => courseRegistryItems2; set => courseRegistryItems2 = value; }
        private ObservableCollection<CourseRegistryItem> courseRegistryItems2Display;
        public ObservableCollection<CourseRegistryItem> CourseRegistryItems2Display { get => courseRegistryItems2Display; set { courseRegistryItems2Display = value; OnPropertyChanged(); } }
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
        private bool _isFirstSearchButtonEnabled;
        public bool IsFirstSearchButtonEnabled { get => _isFirstSearchButtonEnabled; set { _isFirstSearchButtonEnabled = value; OnPropertyChanged(); } }
        public VietnameseStringNormalizer vietnameseStringNormalizer = VietnameseStringNormalizer.Instance;
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
            CourseRegistryItems1 = new ObservableCollection<CourseRegistryItem>
            {
                new CourseRegistryItem(false, "IT008.L21.KHTN", "Lập trình trực quan", 4, 50, 30),
                new CourseRegistryItem(false, "IT009.L21.KHCL", "Không biết", 2, 30, 30),
                new CourseRegistryItem(false, "ENG02.L21", "Anh văn 2", 4, 30, 28)
            };
            CourseRegistryItems2 = new ObservableCollection<CourseRegistryItem>
            {
                new CourseRegistryItem(false, "IT008.L21.KHTN", "Lập trình trực quan", 4, 50, 30),
                new CourseRegistryItem(false, "IT009.L21.KHCL", "Không biết", 2, 30, 30),
                new CourseRegistryItem(false, "ENG02.L21", "Anh văn 2", 4, 30, 28),
                new CourseRegistryItem(false, "IT008.L21.KHTN", "Lập trình trực quan", 4, 50, 30),
                new CourseRegistryItem(false, "IT009.L21.KHCL", "Không biết", 2, 30, 30),
                new CourseRegistryItem(false, "ENG02.L21", "Anh văn 2", 4, 30, 28),
                new CourseRegistryItem(false, "IT008.L21.KHTN", "Lập trình trực quan", 4, 50, 30),
                new CourseRegistryItem(false, "IT009.L21.KHCL", "Không biết", 2, 30, 30),
                new CourseRegistryItem(false, "ENG02.L21", "Anh văn 2", 4, 30, 28)
            };
            CourseRegistryItems2Display = CourseRegistryItems2;

            TotalCredit = CourseRegistryItems1.Sum(x => x.Credit);
            RegisterCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => RegisterSelectedCourses());
            UnregisterCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => UnregisterSelectedCourses());
            SearchCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => Search());
            SwitchSearchButtonCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => SwitchSearchButton());
        }
        #region Methods
        public void RegisterSelectedCourses()
        {
            var SelectedItems = CourseRegistryItems2.Where(x => x.IsSelected == true).ToList();
            foreach (CourseRegistryItem item in SelectedItems)
            {
                item.IsSelected = false;
                CourseRegistryItems1.Add(item);
                CourseRegistryItems2.Remove(item);
            }
            Search();
        }
        public void UnregisterSelectedCourses()
        {
            var SelectedItems = CourseRegistryItems1.Where(x => x.IsSelected == true).ToList();
            foreach (CourseRegistryItem item in SelectedItems)
            {
                item.IsSelected = false;
                CourseRegistryItems2.Add(item);
                CourseRegistryItems1.Remove(item);
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
                var tmp = CourseRegistryItems2.Where(x => x.IdSubjectClass.ToLower().Contains(SearchQuery.ToLower())).ToList();
                CourseRegistryItems2Display = new ObservableCollection<CourseRegistryItem>(tmp);
            }
            else
            {
                var tmp = CourseRegistryItems2.Where(x => vietnameseStringNormalizer.Normalize(x.SubjectName).ToLower().Contains(vietnameseStringNormalizer.Normalize(SearchQuery.ToLower()))).ToList();
                CourseRegistryItems2Display = new ObservableCollection<CourseRegistryItem>(tmp);
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
