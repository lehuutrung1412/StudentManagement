using StudentManagement.Commands;
using StudentManagement.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class FileManagerClassDetailViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private ObservableCollection<FileInfo> _fileData;

        private ListCollectionView _fileDataGroup;
        public ObservableCollection<FileInfo> FileData { get => _fileData; set => _fileData = value; }
        public ListCollectionView FileDataGroup { get => _fileDataGroup; set { _fileDataGroup = value; OnPropertyChanged(); } }

        public ICommand AddFile { get; set; }
        public ICommand AddFolder { get; set; }
        public ICommand CreateFolder { get; set; }
        public ICommand DeleteFile { get; set; }

        public bool IsShowDialog { get => _isShowDialog; set { _isShowDialog = value; OnPropertyChanged(); } }
        private bool _isShowDialog;
        public string NewFolderName
        {
            get => _newFolderName;
            set
            {
                _newFolderName = value;

                // Validation
                _errorBaseViewModel.ClearErrors();
                if (!IsValidString(NewFolderName))
                {
                    _errorBaseViewModel.AddError(nameof(NewFolderName), "Vui lòng nhập tên thư mục!");
                }

                OnPropertyChanged();
            }
        }
        private string _newFolderName;

        public FileManagerClassDetailViewModel()
        {
            _errorBaseViewModel = new ErrorBaseViewModel();
            _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;

            FileData = new ObservableCollection<FileInfo>();
            FileData.CollectionChanged += FileData_CollectionChanged;

            AddFile = new RelayCommand<object>((p) => true, (p) => AddFileFunction(p));
            DeleteFile = new RelayCommand<object>((p) => true, (p) => DeleteFileFunction(p));
            AddFolder = new RelayCommand<object>((p) => true, (p) => AddFolderFunction());
            CreateFolder = new RelayCommand<object>((p) => true, (p) => CreateFolderFunction());
        }

        private void DeleteFileFunction(object p)
        {
            MyMessageBox.Show("Deleted");
        }

        private void CreateFolderFunction()
        {
            if (!IsValidString(NewFolderName))
            {
                return;
            }
            IsShowDialog = false;
            FileData.Add(new FileInfo(null, "", "Hữu Trung", DateTime.Now, Guid.NewGuid(), NewFolderName));
        }

        private void FileData_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            FileDataGroup = new ListCollectionView(FileData);
            FileDataGroup.GroupDescriptions.Add(new PropertyGroupDescription("FolderId"));
        }

        private void AddFileFunction(object folder)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    string name = Path.GetFileName(file);
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (folder != null)
                        {
                            Guid folderId = (Guid)((CollectionViewGroup)folder).Name;
                            string folderName = (((CollectionViewGroup)folder).Items[0] as FileInfo).FolderName;

                            // Delete pseudo file info used for display folder only
                            var pseudoFileInfo = FileData.FirstOrDefault(fileInfo => fileInfo.FolderId == folderId && fileInfo.Id == null);
                            if (pseudoFileInfo != null)
                            {
                                FileData.Remove(pseudoFileInfo);
                            }

                            FileData.Add(new FileInfo(Guid.NewGuid(), name, "Lê Hữu Trung", DateTime.Now, folderId, folderName));
                        }
                        else
                        {
                            FileData.Add(new FileInfo(Guid.NewGuid(), name, "Lê Hữu Trung", DateTime.Now, null, ""));
                        }
                    }
                }
            }
        }
        
        private void AddFolderFunction()
        {
            IsShowDialog = true;
        }

        #region Validation

        private readonly ErrorBaseViewModel _errorBaseViewModel;
        public bool CanCreate => !HasErrors;
        public bool HasErrors => _errorBaseViewModel.HasErrors;

        private bool IsValidString(string propertyName)
        {
            return !string.IsNullOrEmpty(propertyName) && !string.IsNullOrWhiteSpace(propertyName);
        }

        private void ErrorBaseViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanCreate));
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorBaseViewModel.GetErrors(propertyName);
        }
        #endregion
    }

    public class FileInfo : BaseViewModel
    {
        private Guid? _id;
        private string _name;
        private string _publisher;
        private DateTime _uploadTime;
        private Guid? _folderId;
        private string _folderName;

        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public string Publisher { get => _publisher; set { _publisher = value; OnPropertyChanged(); } }
        public DateTime UploadTime { get => _uploadTime; set { _uploadTime = value; OnPropertyChanged(); } }
        public Guid? FolderId { get => _folderId; set { _folderId = value; OnPropertyChanged(); } }
        public string FolderName { get => _folderName; set { _folderName = value; OnPropertyChanged(); } }

        public Guid? Id { get => _id; set { _id = value; OnPropertyChanged(); } }

        public FileInfo(Guid? id, string name, string publisher, DateTime uploadTime, Guid? folderId, string folderName)
        {
            Id = id;
            Name = name;
            Publisher = publisher;
            UploadTime = uploadTime;
            FolderId = folderId;
            FolderName = folderName;
        }
    }
}
