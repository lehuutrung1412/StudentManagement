using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudentManagement.ViewModels.ScoreBoardRightSideBarViewModel;

namespace StudentManagement.ViewModels
{
    public class ScoreBoardRightSideBarItemViewModel : BaseViewModel
    {
        private string _subjectDisplayName;
        public string SubjectDisplayName
        {
            get => _subjectDisplayName;
            set => _subjectDisplayName = value;
        }
        private ObservableCollection<DetailScoreItem> _currentItem;
        public ObservableCollection<DetailScoreItem> CurrentItem { get => _currentItem; set => _currentItem = value; }

        public ScoreBoardRightSideBarItemViewModel()
        {
            CurrentItem = null;
        }

        public ScoreBoardRightSideBarItemViewModel(ObservableCollection<DetailScoreItem> item, string SubjectClassDisplayName)
        {
            CurrentItem = item;
            SubjectDisplayName = SubjectClassDisplayName;
        }
    }
}



