using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class StudentListRightSideBarViewModel : BaseViewModel
    {
        private object _rightSideBarItemViewModel;

        public object RightSideBarItemViewModel
        {
            get { return _rightSideBarItemViewModel; }
            set
            {
                _rightSideBarItemViewModel = value;
                OnPropertyChanged();
            }
        }

        private StudentGrid _selectedItem;
        public StudentGrid SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;

                (_studentListRightSideBarItemViewModel as StudentListRightSideBarItemViewModel).SelectedItem = SelectedItem;
                RightSideBarItemViewModel = _selectedItem == null ? _emptyStateRightSideBarViewModel : _studentListRightSideBarItemViewModel;

                OnPropertyChanged();
            }
        }

        private object _studentListRightSideBarItemViewModel;
        private object _studentListRightSideBarItemEditViewModel;
        private object _emptyStateRightSideBarViewModel;

        SubjectClass SubjectClassDetail { get; set; }

        public StudentListRightSideBarViewModel(SubjectClass subjectClass)
        {
            SubjectClassDetail = subjectClass;

            InitRightSideBarItemViewModel();
        }

        public void InitRightSideBarItemViewModel()
        {
            _emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();

            _studentListRightSideBarItemEditViewModel = new StudentListRightSideBarItemEditViewModel();
            (_studentListRightSideBarItemEditViewModel as StudentListRightSideBarItemEditViewModel).PropertyChanged += StudentListRightSideBarItemEditViewModel_PropertyChanged;

            _studentListRightSideBarItemViewModel = new StudentListRightSideBarItemViewModel(SubjectClassDetail);
            (_studentListRightSideBarItemViewModel as StudentListRightSideBarItemViewModel).PropertyChanged += StudentListRightSideBarItemViewModel_PropertyChanged;

            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }

        private void StudentListRightSideBarItemViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SwitchToEdit")
            {
                var studentListItem = (_studentListRightSideBarItemViewModel as StudentListRightSideBarItemViewModel);
                if (studentListItem.SwitchToEdit)
                {
                    //(_studentListRightSideBarItemEditViewModel as StudentListRightSideBarItemEditViewModel).ActualScore = SelectedScore;
                    //RightSideBarItemViewModel = _studentListRightSideBarItemEditViewModel;
                }
            }
        }

        private void StudentListRightSideBarItemEditViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SwitchToView")
            {
                var studentListItem = (_studentListRightSideBarItemEditViewModel as StudentListRightSideBarItemEditViewModel);
                if (studentListItem.SwitchToView)
                {
                    RightSideBarItemViewModel = _studentListRightSideBarItemViewModel;
                }
            }
        }
    }
}
