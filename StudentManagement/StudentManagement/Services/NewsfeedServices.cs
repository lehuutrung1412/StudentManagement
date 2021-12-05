using StudentManagement.Models;
using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace StudentManagement.Services
{
    public class NewsfeedServices
    {
        private static NewsfeedServices s_instance;

        public static NewsfeedServices Instance => s_instance ?? (s_instance = new NewsfeedServices());

        public Func<StudentManagementEntities> db = () => DataProvider.Instance.Database;

        #region Convert

        public Notification ConvertPostNewsfeedToNotification(PostNewsfeedViewModel post)
        {
            return new Notification()
            {
                Id = post.PostId,
                Topic = post.IdSubjectClass.ToString(),
                Content = post.PostText,
                Time = post.PostTime,
                IdPoster = (Guid)post.IdPoster,
                IdSubjectClass = post.IdSubjectClass
            };
        }

        public PostNewsfeedViewModel ConvertNotificationToPostNewsfeed(Notification notif)
        {
            return new PostNewsfeedViewModel(notif.IdSubjectClass, notif.IdPoster, notif.Content, notif.Time, new System.Collections.ObjectModel.ObservableCollection<string>());
        }

        #endregion Convert

        #region Create

        public async void SavePostToDatabaseAsync(PostNewsfeedViewModel post)
        {
            db().Notifications.AddOrUpdate(ConvertPostNewsfeedToNotification(post));
            await db().SaveChangesAsync();
        }

        #endregion Create

        #region Read

        public List<Notification> GetListNotificationOfSubjectClass(Guid? idSubjectClass)
        {
            return db().Notifications.Where(notif => notif.IdSubjectClass == idSubjectClass).ToList();
        }

        #endregion
    }
}
