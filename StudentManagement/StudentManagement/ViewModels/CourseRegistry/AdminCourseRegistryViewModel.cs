using ClosedXML.Excel;
using ExcelDataReader;
using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Services;
using StudentManagement.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using static StudentManagement.ViewModels.StudentCourseRegistryViewModel;

namespace StudentManagement.ViewModels
{
    public class AdminCourseRegistryViewModel : BaseViewModel
    {
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

        private ObservableCollection<ObservableCollection<CourseItem>> _courseRegistryItemsAll;
        public ObservableCollection<ObservableCollection<CourseItem>> CourseRegistryItemsAll { get => _courseRegistryItemsAll; set => _courseRegistryItemsAll = value; }
        private ObservableCollection<CourseItem> _courseRegistryItems;
        public ObservableCollection<CourseItem> CourseRegistryItems { get => _courseRegistryItems; set => _courseRegistryItems = value; }
        private ObservableCollection<CourseItem> _courseRegistryItemsDisplay;
        public ObservableCollection<CourseItem> CourseRegistryItemsDisplay { get => _courseRegistryItemsDisplay; set { _courseRegistryItemsDisplay = value; OnPropertyChanged(); } }

        public ObservableCollection<Models.Semester> Semesters { get => _semesters; set { _semesters = value; OnPropertyChanged(); } }
        private ObservableCollection<Models.Semester> _semesters;

        public Models.Semester SelectedSemester
        {
            get => _selectedSemester;
            set
            {
                _selectedSemester = value;
                OnPropertyChanged();
                if (value != null)
                    AdminCourseRegistryRightSideBarViewModel.Instance.CanEdit = !(_selectedSemester.CourseRegisterStatus > 0);
            }
        }
        private Models.Semester _selectedSemester;
        public int SelectedSemesterIndex
        {
            get => _selectedSemesterIndex;
            set
            {
                _selectedSemesterIndex = value;
                OnPropertyChanged();
                SelectData();

                AdminCourseRegistryRightSideBarViewModel.Instance.RightSideBarItemViewModel = new EmptyStateRightSideBarViewModel();
            }
        }
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

        public object _creatNewCourseViewModel;

        #region CreateNewSemester
        private ObservableCollection<string> _batches;
        public ObservableCollection<string> Batches { get => _batches; set { _batches = value; OnPropertyChanged(); } }

        private string _selectedBatch;
        public string SelectedBatch { get => _selectedBatch; set { _selectedBatch = value; OnPropertyChanged(); } }

        private string _newSemesterName;
        public string NewSemesterName { get => _newSemesterName; set { _newSemesterName = value; OnPropertyChanged(); } }

        private bool _isDoneVisible;
        private bool _isErrorVisible;
        public bool IsDoneVisible { get => _isDoneVisible; set { _isDoneVisible = value; OnPropertyChanged(); } }
        public bool IsErrorVisible { get => _isErrorVisible; set { _isErrorVisible = value; OnPropertyChanged(); } }
        #endregion
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
        public ICommand CreateNewSemesterCommand { get; set; }
        public ICommand AddFromExcelCommand { get; set; }
        public ICommand ExportExcelCommand { get; set; }
        public ICommand PopupCreateSemester { get; set; }


        #endregion
        private static AdminCourseRegistryViewModel s_instance;
        public static AdminCourseRegistryViewModel Instance
        {
            get => s_instance ?? (s_instance = new AdminCourseRegistryViewModel());

            private set => s_instance = value;
        }
        public AdminCourseRegistryViewModel()
        {
            Instance = this;
            Semesters = SemesterServices.Instance.LoadListSemester();
            SubjectClasses = new ObservableCollection<SubjectClass>(SubjectClassServices.Instance.LoadSubjectClassList().Where(el => el.IsDeleted != true));
            CourseRegistryItemsAll = new ObservableCollection<ObservableCollection<CourseItem>>();
            for (int i = 0; i < Semesters.Count; i++)
            {
                Semester semester = Semesters[i];
                var subjectClasses1Semester = SubjectClasses.Where(x => x.Semester == semester).ToList();
                var courseItems1Semester = new ObservableCollection<CourseItem>();
                foreach (Models.SubjectClass a in subjectClasses1Semester)
                {
                    courseItems1Semester.Add(new CourseItem(a, false));
                }
                CourseRegistryItemsAll.Add(courseItems1Semester);
            }
            SelectData();
            SelectedSemester = Semesters.LastOrDefault();
            InitCreateNewSemesterProperty();
            InitCommand();
        }

