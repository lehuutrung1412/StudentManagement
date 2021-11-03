using StudentManagement.Commands;
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
        public class CourseRegistryItem:BaseViewModel
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

            public bool IsSelected { get => _isSelected;
                set { _isSelected = value; OnPropertyChanged(); } }
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
        private bool _isAllItemsSelected1;
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
        private bool _isAllItemsSelected2;
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

        #endregion
        #region Command
        public ICommand RegisterCommand { get => _registerCommand; set => _registerCommand = value; }
        private ICommand _registerCommand;
        public ICommand UnregisterCommand { get => _unregisterCommand; set => _unregisterCommand = value; }

        private ICommand _unregisterCommand;
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
            
            TotalCredit = CourseRegistryItems1.Sum(x => x.Credit);
            RegisterCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => RegisterSelectedCourses());
            UnregisterCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => UnregisterSelectedCourses());
        }
        #region Methods
        public void RegisterSelectedCourses()
        {
            var SelectedItems = CourseRegistryItems2.Where(x => x.IsSelected == true).ToList();
            foreach(CourseRegistryItem item in SelectedItems)
            {
                item.IsSelected = false;
                CourseRegistryItems1.Add(item);
                CourseRegistryItems2.Remove(item);
            }
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
        #endregion
    }
}
