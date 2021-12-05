using StudentManagement.Models;
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
        // define validation rule
        private readonly ErrorBaseViewModel _errorBaseViewModel;

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
        private User _teacher;
        private Subject _subjectOfClass;
        private SubjectClass subjectClass;
        private string _giaoVien;
        private string _maMon;
        private string _tenMon;

        public SubjectCard()
        {
            _errorBaseViewModel = new ErrorBaseViewModel();
            _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;
        }
        public SubjectCard(int siSo, string giaoVien, string maMon, string tenMon) : this()
        {
            SiSo = siSo;
            GiaoVien = giaoVien;
            MaMon = maMon;
            TenMon = tenMon;
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

        public string MaMon
        {
            get => _maMon;
            set
            {
                _maMon = value;
                // Validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(_maMon))
                {
                    _errorBaseViewModel.AddError(nameof(MaMon), "Vui lòng nhập mã môn!");
                }

            }
        }

        public string TenMon
        {
            get => _tenMon;
            set
            {
                _tenMon = value;
                // Validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValid(_tenMon))
                {
                    _errorBaseViewModel.AddError(nameof(TenMon), "Vui lòng nhập tên môn học!");
                }

            }
        }
    }
}
