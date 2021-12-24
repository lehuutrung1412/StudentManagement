using Microsoft.Win32;
using StudentManagement.Commands;
using StudentManagement.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using StudentManagement.Objects;
using StudentManagement.Services;
using StudentManagement.Models;

namespace StudentManagement.ViewModels
{
    public class ScoreBoardViewModel : BaseViewModel
    {
        private string _selectedSemester;

        public string SelectedSemester
        {
            get => _selectedSemester;
            set
            {
                _selectedSemester = value;
                OnPropertyChanged();
                UpdateScoreBoard(value);
            }
        }

        private ObservableCollection<string> _semesters;
        public ObservableCollection<string> Semesters
        {
            get => _semesters;
            set
            {
                _semesters = value;
            }
        }

        private double _gpa;
        public double GPA
        {
            get => _gpa;
            set => _gpa = value;
        }

        private int _totalCredit;
        public int TotalCredit
        {
            get => _totalCredit;
            set => _totalCredit = value;
        }
        private Guid _idStudent;
        public Guid IdStudent
        {
            get => _idStudent;
            set => _idStudent = value;
        }

        private ObservableCollection<SemesterDataGrid> _databaseSemester;
        public ObservableCollection<SemesterDataGrid> DatabaseSemester
        {
            get => _databaseSemester;
            set => _databaseSemester = value;
        }

        private ObservableCollection<SemesterDataGrid> _displaySemester;
        public ObservableCollection<SemesterDataGrid> DisplaySemester
        {
            get => _displaySemester;
            set
            {
                _displaySemester = value;
                OnPropertyChanged();
            }
        }

        private object _overviewScoreboardItem;
        public object OverviewScoreboardItem { get => _overviewScoreboardItem; set { _overviewScoreboardItem = value; OnPropertyChanged(); } }

        public ICommand OverviewScoreboard { get => _overviewScoreboard; set => _overviewScoreboard = value; }

        private ICommand _overviewScoreboard;

        public ICommand ExportScoreBoard { get => _exportScoreBoard; set => _exportScoreBoard = value; }

        private ICommand _exportScoreBoard;

        public void InitCommand()
        {
            OverviewScoreboard = new RelayCommand<object>((p) => { return true; }, (p) => OverviewScoreboardFunction());
            ExportScoreBoard = new RelayCommand<object>((p) => { return true; }, (p) => ExportScoreBoardFunction());
        }

        public void OverviewScoreboardFunction()
        {
            if (LoginServices.CurrentUser == null)
                return;

            var name = DataProvider.Instance.Database.Students.Where(x => x.Id == IdStudent).FirstOrDefault().User.DisplayName;
            OverviewScoreboardItem = new OverviewScoreboardViewModel(GPA, 90, TotalCredit, name);
            MainViewModel.Instance.DialogViewModel = OverviewScoreboardItem;
            MainViewModel.Instance.IsOpen = true;
        }

        public void ExportScoreBoardFunction()
        {

        }

