using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Utils;
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
        public NotificationCard ConvertNotificationAndIdUserToNotificationCard(Notification notification, Guid idUser)
        {
            var notificationInfo = GetNotificationInfoByIdNotificationAndUserId(notification.Id, idUser);
            NotificationCard notificationCard = new NotificationCard()
            {
                Id = notification.Id,
                IdPoster = notification.IdPoster,
                Content = notification.Content,
                Topic = notification.Topic,
                Time = Convert.ToDateTime(notification.Time),
                IdSubjectClass = notification.IdSubjectClass,
                Type = notification.NotificationType.Content,
            };
            if (notificationInfo.Count > 0)
                notificationCard.Status = Convert.ToBoolean(notificationInfo.FirstOrDefault().IsRead);
            return notificationCard;
        }
        public List<NotificationInfo> GetNotificationInfoByIdNotificationAndUserId(Guid id, Guid idNotification)
        {
            var notificationInfo = DataProvider.Instance.Database.NotificationInfoes.Where(notificationDetail => notificationDetail.IdUserReceiver == id && notificationDetail.IdNotification == idNotification).ToList();
            return notificationInfo;
        }
        public ObservableCollection<NotificationCard> LoadNotificationCardByUserId(Guid id)
        {
            try
            {
                ObservableCollection<NotificationCard> notificationCards = new ObservableCollection<NotificationCard>();
                List<Notification> notificationList = new List<Notification>();
                if (UserServices.Instance.GetUserById(id).UserRole.Role.Contains("Admin"))
                    notificationList = DataProvider.Instance.Database.Notifications.Where(notification => notification.IdSubjectClass == null).ToList();
                else
                    notificationList = DataProvider.Instance.Database.Notifications.Where(notification => notification.Id == id).ToList();
                foreach (Notification notification in notificationList)
                {
                    NotificationCard notificationCard = new NotificationCard(ConvertNotificationAndIdUserToNotificationCard(notification, id));
                    notificationCards.Add(notificationCard);
                }
                return notificationCards;
            }
            catch
            {
                return new ObservableCollection<NotificationCard>();
            }
        }
        public Notification ConvertNotificationCardToNotification(NotificationCard notificationCard)
        {
            Notification notification = new Notification()
            {
                Id = notificationCard.Id,
                Topic = notificationCard.Topic,
                Content = notificationCard.Content,
                Time = notificationCard.Time,
                IdPoster = notificationCard.IdPoster,
                NotificationType = DataProvider.Instance.Database.NotificationTypes.Where(type => type.Content.Contains(notificationCard.Type)).FirstOrDefault(),
            };
            return notification;
        }
        public void AddNotificationByNotificationCard(NotificationCard notificationCard)
        {
            Notification notification = ConvertNotificationCardToNotification(notificationCard);
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

        public void CopyNotificationCardToNotification(NotificationCard notificationCard, Notification updateNotification)
        {

            updateNotification.Topic = notificationCard.Topic;
            updateNotification.Id = notificationCard.Id;
            updateNotification.Content = notificationCard.Content;
            updateNotification.IdSubjectClass = notificationCard.IdSubjectClass;
            updateNotification.IdPoster = notificationCard.IdPoster;
            updateNotification.NotificationType = DataProvider.Instance.Database.NotificationTypes.Where(type => type.Content.Contains(notificationCard.Type)).FirstOrDefault();
            updateNotification.Time = notificationCard.Time;
        }

        public void UpdateNotificationByNotificationCardAndIdUser(NotificationCard notificationCard, Guid idUser)
        {
            var notificationInfoes = GetNotificationInfoByIdNotificationAndUserId(idUser, notificationCard.Id);
            Notification updateNotification = DataProvider.Instance.Database.Notifications.Where(notificationItem => notificationItem.Id == notificationCard.Id).FirstOrDefault();
            CopyNotificationCardToNotification(notificationCard, updateNotification);
            if (notificationInfoes.Count > 0)
            {
                notificationInfoes.FirstOrDefault().IsRead = notificationCard.Status;
            }
            DataProvider.Instance.Database.SaveChanges();
        }

        public Notification FindNotificationByNotificationId(Guid idNotification)
        {
            return DataProvider.Instance.Database.Notifications.Where(notification => notification.Id == idNotification).FirstOrDefault();
        }
        public void DeleteNotificationByNotificationCard(NotificationCard notificationCard)
        {
            var notification = FindNotificationByNotificationId(notificationCard.Id);
            List<NotificationInfo> listNotificationInfo = DataProvider.Instance.Database.NotificationInfoes.Where(notificationInfo => notificationInfo.IdNotification == notification.Id).ToList();
            foreach (var notificationInfo in listNotificationInfo)
            {
                DataProvider.Instance.Database.NotificationInfoes.Remove(notificationInfo);
            }
            DataProvider.Instance.Database.Notifications.Remove(notification);
            DataProvider.Instance.Database.SaveChanges();
        }
        public void MarkAsReadNotificationInfoByNotificationCardAndIdUser(NotificationCard notificationCard, Guid idUser)
        {
            var NotificationInfo = GetNotificationInfoByIdNotificationAndUserId(idUser, notificationCard.Id);
            if (NotificationInfo.ToList().Count == 0)
                return;
            NotificationInfo.FirstOrDefault().IsRead = true;
            DataProvider.Instance.Database.SaveChanges();
        }
        public void MarkAsUnReadNotificationInfoByNotificationCardAndIdUser(NotificationCard notificationCard, Guid idUser)
        {
            var NotificationInfo = GetNotificationInfoByIdNotificationAndUserId(idUser, notificationCard.Id);
            if (NotificationInfo.ToList().Count == 0)
                return;
            NotificationInfo.FirstOrDefault().IsRead = false;
            DataProvider.Instance.Database.SaveChanges();
        }
        public List<NotificationInfo> FindNotificationInfoByNotificationCard(NotificationCard notificationCard)
        {
            return DataProvider.Instance.Database.NotificationInfoes.Where(notificationInfo => notificationInfo.IdNotification == notificationCard.Id).ToList();
        }
        public void MarkAllAsReadNotificationInfoByNotificationCard(NotificationCard notificationCard)
        {
            var NotificationInfoes = FindNotificationInfoByNotificationCard(notificationCard);
            if (NotificationInfoes.Count == 0)
                return;
            foreach (var notificationInfo in NotificationInfoes)
            {
                notificationInfo.IsRead = true;
            }
            DataProvider.Instance.Database.SaveChanges();
        }
    }
}
