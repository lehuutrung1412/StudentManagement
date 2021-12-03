using StudentManagement.Models;
using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class NotificationServices
    {
        private static NotificationServices s_instance;

        public static NotificationServices Instance => s_instance ?? (s_instance = new NotificationServices());

        public NotificationServices() { }
        public NotificationCard ConvertNotificationToNotificationCard(Notification notification)
        {
            NotificationCard notificationCard = new NotificationCard()
            {
                Id = notification.Id,
                Poster = notification.User.Username,
                Content = notification.Content,
                Topic = notification.Topic,
                Time = Convert.ToDateTime(notification.Time),
                Type = notification.NotificationType.Content,
            };
            return notificationCard;
        }

        public ObservableCollection<NotificationCard> LoadNotificationCardByUserId(Guid id)
        {
            ObservableCollection<NotificationCard> notificationCards = new ObservableCollection<NotificationCard>();
            List<Notification> notificationList = new List<Notification>();
            if (UserServices.Instance.GetUserById(id).UserRole.Role.Contains("Admin"))
                notificationList = DataProvider.Instance.Database.Notifications.Where(notification=>notification.IdSubjectClass==null).ToList();
            else
                notificationList = DataProvider.Instance.Database.Notifications.Where(notification=> notification.Id == id).ToList();
            foreach (Notification notification in notificationList)
            {
                NotificationCard notificationCard = new NotificationCard(ConvertNotificationToNotificationCard(notification));
                notificationCards.Add(notificationCard);
            }    
            return notificationCards;
        }
        public Notification ConvertNotificationCardAndIdUserToNotification(NotificationCard notificationCard, Guid idUser)
        {
            Notification notification = new Notification()
            {
                Id = notificationCard.Id,
                Topic = notificationCard.Topic,
                Content = notificationCard.Content,
                Time = notificationCard.Time,
                IdPoster = idUser,
                NotificationType = DataProvider.Instance.Database.NotificationTypes.Where(type=>type.Content == notificationCard.Type).FirstOrDefault(),
            };
            return notification;
        }
        public void AddNotificationByNotificationCardAndUser(NotificationCard notificationCard, Guid idUser)
        {
            Notification notification = ConvertNotificationCardAndIdUserToNotification(notificationCard, idUser);
            DataProvider.Instance.Database.Notifications.Add(notification);
            DataProvider.Instance.Database.SaveChanges();
            AddNotificationInfoByNotificationType(notification);
        }
        public void AddNotificationInfoByNotificationType(Notification notification)
        {
            List<User> users = new List<User>();
            switch (notification.NotificationType.Content)
            {
                case "Thông báo chung":
                    {
                        users = DataProvider.Instance.Database.Users.ToList();
                        break;
                    }
                case "Thông báo sinh viên":
                    {
                        users = DataProvider.Instance.Database.Users.Where(user => user.UserRole.Role.Contains("Học sinh")).ToList();
                        break;
                    }
                case "Thông báo giáo viên":
                    {
                        users = DataProvider.Instance.Database.Users.Where(user => user.UserRole.Role.Contains("Giáo viên")).ToList();
                        break;
                    }
                case "Thông báo Admin":
                    {
                        users = DataProvider.Instance.Database.Users.Where(user => user.UserRole.Role.Contains("Admin")).ToList();
                        break;
                    }
            }
            foreach (var user in users)
            {
                NotificationInfo notificationInfo = new NotificationInfo()
                {
                    Id = Guid.NewGuid(),
                    IdNotification = notification.Id,
                    IdUserReceiver = user.Id,
                };
                DataProvider.Instance.Database.NotificationInfoes.Add(notificationInfo);
            }
            DataProvider.Instance.Database.SaveChanges();
        }

    }
}
