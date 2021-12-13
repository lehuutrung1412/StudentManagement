using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class NotificationTypeServices
    {
        private static NotificationTypeServices s_instance;

        public static NotificationTypeServices Instance => s_instance ?? (s_instance = new NotificationTypeServices());

        public NotificationTypeServices() { }

        public ObservableCollection<string>  GetListNotificationType()
        {
            ObservableCollection<string> listNotificationType = new ObservableCollection<string>();
            DataProvider.Instance.Database.NotificationTypes.ToList().ForEach(notificationType => listNotificationType.Add(notificationType.Content));
            return listNotificationType;
        }
        public NotificationType GetNotificationTypeWithTypeContent(string content)
        {
            return DataProvider.Instance.Database.NotificationTypes.FirstOrDefault(notificationType => notificationType.Content.Contains(content));
        }
    }
}
