using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
                TenHeDaoTao = tenHeDaoTao;
                SoLuongKhoa = soLuongKhoa;
                SoLuongSinhVien = soLuongSinhVien;
            }

            public string TenHeDaoTao { get => _tenHeDaoTao; set => _tenHeDaoTao = value; }
            public int SoLuongKhoa { get => _soLuongKhoa; set => _soLuongKhoa = value; }
            public int SoLuongSinhVien { get => _soLuongSinhVien; set => _soLuongSinhVien = value; }

            public void CopyCardInfo(TrainingFormCard anotherTrainingFormCard)
            {
                TenHeDaoTao = anotherTrainingFormCard.TenHeDaoTao;
                SoLuongKhoa = anotherTrainingFormCard.SoLuongKhoa;
                SoLuongSinhVien = anotherTrainingFormCard.SoLuongSinhVien;
            }

            public void RunOnPropertyChanged()
            {
                foreach (PropertyInfo propertyInfo in this.GetType().GetProperties())
                {
                    OnPropertyChanged(propertyInfo.Name);
                }
            }
        }

        public class FalcutyCard : BaseViewModel
        {
            private string _tenKhoa;
            private DateTime _ngayThanhLap;
            private int _soLuongSinhVien;
            private string _cacHeDaoTao;

            public FalcutyCard() { }
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

            public void CopyCardInfo(FalcutyCard anotherFalcutyCard)
            {
                TenKhoa = anotherFalcutyCard.TenKhoa;
                NgayThanhLap = anotherFalcutyCard.NgayThanhLap;
                SoLuongSinhVien = anotherFalcutyCard.SoLuongSinhVien;
                SoLuongSinhVien = anotherFalcutyCard.SoLuongSinhVien;
            }

            public void RunOnPropertyChanged()
            {
                foreach (PropertyInfo propertyInfo in this.GetType().GetProperties())
                {
                    OnPropertyChanged(propertyInfo.Name);
                }
            }


        }

        static private ObservableCollection<FalcutyCard> _storedFalcutyCards;
        public static ObservableCollection<FalcutyCard> StoredFalcutyCards { get => _storedFalcutyCards; set => _storedFalcutyCards = value; }

        static private ObservableCollection<TrainingFormCard> _trainingFormCards;
        static public ObservableCollection<TrainingFormCard> TrainingFormCards { get => _trainingFormCards; set => _trainingFormCards = value; }

        static private ObservableCollection<FalcutyCard> _falcutyCards = new ObservableCollection<FalcutyCard>();

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
        public ICommand SearchFalcutyCards { get => _searchFalcutyCards; set => _searchFalcutyCards = value; }

        private ICommand _switchSearchButton;

        private ICommand _searchFalcutyCards;

        private bool _isFirstSearchButtonEnabled = true;
        public void SwitchSearchButtonFunction(UserControl p)
        {
            this.IsFirstSearchButtonEnabled = !IsFirstSearchButtonEnabled;
        }

        public void SearchFalcutyCardsFunction(object p)
        {
            var tmp = StoredFalcutyCards.Where(x => x.TenKhoa.ToLower().Contains(SearchQuery.ToLower()));
            FalcutyCards.Clear();
            foreach (FalcutyCard card in tmp)
                FalcutyCards.Add(card);
        }

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
            }
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


            StoredFalcutyCards = new ObservableCollection<FalcutyCard>()
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

            FalcutyCards = new ObservableCollection<FalcutyCard>(StoredFalcutyCards.Select(el => el));

            this.SwitchSearchButton = new RelayCommand<UserControl>((p) => { return true; }, (p) => SwitchSearchButtonFunction(p));
            this.SearchFalcutyCards = new RelayCommand<object>((p) => { return true; }, (p) => SearchFalcutyCardsFunction(p));
        }
    }
}
