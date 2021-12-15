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
    public class FacultyCard : BaseObjectWithBaseViewModel, IBaseCard, INotifyDataErrorInfo
    {
        #region validation
        // define validation rule
        private readonly ErrorBaseViewModel _errorBaseViewModel = new ErrorBaseViewModel();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors
        {
            get => _errorBaseViewModel.HasErrors;
            set { }
        }

        private bool IsValid(string propertyName)
        {
            return !string.IsNullOrEmpty(propertyName) && !string.IsNullOrWhiteSpace(propertyName);
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorBaseViewModel.GetErrors(propertyName);
        }

        private void ErrorBaseViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }
        #endregion


        #region property
        private string _displayName;
        private DateTime _foundationDay;
        private int _numberOfStudents;
        private string _cacHeDaoTao;
        private bool _isDeleted;
        private Guid _id;
        private ObservableCollection<TrainingForm> _trainingFormsOfFacultyList = new ObservableCollection<TrainingForm>();
        private ObservableCollection<TrainingForm> _remainingTrainingFormsOfFacultyList = new ObservableCollection<TrainingForm>();

        public string DisplayName
        {
            get => _displayName; set
            {
                _displayName = value;

                // Validation
                _errorBaseViewModel.ClearErrors();

                if (!IsValid(DisplayName))
                {
                    _errorBaseViewModel.AddError(nameof(DisplayName), "Vui lòng nhập tên khoa!");
                }
            }
        }
        public DateTime FoundationDay { get => _foundationDay; set => _foundationDay = value; }
        public int NumberOfStudents { get => _numberOfStudents; set => _numberOfStudents = value; }
        public string CacHeDaoTao
        {
            get => _cacHeDaoTao; set
            {
                _cacHeDaoTao = value;
                OnPropertyChanged();
            }
        }
        public Guid Id { get => _id; set => _id = value; }
        public bool IsDeleted { get => _isDeleted; set => _isDeleted = value; }
        public ObservableCollection<TrainingForm> TrainingFormsOfFacultyList { get => _trainingFormsOfFacultyList; set { _trainingFormsOfFacultyList = value; OnPropertyChanged(); } }
        public ObservableCollection<TrainingForm> RemainingTrainingFormsOfFacultyList { get => _remainingTrainingFormsOfFacultyList; set { _remainingTrainingFormsOfFacultyList = value; OnPropertyChanged(); } }

        public List<Faculty_TrainingForm> Faculty_TrainingFormList { get; set; }
        #endregion

        public FacultyCard()
        {
            Id = Guid.NewGuid();
            InitTrainingFormOfFacultyList();
            _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;

            CacHeDaoTao = "";
        }
        public FacultyCard(Guid id, string displayName, DateTime foundationDay, int numberOfStudents) : base()
        {
            Id = id;
            DisplayName = displayName;
            FoundationDay = foundationDay;
            NumberOfStudents = numberOfStudents;
            InitTrainingFormOfFacultyList();

        }


        #region methods
        public bool InitTrainingFormOfFacultyList()
        {
            try
            {
                var tempFaculty = Faculty_TrainingFormServices.Instance.LoadTrainingFormByFaculty(FacultyServices.Instance.FindFacultyByFacultyId(Id));

                TrainingFormsOfFacultyList = new ObservableCollection<TrainingForm>(tempFaculty);

                var temp2 = TrainingFormServices.Instance.LoadTrainingFormList().Where(el => el.IsDeleted != true).ToList();

                temp2.RemoveAll(el => tempFaculty.Contains(el));


                RemainingTrainingFormsOfFacultyList = new ObservableCollection<TrainingForm>(temp2);

                Faculty faculty = FacultyServices.Instance.FindFacultyByFacultyId(Id);

                Faculty_TrainingFormList = new List<Faculty_TrainingForm>();

                if (faculty != null)
                {
                    foreach (var relation in faculty.Faculty_TrainingForm)
                    {
                        Faculty_TrainingFormList.Add(relation);
                    }
                }

                CacHeDaoTao = string.Join(", ", TrainingFormsOfFacultyList.Select(el => el.DisplayName).ToList());

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool AddToTrainingFormOfFacultyList(TrainingForm trainingForm)
        {
            try
            {
                TrainingFormsOfFacultyList.Add(trainingForm);
                RemainingTrainingFormsOfFacultyList.Remove(trainingForm);
                Faculty faculty = FacultyServices.Instance.FindFacultyByFacultyId(Id);

                Faculty_TrainingFormList.Add(new Faculty_TrainingForm()
                {
                    Id = Guid.NewGuid(),
                    IdFaculty = Id,
                    IdTrainingForm = trainingForm.Id,
                });

                CacHeDaoTao = string.Join(", ", TrainingFormsOfFacultyList.Select(el => el.DisplayName).ToList());
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool RemoveFromTrainingFormOfFacultyList(TrainingForm trainingForm)
        {
            try
            {
                RemainingTrainingFormsOfFacultyList.Add(trainingForm);
                TrainingFormsOfFacultyList.Remove(trainingForm);
                Faculty_TrainingForm removedRelation = Faculty_TrainingFormList.Where(el => el.IdTrainingForm == trainingForm.Id).FirstOrDefault();
                Faculty_TrainingFormList.Remove(removedRelation);
                DataProvider.Instance.Database.Faculty_TrainingForm.Remove(removedRelation);

                CacHeDaoTao = string.Join(", ", TrainingFormsOfFacultyList.Select(el => el.DisplayName).ToList());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveTrainingFormOfFacultyListToDatabase()
        {
            try
            {
                Faculty faculty = FacultyServices.Instance.FindFacultyByFacultyId(Id);

                faculty?.Faculty_TrainingForm?.Clear();

                foreach (var relation in Faculty_TrainingFormList)
                {
                    faculty.Faculty_TrainingForm.Add(relation);
                }

                DataProvider.Instance.Database.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
