using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static StudentManagement.ViewModels.ScoreBoard.ScoreBoardViewModel;

namespace StudentManagement.ViewModels.ScoreBoard
{
    public class ScoreBoardRightSideBarViewModel : BaseViewModel
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

        private Score _selectedItem;
        public Score SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                if (_selectedItem != null)
                {
                    SelectedScore = ScoreList.Where(x => x.IDSubject == SelectedItem.IDSubject).ToList()[0];
                    _scoreboardRightSideBarItemViewModel = new ScoreBoardRightSideBarItemViewModel(SelectedScore);
                    RightSideBarItemViewModel = _scoreboardRightSideBarItemViewModel;
                }
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

        private ObservableCollection<DetailScore> _scoreList;
        public ObservableCollection<DetailScore> ScoreList { get => _scoreList; set => _scoreList = value; }

        private object _scoreboardRightSideBarItemViewModel;

        private object _emptyStateRightSideBarViewModel;

        public ScoreBoardRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();
            ScoreList = new ObservableCollection<DetailScore>
            {
                new DetailScore() {QuaTrinh = "10", CuoiKi = "10", GiuaKi = "10", DiemTB = "10", ThucHanh = "10", IDSubject = "IT008"}
            };

        }

        public void InitRightSideBarItemViewModel()
        {
            _scoreboardRightSideBarItemViewModel = new ScoreBoardRightSideBarItemViewModel();
            _emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();
            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }

        public class DetailScore
        {
            private string _quaTrinh;
            private string _thucHanh;
            private string _giuaKi;
            private string _cuoiKi;
            private string _diemTB;
            private string _idSubject;

            public string QuaTrinh
            {
                get => _quaTrinh;
                set => _quaTrinh = value;
            }


            public string ThucHanh
            {
                get => _thucHanh;
                set => _thucHanh = value;
            }

            public string GiuaKi
            {
                get => _giuaKi;
                set => _giuaKi = value;
            }

            public string CuoiKi
            {
                get => _cuoiKi;
                set => _cuoiKi = value;
            }

            public string DiemTB
            {
                get => _diemTB;
                set => _diemTB = value;
            }

            public string IDSubject
            {
                get => _idSubject;
                set => _idSubject = value;
            }

        }


    }
}
