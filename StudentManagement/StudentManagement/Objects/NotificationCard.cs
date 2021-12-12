using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public class NotificationCard : BaseObjectWithBaseViewModel, IBaseCard
    {
        private Guid _id;
        private Nullable<Guid> _idPoster;
        private string _content;
        private string _topic;
        private DateTime _time;
        private string _type;
        private Guid? _idSubjectClass;
        private bool _status;
        public NotificationCard() { }
        public NotificationCard(Guid id, Guid idPoster, string type, string content, string topic, DateTime time,bool status = false, Guid? idSubjectClass = null)
        {
            Id = id;
            IdPoster = idPoster;
            Type = type;
            Content = content;
            Topic = topic;
            Time = time;
            Status = status;
            IdSubjectClass = idSubjectClass;
        }
        public NotificationCard(NotificationCard a)
        {
            Id = a.Id;
            Topic = a.Topic;
            IdPoster = a.IdPoster;
            Type = a?.Type;
            Content = a?.Content;
            Time = a.Time;
            Status = a.Status;
            IdSubjectClass = a?.IdSubjectClass;
        }

        public Nullable<Guid> IdPoster { get => _idPoster; set => _idPoster = value; }
        public string Content { get => _content; set => _content = value; }
        public string Topic { get => _topic; set => _topic = value; }
        public DateTime Time { get => _time; set => _time = value; }
        public string Type { get => _type; set => _type = value; }
        public Guid Id { get => _id; set => _id = value; }
        public bool Status { get => _status; set { _status = value; OnPropertyChanged(); } }

        public Guid? IdSubjectClass { get => _idSubjectClass; set => _idSubjectClass = value; }
    }
}
