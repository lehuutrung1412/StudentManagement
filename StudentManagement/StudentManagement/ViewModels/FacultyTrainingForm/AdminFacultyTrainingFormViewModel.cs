using StudentManagement.Commands;
using StudentManagement.Objects;
using StudentManagement.Services;
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
        #region properties
        static private ObservableCollection<FacultyCard> _storedFacultyCards = new ObservableCollection<FacultyCard>();
        public static ObservableCollection<FacultyCard> StoredFacultyCards { get => _storedFacultyCards; set => _storedFacultyCards = value; }

        static private ObservableCollection<IBaseCard> _trainingFormCards;
        static public ObservableCollection<IBaseCard> TrainingFormCards { get => _trainingFormCards; set => _trainingFormCards = value; }

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
            LoadTrainingFormCard();

            LoadFacultyCard();


            SwitchSearchButton = new RelayCommand<UserControl>((p) => { return true; }, (p) => SwitchSearchButtonFunction(p));
            SearchFacultyCards = new RelayCommand<object>((p) => { return true; }, (p) => SearchFacultyCardsFunction(p));
        }

        #region methods
        public void LoadTrainingFormCard()
        {
            //TrainingFormCards = new ObservableCollection<IBaseCard>() {
            //    new EmptyCard(),
            //    new TrainingFormCard(Guid.NewGuid(), "Chương trình chất lượng cao", 5, 1200) ,
            //    new TrainingFormCard(Guid.NewGuid(), "Chương trình tiên tiến", 3, 1123),
            //    new TrainingFormCard(Guid.NewGuid(), "Chương trình đại trà", 10, 3000),
            //    new TrainingFormCard(Guid.NewGuid(), "Chương trình chất lượng cao", 5, 1200),
            //    new TrainingFormCard(Guid.NewGuid(), "Chương trình đại trà", 10, 3000),
            //};

            var trainingForms = TrainingFormServices.Instance.LoadTrainingFormList();

            TrainingFormCards = new ObservableCollection<IBaseCard> { new EmptyCard() };

            trainingForms.ToList().ForEach(trainingForm => TrainingFormCards.Add(TrainingFormServices.Instance.ConvertTrainingFormToTrainingFormCard(trainingForm)));
        }

        public void LoadFacultyCard()
        {
            //StoredFacultyCards = new ObservableCollection<FacultyCard>()
            //{

            //    new FacultyCard(Guid.NewGuid(), "Khoa học máy tính 1", new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            //    new FacultyCard(Guid.NewGuid(), "Khoa học máy tính 2", new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            //    new FacultyCard(Guid.NewGuid(), "Khoa học máy tính 3", new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            //    new FacultyCard(Guid.NewGuid(), "Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            //    new FacultyCard(Guid.NewGuid(), "Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            //    new FacultyCard(Guid.NewGuid(), "Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            //    new FacultyCard(Guid.NewGuid(), "Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            //    new FacultyCard(Guid.NewGuid(), "Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            //    new FacultyCard(Guid.NewGuid(), "Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            //    new FacultyCard(Guid.NewGuid(), "Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            //    new FacultyCard(Guid.NewGuid(), "Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            //    new FacultyCard(Guid.NewGuid(), "Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            //    new FacultyCard(Guid.NewGuid(), "Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            //    new FacultyCard(Guid.NewGuid(), "Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            //    new FacultyCard(Guid.NewGuid(), "Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            //    new FacultyCard(Guid.NewGuid(), "Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            //    new FacultyCard(Guid.NewGuid(), "Khoa học máy tính 4" , new DateTime(2015, 12, 31), 1500, "Đại trà, Chất lượng cao, Tiên tiến, Tài năng"),
            //};
            var faculties = FacultyServices.Instance.LoadFacultyList();

            StoredFacultyCards = new ObservableCollection<FacultyCard>();

            faculties.ToList().ForEach(faculty => StoredFacultyCards.Add(FacultyServices.Instance.ConvertFacultyToFacultyCard(faculty)));

            FacultyCards = new ObservableCollection<FacultyCard>(StoredFacultyCards.Select(el => el));
        }


        public void SearchFacultyCardsFunction(object p)
        {
            var tmp = StoredFacultyCards.Where(x => !IsFirstSearchButtonEnabled ?
                                                    vietnameseStringNormalizer.Normalize(x.DisplayName).Contains(vietnameseStringNormalizer.Normalize(SearchQuery))
                                                    : vietnameseStringNormalizer.Normalize(x.CacHeDaoTao).Contains(vietnameseStringNormalizer.Normalize(SearchQuery)));
            FacultyCards.Clear();
            foreach (FacultyCard card in tmp)
                FacultyCards.Add(card);
        }
        #endregion
    }
}
