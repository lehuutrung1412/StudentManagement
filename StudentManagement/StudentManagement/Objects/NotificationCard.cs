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
        private string _poster;
        private string _content;
        private string _topic;
        private DateTime _time;
        private string _type;
        private bool _status;
        
        public NotificationCard() { }
        public NotificationCard(Guid id, string poster, string type, string content, string topic, DateTime time)
        {
            Id = id;
            Poster = poster;
            Type = type;
            Content = content;
            Topic = topic;
            Time = time;
            Status = false;
        }
        public NotificationCard(NotificationCard a)
        {
            Id = a.Id;
            Topic = a.Topic;
            Poster = a.Poster;
            Type = a.Type;
            Content = a.Content;
            Time = a.Time;
            Status = a.Status;
        }

        public string Poster { get => _poster; set => _poster = value; }
        public string Content { get => _content; set => _content = value; }
        public string Topic { get => _topic; set => _topic = value; }
        public DateTime Time { get => _time; set => _time = value; }
        public string Type { get => _type; set => _type = value; }
        public Guid Id { get => _id; set => _id = value; }
        public bool Status { get => _status; set { _status = value; OnPropertyChanged(); } }
    }
}
