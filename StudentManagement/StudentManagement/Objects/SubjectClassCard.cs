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
    public class SubjectClassCard : BaseObjectWithBaseViewModel, IBaseCard, INotifyDataErrorInfo
    {
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

        private int _numberOfStudents;
        private Guid _id;
        private User _teacher = null;
        private Subject _subjectOfClass = null;
        private SubjectClass _subjectClass = null;
        private string _code;
        private string _giaoVien;

        private ObservableCollection<Subject> _subjectList = new ObservableCollection<Subject>();

        public SubjectClassCard()
        {
            Id = Guid.NewGuid();
            _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;
            InitModelNavigationItem();
        }

        public SubjectClassCard(Guid id, Subject subjectOfClass, SubjectClass subjectClass, string code, string giaoVien, int numberOfStudents) : this()
        {
            _id = id;
            _numberOfStudents = numberOfStudents;
            _subjectOfClass = subjectOfClass;
            _subjectClass = subjectClass;
            _code = code;
            _giaoVien = giaoVien;
            InitModelNavigationItem();
        }

        public void InitModelNavigationItem()
        {
            var subjectList = SubjectServices.Instance.LoadSubjectList().ToList();
            SubjectList = new ObservableCollection<Subject>(subjectList);
        }

        public int SiSo
        {
            get => _numberOfStudents;
            set
            {
                _numberOfStudents = value;

                // Validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(SiSo.ToString()))
                {
                    _errorBaseViewModel.AddError(nameof(SiSo), "Vui lòng nhập sĩ số!");
                }
            }
        }

        public string GiaoVien
        {
            get => _giaoVien;
            set
            {
                _giaoVien = value;
                // Validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(_giaoVien))
                {
                    _errorBaseViewModel.AddError(nameof(GiaoVien), "Vui lòng chọn giáo viên!");
                }
            }
        }

        public string Code
        {
            get => _code; set
            {
                _code = value;

                // Validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(Code))
                {
                    _errorBaseViewModel.AddError(nameof(Code), "Vui lòng nhập mã môn học!");
                }
            }
        }

        public Guid Id { get => _id; set => _id = value; }
        public User Teacher { get => _teacher; set => _teacher = value; }
        public Subject SubjectOfClass
        {
            get => _subjectOfClass; set
            {
                _subjectOfClass = value;
                OnPropertyChanged();
            }
        }
        public SubjectClass SubjectClass
        {
            get => _subjectClass; set
            {
                _subjectClass = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Subject> SubjectList { get => _subjectList; set => _subjectList = value; }
    }
}
