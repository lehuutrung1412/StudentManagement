using ExcelDataReader;
using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Services;
using StudentManagement.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class AdminSubjectViewModel : BaseViewModel
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
        public ICommand AddFromExcelCommand { get; set; }

        #endregion

        public AdminSubjectViewModel()
        {
            LoadSubjectCards();
            SwitchSearchButton = new RelayCommand<object>((p) => { return true; }, (p) => SwitchSearchButtonFunction(p));
            SearchSubjectCards = new RelayCommand<object>((p) => { return true; }, (p) => SearchSubjectCardsFunction());
            AddFromExcelCommand = new RelayCommand<object>((p) => { return true; }, (p) => AddFromExcelFunction());

        }

        #region methods

        public void LoadDataBehind()
        {
            var subjectes = SubjectServices.Instance.LoadSubjectList().Where(subject=>subject.IsDeleted == false).ToList();

            StoredSubjectCards = new ObservableCollection<SubjectCard>();
            
            subjectes.ToList().ForEach(subject => StoredSubjectCards.Add(SubjectServices.Instance.ConvertSubjectToSubjectCard(subject)));
        }

        public void LoadDataFront()
        {
            SubjectCards.Clear();
            foreach (var subject in StoredSubjectCards)
            {
                SubjectCards.Add(subject);
            }
        }
        public void LoadSubjectCards()
        {
            LoadDataBehind();
            SubjectCards = new ObservableCollection<SubjectCard>();
            LoadDataFront();
        }

        public void SwitchSearchButtonFunction(object p)
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

        public void AddFromExcelFunction()
        {
            using (OpenFileDialog op = new OpenFileDialog() { Filter = "Excel|*.xls;*.xlsx;" })
            {
                try
                {
                    if (op.ShowDialog() == DialogResult.OK)
                    {
                        DataTableCollection dataSheets;
                        using (var stream = File.Open(op.FileName, FileMode.Open, FileAccess.Read))
                        {
                            using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                                });
                                dataSheets = result.Tables;
                            }
                        }
                        DataTable data = dataSheets[0];

                        int count = 0;

                        foreach (DataRow rowSubject in data.Rows)
                        {
                            //Tạo 1 subject từ row
                            var newSubject = new Subject()
                            {
                                Id = Guid.NewGuid(),
                                Code = Convert.ToString(rowSubject[0]),
                                DisplayName = Convert.ToString(rowSubject[1]),
                                Credit = Convert.ToInt32(rowSubject[2]),
                                Describe = Convert.ToString(rowSubject[3]),
                                IsDeleted = false,
                            };

                            //Kiểm tra môn mới có trùng Code với StoredSubjectCard
                            var validSubjectCard = StoredSubjectCards.Where(card => card.Code == newSubject.Code).FirstOrDefault();
                            if (validSubjectCard != null)
                            {
                                if (validSubjectCard.DisplayName == newSubject.DisplayName &&
                                    validSubjectCard.Credit == newSubject.Credit &&
                                    validSubjectCard.Describe == newSubject.Describe) //Nếu trùng hết tất cả thông tin thì đâu cần thêm hay sửa gì đâu
                                    continue;
                                if (MyMessageBox.Show(String.Format("Có sự thay đổi trong thông tin môn {0}. Bạn có muốn thay đổi", validSubjectCard.Code), "Thông báo", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                                {
                                    validSubjectCard.DisplayName = newSubject.DisplayName;
                                    validSubjectCard.Credit = newSubject.Credit;
                                    validSubjectCard.Describe = newSubject.Describe;
                                    SubjectServices.Instance.SaveSubjectCardToDatabase(validSubjectCard);
                                    count++;
                                }
                            }
                            else
                            {
                                SubjectServices.Instance.SaveSubjectToDatabase(newSubject);
                                count++;
                                /*StoredSubjectCards.Add(new SubjectCard(newSubject.Id, newSubject.DisplayName, newSubject.Credit, newSubject.Code, newSubject.Describe));*/
                            }

                        }
                        if (count == 0)
                        {
                            MyMessageBox.Show("Không có gì thay đổi");
                        }
                        else
                        {
                            MyMessageBox.Show("Thêm thành công");
                            LoadDataBehind();
                            LoadDataFront();
                            SearchSubjectCardsFunction(); // Phòng trường hợp mình thêm trong lúc đang search
                        }
                        
                    }
                }
                catch
                {
                    MyMessageBox.Show("Không mở được file", "Lỗi");
                }
            }
        }


        #endregion methods
    }
}