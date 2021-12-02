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
        static private ObservableCollection<SubjectCard> _storedSubjectCards;
        public static ObservableCollection<SubjectCard> StoredSubjectCards { get => _storedSubjectCards; set => _storedSubjectCards = value; }

        private static ObservableCollection<SubjectCard> _subjectCards;

        public static ObservableCollection<SubjectCard> SubjectCards { get => _subjectCards; set => _subjectCards = value; }

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

        public ICommand SearchSubjectCards { get => _searchSubjectCards; set => _searchSubjectCards = value; }

        private ICommand _searchSubjectCards;

        public ICommand ShowSubjectClassDetail { get; set; }

        #endregion 

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

            //SubjectCards = new ObservableCollection<SubjectCard>(StoredSubjectCards.Select(el => el));
            // Use this for displaying in design
            SubjectCards = new ObservableCollection<SubjectCard>() {
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
            SwitchSearchButton = new RelayCommand<UserControl>((p) => { return true; }, (p) => SwitchSearchButtonFunction(p));
            SearchSubjectCards = new RelayCommand<object>((p) => { return true; }, (p) => SearchSubjectCardsFunction(p));
            ShowSubjectClassDetail = new RelayCommand<UserControl>((p) => { return true; }, (p) => ShowSubjectClassDetailFunction(p));
        }

        #region methods
        public void SwitchSearchButtonFunction(UserControl p)
        {
            IsFirstSearchButtonEnabled = !IsFirstSearchButtonEnabled;
        }

        public void SearchSubjectCardsFunction(object p)
        {
            var tmp = StoredSubjectCards.Where(x => !IsFirstSearchButtonEnabled ?
                                                    vietnameseStringNormalizer.Normalize(x.TenMon + " " + x.MaMon).Contains(vietnameseStringNormalizer.Normalize(SearchQuery))
                                                    : vietnameseStringNormalizer.Normalize(x.GiaoVien).Contains(vietnameseStringNormalizer.Normalize(SearchQuery)));
            SubjectCards.Clear();
            foreach (SubjectCard card in tmp)
                SubjectCards.Add(card);
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
