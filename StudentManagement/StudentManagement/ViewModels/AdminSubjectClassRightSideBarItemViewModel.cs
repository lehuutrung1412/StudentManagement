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
        public CardInfo CurrentCard { get => _currentCard; set => _currentCard = value; }
        private CardInfo _currentCard;

        public AdminSubjectClassRightSideBarItemViewModel()
        {
            this.CurrentCard = null;
        }

        public AdminSubjectClassRightSideBarItemViewModel(CardInfo card)
        {
            this.CurrentCard = card;
        }
    }
}
