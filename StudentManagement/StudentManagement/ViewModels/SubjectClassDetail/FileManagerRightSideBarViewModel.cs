using StudentManagement.Commands;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class FileManagerRightSideBarViewModel : BaseViewModel
    {
        private object _rightSideBarItemViewModel;

        public object RightSideBarItemViewModel
        {
            get { return _rightSideBarItemViewModel; }
            set
            {
                _rightSideBarItemViewModel = value;
                OnPropertyChanged();
            }
        }

        public FileInfo CurrentFile
        {
            get => _currentFile;
            set
            {
                _currentFile = value;
                RightSideBarItemViewModel = _currentFile == null ? new EmptyStateRightSideBarViewModel() : (object)_currentFile;
            }
        }
        private FileInfo _currentFile;

        public bool IsDeleteFile { get; set; }

        public ICommand DeleteCurrentFile { get; set; }
        public ICommand EditCurrentFile { get; set; }
        public ICommand CancelEditCurrentFile { get; set; }
        public bool IsEditing { get => _isEditing; set { _isEditing = value; OnPropertyChanged(); } }

        private bool _isEditing;

        public string CurrentName { get; set; }

        public FileManagerRightSideBarViewModel()
        {
            DeleteCurrentFile = new RelayCommand<object>((p) => { return true; }, (p) => DeleteCurrentFileFunction());
            EditCurrentFile = new RelayCommand<object>((p) => { return true; }, (p) => EditCurrentFileFunction());
            CancelEditCurrentFile = new RelayCommand<object>((p) => { return true; }, (p) => CancelEditCurrentFileFunction());
            
        }

        private void CancelEditCurrentFileFunction()
        {
            CurrentFile.Name = CurrentName;
            IsEditing = false;
        }

        private async void EditCurrentFileFunction()
        {
            try
            {
                CurrentName = CurrentFile.Name;
                if (IsEditing)
                {
                    await FileServices.Instance.UpdateFileAsync(CurrentFile);
                }
                IsEditing = !IsEditing;
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Vui lòng thử lại sau!", "Lỗi rồi", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void DeleteCurrentFileFunction()
        {
            OnPropertyChanged(nameof(IsDeleteFile));
        }
    }
}
