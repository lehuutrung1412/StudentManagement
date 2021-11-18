using Microsoft.Win32;
using StudentManagement.Commands;
using StudentManagement.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class ScoreBoardViewModel : BaseViewModel
    {
        private string _selectedSemester;

        public string SelectedSemester
        {
            get => _selectedSemester;
            set
            {
                _selectedSemester = value;
                OnPropertyChanged();
                UpdateScoreBoard(value);
            }
        }

        private ObservableCollection<Score> _studentScoreItems;
        public ObservableCollection<Score> StudentScoreItems
        {
            get => _studentScoreItems;
            set => _studentScoreItems = value;
        }


        private ObservableCollection<Score> _currentStudentScore;
        public ObservableCollection<Score> CurrentStudentScore
        {
            get => _currentStudentScore;
            set
            {
                _currentStudentScore = value;
                OnPropertyChanged();
            }
        }

        private string _gpa;
        public string GPA
        {
            get => _gpa;
            set
            {
                _gpa = value;
                OnPropertyChanged();
            }
        }

        private string _trainingScore;
        public string TrainingScore
        {
            get => _trainingScore;
            set
            {
                _trainingScore = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<string> _semesters;
        public ObservableCollection<string> Semesters
        {
            get => _semesters;
            set
            {
                _semesters = value;
            }
        }

        private ObservableCollection<TrainingScore> _studentTrainingScoreItems;
        public ObservableCollection<TrainingScore> StudentTrainingScoreItems
        {
            get => _studentTrainingScoreItems;
            set => _studentTrainingScoreItems = value;
        }

        private ObservableCollection<TrainingScore> _currentTrainingScore;
        public ObservableCollection<TrainingScore> CurrentTrainingScore
        {
            get => _currentTrainingScore;
            set
            {
                _currentTrainingScore = value;
                OnPropertyChanged();
            }
        }

        public ScoreBoardViewModel()
        {
            StudentScoreItems = new ObservableCollection<Score>();
            StudentTrainingScoreItems = new ObservableCollection<TrainingScore>();

            StudentScoreItems.Add(new Score() { IDSubject = "IT008", Subject = "Lập trình trực quan", Credit = "4", Faculty = "KHMT", Status = "Hoàn thành", ID = 1, Semester = "Học kì I, 2018-2019" });
            StudentScoreItems.Add(new Score() { IDSubject = "IT008", Subject = "Lập trình trực quan", Credit = "4", Faculty = "KHMT", Status = "Hoàn thành", ID = 2, Semester = "Học kì II, 2018-2019" });
            StudentScoreItems.Add(new Score() { IDSubject = "IT008", Subject = "Lập trình trực quan", Credit = "4", Faculty = "KHMT", Status = "Hoàn thành", ID = 3, Semester = "Học kì I, 2018-2019" });
            StudentScoreItems.Add(new Score() { IDSubject = "IT008", Subject = "Lập trình trực quan", Credit = "4", Faculty = "KHMT", Status = "Hoàn thành", ID = 4, Semester = "Học kì II, 2018-2019" });
            StudentScoreItems.Add(new Score() { IDSubject = "IT008", Subject = "Lập trình trực quan", Credit = "4", Faculty = "KHMT", Status = "Hoàn thành", ID = 5, Semester = "Học kì I, 2018-2019" });
            StudentScoreItems.Add(new Score() { IDSubject = "IT008", Subject = "Lập trình trực quan", Credit = "4", Faculty = "KHMT", Status = "Hoàn thành", ID = 6, Semester = "Học kì I, 2018-2019" });
            StudentScoreItems.Add(new Score() { IDSubject = "IT001", Subject = "Nhập môn lập trình", Credit = "4", Faculty = "KHMT", Status = "Hoàn thành", ID = 7, Semester = "Học kì II, 2018-2019" });


            StudentTrainingScoreItems.Add(new TrainingScore() { Event = "ACM ICPC Contest", Faculty = "KHMT", Type = "Học thuật", Score = 30, Semester = "Học kì I, 2018-2019" });
            StudentTrainingScoreItems.Add(new TrainingScore() { Event = "Ăn quả nhớ kẻ trồng cây", Faculty = "KHMT", Type = "Văn nghệ", Score = 30, Semester = "Học kì II, 2018-2019" });
            StudentTrainingScoreItems.Add(new TrainingScore() { Event = "Đua xe với mpz", Faculty = "KHMT", Type = "Thể thao", Score = -5, Semester = "Học kì II, 2018-2019" });
            StudentTrainingScoreItems.Add(new TrainingScore() { Event = "Hiến máu nhân đạo", Faculty = "KHMT", Type = "Tình nguyện", Score = 30, Semester = "Học kì II, 2018-2019" });
            StudentTrainingScoreItems.Add(new TrainingScore() { Event = "Thủ lĩnh sinh viên", Faculty = "KHMT", Type = "Ngoại khóa", Score = 30, Semester = "Học kì I, 2018-2019" });
            StudentTrainingScoreItems.Add(new TrainingScore() { Event = "Phương pháp học tập bậc đại học", Faculty = "CNPM", Type = "Chia sẻ", Score = 30, Semester = "Học kì II, 2018-2019" });

            Semesters = new ObservableCollection<string>(StudentScoreItems.Select(x => x.Semester).Distinct().ToList());
            UpdateScoreBoard(Semesters.Last());
            GPA = "Điểm tích lũy: 10";
            TrainingScore = "Điểm rèn luyện: 100";
        }

        private void UpdateScoreBoard(string semester)
        {
            if (semester == null)
                return;

            int size = 0;
            GPA = "Điểm tích lũy: 10";
            TrainingScore = "Điểm rèn luyện: 100";
            CurrentStudentScore = new ObservableCollection<Score>();
            foreach (Score item in StudentScoreItems)
                if (item.Semester.Contains(semester))
                {
                    Score temp = item;
                    temp.STT = size + 1;
                    CurrentStudentScore.Add(temp);
                    size += 1;
                    // Calc GPA please
                }

            int size2 = 0;
            CurrentTrainingScore = new ObservableCollection<TrainingScore>();
            foreach (TrainingScore item in StudentTrainingScoreItems)
                if (item.Semester.Contains(semester))
                {
                    TrainingScore temp = item;
                    temp.STT = size2 + 1;
                    CurrentTrainingScore.Add(temp);
                    size2 += 1;
                }
        }

    }

    public class Score
    {
        private string _idSubject;
        private string _subject;
        private string _credit;
        private string _faculty;
        private string _status;
        private int _id;
        private string _semester;
        private int _stt;

        public int STT
        {
            get => _stt;
            set => _stt = value;
        }
        public int ID
        {
            get => _id;
            set => _id = value;
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

        public string Faculty
        {
            get => _faculty;
            set => _faculty = value;
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

    }

    public class TrainingScore
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