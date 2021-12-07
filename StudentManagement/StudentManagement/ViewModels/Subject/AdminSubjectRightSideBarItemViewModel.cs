using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    public class AdminSubjectRightSideBarItemViewModel : BaseViewModel
    {
        public SubjectCard CurrentCard { get => _currentCard; set => _currentCard = value; }
        private SubjectCard _currentCard;

        public AdminSubjectRightSideBarItemViewModel()
        {
            CurrentCard = null;
        }

        public AdminSubjectRightSideBarItemViewModel(SubjectCard card)
        {
            CurrentCard = card;
        }
    }
}
