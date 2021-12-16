using StudentManagement.Models;
using StudentManagement.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Services;

namespace StudentManagement.Objects
{
    public class ScoreDataGrid : BaseObjectWithBaseViewModel, IBaseCard
    {
        private string _idSubject;
        private string _subject;
        private string _credit;
        private string _nameTeacher;
        private string _status;
        private Guid _idSubjectClass;
        private string _semester;
        private int _stt;

        public int STT
        {
            get => _stt;
            set => _stt = value;
        }

        public Guid IdSubjectClass
        {
            get => _idSubjectClass;
            set => _idSubjectClass = value;
        }
        public string IDSubject
        {
            get => _idSubject;
            set => _idSubject = value;
        }

        public string Subject
        {
            get => _subject;
            set => _subject = value;
        }

        public string Credit
        {
            get => _credit;
            set => _credit = value;
        }

        public string NameTeacher
        {
            get => _nameTeacher;
            set => _nameTeacher = value;
        }

        public string Status
        {
            get => _status;
            set => _status = value;
        }

        public string Semester
        {
            get => _semester;
            set => _semester = value;
        }

        public ScoreDataGrid(Guid ID, string IdSubject, string Subject, string Credit, string NameTeacher)
        {
            this.IdSubjectClass = ID;
            this.Subject = Subject;
            this.IDSubject = IdSubject;
            this.Credit = Credit;
            this.NameTeacher = NameTeacher;
            this.Status = "Hoàn thành";
        }
    }
}
