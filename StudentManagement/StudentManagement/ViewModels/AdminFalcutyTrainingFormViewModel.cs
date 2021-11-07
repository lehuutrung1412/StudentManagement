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
    public class AdminFalcutyTrainingFormViewModel : BaseViewModel
    {
        public class TrainingFormCard : BaseViewModel
        {
            private string _tenHeDaoTao;
            private int _soLuongKhoa;
            private int _soLuongSinhVien;

            public TrainingFormCard() { }

            public TrainingFormCard(string tenHeDaoTao, int soLuongKhoa, int soLuongSinhVien)
            {
                _tenHeDaoTao = tenHeDaoTao;
                _soLuongKhoa = soLuongKhoa;
                _soLuongSinhVien = soLuongSinhVien;
            }

            public void CopyCardInfo(TrainingFormCard anotherTrainingFormCard)
            {
                TenHeDaoTao = anotherTrainingFormCard._tenHeDaoTao;
                SoLuongKhoa = anotherTrainingFormCard._soLuongKhoa;
                SoLuongSinhVien = anotherTrainingFormCard._soLuongSinhVien;
            }

            public string TenHeDaoTao
            {
                get { return _tenHeDaoTao; }
                set
                {
                    _tenHeDaoTao = value;
                    OnPropertyChanged();
                }
            }
            public int SoLuongKhoa
            {
                get { return _soLuongKhoa; }
                set
                {
                    _soLuongKhoa = value;
                    OnPropertyChanged();
                }
            }
            public int SoLuongSinhVien
            {
                get { return _soLuongSinhVien; }
                set
                {
                    _soLuongSinhVien = value;
                    OnPropertyChanged();
                }
            }
        }

        public class FalcutyCard
        {
            private string _tenKhoa;
            private DateTime _ngayThanhLap;
            private int _soLuongSinhVien;
            private string _cacHeDaoTao;

            public FalcutyCard(string tenKhoa, DateTime ngayThanhLap, int soLuongSinhVien, string cacHeDaoTao)
            {
                _tenKhoa = tenKhoa;
                _ngayThanhLap = ngayThanhLap;
                _soLuongSinhVien = soLuongSinhVien;
                _cacHeDaoTao = cacHeDaoTao;
            }

            public string TenKhoa { get => _tenKhoa; set => _tenKhoa = value; }
            public DateTime NgayThanhLap { get => _ngayThanhLap; set => _ngayThanhLap = value; }
            public int SoLuongSinhVien { get => _soLuongSinhVien; set => _soLuongSinhVien = value; }
            public string CacHeDaoTao { get => _cacHeDaoTao; set => _cacHeDaoTao = value; }


        }

        static private ObservableCollection<TrainingFormCard> _trainingFormCards;

        static public ObservableCollection<TrainingFormCard> TrainingFormCards { get => _trainingFormCards; set => _trainingFormCards = value; }

        static private ObservableCollection<FalcutyCard> _falcutyCards;

        static public ObservableCollection<FalcutyCard> FalcutyCards { get => _falcutyCards; set => _falcutyCards = value; }

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
        public void SwitchSearchButtonFunction(UserControl p)
        {
            this.IsFirstSearchButtonEnabled = !IsFirstSearchButtonEnabled;
        }

        public AdminFalcutyTrainingFormViewModel()
        {
            TrainingFormCards = new ObservableCollection<TrainingFormCard>() {
                new TrainingFormCard("Chương trình chất lượng cao", 5, 1200),
                new TrainingFormCard("Chương trình tiên tiến", 3, 1123),
                new TrainingFormCard("Chương trình đại trà", 10, 3000),
                new TrainingFormCard("Chương trình chất lượng cao", 5, 1200),
                new TrainingFormCard("Chương trình đại trà", 10, 3000),
            };

            FalcutyCards = new ObservableCollection<FalcutyCard>()
            {
                new FalcutyCard("Khoa học máy tính 1", new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FalcutyCard("Khoa học máy tính 2", new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FalcutyCard("Khoa học máy tính 3", new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FalcutyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FalcutyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FalcutyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FalcutyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FalcutyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FalcutyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FalcutyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FalcutyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FalcutyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FalcutyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FalcutyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FalcutyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FalcutyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FalcutyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            };

            this.SwitchSearchButton = new RelayCommand<UserControl>((p) => { return true; }, (p) => SwitchSearchButtonFunction(p));
        }
    }
}
