using StudentManagement.Commands;
using StudentManagement.Objects;
using StudentManagement.Utils;
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
    public class AdminFacultyTrainingFormViewModel : BaseViewModel
    {
        #region class
        
        public class FacultyCard : BaseObjectWithBaseViewModel, IBaseCard
        {
            private string _tenKhoa;
            private DateTime _ngayThanhLap;
            private int _soLuongSinhVien;
            private string _cacHeDaoTao;

            public FacultyCard() { }
            public FacultyCard(string tenKhoa, DateTime ngayThanhLap, int soLuongSinhVien, string cacHeDaoTao)
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

        #endregion

        #region properties
        static private ObservableCollection<FacultyCard> _storedFacultyCards;
        public static ObservableCollection<FacultyCard> StoredFacultyCards { get => _storedFacultyCards; set => _storedFacultyCards = value; }

        static private ObservableCollection<TrainingFormCard> _trainingFormCards;
        static public ObservableCollection<TrainingFormCard> TrainingFormCards { get => _trainingFormCards; set => _trainingFormCards = value; }

        static private ObservableCollection<FacultyCard> _FacultyCards = new ObservableCollection<FacultyCard>();

        static public ObservableCollection<FacultyCard> FacultyCards { get => _FacultyCards; set => _FacultyCards = value; }

        public VietnameseStringNormalizer vietnameseStringNormalizer = VietnameseStringNormalizer.Instance;
        public bool IsFirstSearchButtonEnabled
        {
            get { return _isFirstSearchButtonEnabled; }
            set
            {
                _isFirstSearchButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _isFirstSearchButtonEnabled = false;
        public void SwitchSearchButtonFunction(UserControl p)
        {
            IsFirstSearchButtonEnabled = !IsFirstSearchButtonEnabled;
        }

        private string _searchQuery = "";
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region icommand
        public ICommand SwitchSearchButton { get => _switchSearchButton; set => _switchSearchButton = value; }

        private ICommand _switchSearchButton;
        public ICommand SearchFacultyCards { get => _searchFacultyCards; set => _searchFacultyCards = value; }

        private ICommand _searchFacultyCards;
        #endregion




        public AdminFacultyTrainingFormViewModel()
        {
            TrainingFormCards = new ObservableCollection<TrainingFormCard>() {
                new TrainingFormCard("Chương trình chất lượng cao", 5, 1200),
                new TrainingFormCard("Chương trình tiên tiến", 3, 1123),
                new TrainingFormCard("Chương trình đại trà", 10, 3000),
                new TrainingFormCard("Chương trình chất lượng cao", 5, 1200),
                new TrainingFormCard("Chương trình đại trà", 10, 3000),
            };


            StoredFacultyCards = new ObservableCollection<FacultyCard>()
            {
                new FacultyCard("Khoa học máy tính 1", new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FacultyCard("Khoa học máy tính 2", new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FacultyCard("Khoa học máy tính 3", new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FacultyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FacultyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FacultyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FacultyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FacultyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FacultyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FacultyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FacultyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FacultyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FacultyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FacultyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FacultyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FacultyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
                new FacultyCard("Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            };

            FacultyCards = new ObservableCollection<FacultyCard>(StoredFacultyCards.Select(el => el));

            SwitchSearchButton = new RelayCommand<UserControl>((p) => { return true; }, (p) => SwitchSearchButtonFunction(p));
            SearchFacultyCards = new RelayCommand<object>((p) => { return true; }, (p) => SearchFacultyCardsFunction(p));
        }

        #region methods
        public void SearchFacultyCardsFunction(object p)
        {
            var tmp = StoredFacultyCards.Where(x => !IsFirstSearchButtonEnabled ?
                                                    vietnameseStringNormalizer.Normalize(x.TenKhoa).Contains(vietnameseStringNormalizer.Normalize(SearchQuery))
                                                    : vietnameseStringNormalizer.Normalize(x.CacHeDaoTao).Contains(vietnameseStringNormalizer.Normalize(SearchQuery)));
            FacultyCards.Clear();
            foreach (FacultyCard card in tmp)
                FacultyCards.Add(card);
        }
        #endregion
    }
}
