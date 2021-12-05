using StudentManagement.Models;
using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using StudentManagement.Objects;

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
            return new PostNewsfeedViewModel(notif.IdSubjectClass, notif.IdPoster, notif.Id, notif.Content, notif.Time, new System.Collections.ObjectModel.ObservableCollection<string>());
        }

        public NotificationComment ConvertPostCommentToNotificationComment(PostComment comment)
        {
            return new NotificationComment()
            {
                Id = (Guid)comment.Id,
                Time = comment.Time,
                Content = comment.Comment,
                IdUserComment = (Guid)comment.UserId,
                IdNotification = (Guid)comment.PostId
            };
        }

        public PostComment ConvertNotificationCommentToPostComment(NotificationComment comment)
        {
            User user = UserServices.Instance.GetUserById(comment.IdUserComment);
            return new PostComment(comment.Id, comment.IdNotification, comment.IdUserComment, user.DisplayName, comment.Content, comment.Time);
        }

        #endregion Convert

        #region Create

        public async void SavePostToDatabaseAsync(PostNewsfeedViewModel post)
        {
            db().Notifications.AddOrUpdate(ConvertPostNewsfeedToNotification(post));
            await db().SaveChangesAsync();
        }

        public async void SaveCommentToDatabaseAsync(PostComment comment)
        {
            db().NotificationComments.AddOrUpdate(ConvertPostCommentToNotificationComment(comment));
            await db().SaveChangesAsync();
        }

        #endregion Create

        #region Read

        public List<Notification> GetListNotificationOfSubjectClass(Guid? idSubjectClass)
        {
            return db().Notifications.Where(notif => notif.IdSubjectClass == idSubjectClass).ToList();
        }

        public List<NotificationComment> GetListCommentInPost(Guid? postId)
        {
            return db().NotificationComments.Where(cmt => cmt.IdNotification == postId).ToList();
        }

        #endregion
    }
}
