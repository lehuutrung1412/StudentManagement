using StudentManagement.Commands;
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
    public class AdminSubjectClassViewModel : BaseViewModel
    {
        public class SubjectCard
        {
            private int _siSo;
            private string _giaoVien;
            private string _maMon;
            private string _tenMon;

            public SubjectCard(int siSo, string giaoVien, string maMon, string tenMon)
            {
                SiSo = siSo;
                GiaoVien = giaoVien;
                MaMon = maMon;
                TenMon = tenMon;
            }

            public int SiSo { get => _siSo; set => _siSo = value; }
            public string GiaoVien { get => _giaoVien; set => _giaoVien = value; }
            public string MaMon { get => _maMon; set => _maMon = value; }
            public string TenMon { get => _tenMon; set => _tenMon = value; }
        }

        static private ObservableCollection<SubjectCard> _storedSubjectCards;
        public static ObservableCollection<SubjectCard> StoredSubjectCards { get => _storedSubjectCards; set => _storedSubjectCards = value; }

        public ObservableCollection<SubjectCard> _subjectCards;

        public ObservableCollection<SubjectCard> SubjectCards { get => _subjectCards; set => _subjectCards = value; }

        public bool IsFirstSearchButtonEnabled
        {
            get { return _isFirstSearchButtonEnabled; }
            set
            {
                _isFirstSearchButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        public ICommand SwitchSearchButton { get => _switchSearchButton; set => _switchSearchButton = value; }

        private ICommand _switchSearchButton;

        private bool _isFirstSearchButtonEnabled = true;

        public AdminSubjectClassViewModel()
        {
            StoredSubjectCards = new ObservableCollection<SubjectCard>() {
                new SubjectCard(50, "Nguyễn Tấn Toàn", "IT008", "Lập trình trực quan"),
                new SubjectCard(150, "Nguyễn Thị Quý", "SE104", "Nhập môn CNPM"),
                new SubjectCard(20, "Nguyễn Thị Quý", "IT009", "Mạng máy tính"),
                new SubjectCard(30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu test tên siêu dài"),
                new SubjectCard(30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu 1"),
                new SubjectCard(30, "Nguyễn Tấn Toàn", "IT010", "Cơ sở dữ liệu 2"),
                new SubjectCard(30, "Nguyễn Tấn Toàn", "IT011", "Cơ sở dữ liệu test tên siêu dài test tên siêu dài 3"),
                new SubjectCard(30, "Nguyễn Tấn Toàn", "IT012", "Cơ sở dữ liệu 4"),
                new SubjectCard(30, "Nguyễn Tấn Toàn", "IT013", "Cơ sở dữ liệu 5"),
                new SubjectCard(30, "Nguyễn Tấn Toàn", "IT014", "Cơ sở dữ liệu test tên siêu dài 6"),
                new SubjectCard(30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu 7"),
                new SubjectCard(30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu test tên siêu dài 8"),
                new SubjectCard(40, "Nguyễn Tấn Toàn", "CS231", "Xử lý ngôn ngữ tự nhiên")
            };

            SubjectCards = new ObservableCollection<SubjectCard>(StoredSubjectCards.Select(el => el));

            this.SwitchSearchButton = new RelayCommand<UserControl>((p) => { return true; }, (p) => SwitchSearchButtonFunction(p));
        }

        public void SwitchSearchButtonFunction(UserControl p)
        {
            this.IsFirstSearchButtonEnabled = !IsFirstSearchButtonEnabled;
        }
    }
}
