using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public class ComponentScoreInSetting : BaseViewModel
    {
        public Guid Id { get; set; }
        public Guid? IdSubjectClass { get; set; }
        private string _displayName;
        private double? _percent;
        public string DisplayName { get => _displayName; set { _displayName = value; OnPropertyChanged(); } }
        public double? Percent { get => _percent; set { _percent = value; OnPropertyChanged(); } }
    }
}
