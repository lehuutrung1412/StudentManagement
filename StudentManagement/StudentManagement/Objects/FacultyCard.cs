using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public class FacultyCard : BaseObjectWithBaseViewModel, IBaseCard
    {
        private string _displayName;
        private DateTime _foundationDay;
        private int _numberOfStudents;
        private string _cacHeDaoTao;
        private bool _isDeleted;
        private Guid _id;

        public FacultyCard()
        {
            Id = Guid.NewGuid();
        }
        public FacultyCard(Guid id, string displayName, DateTime foundationDay, int numberOfStudents, string cacHeDaoTao)
        {
            Id = id;
            DisplayName = displayName;
            FoundationDay = foundationDay;
            NumberOfStudents = numberOfStudents;
            CacHeDaoTao = cacHeDaoTao;
        }

        public string DisplayName { get => _displayName; set => _displayName = value; }
        public DateTime FoundationDay { get => _foundationDay; set => _foundationDay = value; }
        public int NumberOfStudents { get => _numberOfStudents; set => _numberOfStudents = value; }
        public string CacHeDaoTao { get => _cacHeDaoTao; set => _cacHeDaoTao = value; }
        public Guid Id { get => _id; set => _id = value; }
        public bool IsDeleted { get => _isDeleted; set => _isDeleted = value; }
    }
}
