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

        #endregion

        #region Create

        public async Task SaveCalendarToDatabaseAsync(AbsentAndMakeUpItem item)
        {
            db().AbsentCalendars.AddOrUpdate(ConvertAbsentItemToAbsentCalendar(item));
            await db().SaveChangesAsync();
        }

        #endregion

        #region Read

        public List<AbsentCalendar> GetListAbsentCalendars(Guid idSubjectClass)
        {
            return db().AbsentCalendars.Where(item => item.IdSubjectClass == idSubjectClass).ToList();
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
