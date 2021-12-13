using StudentManagement.Models;
using StudentManagement.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Services;

namespace StudentManagement.Objects
{
    public class UserCard : BaseObjectWithBaseViewModel, IBaseCard
    {
        private string _name;
        private Guid? _id;
        private string _email;
        private string _gender;
        private string _faculty;
        private string _role;
        private int _stt;
        private string _training;
        private bool _isSelected;

        public UserCard() { }

        public UserCard(string role, string name, Guid? id, string email, string gender, string faculty, string training)
        {
            ID = id;
            DisplayName = name;
            Email = email;
            Role = role;
            Faculty = faculty;
            Training = training;
            Gender = gender;
        }

        public UserCard(Student x)
        {
            ID = x.IdUsers;
            DisplayName = UserServices.Instance.GetDisplayNameById((Guid)ID);
            Role = "Sinh viên";
            
            
        }

        public UserCard(Teacher x)
        {
            ID = x.IdUsers;
            DisplayName = UserServices.Instance.GetDisplayNameById((Guid)ID);
            Role = "Giáo viên";
           
           
        }

        public UserCard(Admin x)
        {
            ID =  x.IdUsers;
            DisplayName = UserServices.Instance.GetDisplayNameById((Guid)ID);
            Role = "Admin";
           
          
        }


        public bool IsSelected
        {
            get => _isSelected;
            set => _isSelected = value;
        }

        public string DisplayName
        {
            get => _name;
            set => _name = value;
        }

        public Guid? ID
        {
            get => _id;
            set => _id = value;
        }

        public string Email
        {
            get => _email;
            set => _email = value;
        }

        public string Gender
        {
            get => _gender;
            set => _gender = value;
        }

        public string Faculty
        {
            get => _faculty;
            set => _faculty = value;
        }

        public string Role
        {
            get => _role;
            set => _role = value;
        }

        public int STT
        {
            get => _stt;
            set => _stt = value;
        }

        public string Training
        {
            get => _training;
            set => _training = value;
        }
    }
}
