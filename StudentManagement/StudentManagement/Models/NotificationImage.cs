
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
    
public partial class NotificationImage : BaseViewModel
{

    private System.Guid _id { get; set; }
    public System.Guid Id { get => _id; set { _id = value; OnPropertyChanged(); } }

    private Nullable<System.Guid> _idNotification { get; set; }
    public Nullable<System.Guid> IdNotification { get => _idNotification; set { _idNotification = value; OnPropertyChanged(); } }

    private Nullable<System.Guid> _idDatabaseImageTable { get; set; }
    public Nullable<System.Guid> IdDatabaseImageTable { get => _idDatabaseImageTable; set { _idDatabaseImageTable = value; OnPropertyChanged(); } }



    public virtual DatabaseImageTable DatabaseImageTable { get; set; }

    public virtual Notification Notification { get; set; }

}

}
