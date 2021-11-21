using StudentManagement.Commands;
using StudentManagement.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class AdminNotificationViewModel: BaseViewModel
    {
        public class CardNotification: BaseViewModel
        {
            private int _id;
            private string _nguoiDang;
            private string _noiDung;
            private string _chuDe;
            private DateTime _ngayDang;
            private string _loaiBaiDang;
            private bool _status;

            public CardNotification(int id , string nguoiDang, string loaiBaiDang, string noiDung, string chuDe, DateTime ngayDang)
            {
                Id = id;
                NguoiDang = nguoiDang;
                LoaiBaiDang = loaiBaiDang;
                NoiDung = noiDung;
                ChuDe = chuDe;
                NgayDang = ngayDang;
                Status = false;
            }
            public CardNotification(CardNotification a)
            {
                Id = a.Id;
                ChuDe = a.ChuDe;
                NguoiDang = a.NguoiDang;
                LoaiBaiDang = a.LoaiBaiDang;
                NoiDung = a.NoiDung;
                NgayDang = a.NgayDang;
                Status = a.Status;
            }

            public string NguoiDang { get => _nguoiDang; set => _nguoiDang = value; }
            public string NoiDung { get => _noiDung; set => _noiDung = value; }
            public string ChuDe { get => _chuDe; set => _chuDe = value; }
            public DateTime NgayDang { get => _ngayDang; set => _ngayDang = value; }
            public string LoaiBaiDang { get => _loaiBaiDang; set => _loaiBaiDang = value; }
            public int Id { get => _id; set => _id = value; }
            public bool Status { get => _status; set { _status = value; OnPropertyChanged(); } }
        }

        private static AdminNotificationViewModel s_instance;
        public static AdminNotificationViewModel Instance
        {
            get => s_instance ?? (s_instance = new AdminNotificationViewModel());

            private set => s_instance = value;
        }

        public ObservableCollection<CardNotification> _cards;
        private ObservableCollection<CardNotification> _realCards;
        private ObservableCollection<string> _type;
        private ObservableCollection<string> _typeInMain;
        public ObservableCollection<string> Type { get => _type; set => _type = value; }
        public ObservableCollection<CardNotification> Cards { get => _cards; set => _cards = value; }
        public ObservableCollection<CardNotification> RealCards { get => _realCards; set { _realCards = value; OnPropertyChanged(); } }
        public ObservableCollection<string> TypeInMain { get => _typeInMain; set => _typeInMain = value; }

        public VietnameseStringNormalizer vietnameseStringNormalizer = VietnameseStringNormalizer.Instance;

        private ICommand _popUpNotification;
        public ICommand PopUpNotification { get => _popUpNotification; set => _popUpNotification = value; }

        private ICommand _searchCommand;
        public ICommand SearchCommand { get => _searchCommand; set => _searchCommand = value; }

        public ICommand UpdateNotificationCommand { get => _updateNotification; set => _updateNotification = value; }
        private ICommand _updateNotification;

        public ICommand DeleteNotificationCommand { get => _deleteNotification; set => _deleteNotification = value; }
        private ICommand _deleteNotification;

        public ICommand CreateNotificationCommand { get => _createNotificationCommand; set => _createNotificationCommand = value; }
        private ICommand _createNotificationCommand;

        public ICommand ShowDetailNotificationCommand { get => _showDetailNotificationCommand; set => _showDetailNotificationCommand = value; }
        private ICommand _showDetailNotificationCommand;

        public ICommand SeenNotificationCommand { get => _seenNotificationCommand; set => _seenNotificationCommand = value; }
        private ICommand _seenNotificationCommand;

        public ICommand MarkAllAsReadCommand { get => _markAllAsReadCommand; set => _markAllAsReadCommand = value; }
        private ICommand _markAllAsReadCommand;

        public ICommand MarkAsUnreadCommand { get => _markAsUnreadCommand; set => _markAsUnreadCommand = value; }
        private ICommand _markAsUnreadCommand;

        public ICommand MarkAsReadCommand { get => _markAsReadCommand; set => _markAsReadCommand = value; }
        private ICommand _markAsReadCommand;

        public ICommand DeleteNotificationInBadgeCommand { get => _deleteNotificationInBadgeCommand; set => _deleteNotificationInBadgeCommand = value; }
        private ICommand _deleteNotificationInBadgeCommand;



        private string _searchInfo;
        public string SearchInfo 
        { 
            get => _searchInfo; 
            set 
            { 
                _searchInfo = value; 
                OnPropertyChanged(); 
            } 
        }

        private DateTime? _searchDate;
        public DateTime? SearchDate
        {
            get => _searchDate;
            set
            {
                _searchDate = value;
                OnPropertyChanged();
            }
        }

        private string _searchType;
        public string SearchType
        {
            get => _searchType;
            set
            {
                _searchType = value;
                OnPropertyChanged();
            }
        }

        private object _dialogItemViewModel;
        public object DialogItemViewModel
        {
            get { return _dialogItemViewModel; }
            set
            {
                _dialogItemViewModel = value;
                OnPropertyChanged();
            }
        }

        public int NumCardInBadged { get => _numCardInBadged; set { _numCardInBadged = value; OnPropertyChanged(); } }

        private int _numCardInBadged;

        public object _creatNewNotificationViewModel;
        public object _showDetailNotificationViewModel;


        public AdminNotificationViewModel()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            Instance = this;
            Type = new ObservableCollection<string>() { "Thông báo chung", "Thông báo sinh viên", "Thông báo giáo viên" };
            TypeInMain = new ObservableCollection<string>(Type);
            TypeInMain.Add("Tất cả");
            SearchInfo = "";
            SearchType = "Tất cả";
            SearchDate = null;
            Cards = new ObservableCollection<CardNotification>() {
                new CardNotification(0,"Nguyễn Tấn Toàn","Thông báo chung","Chào các bạn sinh viên! Trung tâm Khảo thí và Đánh giá chất lượng đào tạo - ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Tổ chức thi chứng chỉ tiếng Anh VNU-OPT", DateTime.Now),
                new CardNotification( 1,"Nguyễn Thị Quý","Thông báo sinh viên","ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Tổ chức thi chứng chỉ tiếng Anh VNU-OPT", DateTime.Now),
                new CardNotification(2,"Nguyễn Thị Quý","Thông báo giáo viên","Chào các bạn sinh viên! Trung tâm Khảo thí và Đánh giá chất lượng đào tạo - ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Tổ chức thi chứng chỉ tiếng Anh VNU-OPT", DateTime.Now),
                new CardNotification(3,"Nguyễn Tấn Toàn","Thông báo chung","Chào các bạn sinh viên! Trung tâm Khảo thí và Đánh giá chất lượng đào tạo - ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Tổ chức thi chứng chỉ tiếng Anh VNU-OPT", DateTime.Now),
                new CardNotification(4,"Nguyễn Tấn Toàn","Thông báo chung","Chào các bạn sinh viên! Trung tâm Khảo thí và Đánh giá chất lượng đào tạo - ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Cường chức thi chứng chỉ tiếng Anh VNU-OPT", DateTime.Now)

            };
            NumCardInBadged = Cards.Count;
            RealCards = Cards;
            SearchCommand = new RelayCommand<object>((p) => { return true; }, (p) => Search());
            UpdateNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => UpdateNotification());
            DeleteNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => DeleteNotification());
            CreateNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => CreateNewNotification());
            ShowDetailNotificationCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => ShowDetailNotification(p));
            SeenNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => SeenNotification());
            MarkAllAsReadCommand = new RelayCommand<object>((p) => { return true; }, (p) => MarkAllAsRead());
            MarkAsUnreadCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => MarkAsUnread(p));
            MarkAsReadCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => MarkAsRead(p));
            DeleteNotificationInBadgeCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => DeleteNotificationCardInBadge(p));
        }
        
        public void DeleteNotificationCardInBadge(UserControl p)
        {
            if (p.DataContext == null)
                return;
            var card = p.DataContext as CardNotification;
            Cards.Remove(card);
        }
        public void MarkAsRead(UserControl p)
        {
            if (p.DataContext == null)
                return;
            var card = p.DataContext as CardNotification;
            card.Status = true;
        }
        public void MarkAsUnread(UserControl p)
        {
            if (p.DataContext == null)
                return;
            var card = p.DataContext as CardNotification;
            card.Status = false;
        }
        public void MarkAllAsRead()
        {
            for(int i=0; i<Cards.Count;i++)
            {
                Cards[i].Status = true;
            }    
        }
        public void SeenNotification()
        {
            NumCardInBadged = 0;
        }

        public void ShowDetailNotification(UserControl p)
        {
            if (p.DataContext == null)
                return;
            var card = p.DataContext as CardNotification;
            card.Status = true;
            this._showDetailNotificationViewModel = new ShowDetailNotificationViewModel(card);
            this.DialogItemViewModel = this._showDetailNotificationViewModel;
        }
     
        public void Search()
        {
            RealCards = Cards;
            var tmp = Cards.Where(x => vietnameseStringNormalizer.Normalize(x.ChuDe).Contains(vietnameseStringNormalizer.Normalize(SearchInfo.ToLower())));
            if (SearchDate != null)
            {
                tmp = tmp.Where(x => x.NgayDang.Date == _searchDate);
            }
            if(!SearchType.Equals("Tất cả"))
            {
                tmp = tmp.Where(x => x.LoaiBaiDang.Contains(SearchType));
            }    
            RealCards = new ObservableCollection<CardNotification>(tmp);
        }
        public void UpdateNotification()
        {
            var AdminNotificationRightSideBarVM = AdminNotificationRightSideBarViewModel.Instance;
            CardNotification card = (AdminNotificationRightSideBarVM._adminNotificationRightSideBarEditViewModel as AdminNotificationRightSideBarEditViewModel).CurrentCard;
            (AdminNotificationRightSideBarVM._adminNotificationRightSideBarItemViewModel as AdminNotificationRightSideBarItemViewModel).CurrentCard = card;
            AdminNotificationRightSideBarVM.RightSideBarItemViewModel = AdminNotificationRightSideBarVM._adminNotificationRightSideBarItemViewModel;
        
            for (int i = 0; i < Cards.Count; i++)
                if (Cards[i].Id == card.Id)
                {
                    Cards[i] = card;
                    break;
                }
            for (int i = 0; i < RealCards.Count; i++)
                if (RealCards[i].Id == card.Id)
                {
                    RealCards[i] = card;
                    break;
                }
        }
        public void DeleteNotification()
        {
            if (MyMessageBox.Show("Bạn có chắc muốn xoá thông báo này", "Thông báo", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Warning) != System.Windows.MessageBoxResult.OK)
                return;
            var AdminNotificationRightSideBarVM = AdminNotificationRightSideBarViewModel.Instance;
            AdminNotificationRightSideBarVM.RightSideBarItemViewModel = AdminNotificationRightSideBarVM._emptyStateRightSideBarViewModel;
            var tmp = Cards.Where(x => x.Id == AdminNotificationRightSideBarVM.CurrentCard.Id).FirstOrDefault();
            Cards.Remove(tmp);
            RealCards.Remove(tmp);
        }

        public void CreateNewNotification()
        {
            var card = new CardNotification(Cards.LastOrDefault().Id + 1, "Cuong", "", "", "", DateTime.Now);
            this._creatNewNotificationViewModel = new CreateNewNotificationViewModel(card);
            this.DialogItemViewModel = this._creatNewNotificationViewModel;
        }  
    }
}
