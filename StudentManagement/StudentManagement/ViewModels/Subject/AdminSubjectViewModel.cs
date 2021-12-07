using StudentManagement.Commands;
using StudentManagement.Objects;
using StudentManagement.Services;
using StudentManagement.Utils;
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
    class AdminSubjectViewModel : BaseViewModel
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
                SearchSubjectCardsFunction();
                OnPropertyChanged();
            }
        }
        #endregion

        #region icommand
        public ICommand SwitchSearchButton { get => _switchSearchButton; set => _switchSearchButton = value; }

        private ICommand _switchSearchButton;

        public ICommand SearchSubjectCards { get => _searchSubjectCards; set => _searchSubjectCards = value; }

        private ICommand _searchSubjectCards;

        #endregion

        public AdminSubjectViewModel()
        {
            LoadSubjectCards();
            SwitchSearchButton = new RelayCommand<UserControl>((p) => { return true; }, (p) => SwitchSearchButtonFunction(p));
            SearchSubjectCards = new RelayCommand<object>((p) => { return true; }, (p) => SearchSubjectCardsFunction());
        }

        #region methods

        public void LoadSubjectCards()
        {
            var subjectes = SubjectServices.Instance.LoadSubjectList();

            StoredSubjectCards = new ObservableCollection<SubjectCard>();
            SubjectCards = new ObservableCollection<SubjectCard>();

            subjectes.ToList().ForEach(subject => StoredSubjectCards.Add(SubjectServices.Instance.ConvertSubjectToSubjectCard(subject)));

            foreach (var subject in StoredSubjectCards)
            {
                SubjectCards.Add(subject);
            }
        }

        public void SwitchSearchButtonFunction(UserControl p)
        {
            IsFirstSearchButtonEnabled = !IsFirstSearchButtonEnabled;
        }

        public void SearchSubjectCardsFunction()
        {
            var tmp = StoredSubjectCards.Where(x => !IsFirstSearchButtonEnabled ?
                                                    vietnameseStringNormalizer.Normalize(x.Code).Contains(vietnameseStringNormalizer.Normalize(SearchQuery))
                                                    : vietnameseStringNormalizer.Normalize(x.DisplayName).Contains(vietnameseStringNormalizer.Normalize(SearchQuery)));
            SubjectCards.Clear();
            foreach (SubjectCard card in tmp)
                SubjectCards.Add(card);
        }

        #endregion methods
    }
}