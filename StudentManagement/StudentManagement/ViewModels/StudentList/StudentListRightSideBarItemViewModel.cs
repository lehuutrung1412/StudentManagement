using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class StudentListRightSideBarItemViewModel : BaseViewModel
    {
        private StudentGrid _selectedItem;
        public StudentGrid SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;

                if (_selectedItem != null)
                {
                    BindingScore.Clear();
                    DetailScores.Where(score => score.IdStudent == _selectedItem.Id).ToList()
                        .ForEach(studentScore => BindingScore.Add(studentScore));

                    AverageScore = ScoreServices.Instance.CalculateAverageScore(BindingScore.ToList());
                }

                OnPropertyChanged();
            }
        }

        public double? AverageScore { get => _averageScore; set { _averageScore = value; OnPropertyChanged(); } }
        private double? _averageScore;

        public ObservableCollection<StudentDetailScore> DetailScores { get => _detailScores; set { _detailScores = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentDetailScore> _detailScores;

        public ObservableCollection<StudentDetailScore> BindingScore
        {
            get => _bindingScore;
            set
            {
                _bindingScore = value;

                AverageScore = ScoreServices.Instance.CalculateAverageScore(_bindingScore.ToList());

                FirstLoadData();

                OnPropertyChanged();
            }
        }
        private ObservableCollection<StudentDetailScore> _bindingScore;

        public ICommand EditDetailScore { get; set; }
        public bool SwitchToEdit { get => _switchToEdit; set { _switchToEdit = value; OnPropertyChanged(); } }


        private bool _switchToEdit;

        SubjectClass SubjectClassDetail { get; set; }

        public StudentListRightSideBarItemViewModel(SubjectClass subjectClass)
        {
            SubjectClassDetail = subjectClass;

            FirstLoadData();

            BindingScore = new ObservableCollection<StudentDetailScore>();

            EditDetailScore = new RelayCommand<object>((p) => { return true; }, (p) => EditDetailScoreFunction(p));
        }

        public void FirstLoadData()
        {
            try
            {
                var scores = ScoreServices.Instance.LoadScoreStudentInSubjectClass(SubjectClassDetail.Id);
                scores.RemoveAll(score => score.Score == null);
                DetailScores = new ObservableCollection<StudentDetailScore>(scores);
                foreach (var score in DetailScores)
                {
                    score.PropertyChanged += Score_PropertyChanged;
                }
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Không thể tải điểm sinh viên", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Score_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Score")
            {
                AverageScore = ScoreServices.Instance.CalculateAverageScore(BindingScore.ToList());
            }
        }

        public void EditDetailScoreFunction(object p)
        {
            SwitchToEdit = true;
        }

    }
}
