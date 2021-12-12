using StudentManagement.Models;
using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using StudentManagement.Objects;
using System.Data.Entity;
using System.Collections.ObjectModel;

namespace StudentManagement.Services
{
    public class AbsentCalendarServices
    {
        private static AbsentCalendarServices s_instance;

        public static AbsentCalendarServices Instance => s_instance ?? (s_instance = new AbsentCalendarServices());

        public Func<StudentManagementEntities> db = () => DataProvider.Instance.Database;

        #region Convert

        public AbsentCalendar ConvertAbsentItemToAbsentCalendar(AbsentAndMakeUpItem item)
        {
            return new AbsentCalendar()
            {
                Id = item.Id,
                Date = item.Date,
                IdSubjectClass = item.IdSubjectClass,
                Period = item.Period,
                Type = item.Type == "Học bù" ? 0 : 1
            };
        }

        public AbsentAndMakeUpItem ConvertAbsentCalendarToAbsentItem(AbsentCalendar item)
        {
            return new AbsentAndMakeUpItem(item.Id, (Guid)item.IdSubjectClass, (DateTime)item.Date, item.Period, item.Type == 0 ? "Học bù" : "Nghỉ học");
        }

        public Notification ConvertAbsentItemToNotification(AbsentAndMakeUpItem item)
        {
            AbsentCalendar absentCalendar = GetAbsentCalenderByIdAbsentCalender(item.Id);
            Notification notification = new Notification()
            {
                Id = item.Id,
                Time = DateTime.Now,
                IdPoster = LoginServices.CurrentUser.Id,
                IdSubjectClass = absentCalendar.IdSubjectClass,
            };
            notification.Topic = absentCalendar.SubjectClass.Code;
            if (item.Type == "Học bù")
                notification.Content = "Thông báo học bù môn ";
            else
                notification.Content = "Thông báo nghỉ học môn ";

            var culture = new System.Globalization.CultureInfo("vi-VN");
            var dayInfo = (DateTime)absentCalendar.Date;
            notification.Content += absentCalendar.SubjectClass.Subject.DisplayName + String.Format(" vào {0} Tiết {1} ngày {2}", 
                culture.DateTimeFormat.GetDayName(dayInfo.DayOfWeek) , absentCalendar.Period,(dayInfo.ToString("dd/MM/yyyy")));
            return notification;
        }

        #endregion

        #region Create

        public async Task SaveCalendarToDatabaseAsync(AbsentAndMakeUpItem item)
        {
            db().AbsentCalendars.AddOrUpdate(ConvertAbsentItemToAbsentCalendar(item));
            await db().SaveChangesAsync();
        }
        public async Task SaveCalendarToNotification(AbsentAndMakeUpItem item)
        {
            db().Notifications.AddOrUpdate(ConvertAbsentItemToNotification(item));
            await db().SaveChangesAsync();
        }
        public async Task SaveCalendarToNotificationInfo(AbsentAndMakeUpItem item)
        {
            var notification = NotificationServices.Instance.FindNotificationByNotificationId(item.Id);
            var listCourseRegister = notification.SubjectClass.CourseRegisters.ToList();
            foreach (var courseRegister in listCourseRegister)
            {
                var notificationInfo = new NotificationInfo()
                {
                    Id = Guid.NewGuid(),
                    IdNotification = notification.Id,
                    IdUserReceiver = courseRegister.Student.IdUsers,
                    IsRead = false,
                };
                db().NotificationInfoes.AddOrUpdate(notificationInfo);
            }
            await db().SaveChangesAsync();
        }

        #endregion

        #region Read

        public List<AbsentCalendar> GetListAbsentCalendars(Guid idSubjectClass)
        {
            return db().AbsentCalendars.Where(item => item.IdSubjectClass == idSubjectClass).ToList();
        }
        public AbsentCalendar GetAbsentCalenderByIdAbsentCalender(Guid idAbsentCalender)
        {
            return db().AbsentCalendars.FirstOrDefault(item => item.Id == idAbsentCalender);
        }

        #endregion

        #region Delete

        public async Task<int> DeleteAbsentCalendarAsync(Guid? idSubjectClass, DateTime date)
        {
            var events = db().AbsentCalendars.Where(calendar => calendar.IdSubjectClass == idSubjectClass && calendar.Date == date);
            db().AbsentCalendars.RemoveRange(events);

            return await db().SaveChangesAsync();
        }

        #endregion
    }
}
