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
    
    public partial class Subject : BaseViewModel
    {
        public Subject()
        {
            this.SubjectClasses = new HashSet<SubjectClass>();
        }
    
        private System.Guid _id { get; set; }
        public System.Guid Id { get => _id; set { _id = value; OnPropertyChanged(); } }
        private Nullable<int> _credit { get; set; }
        public Nullable<int> Credit { get => _credit; set { _credit = value; OnPropertyChanged(); } }
        private string _displayName { get; set; }
        public string DisplayName { get => _displayName; set { _displayName = value; OnPropertyChanged(); } }
        private string _code { get; set; }
        public string Code { get => _code; set { _code = value; OnPropertyChanged(); } }
        private string _describe { get; set; }
        public string Describe { get => _describe; set { _describe = value; OnPropertyChanged(); } }
        private Nullable<bool> _isDeleted { get; set; }
        public Nullable<bool> IsDeleted { get => _isDeleted; set { _isDeleted = value; OnPropertyChanged(); } }
    
        public virtual ICollection<SubjectClass> SubjectClasses { get; set; }
    }
}
