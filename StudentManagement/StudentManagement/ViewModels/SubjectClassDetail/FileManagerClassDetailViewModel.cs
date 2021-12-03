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
using StudentManagement.Services;
using FileInfo = StudentManagement.Objects.FileInfo;
using StudentManagement.Models;

namespace StudentManagement.ViewModels
{
    public class FileManagerClassDetailViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private ObservableCollection<FileInfo> _fileData;

        private ListCollectionView _fileDataGroup;
        public ObservableCollection<FileInfo> FileData { get => _fileData; set => _fileData = value; }
        public ObservableCollection<FileInfo> BindingFileData { get; set; }
        public ListCollectionView FileDataGroup { get => _fileDataGroup; set { _fileDataGroup = value; OnPropertyChanged(); } }

        public ICommand AddFile { get; set; }
        public ICommand AddFolder { get; set; }
        public ICommand CreateFolder { get; set; }
        public ICommand DeleteFile { get; set; }
        public ICommand DeleteFolder { get; set; }
        public ICommand SearchFile { get; set; }
        public ICommand ShowFolderInfo { get; set; }
        public ICommand RenameFolder { get; set; }
        public ICommand SubmitFolderName { get; set; }

        public Guid? FolderEditingId { get => _folderEditingId; set { _folderEditingId = value; OnPropertyChanged(); } }
        private Guid? _folderEditingId;

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

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                SearchFileFunction();
                OnPropertyChanged();
            }
        }
        private string _searchQuery;

        public object SelectedFile { get => _selectedFile; set { _selectedFile = value; OnPropertyChanged(); } }
        private object _selectedFile;

        //
        Guid idSubjectClass = new Guid();
        User publisher = UserServices.Instance.GetUserInfo();

        public FileManagerClassDetailViewModel()
        {
            _errorBaseViewModel = new ErrorBaseViewModel();
            _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;

            FileData = new ObservableCollection<FileInfo>();
            FileData.CollectionChanged += FileData_CollectionChanged;
            BindingFileData = new ObservableCollection<FileInfo>(FileData);
            BindingFileData.CollectionChanged += BindingFileData_CollectionChanged;

            AddFile = new RelayCommand<object>((p) => true, (p) => AddFileFunction(p));
            DeleteFile = new RelayCommand<object>((p) => true, (p) => DeleteFileFunction(p));
            AddFolder = new RelayCommand<object>((p) => true, (p) => AddFolderFunction());
            CreateFolder = new RelayCommand<object>((p) => true, (p) => CreateFolderFunction());
            DeleteFolder = new RelayCommand<object>((p) => true, (p) => DeleteFolderFunction(p));
            SearchFile = new RelayCommand<object>((p) => true, (p) => SearchFileFunction());
            ShowFolderInfo = new RelayCommand<object>((p) => { return true; }, (p) => ShowFolderInfoFunction(p));
            RenameFolder = new RelayCommand<object>((p) => true, (p) => RenameFolderFunction(p));
            SubmitFolderName = new RelayCommand<object>((p) => true, (p) => SubmitFolderNameFunction());

        }

        private void SubmitFolderNameFunction()
        {
            FolderEditingId = null;
        }

        private void RenameFolderFunction(object p)
        {
            if (p is CollectionViewGroup collectionViewGroup)
                FolderEditingId = (Guid?)collectionViewGroup.Name;
        }

        private void ShowFolderInfoFunction(object p)
        {
            MyMessageBox.Show("Oke");
        }

        private void BindingFileData_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            FileDataGroup = new ListCollectionView(BindingFileData);
            FileDataGroup.GroupDescriptions.Add(new PropertyGroupDescription("FolderId"));
        }

        private void SearchFileFunction()
        {
            if (SearchQuery == null)
            {
                SearchQuery = "";
            }
            var searchResult = FileData.Where
            (
                file => 
                    VietnameseStringNormalizer.Instance.Normalize(file.Name)
                    .Contains(VietnameseStringNormalizer.Instance.Normalize(SearchQuery))
                    ||
                    VietnameseStringNormalizer.Instance.Normalize(file.FolderName)
                    .Contains(VietnameseStringNormalizer.Instance.Normalize(SearchQuery))
            );
            BindingFileData.Clear();
            foreach (var item in searchResult)
            {
                BindingFileData.Add(item);
            }
        }

        private async void DeleteFolderFunction(object parameter)
        {
            try
            {
                if (parameter is CollectionViewGroup collectionViewGroup)
                {
                    if (MyMessageBox.Show($"Tất cả {collectionViewGroup.ItemCount} tài liệu sẽ bị xóa." +
                                          $" Bạn có chắc chắn muốn xóa thư mục này không?",
                                          "Xóa thư mục",
                                          MessageBoxButton.OKCancel,
                                          MessageBoxImage.Question) == MessageBoxResult.OK)
                    {
                        var collection = collectionViewGroup.Items.ToList();
                        FileInfo folderToDeleted = new FileInfo((FileInfo)collection[0]);
                        var listOfTasks = new List<Task<int>>();

                        Parallel.ForEach(collection, item =>
                        {
                            FileInfo fileInfo = item as FileInfo;
                            FileInfo fileToBeDeleted = FileData.FirstOrDefault(file => file.Id == fileInfo.Id && file.FolderId == fileInfo.FolderId);
                            FileData.Remove(fileToBeDeleted);
                            listOfTasks.Add(FileServices.Instance.DeleteFileAsync(fileToBeDeleted));
                        });

                        await Task.WhenAll(listOfTasks);
                        await FileServices.Instance.DeleteFolderAsync(folderToDeleted);
                        //foreach (var item in collection)
                        //{
                        //    FileInfo fileInfo = item as FileInfo;
                        //    FileInfo fileToBeDeleted = FileData.FirstOrDefault(file => file.Id == fileInfo.Id && file.FolderId == fileInfo.FolderId);
                        //    FileData.Remove(fileToBeDeleted);
                        //    await FileServices.Instance.DeleteFileAsync(fileToBeDeleted);
                        //}
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

        public async void DeleteFileFunction(object parameter)
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
                            if (item.FolderId != null && (FileData.Where(file => file.FolderId == item.FolderId).Count() == 1))
                            {
                                FileData.Add(new FileInfo(null, "", item.PublisherId, item.Publisher, "", item.UploadTime, 0, item.FolderId, item.FolderName, idSubjectClass));
                            }
                            var fileToBeDeleted = FileData.FirstOrDefault(file => file.Id == item.Id && file.FolderId == item.FolderId);
                            await FileServices.Instance.DeleteFileAsync(fileToBeDeleted);
                            FileData.Remove(fileToBeDeleted);
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

        private async void CreateFolderFunction()
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
                FileInfo newFolder = new FileInfo(
                                         id: null,
                                         name: "",
                                         publisherId: publisher.Id,
                                         publisher: publisher.DisplayName,
                                         content: "",
                                         uploadTime: DateTime.Now,
                                         size: 0,
                                         folderId: Guid.NewGuid(),
                                         folderName: NewFolderName,
                                         idSubjectClass: idSubjectClass);
                FileData.Add(newFolder);
                NewFolderName = null;
                await FileServices.Instance.SaveFolderOfSubjectClassToDatabaseAsync(newFolder);
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
            SearchFileFunction();
        }

        private async void AddFileFunction(object folder)
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

                    var listOfLinks = await UploadFileToCloud(openFileDialog.FileNames, folderId);

                    int index = 0;
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

                            FileInfo newFile = null;

                            if (folder != null)
                            {
                                // Delete pseudo file info used for display folder only
                                var pseudoFileInfo = FileData.FirstOrDefault(fileInfo => fileInfo.FolderId == folderId && fileInfo.Id == null);
                                if (pseudoFileInfo != null)
                                {
                                    FileData.Remove(pseudoFileInfo);
                                }
                                newFile = new FileInfo(
                                              id: Guid.NewGuid(), 
                                              name: name,
                                              publisherId: publisher.Id,
                                              publisher: publisher.DisplayName,
                                              content: listOfLinks[index],
                                              uploadTime: DateTime.Now,
                                              size: fileSize,
                                              folderId: folderId,
                                              folderName: folderName,
                                              idSubjectClass: idSubjectClass);
                            }
                            else
                            {
                                newFile = new FileInfo(
                                              id: Guid.NewGuid(),
                                              name: name,
                                              publisherId: publisher.Id,
                                              publisher: publisher.DisplayName,
                                              content: listOfLinks[index],
                                              uploadTime: DateTime.Now,
                                              size: fileSize,
                                              folderId: null,
                                              folderName: "",
                                              idSubjectClass: idSubjectClass);
                            }

                            if (newFile != null)
                            {
                                FileData.Add(newFile);
                                await FileServices.Instance.SaveFileOfSubjectClassToDatabaseAsync(newFile);
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

        private async Task<string[]> UploadFileToCloud(string[] filePaths, Guid? folderId)
        {
            List<Task<string>> listOfTasks = new List<Task<string>>();

            Parallel.ForEach(filePaths, file =>
            {
                string name = Path.GetFileName(file);
                if (!string.IsNullOrEmpty(name))
                {
                    // File size limit: 10MB
                    long fileSize = GetFileSize(file);
                    if (!IsValidFileSize(fileSize))
                    {
                        return;
                    }

                    // Detect exist file
                    var existFile = FileData.FirstOrDefault(fileInfo => fileInfo.Name == name && fileInfo.FolderId == folderId);
                    if (existFile != null)
                    {
                        return;
                    }

                    listOfTasks.Add(FileUploader.Instance.UploadAsync(file));
                }
            });

            return await Task.WhenAll(listOfTasks);
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
}
