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
        private static AdminFacultyTrainingFormViewModel s_instance;
        public static AdminFacultyTrainingFormViewModel Instance => s_instance ?? (s_instance = new AdminFacultyTrainingFormViewModel());

        private int _currentFacultyPageView = 1;
        private int _ItemsPerFacultyPageView = 5;

        // store all faculty cards
        static private ObservableCollection<FacultyCard> _storedFacultyCards = new ObservableCollection<FacultyCard>();
        public static ObservableCollection<FacultyCard> StoredFacultyCards { get => _storedFacultyCards; set => _storedFacultyCards = value; }

        static private ObservableCollection<IBaseCard> _trainingFormCards;
        static public ObservableCollection<IBaseCard> TrainingFormCards { get => _trainingFormCards; set => _trainingFormCards = value; }

        // store all searched faculty cards
        static private ObservableCollection<FacultyCard> _FacultyCards = new ObservableCollection<FacultyCard>();

        static public ObservableCollection<FacultyCard> FacultyCards { get => _FacultyCards; set => _FacultyCards = value; }

        // store all faculty cards in current pageview
        static private ObservableCollection<FacultyCard> _CurrentFacultyCards = new ObservableCollection<FacultyCard>();

        public static ObservableCollection<FacultyCard> CurrentFacultyCards { get => _CurrentFacultyCards; set => _CurrentFacultyCards = value; }

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
                SearchFacultyCardsFunction();
                OnPropertyChanged();
            }
        }
        #endregion

        #region icommand
        public ICommand SwitchSearchButton { get => _switchSearchButton; set => _switchSearchButton = value; }

        private ICommand _switchSearchButton;
        public ICommand SearchFacultyCards { get => _searchFacultyCards; set => _searchFacultyCards = value; }

        private ICommand _searchFacultyCards;

        public int CurrentFacultyPageView
        {
            get => _currentFacultyPageView;
            set
            {
                _currentFacultyPageView = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NumberOfFacultyPageView));
            }
        }
        public int NumberOfFacultyPageView
        {
            get
            {
                return (FacultyCards.Count - 1) / this._ItemsPerFacultyPageView + 1;
            }
        }

        public ICommand NextFacultyPageView { get => _nextFacultyPageView; set => _nextFacultyPageView = value; }
        public ICommand PreviousFacultyPageView { get => _previousFacultyPageView; set => _previousFacultyPageView = value; }
        public ICommand JumpToFacultyPageView { get => _jumpToFacultyPageView; set => _jumpToFacultyPageView = value; }

        private ICommand _nextFacultyPageView;
        private ICommand _previousFacultyPageView;
        private ICommand _jumpToFacultyPageView;
        #endregion




        public AdminFacultyTrainingFormViewModel()
        {
            AdminFacultyTrainingFormViewModel.s_instance = this;

            LoadTrainingFormCard();

            LoadFacultyCard();

            LoadFacultyByPageView();

            SwitchSearchButton = new RelayCommand<UserControl>((p) => { return true; }, (p) => SwitchSearchButtonFunction(p));
            SearchFacultyCards = new RelayCommand<object>((p) => { return true; }, (p) => SearchFacultyCardsFunction());
            NextFacultyPageView = new RelayCommand<object>((p) =>
            {
                if (CurrentFacultyPageView < NumberOfFacultyPageView)
                    return true;
                else
                    return false;
            }, (p) => NextFacultyPageViewCommand());
            PreviousFacultyPageView = new RelayCommand<object>((p) =>
            {
                if (CurrentFacultyPageView > 1)
                    return true;
                else
                    return false;
            }, (p) => PreviousFacultyPageViewCommand());
            JumpToFacultyPageView = new RelayCommand<object>((p) => { return true; }, (p) => JumpToFacultyPageViewCommand(p));
        }

        #region methods
        public void LoadFacultyByPageView()
        {
            if (CurrentFacultyPageView > NumberOfFacultyPageView)
            {
                CurrentFacultyPageView = NumberOfFacultyPageView;
            }
            else if (CurrentFacultyPageView < 1)
            {
                CurrentFacultyPageView = 1;
            }

            int minIndex = (CurrentFacultyPageView - 1) * _ItemsPerFacultyPageView;
            int maxIndex = (CurrentFacultyPageView) * _ItemsPerFacultyPageView;
            CurrentFacultyCards.Clear();
            FacultyCards.Where((el, index) => index < maxIndex && index >= minIndex).ToList().ForEach(el => CurrentFacultyCards.Add(el));

            OnPropertyChanged(nameof(CurrentFacultyPageView));
            OnPropertyChanged(nameof(NumberOfFacultyPageView));
        }

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

            var trainingForms = TrainingFormServices.Instance.LoadTrainingFormList().Where(el => el.IsDeleted != true);

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
            var faculties = FacultyServices.Instance.LoadFacultyList().Where(el => el.IsDeleted != true);

            StoredFacultyCards = new ObservableCollection<FacultyCard>();

            faculties.ToList().ForEach(faculty => StoredFacultyCards.Add(FacultyServices.Instance.ConvertFacultyToFacultyCard(faculty)));

            FacultyCards = new ObservableCollection<FacultyCard>(StoredFacultyCards.Select(el => el));
        }


        public void NextFacultyPageViewCommand()
        {
            this.CurrentFacultyPageView = Math.Min(this.CurrentFacultyPageView + 1, NumberOfFacultyPageView);
            LoadFacultyByPageView();
        }

        public void PreviousFacultyPageViewCommand()
        {
            this.CurrentFacultyPageView = Math.Max(this.CurrentFacultyPageView - 1, 1);
            LoadFacultyByPageView();
        }

        public void JumpToFacultyPageViewCommand(object p)
        {
            string temp = (p as string) != "" ? (p as string) : "1";

            int pageNumber;

            bool success = int.TryParse(temp, out pageNumber);

            if (success)
            {
                this.CurrentFacultyPageView = pageNumber;
            }


            LoadFacultyByPageView();
        }

        public void SearchFacultyCardsFunction()
        {
            var tmp = StoredFacultyCards.Where(x => !IsFirstSearchButtonEnabled ?
                                                    vietnameseStringNormalizer.Normalize(x.DisplayName).Contains(vietnameseStringNormalizer.Normalize(SearchQuery))
                                                    : vietnameseStringNormalizer.Normalize(x.CacHeDaoTao).Contains(vietnameseStringNormalizer.Normalize(SearchQuery)));
            FacultyCards.Clear();

            foreach (FacultyCard card in tmp)
                FacultyCards.Add(card);
            LoadFacultyByPageView();
        }
        #endregion
    }
}
