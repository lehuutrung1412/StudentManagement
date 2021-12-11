using StudentManagement.Commands;
using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class StudentListRightSideBarItemViewModel : BaseViewModel
    {
        private StudentGrid _selectedItem;
        public StudentGrid SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        private DetailScore _currentScore;
        public DetailScore CurrentScore { get => _currentScore; set => _currentScore = value; }
        public ICommand EditDetailScore { get; set; }
        public bool SwitchToEdit { get => _switchToEdit; set { _switchToEdit = value; OnPropertyChanged(); } }

        private bool _switchToEdit;

        public StudentListRightSideBarItemViewModel()
        {
            CurrentScore = null;
            EditDetailScore = new RelayCommand<object>((p) => { return true; }, (p) => EditDetailScoreFunction(p));
        }

        public void EditDetailScoreFunction(object p)
        {
            SwitchToEdit = true;
        }

    }
}
