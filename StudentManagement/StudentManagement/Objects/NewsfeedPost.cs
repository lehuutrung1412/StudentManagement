using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public class NewsfeedPost : BaseViewModel
    {
        public Guid PostId { get; set; }
        public Guid? IdSubjectClass { get; set; }
        public Guid IdPoster { get; set; }
        public string PosterName { get; set; }
        public string PosterAvatar { get; set; }
        public DateTime? PostTime { get; set; }
        public string PostText { get => _postText; set { _postText = value; OnPropertyChanged(); } }
        private string _postText;
        public string Topic { get; set; }
    }

    public class PostComment
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public Guid UserId { get; set; }
        public string UserAvatar { get; set; }
        public string Comment { get; set; }
        public DateTime? Time { get; set; }
        public Guid PostId { get; set; }

        public PostComment(Guid id, Guid postId, Guid userId, string avatar, string username, string comment, DateTime? time)
        {
            Id = id;
            PostId = postId;
            UserId = userId;
            Username = username;
            Comment = comment;
            Time = time;
            UserAvatar = avatar;
        }
    }
}
