using StudentManagement.Commands;
using StudentManagement.Objects;
using StudentManagement.Utils;
using StudentManagement.Views;
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
    public class AdminSubjectClassViewModel : BaseViewModel
    {
        #region properties
        static private ObservableCollection<SubjectClassCard> _storedSubjectClassCards;
        public static ObservableCollection<SubjectClassCard> StoredSubjectClassCards { get => _storedSubjectClassCards; set => _storedSubjectClassCards = value; }

        private static ObservableCollection<SubjectClassCard> _subjectClassCards;

        public static ObservableCollection<SubjectClassCard> SubjectClassCards { get => _subjectClassCards; set => _subjectClassCards = value; }

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

        public ICommand SearchSubjectClassCards { get => _searchSubjectClassCards; set => _searchSubjectClassCards = value; }

        private ICommand _searchSubjectClassCards;

        public ICommand ShowSubjectClassDetail { get; set; }

        #endregion 

        public AdminSubjectClassViewModel()
        {
            StoredSubjectClassCards = new ObservableCollection<SubjectClassCard>() {
                new SubjectClassCard(Guid.NewGuid(), 50, "Nguyễn Tấn Toàn", "IT008", "Lập trình trực quan"),
                new SubjectClassCard(Guid.NewGuid(), 150, "Nguyễn Thị Quý", "SE104", "Nhập môn CNPM"),
                new SubjectClassCard(Guid.NewGuid(), 20, "Nguyễn Thị Quý", "IT009", "Mạng máy tính"),
                new SubjectClassCard(Guid.NewGuid(), 30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu test tên siêu dài"),
                new SubjectClassCard(Guid.NewGuid(), 30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu 1"),
                new SubjectClassCard(Guid.NewGuid(), 30, "Nguyễn Tấn Toàn", "IT010", "Cơ sở dữ liệu 2"),
                new SubjectClassCard(Guid.NewGuid(), 30, "Nguyễn Tấn Toàn", "IT011", "Cơ sở dữ liệu test tên siêu dài test tên siêu dài 3"),
                new SubjectClassCard(Guid.NewGuid(), 30, "Nguyễn Tấn Toàn", "IT012", "Cơ sở dữ liệu 4"),
                new SubjectClassCard(Guid.NewGuid(), 30, "Nguyễn Tấn Toàn", "IT013", "Cơ sở dữ liệu 5"),
                new SubjectClassCard(Guid.NewGuid(), 30, "Nguyễn Tấn Toàn", "IT014", "Cơ sở dữ liệu test tên siêu dài 6"),
                new SubjectClassCard(Guid.NewGuid(), 30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu 7"),
                new SubjectClassCard(Guid.NewGuid(), 30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu test tên siêu dài 8"),
                new SubjectClassCard(Guid.NewGuid(), 40, "Nguyễn Tấn Toàn", "CS231", "Xử lý ngôn ngữ tự nhiên")
            };

            //SubjectClassCards = new ObservableCollection<SubjectClassCard>(StoredSubjectClassCards.Select(el => el));
            // Use this for displaying in design
            SubjectClassCards = new ObservableCollection<SubjectClassCard>() {
                new SubjectClassCard(Guid.NewGuid(), 50, "Nguyễn Tấn Toàn", "IT008", "Lập trình trực quan"),
                new SubjectClassCard(Guid.NewGuid(), 150, "Nguyễn Thị Quý", "SE104", "Nhập môn CNPM"),
                new SubjectClassCard(Guid.NewGuid(), 20, "Nguyễn Thị Quý", "IT009", "Mạng máy tính"),
                new SubjectClassCard(Guid.NewGuid(), 30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu test tên siêu dài"),
                new SubjectClassCard(Guid.NewGuid(), 30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu 1"),
                new SubjectClassCard(Guid.NewGuid(), 30, "Nguyễn Tấn Toàn", "IT010", "Cơ sở dữ liệu 2"),
                new SubjectClassCard(Guid.NewGuid(), 30, "Nguyễn Tấn Toàn", "IT011", "Cơ sở dữ liệu test tên siêu dài test tên siêu dài 3"),
                new SubjectClassCard(Guid.NewGuid(), 30, "Nguyễn Tấn Toàn", "IT012", "Cơ sở dữ liệu 4"),
                new SubjectClassCard(Guid.NewGuid(), 30, "Nguyễn Tấn Toàn", "IT013", "Cơ sở dữ liệu 5"),
                new SubjectClassCard(Guid.NewGuid(), 30, "Nguyễn Tấn Toàn", "IT014", "Cơ sở dữ liệu test tên siêu dài 6"),
                new SubjectClassCard(Guid.NewGuid(), 30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu 7"),
                new SubjectClassCard(Guid.NewGuid(), 30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu test tên siêu dài 8"),
                new SubjectClassCard(Guid.NewGuid(), 40, "Nguyễn Tấn Toàn", "CS231", "Xử lý ngôn ngữ tự nhiên")
            };
            SwitchSearchButton = new RelayCommand<UserControl>((p) => { return true; }, (p) => SwitchSearchButtonFunction(p));
            SearchSubjectClassCards = new RelayCommand<object>((p) => { return true; }, (p) => SearchSubjectClassCardsFunction(p));
            ShowSubjectClassDetail = new RelayCommand<UserControl>((p) => { return true; }, (p) => ShowSubjectClassDetailFunction(p));
        }

        #region methods
        public void SwitchSearchButtonFunction(UserControl p)
        {
            IsFirstSearchButtonEnabled = !IsFirstSearchButtonEnabled;
        }

        public void SearchSubjectClassCardsFunction(object p)
        {
            var tmp = StoredSubjectClassCards.Where(x => !IsFirstSearchButtonEnabled ?
                                                    vietnameseStringNormalizer.Normalize(x.TenMon + " " + x.MaMon).Contains(vietnameseStringNormalizer.Normalize(SearchQuery))
                                                    : vietnameseStringNormalizer.Normalize(x.GiaoVien).Contains(vietnameseStringNormalizer.Normalize(SearchQuery)));
            SubjectClassCards.Clear();
            foreach (SubjectClassCard card in tmp)
                SubjectClassCards.Add(card);
        }

        public void ShowSubjectClassDetailFunction(UserControl cardComponent)
        {
            SubjectClassDetail subjectClassDetail = new SubjectClassDetail
            {
                DataContext = new SubjectClassDetailViewModel(cardComponent)
            };
            subjectClassDetail.Show();
        }
        #endregion methods
    }
}
