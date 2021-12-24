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
                foreach(CourseItem course in CourseRegistryItems2)
                {
                    if (course.IsSelected == !value && course.IsConflict == false && course.IsValidSubject == false)
                    {
                        course.IsSelected = value;
                        UpdateConflictionByEditCourse(course);
                    }
                }
            }
        }
        private ObservableCollection<CourseItem> courseRegistryItems1;
        public ObservableCollection<CourseItem> CourseRegistryItems1 { get => courseRegistryItems1; set => courseRegistryItems1 = value; }
        private ObservableCollection<CourseItem> courseRegistryItems2;
        public ObservableCollection<CourseItem> CourseRegistryItems2 { get => courseRegistryItems2; set => courseRegistryItems2 = value; }
        private ObservableCollection<CourseItem> courseRegistryItems2Display;
        public ObservableCollection<CourseItem> CourseRegistryItems2Display { get => courseRegistryItems2Display; set { courseRegistryItems2Display = value; OnPropertyChanged(); } }
        private ObservableCollection<CourseItem> courseRegistryItemsChecked;
        public ObservableCollection<CourseItem> CourseRegistryItemsChecked { get => courseRegistryItemsChecked; set { courseRegistryItemsChecked = value; OnPropertyChanged(); } }
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
        private ScheduleItem _selectedScheduleItem2;
        public ScheduleItem SelectedScheduleItem2 { get => _selectedScheduleItem2; set { _selectedScheduleItem2 = value; UpdateScheduleItem2(); OnPropertyChanged(); } }

        private bool _inLoadingCourseRegistries = false;
        public bool InLoadingCourseRegistries
        {
            get => _inLoadingCourseRegistries; set
            {
                _inLoadingCourseRegistries = value;
                OnPropertyChanged();
            }
        }
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

        public ICommand CourseCheckChangedCommand { get; set; }

        private ICommand _synchronizeCourseRegistry;
        public ICommand SynchronizeCourseRegistry { get => _synchronizeCourseRegistry; set => _synchronizeCourseRegistry = value; }
        #endregion

        private static StudentCourseRegistryViewModel s_instance;
        public static StudentCourseRegistryViewModel Instance
        {
            get => s_instance ?? (s_instance = new StudentCourseRegistryViewModel());

            private set => s_instance = value;
        }

        public StudentCourseRegistryViewModel()
        {
            Instance = this;
            LoginServices.UpdateCurrentUser += UpdateCurrentUser;
            InitCommand();
        }


        #region Methods

        public void UpdateData()
        {
            InLoadingCourseRegistries = true;
            try
            {
                UpdateSemester();
                CurrentStudent = LoginServices.CurrentUser.Students.FirstOrDefault();

                courseRegistryItemsChecked = new ObservableCollection<CourseItem>();
                if (CurrentSemester == null || CurrentStudent == null)
                {
                    CourseRegistryItems1 = new ObservableCollection<CourseItem>();
                    CourseRegistryItems2 = new ObservableCollection<CourseItem>();
                }
                else
                {
                    CourseRegistryItems1 = CourseItem.ConvertToListCourseItem(CourseRegisterServices.Instance.LoadCourseRegisteredListBySemesterIdAndStudentId(CurrentSemester.Id, CurrentStudent.Id));
                    CourseRegistryItems2 = CourseItem.ConvertToListCourseItem(CourseRegisterServices.Instance.LoadCourseUnregisteredListBySemesterIdAndStudentId(CurrentSemester.Id, CurrentStudent.Id));

                    foreach (CourseItem course in CourseRegistryItems2.Where(fullCourse => fullCourse.NumberOfStudents >= fullCourse.MaxNumberOfStudents).ToList())
                        CourseRegistryItems2.Remove(course);

                    UpdateScheduleItems();
                    UploadConflictCourseRegistry();
                }
                CourseRegistryItems2Display = CourseRegistryItems2;
                TotalCredit = CourseRegistryItems1.Sum(x => Convert.ToInt32(x.Subject.Credit));
            }
            catch { }
            InLoadingCourseRegistries = false;
        }
        public void InitCommand()
        {
            RegisterCommand = new RelayCommand<UserControl>((p) =>
            {
                foreach (CourseItem course in CourseRegistryItems2)
                    if (course.IsSelected)
                        return true;
                return false;
            }, (p) => RegisterSelectedCourses());
            UnregisterCommand = new RelayCommand<UserControl>((p) =>
            {
                foreach (CourseItem course in CourseRegistryItems1)
                    if (course.IsSelected)
                        return true;
                return false;
            }, (p) => UnregisterSelectedCourses());
            SearchCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => Search());
            SwitchSearchButtonCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => SwitchSearchButton());
            CourseCheckChangedCommand = new RelayCommand<DataGridBeginningEditEventArgs>((p) => { return true; }, (p) => CourseCheckChanged(p));
            SynchronizeCourseRegistry = new RelayCommand<UserControl>((p) => { return true; }, (p) => UpdateData());
        }

        public void UpdateSemester()
        {
            CurrentSemester = SemesterServices.Instance.GetLastOpenningRegisterSemester();
        }
        public void UpdateScheduleItems()
        {
            ScheduleItemsRegistered = new ObservableCollection<ScheduleItem>();
            if (SubjectClassServices.Instance.LoadSubjectClassList().Where(el => el.IsDeleted != true).Count() == 0)
                return;
            foreach (SubjectClass item in CourseRegisterServices.Instance.LoadCourseRegisteredListBySemesterIdAndStudentId(CurrentSemester.Id, CurrentStudent.Id))
            {
                ScheduleItem temp = new ScheduleItem(item, false, false, 0, false);
                ScheduleItemsRegistered.Add(temp);
            }
        }
        public void UploadConflictCourseRegistry()
        {
            foreach (CourseItem item in CourseRegistryItems2)
            {
                item.IsConflict = CourseItem.IsConflictCourseRegistry(CourseRegistryItems1, item);
                item.IsValidSubject = CourseItem.IsSameSubjectCourseRegistry(CourseRegistryItems1, item);
            }
        }
        public void RegisterSelectedCourses()
        {
            var SelectedItems = CourseRegistryItems2.Where(x => x.IsSelected == true).ToList();
            string strListError = "";
            foreach (CourseItem item in SelectedItems)
            {
                if (item.IsConflict)
                    continue;
                item.IsSelected = false;

                // Nếu đăng ký thành công thì mới thay đổi
                if (CourseRegisterServices.Instance.StudentRegisterSubjectClassToDatabase(CurrentSemester.Id, CurrentStudent.Id, item.ConvertToSubjectClass()))
                {
                    CourseRegistryItems1.Add(item);
                    CourseRegistryItems2.Remove(item);
                    item.NumberOfStudents += 1;
                    TotalCredit += Convert.ToInt32(item.Subject.Credit);
                }
                else
                {
                    strListError += item.Code + "\n";
                }
            }
            if (strListError.Length > 0)
                MyMessageBox.Show("Những lớp đăng ký không thành công:\n" + strListError, "Lỗi đăng ký");
            else
                MyMessageBox.Show("Đăng ký thành công", "Thành công");
            UploadConflictCourseRegistry();
            Search();
            UpdateScheduleItems();
            CourseRegistryItemsChecked = new ObservableCollection<CourseItem>();
        }
        public void UnregisterSelectedCourses()
        {
            if (MyMessageBox.Show("Bạn thật sự muốn hủy đăng ký?", "Thông báo", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
            {
                var SelectedItems = CourseRegistryItems1.Where(x => x.IsSelected == true).ToList();
                foreach (CourseItem item in SelectedItems)
                {
                    item.NumberOfStudents -= 1;
                    TotalCredit -= Convert.ToInt32(item.Subject.Credit);
                    item.IsSelected = false;
                    CourseRegistryItems2.Add(item);
                    CourseRegistryItems1.Remove(item);
                    CourseRegisterServices.Instance.StudentUnregisterSubjectClassToDatabase(CurrentSemester.Id, CurrentStudent.Id, item.ConvertToSubjectClass());
                }
                MyMessageBox.Show("Hủy đăng ký thành công", "Thành công");
                UploadConflictCourseRegistry();
                Search();
                UpdateScheduleItems();
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

        public void UpdateScheduleItem2()
        {
            if (CourseRegistryItems2.Where(course => course.Id == SelectedScheduleItem2.Id).Count() != 0)
                ScheduleItemsRegistered.Add(SelectedScheduleItem2);
        }
        public void CourseCheckChanged(DataGridBeginningEditEventArgs e)
        {
            CourseItem editCourseItem = e.Row.Item as CourseItem;
            if (editCourseItem.IsConflict || editCourseItem.IsValidSubject)
                return;
            editCourseItem.IsSelected = !editCourseItem.IsSelected;

            UpdateConflictionByEditCourse(editCourseItem);

            editCourseItem.IsSelected = !editCourseItem.IsSelected;
            e.Cancel = true;
        }
        private void UpdateCurrentUser(object sender, LoginServices.LoginEvent e)
        {
            UpdateData();
        }

        public void UpdateConflictionByEditCourse(CourseItem editCourseItem)
        {
            if (editCourseItem.IsSelected)
            {
                CourseRegistryItemsChecked.Add(editCourseItem);
                /*UpdateConflict*/
                foreach (CourseItem course in CourseRegistryItems2)
                {
                    if (course.IsSelected)
                        continue;
                    if (course == editCourseItem)
                        continue;
                    if (!course.IsValidSubject)
                        if (course.Subject.Id == editCourseItem.Subject.Id)
                            course.IsValidSubject = true;
                    if (course.IsConflict)
                        continue;
                    course.IsConflict = CourseItem.IsConflictCourseRegistry(CourseRegistryItemsChecked, course);
                }
                ScheduleItemsRegistered.Add(new ScheduleItem(editCourseItem.ConvertToSubjectClass(), true, false, 3, false));
            }
            else
            {
                CourseRegistryItemsChecked.Remove(editCourseItem);
                foreach (CourseItem course in CourseRegistryItems2)
                {
                    if (course == editCourseItem)
                        continue;
                    if (course.IsValidSubject)
                        if (course.Subject.Id == editCourseItem.Subject.Id)
                            course.IsValidSubject = false;
                    if (!course.IsConflict)
                        continue;
                    course.IsConflict = CourseItem.IsConflictCourseRegistry(CourseRegistryItemsChecked, course) || CourseItem.IsConflictCourseRegistry(CourseRegistryItems1, course);
                }
                ScheduleItem thisSchedule = ScheduleItemsRegistered.Where(schedule => schedule.Id == editCourseItem.Id).FirstOrDefault();
                ScheduleItemsRegistered.Remove(thisSchedule);
            }
        }
        #endregion
    }
}
