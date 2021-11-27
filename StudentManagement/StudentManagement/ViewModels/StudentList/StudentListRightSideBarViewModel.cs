using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static StudentManagement.ViewModels.AdminStudentListViewModel;
using Student = StudentManagement.ViewModels.AdminStudentListViewModel.Student;

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

        private Student _selectedItem;
        public Student SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                
                if (_selectedItem != null)
                {
                    SelectedScore = StudentScore.Where(x => x.IDStudent == SelectedItem.IDStudent).ToList()[0];
                    _studentListRightSideBarItemViewModel = new StudentListRightSideBarItemViewModel(SelectedScore, SelectedItem);
                    RightSideBarItemViewModel = _studentListRightSideBarItemViewModel;
                }
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
            _studentListRightSideBarItemViewModel = new StudentListRightSideBarItemViewModel();
            _emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();
            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }
    }
}
