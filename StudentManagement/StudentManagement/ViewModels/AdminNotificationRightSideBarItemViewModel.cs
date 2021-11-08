using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudentManagement.ViewModels.AdminNotificationViewModel;

namespace StudentManagement.ViewModels
{
    class AdminNotificationRightSideBarItemViewModel
    {
        public CardNotification CurrentCard { get => _currentCard; set => _currentCard = value; }
        private CardNotification _currentCard;

        public AdminNotificationRightSideBarItemViewModel()
        {
            this.CurrentCard = null;
        }

        public AdminNotificationRightSideBarItemViewModel(CardNotification card)
        {
            this.CurrentCard = card;
        }
    }
}
