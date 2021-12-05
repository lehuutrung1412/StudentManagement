using StudentManagement.Commands;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using FileInfo = StudentManagement.Objects.FileInfo;

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
        public ICommand DownloadCurrentFile { get; set; }
        public bool IsEditing { get => _isEditing; set { _isEditing = value; OnPropertyChanged(); } }

        private bool _isEditing;

        public string CurrentName { get; set; }

        public FileManagerRightSideBarViewModel()
        {
            DeleteCurrentFile = new RelayCommand<object>((p) => { return true; }, (p) => DeleteCurrentFileFunction());
            EditCurrentFile = new RelayCommand<object>((p) => { return true; }, (p) => EditCurrentFileFunction());
            CancelEditCurrentFile = new RelayCommand<object>((p) => { return true; }, (p) => CancelEditCurrentFileFunction());
            DownloadCurrentFile = new RelayCommand<object>((p) => { return true; }, (p) => DownloadCurrentFileFunction());
        }

        private async void DownloadCurrentFileFunction()
        {
            var dialog = new SaveFileDialog();
            var ext = Path.GetExtension(CurrentFile.Name);
            dialog.Filter = $"File (*{ext})|*{ext}";
            dialog.FileName = CurrentFile.Name;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await FileUploader.Instance.DownloadFileAsync(CurrentFile.Content, dialog.FileName);
                }
                catch (Exception)
                {
                    MyMessageBox.Show("Server hiện đang bận! Vui lòng thử lại sau!", "Không thể tải tài liệu", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                try
                {
                    Process.Start("explorer.exe", Path.GetDirectoryName(dialog.FileName));
                }
                catch (Exception)
                {
                    MyMessageBox.Show("Đường dẫn không tồn tại!", "Không thể mở tài liệu", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                
            }
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
