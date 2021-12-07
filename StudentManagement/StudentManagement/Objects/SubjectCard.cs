using StudentManagement.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public class SubjectCard : BaseObjectWithBaseViewModel, IBaseCard, INotifyDataErrorInfo
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
        private Guid _id;
        private string _displayName;
        private int? _credit;
        private string _code;
        private string _describe;
        private bool _isDeleted;

        public Guid Id { get => _id; set => _id = value; }
        public string DisplayName
        {
            get => _displayName; set
            {
                _displayName = value;

                // Validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(DisplayName))
                {
                    _errorBaseViewModel.AddError(nameof(DisplayName), "Vui lòng tên môn học!");
                }
            }
        }
        public int? Credit
        {
            get => _credit; set
            {
                _credit = value;

                // Validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(Credit.ToString()))
                {
                    _errorBaseViewModel.AddError(nameof(Credit), "Vui lòng nhập số tín chỉ!");
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

        public string Describe { get => _describe; set => _describe = value; }
        public bool IsDeleted { get => _isDeleted; set => _isDeleted = value; }
        #endregion

        public SubjectCard()
        {
            Id = Guid.NewGuid();
            _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;
        }

        public SubjectCard(Guid id, string displayName, int? credit, string code, string describe = "Chưa có mô tả", bool isDeleted = false)
        {
            Id = id;
            DisplayName = displayName;
            Credit = credit;
            Code = code;
            Describe = describe;
            IsDeleted = isDeleted;
        }
    }
}
