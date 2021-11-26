using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public class TrainingFormCard : BaseObjectWithBaseViewModel, IBaseCard
    {
        private string _displayName;
        private int _numberOfFaculties;
        private int _numberOfStudents;
        private bool _isDeleted;
        private Guid _id;

        public TrainingFormCard()
        {
            Id = Guid.NewGuid();
        }

        public TrainingFormCard(Guid id, string displayName, int numberOfFaculties, int numberOfStudents)
        {
            Id = id;
            DisplayName = displayName;
            NumberOfFaculties = numberOfFaculties;
            NumberOfStudents = numberOfStudents;
        }

        public string DisplayName { get => _displayName; set => _displayName = value; }
        public int NumberOfFaculties { get => _numberOfFaculties; set => _numberOfFaculties = value; }
        public int NumberOfStudents { get => _numberOfStudents; set => _numberOfStudents = value; }
        public bool IsDeleted { get => _isDeleted; set => _isDeleted = value; }
        public Guid Id { get => _id; set => _id = value; }
    }
}
