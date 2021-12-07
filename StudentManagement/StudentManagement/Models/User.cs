//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StudentManagement.Models
{
    using StudentManagement.ViewModels;
    using System;
    using System.Collections.Generic;
    
    public partial class User : BaseViewModel
    {
        public User()
        {
            this.Admins = new HashSet<Admin>();
            this.Documents = new HashSet<Document>();
            this.Folders = new HashSet<Folder>();
            this.Notifications = new HashSet<Notification>();
            this.Notifications1 = new HashSet<Notification>();
            this.NotificationComments = new HashSet<NotificationComment>();
            this.NotificationInfoes = new HashSet<NotificationInfo>();
            this.Students = new HashSet<Student>();
            this.Teachers = new HashSet<Teacher>();
            this.User_UserRole_UserInfo = new HashSet<User_UserRole_UserInfo>();
        }
    
        private System.Guid _id { get; set; }
        public System.Guid Id { get => _id; set { _id = value; OnPropertyChanged(); } }
        private string _username { get; set; }
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        private string _password { get; set; }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        private string _displayName { get; set; }
        public string DisplayName { get => _displayName; set { _displayName = value; OnPropertyChanged(); } }
        private Nullable<bool> _online { get; set; }
        public Nullable<bool> Online { get => _online; set { _online = value; OnPropertyChanged(); } }
        private Nullable<System.Guid> _idUserRole { get; set; }
        public Nullable<System.Guid> IdUserRole { get => _idUserRole; set { _idUserRole = value; OnPropertyChanged(); } }
        private Nullable<System.Guid> _idFaculty { get; set; }
        public Nullable<System.Guid> IdFaculty { get => _idFaculty; set { _idFaculty = value; OnPropertyChanged(); } }
        private Nullable<System.Guid> _idAvatar { get; set; }
        public Nullable<System.Guid> IdAvatar { get => _idAvatar; set { _idAvatar = value; OnPropertyChanged(); } }
    
        public virtual ICollection<Admin> Admins { get; set; }
        public virtual DatabaseImageTable DatabaseImageTable { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual Faculty Faculty { get; set; }
        public virtual ICollection<Folder> Folders { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Notification> Notifications1 { get; set; }
        public virtual ICollection<NotificationComment> NotificationComments { get; set; }
        public virtual ICollection<NotificationInfo> NotificationInfoes { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
        public virtual ICollection<User_UserRole_UserInfo> User_UserRole_UserInfo { get; set; }
        public virtual UserRole UserRole { get; set; }
    }
}
