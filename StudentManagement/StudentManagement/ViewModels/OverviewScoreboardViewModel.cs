using StudentManagement.Commands;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class OverviewScoreboardViewModel
    {
        private string _gpaColor;
        public string GPAColor
        {
            get => _gpaColor;
            set => _gpaColor = value;
        }

        private string _trainingColor;
        public string TrainingColor
        {
            get => _trainingColor;
            set => _trainingColor = value;
        }

        private string _creditColor;
        public string CreditColor
        {
            get => _creditColor;
            set => _creditColor = value;
        }

        private string _percentTraining;
        public string PercentTraining
        {
            get => _percentTraining;
            set => _percentTraining = value;
        }

        private string _percentGPA;

        public string PercentGPA
        {
            get => _percentGPA;
            set => _percentGPA = value;
        }

        private string _percentCredit;
        public string PercentCredit
        {
            get => _percentCredit;
            set => _percentCredit = value;
        }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set => _displayName = value;
        }

        private double _gpa;
        public double GPA
        {
            get => _gpa;
            set => _gpa = value;
        }

        public double _training;
        public double Training
        {
            get => _training;
            set => _training = value;
        }

        private double _credit;
        public double Credit
        {
            get => _credit;
            set => _credit = value;
        }
        private void FillColor(ref string Color, int Target)
        {
            if (Target >= 90)
            {
                Color = "#008613";
            }
            else if (Target >= 80)
            {
                Color = "#008600";
            }
            else if (Target >= 65)
            {
                Color = "#ffe5ff00";
            }
            else if (Target >= 50)
            {
                Color = "#d44100";
            }
            else
                Color = "#d50000";
        }

        int CalculationPercent(double Current, double Total)
        {
            return (int) ((1.0 * Current / Total) * 100);
        }

        public OverviewScoreboardViewModel(double GPA, double Training, int Credit, string NameUser)
        {
            FillColor(ref _gpaColor, CalculationPercent(GPA, 10));
            FillColor(ref _trainingColor, CalculationPercent(Training, 100));
            FillColor(ref _creditColor, CalculationPercent(Credit, 130));

            GPAColor = _gpaColor;
            TrainingColor = _trainingColor;
            CreditColor = _creditColor;

            PercentGPA = Convert.ToString(CalculationPercent(GPA, 10));
            PercentCredit = Convert.ToString(CalculationPercent(Credit, 130));
            PercentTraining = Convert.ToString(CalculationPercent(Training, 100));

            DisplayName = NameUser;
            this.GPA = GPA;
            this.Credit = Credit;
            this.Training = Training;


        }

    }
}
