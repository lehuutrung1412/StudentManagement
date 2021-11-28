using StudentManagement.Objects;
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
        public NotificationCard CurrentCard { get => _currentCard; set => _currentCard = value; }
        private NotificationCard _currentCard;

        public AdminNotificationRightSideBarItemViewModel()
        {
            CurrentCard = null;
        }

        public AdminNotificationRightSideBarItemViewModel(NotificationCard card)
        {
            CurrentCard = card;
        }
    }
}
