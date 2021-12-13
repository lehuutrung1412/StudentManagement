using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public class StudentDetailScore : BaseViewModel
    {
        private Guid _id;
        private Guid? _idStudent;
        private Guid? _idComponentScore;
        private Guid? _idSubjectClass;
        private double? _score;
        private string _displayName;
        private double? _percent;

        public Guid Id { get => _id; set => _id = value; }
        public Guid? IdStudent { get => _idStudent; set => _idStudent = value; }
        public Guid? IdComponentScore { get => _idComponentScore; set => _idComponentScore = value; }
        public Guid? IdSubjectClass { get => _idSubjectClass; set => _idSubjectClass = value; }
        public double? Score
        {
            get => _score;
            set
            {
                _score = value;
                OnPropertyChanged();
            }
        }
        public string DisplayName { get => _displayName; set => _displayName = value; }
        public double? Percent { get => _percent; set => _percent = value; }
    }
}
