using StudentManagement.Commands;
using StudentManagement.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class FileManagerClassDetailViewModel : BaseViewModel
    {
        private ObservableCollection<FileInfo> _fileData;

        private ListCollectionView _fileDataGroup;
        public ObservableCollection<FileInfo> FileData { get => _fileData; set => _fileData = value; }
        public ListCollectionView FileDataGroup { get => _fileDataGroup; set { _fileDataGroup = value; OnPropertyChanged(); } }

        public ICommand AddFile { get; set; }

        public FileManagerClassDetailViewModel()
        {
            //FileData = new ObservableCollection<FileInfo>()
            //{
            //    new FileInfo("Slide_chuong_1.pptx", "Lê Hữu Trung", DateTime.Now, null, ""),
            //    new FileInfo("Slide_chuong_2.ppt", "Hữu Trung", DateTime.Now, new Guid(), "Folder nì"),
            //    new FileInfo("Slide_chuong_4.doc", "Lê Hữu Trung", new DateTime(2021, 10, 5), new Guid(), "Folder nì"),
            //    new FileInfo("Slide_chuong_6.docx", "Trung", DateTime.Now, null, ""),
            //    new FileInfo("Slide_chuong_3.pdf", "Lê Hữu Trung", DateTime.Now, new Guid(), "Folder nì"),
            //    new FileInfo("Slide_chuong_5.zip", "Lê Hữu Trung", DateTime.Now, null, ""),
            //    new FileInfo("Slide_chuong_5.png", "Lê Hữu Trung", DateTime.Now, null, ""),
            //    new FileInfo("Slide_chuong_5.jpg", "Lê Hữu Trung", DateTime.Now, null, ""),
            //    new FileInfo("Slide_chuong_5.xls", "Lê Hữu Trung", DateTime.Now, null, ""),
            //    new FileInfo("Slide_chuong_5.xlsx", "Lê Hữu Trung", DateTime.Now, null, ""),
            //    new FileInfo("Slide_chuong_5.txt", "Lê Hữu Trung", DateTime.Now, null, ""),
            //    new FileInfo("Slide_chuong_5", "Lê Hữu Trung", DateTime.Now, null, "")
            //};

            //FileDataGroup = new ListCollectionView(FileData);
            //FileDataGroup.GroupDescriptions.Add(new PropertyGroupDescription("FolderId"));
            FileData = new ObservableCollection<FileInfo>();
            FileData.CollectionChanged += FileData_CollectionChanged;

            FileData.Add(new FileInfo("Slide_chuong_2.ppt", "Hữu Trung", DateTime.Now, new Guid(), "Folder nì"));
            FileData.Add(new FileInfo("Slide_chuong_4.doc", "Lê Hữu Trung", new DateTime(2021, 10, 5), new Guid(), "Folder nì"));

            AddFile = new RelayCommand<object>((p) => true, (p) => AddFileFunction());
        }

        private void FileData_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            FileDataGroup = new ListCollectionView(FileData);
            FileDataGroup.GroupDescriptions.Add(new PropertyGroupDescription("FolderId"));
        }

        private void AddFileFunction()
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
                        FileData.Add(new FileInfo(name, "Lê Hữu Trung", DateTime.Now, null, ""));
                    }
                }
            }
        }
    }

    public class FileInfo : BaseViewModel
    {
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

        public FileInfo(string name, string publisher, DateTime uploadTime, Guid? folderId, string folderName)
        {
            Name = name;
            Publisher = publisher;
            UploadTime = uploadTime;
            FolderId = folderId;
            FolderName = folderName;
        }
    }
}
