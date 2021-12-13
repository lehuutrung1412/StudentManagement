using StudentManagement.Commands;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using StudentManagement.Models;

namespace StudentManagement.ViewModels
{
    public class StudentListRightSideBarItemEditViewModel : BaseViewModel
    {
        private ObservableCollection<StudentDetailScore> _currentScore;
        public ObservableCollection<StudentDetailScore> CurrentScore
        {
            get => _currentScore;
            set
            {
                _currentScore = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<StudentDetailScore> _actualScore;
        public ObservableCollection<StudentDetailScore> ActualScore
        {
            get => _actualScore;
            set
            {
                _actualScore = value;

                try
                {
                    if (_actualScore != null)
                    {
                        var scores = ScoreServices.Instance.LoadScoreStudentById(SubjectClassDetail.Id, SelectedItem.Id);
                        CurrentScore = new ObservableCollection<StudentDetailScore>(scores);
                        foreach (var score in CurrentScore)
                        {
                            score.PropertyChanged += NewStudentScore_PropertyChanged;
                        }

                        AverageScore = ScoreServices.Instance.CalculateAverageScore(CurrentScore.ToList());
                    }
                }
                catch (Exception)
                {
                }

                OnPropertyChanged();
            }
        }

        private void NewStudentScore_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Score")
            {
                AverageScore = ScoreServices.Instance.CalculateAverageScore(CurrentScore.ToList());
            }
        }

        private StudentGrid _selectedItem;
        public StudentGrid SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;

                OnPropertyChanged();
            }
        }

        public double? AverageScore { get => _averageScore; set { _averageScore = value; OnPropertyChanged(); } }
        private double? _averageScore;

        public bool SwitchToView { get => _switchToView; set { _switchToView = value; OnPropertyChanged(); } }

        private bool _switchToView;

        public ICommand ConfirmEditDetailScore { get; set; }

        public ICommand CancelEditDetailScore { get; set; }

        SubjectClass SubjectClassDetail { get; set; }

        public StudentListRightSideBarItemEditViewModel(SubjectClass subjectClass)
        {
            SubjectClassDetail = subjectClass;

            InitCommand();
        }

        public void InitCommand()
        {
            CancelEditDetailScore = new RelayCommand<object>((p) => { return true; }, (p) => CancelEditDetailScoreFunction());
            ConfirmEditDetailScore = new RelayCommand<object>((p) => { return true; }, (p) => ConfirmEditDetailScoreFunction());
        }

        public void CancelEditDetailScoreFunction()
        {
            ReturnToShowDetailScore();
        }

        public async void ConfirmEditDetailScoreFunction()
        {
            ActualScore = new ObservableCollection<StudentDetailScore>(CurrentScore);

            await ScoreServices.Instance.SaveStudentScoreDatabaseAsync(ActualScore.ToList());

            ReturnToShowDetailScore();
        }

        public void ReturnToShowDetailScore()
        {
            SwitchToView = true;
        }
    }
}
