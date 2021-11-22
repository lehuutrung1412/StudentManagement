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
using System.Windows;
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
        public ICommand DeleteFolder { get; set; }

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
                if (NewFolderName == string.Empty)
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
            DeleteFolder = new RelayCommand<object>((p) => true, (p) => DeleteFolderFunction(p));
        }

        private void DeleteFolderFunction(object parameter)
        {
            try
            {
                if (parameter is CollectionViewGroup collectionViewGroup)
                {
                    if (MyMessageBox.Show($"Tất cả {collectionViewGroup.ItemCount} tài liệu sẽ bị xóa." +
                                          $" Bạn có chắc chắn muốn xóa thư mục này không?",
                                          "Xóa thư mục",
                                          System.Windows.MessageBoxButton.OKCancel,
                                          System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.OK)
                    {
                        foreach (var item in collectionViewGroup.Items.ToList())
                        {
                            FileInfo fileInfo = item as FileInfo;
                            FileData.Remove(FileData.FirstOrDefault(file => file.Id == fileInfo.Id && file.FolderId == fileInfo.FolderId));
                        }
                    }
                }
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Không thể xóa thư mục.",
                                  "Xóa thư mục",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
            }
        }

        private void DeleteFileFunction(object parameter)
        {
            try
            {
                if (parameter != null)
                {
                    var collection = ((IList)parameter).Cast<FileInfo>().ToList();
                    MessageBoxResult userResponse = collection.Count > 1
                        ? MyMessageBox.Show($"Bạn có chắc chắn muốn xóa {collection.Count} tài liệu này không?",
                                          "Xóa tài liệu",
                                          MessageBoxButton.OKCancel,
                                          MessageBoxImage.Question)
                        : MyMessageBox.Show("Bạn có chắc chắn muốn xóa tài liệu này không?",
                                          "Xóa tài liệu",
                                          MessageBoxButton.OKCancel,
                                          MessageBoxImage.Question);
                    if (userResponse == MessageBoxResult.OK)
                    {
                        foreach (var item in collection)
                        {
                            FileData.Remove(FileData.FirstOrDefault(file => file.Id == item.Id && file.FolderId == item.FolderId));
                        }
                    }
                }
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Không thể xóa tài liệu.",
                                  "Xóa tài liệu",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
            }
        }

        private void CreateFolderFunction()
        {
            if (!IsValidString(NewFolderName))
            {
                return;
            }
            try
            {
                var existFile = FileData.FirstOrDefault(fileInfo => fileInfo.FolderName == NewFolderName);
                if (existFile != null)
                {
                    MyMessageBox.Show("Thư mục này đã tồn tại trong lớp học",
                                      "Thêm thư mục",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Error);
                    return;
                }
                IsShowDialog = false;
                FileData.Add(new FileInfo(null, "", "Hữu Trung", DateTime.Now, 0, Guid.NewGuid(), NewFolderName));
                NewFolderName = null;
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Không thể thêm thư mục.",
                                  "Thêm thư mục",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
            }
        }

        private void FileData_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            FileDataGroup = new ListCollectionView(FileData);
            FileDataGroup.GroupDescriptions.Add(new PropertyGroupDescription("FolderId"));
        }

        private void AddFileFunction(object folder)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Multiselect = true,
                    Filter = "All files (*.*)|*.*"
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get data here to prevent error when data was removed
                    Guid? folderId = null;
                    string folderName = "";
                    if (folder != null)
                    {
                        folderId = (Guid)((CollectionViewGroup)folder).Name;
                        folderName = (((CollectionViewGroup)folder).Items[0] as FileInfo).FolderName;
                    }

                    int existFileCount = 0;
                    int notValidFileSizeCount = 0;
                    foreach (string file in openFileDialog.FileNames)
                    {
                        string name = Path.GetFileName(file);
                        if (!string.IsNullOrEmpty(name))
                        {
                            // File size limit: 10MB
                            long fileSize = GetFileSize(file);
                            if (!IsValidFileSize(fileSize))
                            {
                                notValidFileSizeCount++;
                                continue;
                            }

                            // Detect exist file
                            var existFile = FileData.FirstOrDefault(fileInfo => fileInfo.Name == name && fileInfo.FolderId == folderId);
                            if (existFile != null)
                            {
                                existFileCount++;
                                continue;
                            }

                            if (folder != null)
                            {
                                // Delete pseudo file info used for display folder only
                                var pseudoFileInfo = FileData.FirstOrDefault(fileInfo => fileInfo.FolderId == folderId && fileInfo.Id == null);
                                if (pseudoFileInfo != null)
                                {
                                    FileData.Remove(pseudoFileInfo);
                                }

                                FileData.Add(new FileInfo(Guid.NewGuid(), name, "Lê Hữu Trung", DateTime.Now, fileSize, folderId, folderName));
                            }
                            else
                            {
                                FileData.Add(new FileInfo(Guid.NewGuid(), name, "Lê Hữu Trung", DateTime.Now, fileSize, null, ""));
                            }
                        }
                    }

                    if (existFileCount > 0)
                    {
                        MyMessageBox.Show($"Có {existFileCount} tài liệu đã tồn tại.",
                                          "Thêm tài liệu",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
                    }

                    if (notValidFileSizeCount > 0)
                    {
                        MyMessageBox.Show($"Không thể tải lên {notValidFileSizeCount} tài liệu có dung lượng > 10MB.",
                                          "Thêm tài liệu",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Không thể thêm tài liệu.",
                                  "Thêm tài liệu",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
            }

        }

        private long GetFileSize(string filePath)
        {
            if (File.Exists(filePath))
            {
                return new System.IO.FileInfo(filePath).Length;
            }
            return 0;
        }

        private bool IsValidFileSize(long fileSizeInBytes)
        {
            // File limit: 10MB
            long FILE_SIZE_LIMIT = 10485760;
            return fileSizeInBytes <= FILE_SIZE_LIMIT;
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
            return !string.IsNullOrWhiteSpace(propertyName);
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
        private long _size;

        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public string Publisher { get => _publisher; set { _publisher = value; OnPropertyChanged(); } }
        public DateTime UploadTime { get => _uploadTime; set { _uploadTime = value; OnPropertyChanged(); } }
        public Guid? FolderId { get => _folderId; set { _folderId = value; OnPropertyChanged(); } }
        public string FolderName { get => _folderName; set { _folderName = value; OnPropertyChanged(); } }

        public Guid? Id { get => _id; set { _id = value; OnPropertyChanged(); } }

        public long Size { get => _size; set { _size = value; OnPropertyChanged(); } }

        public FileInfo(Guid? id, string name, string publisher, DateTime uploadTime, long size, Guid? folderId, string folderName)
        {
            Id = id;
            Name = name;
            Publisher = publisher;
            UploadTime = uploadTime;
            Size = size;
            FolderId = folderId;
            FolderName = folderName;
        }
    }
}
