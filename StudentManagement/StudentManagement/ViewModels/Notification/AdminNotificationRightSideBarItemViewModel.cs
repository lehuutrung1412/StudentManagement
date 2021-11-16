using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudentManagement.ViewModels.Notification.AdminNotificationViewModel;

namespace StudentManagement.ViewModels.Notification
{
    class AdminNotificationRightSideBarItemViewModel
    {
        public CardNotification CurrentCard { get => _currentCard; set => _currentCard = value; }
        private CardNotification _currentCard;

        public AdminNotificationRightSideBarItemViewModel()
        {
            CurrentCard = null;
        }

        public AdminNotificationRightSideBarItemViewModel(CardNotification card)
        {
            CurrentCard = card;
        }
    }
}
