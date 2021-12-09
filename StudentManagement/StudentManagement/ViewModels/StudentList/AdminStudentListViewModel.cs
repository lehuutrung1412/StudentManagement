using StudentManagement.Commands;
using StudentManagement.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StudentManagement.Objects;

namespace StudentManagement.ViewModels
{
    public class AdminStudentListViewModel : BaseViewModel
    {
        #region object
        public class Student : BaseViewModel
        {
            private string _displayName;
            private string _username;
            private string _email;
            private string _gender;
            private string _faculty;
            private string _status;
            private int _number;
            private string _trainingForm;
            private bool _isSelected;

            public bool IsSelected
            {
                get => _isSelected;
                set { _isSelected = value; OnPropertyChanged(); }
            }

            public string DisplayName
            {
                get => _displayName;
                set => _displayName = value;
            }

            public string Username
            {
                get => _username;
                set => _username = value;
            }

            public string Email
            {
                get => _email;
                set => _email = value;
            }

            public string Gender
            {
                get => _gender;
                set => _gender = value;
            }

            public string Faculty
            {
                get => _faculty;
                set => _faculty = value;
            }

            public string Status
            {
                get => _status;
                set => _status = value;
            }

            public int Number
            {
                get => _number;
                set => _number = value;
            }

            public string TrainingForm
            {
                get => _trainingForm;
                set => _trainingForm = value;
            }
        }
        #endregion

        public string SearchQuery { get => _searchQuery; set { _searchQuery = value; SearchNameFunction(); OnPropertyChanged(); } }
        private string _searchQuery;

        private Canvas _mainCanvas;
        public Canvas MainCanvas
        {
            get => _mainCanvas;
            set
            {
                _mainCanvas = value;
            }
        }