        public void InitCreateNewSemesterProperty()
        {
            NewSemesterName = "Học kỳ 1";

            CreateNewBatch();
            SelectedBatch = Batches.Last();
        }
        public void InitCommand()
        {
            SwitchSearchButton = new RelayCommand<object>((p) => { return true; }, (p) => SwitchSearchButtonFunction(p));
            SearchCourseRegistryItems = new RelayCommand<object>((p) => { return true; }, (p) => SearchCourseRegistryItemsFunction());
            DeleteSelectedItemsCommand = new RelayCommand<object>(
                (p) =>
                {
                    if (SelectedSemester == null)
                        return false;
                    return CourseRegistryItemsDisplay.Where(x => x.IsSelected == true).Count() > 0 && !(SelectedSemester.CourseRegisterStatus > 1);
                },
                (p) =>
                {
                    DeleteSelectedItems();
                });
            CreateNewCourseCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedSemester == null)
                    return false;
                return !(SelectedSemester.CourseRegisterStatus > 1);
            }, (p) => CreateNewCourse());
            OpenSemesterCommand = new RelayCommand<object>((p) => true, (p) => { SelectedSemester.CourseRegisterStatus = 1; SemesterServices.Instance.SaveSemesterToDatabase(SelectedSemester); });
            PauseSemesterCommand = new RelayCommand<object>((p) => true, (p) => { SelectedSemester.CourseRegisterStatus = 0; SemesterServices.Instance.SaveSemesterToDatabase(SelectedSemester); });
            StopSemesterCommand = new RelayCommand<object>((p) => true, (p) => { SelectedSemester.CourseRegisterStatus = 2; SemesterServices.Instance.SaveSemesterToDatabase(SelectedSemester); });

            CreateNewSemesterCommand = new RelayCommand<object>((p) =>
            {
                if (String.IsNullOrEmpty(SelectedBatch) || String.IsNullOrEmpty(NewSemesterName))
                    return false;
                if (Semesters.Where(x => x.Batch.Replace(" ", "") == SelectedBatch.Replace(" ", "")).
                                Where(y => y.DisplayName.Replace(" ", "") == NewSemesterName.Replace(" ", "")).Count() > 0)
                    return false;
                return true;
            }, (p) => CreateNewSemester());

            AddFromExcelCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedSemester == null)
                    return false;
                return !(SelectedSemester.CourseRegisterStatus > 1);
            }, (p) => AddFromExcel());

            PopupCreateSemester = new RelayCommand<object>((p) => true, (p) => ResetDoneErrorVisibility());
            ExportExcelCommand = new RelayCommand<object>((p) => true, (p) => ExportExcel());
        }
        public void SelectData()
        {
            if (Semesters.Count == 0)
            {
                CourseRegistryItems = new ObservableCollection<CourseItem>();
                CourseRegistryItemsDisplay = CourseRegistryItems;
                return;
            }
            CourseRegistryItems = CourseRegistryItemsAll[SelectedSemesterIndex];
            CourseRegistryItemsDisplay = CourseRegistryItems;
        }
        public void SwitchSearchButtonFunction(object p)
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
                var tmp = CourseRegistryItems.Where(x => x.Code.ToLower().Contains(SearchQuery.ToLower())).ToList();
                CourseRegistryItemsDisplay = new ObservableCollection<CourseItem>(tmp);
            }
            else
            {
                var tmp = CourseRegistryItems.Where(x => vietnameseStringNormalizer.Normalize(x.Subject.DisplayName).ToLower().Contains(vietnameseStringNormalizer.Normalize(SearchQuery.ToLower()))).ToList();
                CourseRegistryItemsDisplay = new ObservableCollection<CourseItem>(tmp);
            }
        }
        public void DeleteSelectedItems()
        {
            if (MyMessageBox.Show("Bạn thật sự muốn xóa những lớp đã chọn?", "Thông báo", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
            {
                var SelectedItems = CourseRegistryItems.Where(x => x.IsSelected == true).ToList();
                string listErrorDelete = "";
                foreach (CourseItem item in SelectedItems)
                {
                    try
                    {
                        if (SubjectClassServices.Instance.RemoveSubjectClassFromDatabaseBySubjectClassId(item.Id))
                            CourseRegistryItems.Remove(item);
                    }
                    catch
                    {
                        listErrorDelete += item.Code + "\n";
                    }
                }
                if (listErrorDelete == "")
                    MyMessageBox.Show("Xóa tất cả lớp được chọn thành công", "Thành công");
                else
                    MyMessageBox.Show("Xóa thất bại:\n" + listErrorDelete, "Lỗi");
                SearchCourseRegistryItemsFunction();
                /*StudentCourseRegistryViewModel.Instance.UpdateData();*/
            }

        }
        public void CreateNewCourse()
        {
            var newSubjectClass = new SubjectClass(); 
            _creatNewCourseViewModel = new CreateNewCourseViewModel(newSubjectClass, SelectedSemester, CourseRegistryItemsAll[SelectedSemesterIndex]);
            MainViewModel.Instance.DialogViewModel = this._creatNewCourseViewModel;
            MainViewModel.Instance.IsOpen = true;
        }

        public void CreateNewSemester()
        {
            try
            {
                Semester temp = new Semester() { Batch = SelectedBatch, CourseRegisterStatus = 0, DisplayName = NewSemesterName, Id = Guid.NewGuid() };
                Semesters.Add(temp);
                SemesterServices.Instance.SaveSemesterToDatabase(temp);
                IsDoneVisible = true;
                var courseItemsNewSemester = new ObservableCollection<CourseItem>() { };
                CourseRegistryItemsAll.Add(courseItemsNewSemester);
                SelectedSemester = Semesters.Last();
                /*Semesters = new ObservableCollection<Semester>(Semesters.OrderBy(y => y.DisplayName).OrderBy(x => x.Batch).ToList());*/
                CreateNewBatch();

            }
            catch
            {
                IsErrorVisible = true;
            }
            
        }

        public void CreateNewBatch()
        {
            var temp = Semesters.Select(x => x.Batch).Distinct().ToList();
            if (temp.Count == 0)
            {
                Batches = new ObservableCollection<string>()
                {
                    DateTime.Now.AddYears(-1).Year.ToString() + "-" + DateTime.Now.Year.ToString(),
                    DateTime.Now.Year.ToString() + "-" + DateTime.Now.AddYears(1).Year.ToString(),
                };
                return;
            }
            Batches = new ObservableCollection<string>(temp);
            string defaultNewBatch = "";
            foreach (string year in Batches.Last().Split('-'))
            {
                defaultNewBatch += Convert.ToString(Convert.ToInt32(year) + 1) + '-';
            }
            defaultNewBatch = defaultNewBatch.Remove(defaultNewBatch.Length - 1);
            Batches.Add(defaultNewBatch);
        }

        public void AddFromExcel()
        {
            using (OpenFileDialog op = new OpenFileDialog() { Filter = "Excel|*.xls;*.xlsx;" })
            {
                try
                {
                    if (op.ShowDialog() == DialogResult.OK)
                    {
                        DataTableCollection dataSheets;
                        using (var stream = File.Open(op.FileName, FileMode.Open, FileAccess.Read))
                        {
                            using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                                });
                                dataSheets = result.Tables;
                            }
                        }
                        DataTable data = dataSheets[0];

                        int countNeworChangeSubjectClass = 0;

                        foreach (DataRow courseRow in data.Rows)
                        {
                            string TFName = courseRow[7].ToString();
                            string userNameTeacher = Convert.ToString(courseRow[8]);
                            var tempSubjectClass = new SubjectClass()
                            {
                                Id = Guid.NewGuid(),
                                Semester = SelectedSemester,
                                Subject = SubjectServices.Instance.FindSubjectBySubjectName(Convert.ToString(courseRow[0])),   //Column SubjectName NVARCHAR
                                StartDate = Convert.ToDateTime(courseRow[1]),                                                  //Column StartDate Date
                                EndDate = Convert.ToDateTime(courseRow[2]),                                                  //Column EndDate Date
                                Period = Convert.ToString(courseRow[3]),                                                       //Column Period NVARCHAR
                                WeekDay = Convert.ToInt32(courseRow[4]),                                                      //Column WeekDay NVARCHAR
                                Code = Convert.ToString(courseRow[5]),
                                MaxNumberOfStudents = Convert.ToInt32(courseRow[6]),
                                TrainingForm = DataProvider.Instance.Database.TrainingForms.Where(tf => tf.DisplayName.Equals(TFName)).FirstOrDefault(),
                                Teachers = new ObservableCollection<Teacher>() { TeacherServices.Instance.FindTeacherByUserName(userNameTeacher) },
                                DatabaseImageTable = /*DatabaseImageTableServices.Instance.GetFirstDatabaseImageTable()*/ null,           //Thiếu image
                                NumberOfStudents = 0
                            };
                            if (tempSubjectClass.Teachers.FirstOrDefault() == null)
                            {
                                tempSubjectClass.Teachers.Clear();
                                tempSubjectClass.Teachers.Add(DataProvider.Instance.Database.Teachers.FirstOrDefault());
                            }
                            SubjectClassServices.Instance.UpdateIds(tempSubjectClass);

                            var conflictAvailableCourse = CourseRegistryItemsAll[SelectedSemesterIndex].Where(x => x.Code == tempSubjectClass.Code).FirstOrDefault();
                            if (conflictAvailableCourse != null)
                            {
                                tempSubjectClass.Id = conflictAvailableCourse.Id;
                                if (conflictAvailableCourse.IsEqualProperty(tempSubjectClass))
                                    continue;
                                else
                                {
                                    if (MyMessageBox.Show(String.Format("Có sự thay đổi trong thông tin lớp {0}. Bạn có muốn thay đổi", tempSubjectClass.Code), "Thông báo", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                                    {
                                        var tempCourse = new CourseItem(tempSubjectClass, false);
                                        tempCourse.CopyProperties(conflictAvailableCourse);
                                        SubjectClassServices.Instance.GenerateDefaultCommponentScore(tempSubjectClass);
                                        SubjectClassServices.Instance.SaveSubjectClassToDatabase(tempSubjectClass);
                                        countNeworChangeSubjectClass++;
                                    }
                                }
                            }
                            else
                            {
                                var tempCourse = new CourseItem(tempSubjectClass, false);
                                SubjectClassServices.Instance.GenerateDefaultCommponentScore(tempSubjectClass);
                                SubjectClassServices.Instance.SaveSubjectClassToDatabase(tempSubjectClass);
                                countNeworChangeSubjectClass++;
                                CourseRegistryItemsAll[SelectedSemesterIndex].Add(tempCourse);
                            }
                        }
                        if (countNeworChangeSubjectClass > 0)
                        {
                            SelectData();
                            SearchQuery = "";
                            MyMessageBox.Show("Hoàn thành thêm từ file", "Thành công");
                        }
                        else
                            MyMessageBox.Show("Không có sự thay đổi nào cả", "Thành công");
                        
                        /*StudentCourseRegistryViewModel.Instance.UpdateData();*/
                    }
                }
                catch
                {
                    MyMessageBox.Show("Lỗi không đọc file được", "Lỗi");
                }
            }
        }

        public void ResetDoneErrorVisibility()
        {
            IsDoneVisible = false;
            IsErrorVisible = false;
        }

        public void ExportExcel()
        {
            try
            {
                using (SaveFileDialog saveDlg = new SaveFileDialog() { Filter = "Excel|*.xlsx;" })
                {
                    if (saveDlg.ShowDialog() == DialogResult.OK)
                    {
                        var workbook = new XLWorkbook();

                        foreach (ObservableCollection<CourseItem> course1Semester in CourseRegistryItemsAll)
                        {
                            Semester wsSemester = Semesters[CourseRegistryItemsAll.IndexOf(course1Semester)];
                            workbook.AddWorksheet(wsSemester.Batch + ", " + wsSemester.DisplayName);
                            var ws = workbook.Worksheet(wsSemester.Batch + ", " + wsSemester.DisplayName);

                            // Tạo header
                            int row = 1;
                            ws.Cell("A" + row.ToString()).Value = "Tên môn học";
                            ws.Cell("B" + row.ToString()).Value = "Ngày bắt đầu";
                            ws.Cell("C" + row.ToString()).Value = "Ngày kết thúc";
                            ws.Cell("D" + row.ToString()).Value = "Tiết";
                            ws.Cell("E" + row.ToString()).Value = "Thứ";
                            ws.Cell("F" + row.ToString()).Value = "Mã lớp";
                            ws.Cell("G" + row.ToString()).Value = "Giới hạn ĐK";
                            ws.Cell("H" + row.ToString()).Value = "Tên hệ đào tạo";
                            ws.Cell("I" + row.ToString()).Value = "Tên tài khoản giáo viên";

                            //Điền data
                            row++;
                            foreach (var item in CourseRegistryItems)
                            {
                                Type a = item.GetType();
                                ws.Cell("A" + row.ToString()).Value = item.Subject.DisplayName;
                                ws.Cell("B" + row.ToString()).Value = item.StartDate;
                                ws.Cell("C" + row.ToString()).Value = item.EndDate;
                                ws.Cell("D" + row.ToString()).Value = item.Period;
                                ws.Cell("E" + row.ToString()).Value = item.WeekDay;
                                ws.Cell("F" + row.ToString()).Value = item.Code;
                                ws.Cell("G" + row.ToString()).Value = item.MaxNumberOfStudents;
                                ws.Cell("H" + row.ToString()).Value = item.TrainingForm.DisplayName;
                                ws.Cell("I" + row.ToString()).Value = item.MainTeacher.User.Username;
                                row++;
                            }
                        }




                        //Save file
                        workbook.SaveAs(saveDlg.FileName);
                        MyMessageBox.Show("Xuất file thành công", "Thành công");
                    }
                }
            }
            catch
            {
                MyMessageBox.Show("Đã có vấn đề xảy ra khi xuất file", "Lỗi");
            }
        }
    }
}
