using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public class StudentGrid
    {
        private string _displayName;
        private string _username;
        private string _email;
        private string _gender;
        private string _faculty;
        private string _status;
        private int _number;
        private string _trainingForm;
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => _isSelected = value;
        }

        public string DisplayName
        {
            get => _displayName;
            set => _displayName = value;
        }

        public string Username
        {
            get => _username;
            set => _username = value;
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

        public string Status
        {
            get => _status;
            set => _status = value;
        }

        public int Number
        {
            get => _number;
            set => _number = value;
        }

        public string TrainingForm
        {
            get => _trainingForm;
            set => _trainingForm = value;
        }
    }
}