        private ObservableCollection<PieChartElement> _dataPieChart;
        public ObservableCollection<PieChartElement> DataPieChart
        {
            get => _dataPieChart;
            set
            {
                _dataPieChart = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Student> _bindingData;
        public ObservableCollection<Student> BindingData
        {
            get => _bindingData;
            set
            {
                _bindingData = value;
                OnPropertyChanged();
            }
        }

        private Student _selectedItem;
        public Student SelectedItem
        {
            get => _selectedItem;
            set => _selectedItem = value;
        }

        private bool _isSelectedAll;
        public bool IsSelectedAll
        {
            get => _isSelectedAll;
            set
            {
                _isSelectedAll = value;

                foreach (var student in BindingData)
                {
                    student.IsSelected = value;
                }

                OnPropertyChanged();
            }
        }

        private ObservableCollection<DetailScore> _studentScore;
        public ObservableCollection<DetailScore> StudentScore
        {
            get => _studentScore;
            set => _studentScore = value;
        }

        private ObservableCollection<Student> _studentDatabase;
        public ObservableCollection<Student> StudentDatabase
        {
            get => _studentDatabase;
            set => _studentDatabase = value;
        }

        private ObservableCollection<Student> _studentClass;
        public ObservableCollection<Student> StudentClass
        {
            get => _studentClass;
            set => _studentClass = value;
        }

        private ObservableCollection<Student> _deleteStudentList;
        public ObservableCollection<Student> DeleteStudentList
        {
            get => _deleteStudentList;
            set => _deleteStudentList = value;
        }

        public ICommand SearchName { get; set; }
        public ICommand DeleteStudent { get; set; }
        public ICommand AddStudent { get; set; }

        private float _pieWidth, _pieHeight, _centerX, _centerY, _radius;

        public AdminStudentListViewModel()
        {
            InitChartParemeter();

            StudentDatabase = new ObservableCollection<Student>();
            StudentDatabase.Add(new Student { DisplayName = "Nguyễn Tấn Trần Minh Khang", Email = "example0@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520123", Number = 1 });
            StudentDatabase.Add(new Student { DisplayName = "Ngô Quang Vinh", Email = "example1@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520124", Number = 2 });
            StudentDatabase.Add(new Student { DisplayName = "Lê Hữu Trung", Email = "example2@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520125", Number = 3 });
            StudentDatabase.Add(new Student { DisplayName = "Hứa Thanh Tân", Email = "example3@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520126", Number = 4 });
            StudentDatabase.Add(new Student { DisplayName = "Nguyễn Đỗ Mạnh Cường", Email = "example4@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520127", Number = 5 });
            StudentDatabase.Add(new Student { DisplayName = "Nguyễn Đình Bình An", Email = "example5@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520128", Number = 6 });
            StudentDatabase.Add(new Student { DisplayName = "Nguyễn Minh Huy", Email = "example6@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520129", Number = 6 });
            StudentDatabase.Add(new Student { DisplayName = "Nguyễn Minh Huy Cầu Vòng", Email = "example7@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520130", Number = 6 });
            StudentDatabase.Add(new Student { DisplayName = "Nguyễn Minh RainbowShine", Email = "example8@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520131", Number = 6 });
            StudentDatabase.Add(new Student { DisplayName = "Nguyễn Minh UIT.Leader", Email = "example9@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520132", Number = 6 });

            StudentClass = new ObservableCollection<Student>();
            StudentClass.Add(new Student { DisplayName = "Nguyễn Tấn Trần Minh Khang", Email = "example0@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520123", IsSelected = false });
            StudentClass.Add(new Student { DisplayName = "Ngô Quang Vinh", Email = "example1@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520124", IsSelected = false });
            StudentClass.Add(new Student { DisplayName = "Lê Hữu Trung", Email = "example2@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520125", IsSelected = false });
            StudentClass.Add(new Student { DisplayName = "Hứa Thanh Tân", Email = "example3@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520126", IsSelected = false });
            StudentClass.Add(new Student { DisplayName = "Nguyễn Đỗ Mạnh Cường", Email = "example4@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520127", IsSelected = false });
            StudentClass.Add(new Student { DisplayName = "Nguyễn Đình Bình An", Email = "example5@gmail.com", Gender = "Nam", Faculty = "KHMT", Status = "Online", Username = "19520128", IsSelected = false });

            StudentScore = new ObservableCollection<DetailScore>();
            StudentScore.Add(new DetailScore { CuoiKi = "10", GiuaKi = "10", QuaTrinh = "10", ThucHanh = "10", DiemTB = "1", IDStudent = "19520123" });
            StudentScore.Add(new DetailScore { CuoiKi = "10", GiuaKi = "10", QuaTrinh = "10", ThucHanh = "10", DiemTB = "6", IDStudent = "19520124" });
            StudentScore.Add(new DetailScore { CuoiKi = "10", GiuaKi = "10", QuaTrinh = "10", ThucHanh = "10", DiemTB = "7", IDStudent = "19520125" });
            StudentScore.Add(new DetailScore { CuoiKi = "10", GiuaKi = "10", QuaTrinh = "10", ThucHanh = "10", DiemTB = "8", IDStudent = "19520126" });
            StudentScore.Add(new DetailScore { CuoiKi = "10", GiuaKi = "10", QuaTrinh = "10", ThucHanh = "10", DiemTB = "9", IDStudent = "19520127" });
            StudentScore.Add(new DetailScore { CuoiKi = "10", GiuaKi = "10", QuaTrinh = "10", ThucHanh = "10", DiemTB = "10", IDStudent = "19520128" });

            BindingData = new ObservableCollection<Student>(StudentClass);
            InitCommand();

            SearchNameFunction();
        }

        #region Methods

        private void InitChartParemeter()
        {
            _pieWidth = 180; _pieHeight = 180;
            _centerX = _pieWidth / 2; _centerY = _pieHeight / 2;
            _radius = _pieWidth / 2;

            DataPieChart = new ObservableCollection<PieChartElement>();

            MainCanvas = new Canvas
            {
                Width = _pieWidth,
                Height = _pieHeight
            };
        }

        private void InitCommand()
        {
            SearchName = new RelayCommand<object>((p) => true, (p) => SearchNameFunction());
            DeleteStudent = new RelayCommand<object>((p) => true, (p) => DeleteStudentFunction());
            AddStudent = new RelayCommand<object>((p) => true, (p) => AddStudentFunction());
        }

        private float RecalculateStudentRating(int number, int totalNumber)
        {
            return (float)number * 100 / totalNumber;
        }

        void CalculationPercentage()
        {
            DataPieChart.Clear();

            int exellent = 0, veryGood = 0, good = 0, avg = 0, bad = 0;

            foreach (var student in StudentClass)
                foreach (var score in StudentScore)
                    if (student.Username == score.IDStudent)
                    {
                        double currentScore = Convert.ToDouble(score.DiemTB);
                        if (currentScore >= 9)
                            exellent += 1;
                        else if (currentScore >= 8)
                            veryGood += 1;
                        else if (currentScore >= 7)
                            good += 1;
                        else if (currentScore >= 6)
                            avg += 1;
                        else
                            bad += 1;
                        break;
                    }

            int sizeClass = StudentClass.Count();

            DataPieChart.Add(new PieChartElement { Title = "Sinh viên xuất sắc", ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4472C4")), Percentage = RecalculateStudentRating(exellent, sizeClass) }); ;
            DataPieChart.Add(new PieChartElement { Title = "Sinh viên giỏi", ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ED7D31")), Percentage = RecalculateStudentRating(veryGood, sizeClass) });
            DataPieChart.Add(new PieChartElement { Title = "Sinh viên khá", ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC000")), Percentage = RecalculateStudentRating(good, sizeClass) });
            DataPieChart.Add(new PieChartElement { Title = "Sinh viên trung bình", ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5B9BD5")), Percentage = RecalculateStudentRating(avg, sizeClass) });
            DataPieChart.Add(new PieChartElement { Title = "Sinh viên yếu", ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5A5A5")), Percentage = RecalculateStudentRating(bad, sizeClass) });
        }

        private bool IsStudentExistInClass(string studentId)
        {
            return StudentClass.Any(student => student.Username == studentId);
        }

        private Student FindSearchQueryInDatabase(string studentId)
        {
            return StudentDatabase.FirstOrDefault(student => student.Username == studentId);
        }

        private void AddStudentFunction()
        {
            if (IsStudentExistInClass(SearchQuery))
            {
                MyMessageBox.Show("Sinh viên " + SearchQuery + " đã tồn tại ở lớp học!", "Thêm sinh viên", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Student findStudent = FindSearchQueryInDatabase(SearchQuery);

            if (findStudent == null)
            {
                MyMessageBox.Show("Mã số sinh viên " + SearchQuery + " không hợp lệ !", "Thêm sinh viên", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            StudentClass.Add(findStudent);
            StudentScore.Add(new DetailScore { CuoiKi = "0", GiuaKi = "0", DiemTB = "0", QuaTrinh = "0", ThucHanh = "0", IDStudent = findStudent.Username });
            MyMessageBox.Show("Sinh viên " + SearchQuery + " đã được thêm vào lớp học!", "Thêm sinh viên", MessageBoxButton.OK, MessageBoxImage.Information);

            SearchQuery = "";
        }

        void DrawPieChart()
        {
            try
            {
                CalculationPercentage();

                float angle = 0, prevAngle = 0;
                foreach (var category in DataPieChart)
                {
                    double line1X = (_radius * Math.Cos(angle * Math.PI / 180)) + _centerX;
                    double line1Y = (_radius * Math.Sin(angle * Math.PI / 180)) + _centerY;

                    angle = category.Percentage * (float)360 / 100 + prevAngle;

                    double arcX = (_radius * Math.Cos(angle * Math.PI / 180)) + _centerX;
                    double arcY = (_radius * Math.Sin(angle * Math.PI / 180)) + _centerY;

                    var line1Segment = new LineSegment(new Point(line1X, line1Y), false);
                    double arcWidth = _radius, arcHeight = _radius;
                    bool isLargeArc = category.Percentage > 50;
                    var arcSegment = new ArcSegment()
                    {
                        Size = new Size(arcWidth, arcHeight),
                        Point = new Point(arcX, arcY),
                        SweepDirection = SweepDirection.Clockwise,
                        IsLargeArc = isLargeArc,
                    };
                    var line2Segment = new LineSegment(new Point(_centerX, _centerY), false);

                    var pathFigure = new PathFigure(
                        new Point(_centerX, _centerY),
                        new List<PathSegment>()
                        {
                        line1Segment,
                        arcSegment,
                        line2Segment,
                        },
                        true);

                    var pathFigures = new List<PathFigure>() { pathFigure, };
                    var pathGeometry = new PathGeometry(pathFigures);
                    var path = new System.Windows.Shapes.Path()
                    {
                        Fill = category.ColorBrush,
                        Data = pathGeometry,
                    };
                    MainCanvas.Children.Add(path);

                    prevAngle = angle;


                    // draw outlines
                    var outline1 = new Line()
                    {
                        X1 = _centerX,
                        Y1 = _centerY,
                        X2 = line1Segment.Point.X,
                        Y2 = line1Segment.Point.Y,
                        Stroke = Brushes.White,
                        StrokeThickness = 5,
                    };
                    var outline2 = new Line()
                    {
                        X1 = _centerX,
                        Y1 = _centerY,
                        X2 = arcSegment.Point.X,
                        Y2 = arcSegment.Point.Y,
                        Stroke = Brushes.White,
                        StrokeThickness = 5,
                    };

                    MainCanvas.Children.Add(outline1);
                    MainCanvas.Children.Add(outline2);
                }
            }
            catch (Exception)
            {
                return;
            }
            
        }

        void SearchNameFunction()
        {
            if (SearchQuery == null)
            {
                SearchQuery = "";
            }

            int stt = 0;
            BindingData.Clear();
            foreach (var student in StudentClass)
            {
                if (VietnameseStringNormalizer.Instance.Normalize(student.DisplayName)
                    .Contains(VietnameseStringNormalizer.Instance.Normalize(SearchQuery))
                    || student.Username.Contains(SearchQuery)
                )
                {
                    student.Number = stt + 1;
                    stt += 1;
                    BindingData.Add(student);
                }
            }

            DrawPieChart();
        }

        void DeleteStudentFunction()
        {
            DeleteStudentList = new ObservableCollection<Student>();
            foreach (var student in BindingData)
            {
                if (student.IsSelected)
                {
                    DeleteStudentList.Add(student);
                }
            }

            if (DeleteStudentList.Count() == 0)
            {
                MyMessageBox.Show("Vui lòng chọn sinh viên cần xóa!", "Xóa sinh viên", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult messageBoxResult = MyMessageBox.Show(
                $"Bạn có chắc chắn muốn xóa {DeleteStudentList.Count()} sinh viên đã chọn?",
                "Xóa sinh viên",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                foreach (var student in DeleteStudentList)
                    StudentClass.Remove(student);
                IsSelectedAll = false;
            }
            else return;

            SearchNameFunction();
        }

        #endregion
    }
}
