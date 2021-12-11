using StudentManagement.Commands;
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

        private ObservableCollection<DetailScore> _studentScore;
        public ObservableCollection<DetailScore> StudentScore
        {
            get => _studentScore;
            set => _studentScore = value;
        }

        private StudentGrid _selectedItem;
        public StudentGrid SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;

                //if (_selectedItem != null)
                //{
                //    SelectedScore = StudentScore.Where(x => x.IDStudent == SelectedItem.Username).ToList().FirstOrDefault();
                //    if (SelectedScore == null)
                //    {
                //        StudentScore.Add(new DetailScore { CuoiKi = "0", GiuaKi = "0", QuaTrinh = "0", ThucHanh = "0", DiemTB = "0", IDStudent = SelectedItem.Username });
                //        SelectedScore = StudentScore.Where(x => x.IDStudent == SelectedItem.Username).ToList().FirstOrDefault();
                //    }
                //    _studentListRightSideBarItemViewModel = new StudentListRightSideBarItemViewModel(SelectedScore);
                //    RightSideBarItemViewModel = _studentListRightSideBarItemViewModel;
                //}
                //SelectedScore = StudentScore.Where(x => x.IDStudent == SelectedItem.Username).ToList().FirstOrDefault();
                (_studentListRightSideBarItemViewModel as StudentListRightSideBarItemViewModel).SelectedItem = SelectedItem;
                (_studentListRightSideBarItemViewModel as StudentListRightSideBarItemViewModel).CurrentScore = SelectedScore;
                RightSideBarItemViewModel = _selectedItem == null ? _emptyStateRightSideBarViewModel : _studentListRightSideBarItemViewModel;

                OnPropertyChanged();
            }
        }

        private DetailScore _selectedScore;
        public DetailScore SelectedScore
        {
            get => _selectedScore; set
            {
                _selectedScore = value;
                OnPropertyChanged();
            }
        }

        private object _studentListRightSideBarItemViewModel;
        private object _studentListRightSideBarItemEditViewModel;
        private object _emptyStateRightSideBarViewModel;

        public StudentListRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();

            StudentScore = new ObservableCollection<DetailScore>();
            StudentScore.Add(new DetailScore { CuoiKi = "10", GiuaKi = "10", QuaTrinh = "10", ThucHanh = "10", DiemTB = "10", IDStudent = "19520123" });
            StudentScore.Add(new DetailScore { CuoiKi = "10", GiuaKi = "10", QuaTrinh = "10", ThucHanh = "10", DiemTB = "10", IDStudent = "19520124" });
            StudentScore.Add(new DetailScore { CuoiKi = "10", GiuaKi = "10", QuaTrinh = "10", ThucHanh = "10", DiemTB = "10", IDStudent = "19520125" });
            StudentScore.Add(new DetailScore { CuoiKi = "10", GiuaKi = "10", QuaTrinh = "10", ThucHanh = "10", DiemTB = "10", IDStudent = "19520126" });
            StudentScore.Add(new DetailScore { CuoiKi = "10", GiuaKi = "10", QuaTrinh = "10", ThucHanh = "10", DiemTB = "10", IDStudent = "19520127" });
            StudentScore.Add(new DetailScore { CuoiKi = "10", GiuaKi = "10", QuaTrinh = "10", ThucHanh = "10", DiemTB = "10", IDStudent = "19520128" });
        }

        public void InitRightSideBarItemViewModel()
        {
            _emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();

            _studentListRightSideBarItemEditViewModel = new StudentListRightSideBarItemEditViewModel();
            (_studentListRightSideBarItemEditViewModel as StudentListRightSideBarItemEditViewModel).PropertyChanged += StudentListRightSideBarItemEditViewModel_PropertyChanged;

            _studentListRightSideBarItemViewModel = new StudentListRightSideBarItemViewModel();
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
                    (_studentListRightSideBarItemEditViewModel as StudentListRightSideBarItemEditViewModel).ActualScore = SelectedScore;
                    RightSideBarItemViewModel = _studentListRightSideBarItemEditViewModel;
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
