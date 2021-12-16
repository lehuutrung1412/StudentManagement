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

namespace StudentManagement.Objects
{
    public class DetailScoreItem : BaseObjectWithBaseViewModel, IBaseCard
    {
        private string _percent;
        private string _displayName;
        private string _score;
        public string Percent
        {
            get => _percent;
            set => _percent = value;
        }

        public string DisplayName
        {
            get => _displayName;
            set => _displayName = value;
        }

        public string Score
        {
            get => _score;
            set => _score = value;
        }

        public DetailScoreItem(string DisplayName, string Percent, string Score)
        {
            this.DisplayName = DisplayName;
            this.Percent = Percent;
            this.Score = Score;
        }

    }
}
