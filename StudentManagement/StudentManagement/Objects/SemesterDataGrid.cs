using StudentManagement.Models;
using StudentManagement.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Services;
using System.Collections.ObjectModel;

namespace StudentManagement.Objects
{
    public class SemesterDataGrid : BaseObjectWithBaseViewModel, IBaseCard
    {
        private double _gpa;
        private int _totalTrainingScore;
        private ObservableCollection<ScoreDataGrid> _currentScore;
        private ObservableCollection<TrainingScoreDataGrid> _currentTrainingScore;
        private string _displayName;
        private Guid _idSemester;
        private string _batch;
       
        public string Batch
        {
            get => _batch;
            set => _batch = value;
        }
        public Guid IdSemester
        {
            get => _idSemester;
            set => _idSemester = value;
        }
        public string DisplayName
        {
            get => _displayName;
            set => _displayName = value;
        }
        public double GPA
        {
            get => _gpa;
            set => _gpa = value;
        }

        public int TotalTrainingScore
        {
            get => _totalTrainingScore;
            set => _totalTrainingScore = value;
        }

        public ObservableCollection<ScoreDataGrid> CurrentScore
        {
            get => _currentScore;
            set => _currentScore = value;
        }

        public ObservableCollection<TrainingScoreDataGrid> CurrentTrainingScore
        {
            get => _currentTrainingScore;
            set => _currentTrainingScore = value;
        }

        public SemesterDataGrid(Guid IdSemester, string DisplayName, string Batch, double GPA, int Total, ObservableCollection<ScoreDataGrid> CurrentScore, ObservableCollection<TrainingScoreDataGrid> CurrentTrainingScore)
        {
            this.IdSemester = IdSemester;
            this.Batch = Batch;
            this.DisplayName = DisplayName;
            this.GPA = GPA;
            this.TotalTrainingScore = Total;
            this.CurrentScore = CurrentScore;
            this.CurrentTrainingScore = CurrentTrainingScore;
        }

    }
}
