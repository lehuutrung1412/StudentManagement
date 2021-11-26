using StudentManagement.Models;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Faculty> TrainingFormsOfFacultyList = new ObservableCollection<Faculty>();


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

            var tempTrainingForm = TrainingFormServices.Instance.FindTrainingFormByTrainingFormId(Id);

            TrainingFormsOfFacultyList = new ObservableCollection<Faculty>(Faculty_TrainingFormServices.Instance.LoadFacultyByTrainingForm(tempTrainingForm));
        }

        public string DisplayName { get => _displayName; set => _displayName = value; }
        public int NumberOfFaculties { get => _numberOfFaculties; set => _numberOfFaculties = value; }
        public int NumberOfStudents { get => _numberOfStudents; set => _numberOfStudents = value; }
        public bool IsDeleted { get => _isDeleted; set => _isDeleted = value; }
        public Guid Id { get => _id; set => _id = value; }
    }
}
