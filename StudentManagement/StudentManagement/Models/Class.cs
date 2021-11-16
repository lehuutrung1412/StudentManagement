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
    
    public partial class Class : BaseViewModel
    {
        private System.Guid _id { get; set; }
        public System.Guid Id { get => _id; set { _id = value; OnPropertyChanged(); } }
        private System.Guid _idTrainingForm { get; set; }
        public System.Guid IdTrainingForm { get => _idTrainingForm; set { _idTrainingForm = value; OnPropertyChanged(); } }
        private System.Guid _idFalcuty { get; set; }
        public System.Guid IdFalcuty { get => _idFalcuty; set { _idFalcuty = value; OnPropertyChanged(); } }
        private string _displayName { get; set; }
        public string DisplayName { get => _displayName; set { _displayName = value; OnPropertyChanged(); } }
        private System.Guid _idTeacher { get; set; }
        public System.Guid IdTeacher { get => _idTeacher; set { _idTeacher = value; OnPropertyChanged(); } }
        private Nullable<bool> _isDeleted { get; set; }
        public Nullable<bool> IsDeleted { get => _isDeleted; set { _isDeleted = value; OnPropertyChanged(); } }
        private System.Guid _idThumbnail { get; set; }
        public System.Guid IdThumbnail { get => _idThumbnail; set { _idThumbnail = value; OnPropertyChanged(); } }
    
        public virtual Falcuty Falcuty { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual DatabaseImageTable DatabaseImageTable { get; set; }
        public virtual TrainingForm TrainingForm { get; set; }
    }
}