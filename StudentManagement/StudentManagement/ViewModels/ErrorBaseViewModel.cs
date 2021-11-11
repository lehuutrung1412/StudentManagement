using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StudentManagement.ViewModels
{
    public class ErrorBaseViewModel : INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();
        public bool HasErrors => _propertyErrors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _propertyErrors.TryGetValue(propertyName, out List<string> result) ? result : null;
        }

        public void AddError([CallerMemberName] string propertyName = "", string errorMessage = "Đây là trường bắt buộc!")
        {
            if (!_propertyErrors.ContainsKey(propertyName))
            {
                _propertyErrors.Add(propertyName, new List<string>());
            }

            _propertyErrors[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }

        public void ClearErrors([CallerMemberName] string propertyName = "")
        {
            if (_propertyErrors.Remove(propertyName))
            {
                OnErrorsChanged(propertyName);
            }
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
