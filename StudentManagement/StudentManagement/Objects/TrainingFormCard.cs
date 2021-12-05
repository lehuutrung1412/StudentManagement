using StudentManagement.Models;
using StudentManagement.Services;
using StudentManagement.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public class TrainingFormCard : BaseObjectWithBaseViewModel, IBaseCard, INotifyDataErrorInfo
    {
        #region validations
        // define validation rule
        private readonly ErrorBaseViewModel _errorBaseViewModel = new ErrorBaseViewModel();

        public bool HasErrors
        {
            get => _errorBaseViewModel.HasErrors;
            set { }
        }

        private bool IsValid(string propertyName)
        {
            return !string.IsNullOrEmpty(propertyName) && !string.IsNullOrWhiteSpace(propertyName);
        }

        private void ErrorBaseViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            //OnPropertyChanged(nameof(CanLogin));
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorBaseViewModel.GetErrors(propertyName);
        }
        #endregion

        #region properties
        private string _displayName;
        private int _numberOfFaculties;
        private int _numberOfStudents;
        private bool _isDeleted;
        private Guid _id;
        public ObservableCollection<Faculty> TrainingFormsOfFacultyList = new ObservableCollection<Faculty>();

        public string DisplayName
        {
            get => _displayName; set
            {
                _displayName = value;

                // Validation
                _errorBaseViewModel.ClearErrors();

                if (!IsValid(DisplayName))
                {
                    _errorBaseViewModel.AddError(nameof(DisplayName), "Vui lòng nhập tên hệ đào tạo!");
                }
            }
        }
        public int NumberOfFaculties { get => _numberOfFaculties; set => _numberOfFaculties = value; }
        public int NumberOfStudents { get => _numberOfStudents; set => _numberOfStudents = value; }
        public bool IsDeleted { get => _isDeleted; set => _isDeleted = value; }
        public Guid Id { get => _id; set => _id = value; }
        #endregion


        public TrainingFormCard()
        {
            Id = Guid.NewGuid();
            _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;
        }

        public TrainingFormCard(Guid id, string displayName, int numberOfFaculties, int numberOfStudents) : base()
        {
            Id = id;
            DisplayName = displayName;
            NumberOfFaculties = numberOfFaculties;
            NumberOfStudents = numberOfStudents;

            var tempTrainingForm = TrainingFormServices.Instance.FindTrainingFormByTrainingFormId(Id);

            TrainingFormsOfFacultyList = new ObservableCollection<Faculty>(Faculty_TrainingFormServices.Instance.LoadFacultyByTrainingForm(tempTrainingForm));
        }

    }
}