        public ScoreBoardViewModel()
        {
            if (LoginServices.CurrentUser != null)
            {
                var student = DataProvider.Instance.Database.Students.Where(x => x.IdUsers == LoginServices.CurrentUser.Id).FirstOrDefault();
                if (student == null)
                    return;
                IdStudent = student.Id;
            }

            GPA = 0;
            TotalCredit = 0;
            DatabaseSemester = new ObservableCollection<SemesterDataGrid>();
            DisplaySemester = new ObservableCollection<SemesterDataGrid>();
            Semesters = new ObservableCollection<string>();
            Semesters.Add("Tất cả các kỳ");
            SelectedSemester = "Tất cả các kỳ";
            InitCommand();

            try
            {
                List<CourseRegister> ListCourses = DataProvider.Instance.Database.CourseRegisters.Where(x => x.IdStudent == IdStudent && x.SubjectClass.IsDeleted != true).ToList();
                if (ListCourses == null)
                {
                    return;    
                }

                ObservableCollection<Guid> ListSemester = new ObservableCollection<Guid>();

                foreach (var item in ListCourses)
                {
                    Guid CurrentSemester = item.SubjectClass.Semester.Id;

                    var temp = ListSemester.Where(x => x == CurrentSemester).FirstOrDefault();
                    if (temp != Guid.Empty)
                        continue;

                    ListSemester.Add(CurrentSemester);
                }

                if (ListSemester.Count == 0)
                    return;

                foreach (var id in ListSemester)
                {
                    ObservableCollection<ScoreDataGrid> TempScore = new ObservableCollection<ScoreDataGrid>();
                    double semesterGPA = 0;
                    int semesterCredit = 0;

                    foreach (var item in ListCourses)
                    {
                        if (item.SubjectClass.IdSemester != id)
                            continue;

                        double gpa = 0;

                        var ListComponentScore = DataProvider.Instance.Database.ComponentScores.Where(x => x.IdSubjectClass == item.IdSubjectClass);
                        foreach (var component in ListComponentScore)
                        {
                            var score = DataProvider.Instance.Database.DetailScores.Where(x => x.IdComponentScore == component.Id).FirstOrDefault();
                            if (score == null || score?.Score == null)
                                continue;
                            gpa += (double)score.Score * (double)component.ContributePercent / 100;
                        }

                        TotalCredit += (int)item.SubjectClass.Subject.Credit;
                        semesterCredit += (int)item.SubjectClass.Subject.Credit;
                        GPA += gpa * (int)item.SubjectClass.Subject.Credit;
                        semesterGPA += gpa * (int)item.SubjectClass.Subject.Credit;

                        var teacher = item.SubjectClass.Teachers.FirstOrDefault();
                        string nameTeacher = null;
                        if (teacher != null)
                            nameTeacher = DataProvider.Instance.Database.Users.Where(x => x.Id == teacher.IdUsers).FirstOrDefault().DisplayName;

                        TempScore.Add(new ScoreDataGrid(item.SubjectClass.Id, item.SubjectClass.Code, item.SubjectClass.Subject.DisplayName, Convert.ToString(item.SubjectClass.Subject.Credit), nameTeacher));

                    }

                    if (semesterCredit == 0)
                        semesterGPA = 0;
                    else 
                        semesterGPA = semesterGPA / semesterCredit;
                    
                    var CurrentSemester = DataProvider.Instance.Database.Semesters.Where(x => x.Id == id).FirstOrDefault();
                    if (CurrentSemester != null)
                        DatabaseSemester.Add(new SemesterDataGrid(id, CurrentSemester.DisplayName, CurrentSemester.Batch, semesterGPA, 0, TempScore, null));
                }

                if (TotalCredit == 0)
                    GPA = 0;
                else 
                    GPA = 1.0 * GPA / TotalCredit;

                DatabaseSemester = new ObservableCollection<SemesterDataGrid>(DatabaseSemester.OrderBy(x => x.Batch + x.DisplayName));
                foreach (var item in DatabaseSemester)
                {
                    Semesters.Add(item.DisplayName + ", năm học " + item.Batch);
                }

                UpdateScoreBoard("Tất cả các kỳ");
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra");
            }
        }

        private void UpdateScoreBoard(string semester)
        {
            try
            {
                if (semester == "Tất cả các kỳ")
                {
                    DisplaySemester = new ObservableCollection<SemesterDataGrid>(DatabaseSemester);
                    return;
                }

                DisplaySemester = new ObservableCollection<SemesterDataGrid>();
                foreach (var item in DatabaseSemester)
                {
                    string nameSemester = item.DisplayName + ", năm học " + item.Batch;
                    if (semester == nameSemester)
                    {
                        DisplaySemester.Add(item);
                        return;
                    }
                }
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra");
            }
        }

    }
}