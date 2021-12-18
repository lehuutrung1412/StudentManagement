using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static StudentManagement.Services.LoginServices;
using static StudentManagement.ViewModels.ScoreBoardViewModel;

namespace StudentManagement.ViewModels
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

        private Guid _idUser;
        public Guid IdStudent
        {
            get => _idUser;
            set => _idUser = value;
        }
        private ScoreDataGrid _selectedItem;
        public ScoreDataGrid SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                try
                {
                    if (_selectedItem != null)
                    {
                        ShowDetailScore();
                        string SubjectClassDisplayName = DataProvider.Instance.Database.SubjectClasses.Where(x => x.Id == _selectedItem.IdSubjectClass).FirstOrDefault().Subject.DisplayName;
                        _scoreboardRightSideBarItemViewModel = new ScoreBoardRightSideBarItemViewModel(CurrentScore, SubjectClassDisplayName);
                        RightSideBarItemViewModel = _scoreboardRightSideBarItemViewModel;
                    }
                }
                catch (Exception)
                {
                    MyMessageBox.Show("Đã có lỗi xảy ra");
                }
            }
        }

        private ObservableCollection<DetailScoreItem> _currentScore;
        public ObservableCollection<DetailScoreItem> CurrentScore
        {
            get => _currentScore;
            set => _currentScore = value;
        }

        private object _scoreboardRightSideBarItemViewModel;

        private object _emptyStateRightSideBarViewModel;

        public ScoreBoardRightSideBarViewModel()
        {
            InitRightSideBarItemViewModel();
            var user = LoginServices.CurrentUser;
            if (user == null)
                return;

            LoginServices.UpdateCurrentUser += FreeRightSideBar;

            IdStudent = DataProvider.Instance.Database.Students.Where(x => x.IdUsers == user.Id).FirstOrDefault().Id;

            CurrentScore = new ObservableCollection<DetailScoreItem>();

        }

        void ShowDetailScore()
        {
            try
            {
                double gpa = 0;
                CurrentScore = new ObservableCollection<DetailScoreItem>();

                var ListDetailScore = DataProvider.Instance.Database.DetailScores.Where(x => x.IdStudent == IdStudent && x.ComponentScore.IdSubjectClass == SelectedItem.IdSubjectClass);
                foreach (var item in ListDetailScore)
                {
                    if (item?.Score != null)
                    {
                        gpa += (double)item.Score * (double)item.ComponentScore.ContributePercent / 100;
                        CurrentScore.Add(new DetailScoreItem(item.ComponentScore.DisplayName, Convert.ToString(item.ComponentScore.ContributePercent) + "%", Convert.ToString(item.Score)));
                    }
                }

                CurrentScore.Add(new DetailScoreItem("Điểm trung bình", "Điểm trung bình", Convert.ToString(gpa)));
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra");
            }
        }

        public void InitRightSideBarItemViewModel()
        {
            _scoreboardRightSideBarItemViewModel = new ScoreBoardRightSideBarItemViewModel();
            _emptyStateRightSideBarViewModel = new EmptyStateRightSideBarViewModel();
            RightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }

        private void FreeRightSideBar(object sender, LoginEvent e)
        {
            _rightSideBarItemViewModel = _emptyStateRightSideBarViewModel;
        }

    }
}
