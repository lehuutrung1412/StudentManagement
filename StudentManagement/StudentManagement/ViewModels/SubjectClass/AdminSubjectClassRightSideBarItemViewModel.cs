using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudentManagement.ViewModels.AdminSubjectClassViewModel;

namespace StudentManagement.ViewModels
{
    public class AdminSubjectClassRightSideBarItemViewModel : BaseViewModel
    {
        public SubjectCard CurrentCard { get => _currentCard; set => _currentCard = value; }
        private SubjectCard _currentCard;

        public AdminSubjectClassRightSideBarItemViewModel()
        {
            CurrentCard = null;
        }

        public AdminSubjectClassRightSideBarItemViewModel(SubjectCard card)
        {
            CurrentCard = card;
        }
    }
}
