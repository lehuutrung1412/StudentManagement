using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Services;
using StudentManagement.Utils;
using StudentManagement.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static StudentManagement.Services.LoginServices;

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
                SearchSubjectClassCardsFunction();
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
            LoginServices.UpdateCurrentUser += UpdateCurrentUser;

            SwitchSearchButton = new RelayCommand<UserControl>((p) => { return true; }, (p) => SwitchSearchButtonFunction(p));
            SearchSubjectClassCards = new RelayCommand<object>((p) => { return true; }, (p) => SearchSubjectClassCardsFunction());
            ShowSubjectClassDetail = new RelayCommand<UserControl>((p) => { return p != null; }, (p) => ShowSubjectClassDetailFunction(p));
        }

        #region methods
        public void LoadSubjectClassCards()
        {
            var subjectClasses = LoadSubjectClassListByRole();

            StoredSubjectClassCards = new ObservableCollection<SubjectClassCard>();
            SubjectClassCards = new ObservableCollection<SubjectClassCard>();

            subjectClasses.ForEach(subjectClass => StoredSubjectClassCards.Add(SubjectClassServices.Instance.ConvertSubjectClassToSubjectClassCard(subjectClass)));

            foreach (var subjectClass in StoredSubjectClassCards)
            {
                SubjectClassCards.Add(subjectClass);
            }

            #region temporary code
            /*
            StoredSubjectClassCards = new ObservableCollection<SubjectClassCard>()
            {
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 50, "Nguyễn Tấn Toàn", "IT008", "Lập trình trực quan"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 150, "Nguyễn Thị Quý", "SE104", "Nhập môn CNPM"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 20, "Nguyễn Thị Quý", "IT009", "Mạng máy tính"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu test tên siêu dài"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu 1"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 30, "Nguyễn Tấn Toàn", "IT010", "Cơ sở dữ liệu 2"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 30, "Nguyễn Tấn Toàn", "IT011", "Cơ sở dữ liệu test tên siêu dài test tên siêu dài 3"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 30, "Nguyễn Tấn Toàn", "IT012", "Cơ sở dữ liệu 4"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 30, "Nguyễn Tấn Toàn", "IT013", "Cơ sở dữ liệu 5"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 30, "Nguyễn Tấn Toàn", "IT014", "Cơ sở dữ liệu test tên siêu dài 6"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu 7"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu test tên siêu dài 8"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 40, "Nguyễn Tấn Toàn", "CS231", "Xử lý ngôn ngữ tự nhiên")
            };

            //SubjectClassCards = new ObservableCollection<SubjectClassCard>(StoredSubjectClassCards.Select(el => el));
            // Use this for displaying in design
            SubjectClassCards = new ObservableCollection<SubjectClassCard>()
            {
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 50, "Nguyễn Tấn Toàn", "IT008", "Lập trình trực quan"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 150, "Nguyễn Thị Quý", "SE104", "Nhập môn CNPM"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 20, "Nguyễn Thị Quý", "IT009", "Mạng máy tính"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu test tên siêu dài"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu 1"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 30, "Nguyễn Tấn Toàn", "IT010", "Cơ sở dữ liệu 2"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 30, "Nguyễn Tấn Toàn", "IT011", "Cơ sở dữ liệu test tên siêu dài test tên siêu dài 3"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 30, "Nguyễn Tấn Toàn", "IT012", "Cơ sở dữ liệu 4"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 30, "Nguyễn Tấn Toàn", "IT013", "Cơ sở dữ liệu 5"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 30, "Nguyễn Tấn Toàn", "IT014", "Cơ sở dữ liệu test tên siêu dài 6"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu 7"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu test tên siêu dài 8"),
                new SubjectClassCard(Guid.NewGuid(), "IT008.M11.KHTN", 40, "Nguyễn Tấn Toàn", "CS231", "Xử lý ngôn ngữ tự nhiên")
            };
            */
            #endregion
        }

        public List<SubjectClass> LoadSubjectClassListByRole()
        {
            var subjectClasses = SubjectClassServices.Instance.LoadSubjectClassList();

            switch (LoginServices.CurrentUser.UserRole.Role)
            {
                case "Admin":
                    return subjectClasses.Where(el => true).ToList();
                case "Giáo viên":
                    return subjectClasses.Where(el => true).ToList();
                default:
                    return subjectClasses.Where(el => false).ToList();
            }
        }

        public void SwitchSearchButtonFunction(UserControl p)
        {
            IsFirstSearchButtonEnabled = !IsFirstSearchButtonEnabled;
        }

        public void SearchSubjectClassCardsFunction()
        {
            var tmp = StoredSubjectClassCards.Where(x => !IsFirstSearchButtonEnabled ?
                                                    vietnameseStringNormalizer.Normalize(x.SubjectOfClass.DisplayName + " " + x.Code).Contains(vietnameseStringNormalizer.Normalize(SearchQuery))
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

        #region eventhandler
        private void UpdateCurrentUser(object sender, LoginEvent e)
        {
            LoadSubjectClassCards();
        }
        #endregion
    }
}
