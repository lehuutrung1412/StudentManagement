using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public class FileInfo : BaseViewModel
    {
        private Guid? _id;
        private string _name;
        private string _publisher;
        private DateTime? _uploadTime;
        private Guid? _folderId;
        private string _folderName;
        private long? _size;
        private Guid _publisherId;
        private Guid _idSubjectClass;
        private string _content;

        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public string Publisher { get => _publisher; set { _publisher = value; OnPropertyChanged(); } }
        public DateTime? UploadTime { get => _uploadTime; set { _uploadTime = value; OnPropertyChanged(); } }
        public Guid? FolderId { get => _folderId; set { _folderId = value; OnPropertyChanged(); } }
        public string FolderName { get => _folderName; set { _folderName = value; OnPropertyChanged(); } }

        public Guid? Id { get => _id; set { _id = value; OnPropertyChanged(); } }

        public long? Size { get => _size; set { _size = value; OnPropertyChanged(); } }

        public Guid PublisherId { get => _publisherId; set { _publisherId = value; OnPropertyChanged(); } }

        public Guid IdSubjectClass { get => _idSubjectClass; set => _idSubjectClass = value; }
        public string Content { get => _content; set => _content = value; }

        public FileInfo(Guid? id, string name, Guid publisherId, string publisher, string content, DateTime? uploadTime, long? size, Guid? folderId, string folderName, Guid idSubjectClass)
        {
            Id = id;
            Name = name;
            PublisherId = publisherId;
            Publisher = publisher;
            Content = content;
            UploadTime = uploadTime;
            Size = size;
            FolderId = folderId;
            FolderName = folderName;
            IdSubjectClass = idSubjectClass;
        }

        public FileInfo(FileInfo file)
        {
            Id = file.Id;
            Name = file.Name;
            PublisherId = file.PublisherId;
            Publisher = file.Publisher;
            Content = file.Content;
            UploadTime = file.UploadTime;
            Size = file.Size;
            FolderId = file.FolderId;
            FolderName = file.FolderName;
            IdSubjectClass = file.IdSubjectClass;
        }
    }
}
