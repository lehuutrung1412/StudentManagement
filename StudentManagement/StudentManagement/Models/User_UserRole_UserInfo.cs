
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
    
public partial class User_UserRole_UserInfo : BaseViewModel
{

    private System.Guid _id { get; set; }
    public System.Guid Id { get => _id; set { _id = value; OnPropertyChanged(); } }

    private System.Guid _idUser { get; set; }
    public System.Guid IdUser { get => _idUser; set { _idUser = value; OnPropertyChanged(); } }

    private System.Guid _idUserRole_Info { get; set; }
    public System.Guid IdUserRole_Info { get => _idUserRole_Info; set { _idUserRole_Info = value; OnPropertyChanged(); } }

    private string _content { get; set; }
    public string Content { get => _content; set { _content = value; OnPropertyChanged(); } }



    public virtual User User { get; set; }

    public virtual UserRole_UserInfo UserRole_UserInfo { get; set; }

}

}
