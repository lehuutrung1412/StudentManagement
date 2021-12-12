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

                    AverageScore = BindingScore.Average(score => score.Score);
                }

                OnPropertyChanged();
            }
        }

        public double? AverageScore { get => _averageScore; set { _averageScore = value; OnPropertyChanged(); } }
        private double? _averageScore;

        public ObservableCollection<StudentDetailScore> DetailScores { get => _detailScores; set { _detailScores = value; OnPropertyChanged(); } }
        private ObservableCollection<StudentDetailScore> _detailScores;

        public ObservableCollection<StudentDetailScore> BindingScore { get; set; }

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

        private void FirstLoadData()
        {
            try
            {
                var scores = ScoreServices.Instance.LoadScoreStudentInSubjectClass(SubjectClassDetail.Id);
                DetailScores = new ObservableCollection<StudentDetailScore>(scores);
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Không thể tải điểm sinh viên", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void EditDetailScoreFunction(object p)
        {
            SwitchToEdit = true;
        }

    }
}
