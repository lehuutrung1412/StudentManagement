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
    public class TrainingScoreDataGrid : BaseObjectWithBaseViewModel, IBaseCard
    {
        private string _event;
        private string _faculty;
        private int _score;
        private int _stt;
        private string _semester;
        private string _type;

        public string Event
        {
            get => _event;
            set => _event = value;
        }

        public string Faculty
        {
            get => _faculty;
            set => _faculty = value;
        }

        public int Score
        {
            get => _score;
            set => _score = value;
        }

        public int STT
        {
            get => _stt;
            set => _stt = value;
        }

        public string Semester
        {
            get => _semester;
            set => _semester = value;
        }

        public string Type
        {
            get => _type;
            set => _type = value;
        }  
    }
}
