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

        public Notification ConvertPostNewsfeedToNotification(NewsfeedPost post)
        {
            return new Notification()
            {
                Id = post.PostId,
                Topic = post.Topic,
                Content = post.PostText,
                Time = post.PostTime,
                IdPoster = post.IdPoster,
                IdSubjectClass = post.IdSubjectClass
            };
        }

        public NewsfeedPost ConvertNotificationToPostNewsfeed(Notification notif)
        {
            var poster = UserServices.Instance.GetUserById((Guid)notif.IdPoster);

            return new NewsfeedPost()
            {
                PostId = notif.Id,
                PostText = notif.Content,
                PostTime = notif.Time,
                IdSubjectClass = notif.IdSubjectClass,
                IdPoster = poster.Id,
                PosterName = poster.DisplayName,
                Topic = notif.Topic
            };
        }

        public NotificationComment ConvertPostCommentToNotificationComment(PostComment comment)
        {
            return new NotificationComment()
            {
                Id = comment.Id,
                Time = comment.Time,
                Content = comment.Comment,
                IdUserComment = comment.UserId,
                IdNotification = comment.PostId
            };
        }

        public PostComment ConvertNotificationCommentToPostComment(NotificationComment comment)
        {
            User user = UserServices.Instance.GetUserById((Guid)comment.IdUserComment);
            return new PostComment((Guid)comment.Id, (Guid)comment.IdNotification, (Guid)comment.IdUserComment, user.DisplayName, comment.Content, comment.Time);
        }

        #endregion Convert

        #region Create

        public async void SavePostToDatabaseAsync(NewsfeedPost post)
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

        public List<NotificationComment> GetListCommentInPost(Guid postId)
        {
            return db().NotificationComments.Where(cmt => cmt.IdNotification == postId).ToList();
        }

        #endregion
    }
}
