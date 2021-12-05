using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Services;
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
        #region properties
        private static AdminNotificationViewModel s_instance;
        public static AdminNotificationViewModel Instance
        {
            get => s_instance ?? (s_instance = new AdminNotificationViewModel());

            private set => s_instance = value;
        }

        public ObservableCollection<NotificationCard> _cards;
        public ObservableCollection<NotificationCard> Cards { get => _cards; set { _cards = value; OnPropertyChanged(); } }

        public ObservableCollection<NotificationCard> _cardsInBadge;
        public ObservableCollection<NotificationCard> CardsInBadge { get => _cardsInBadge; set { _cardsInBadge = value; OnPropertyChanged(); } }

        private ObservableCollection<NotificationCard> _realCards;
        public ObservableCollection<NotificationCard> RealCards { get => _realCards; set { _realCards = value; OnPropertyChanged(); } }

        private ObservableCollection<string> _type;
        public ObservableCollection<string> Type { get => _type; set => _type = value; }


        private ObservableCollection<string> _typeInMain;
        public ObservableCollection<string> TypeInMain { get => _typeInMain; set => _typeInMain = value; }

        private Guid _idUser;
        public Guid IdUser { get => _idUser; set => _idUser = value; }

        public VietnameseStringNormalizer vietnameseStringNormalizer = VietnameseStringNormalizer.Instance;
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

        private object _isOpen;
        public object IsOpen
        {
            get { return _isOpen; }
            set
            {
                _isOpen = value;
                OnPropertyChanged();
            }
        }


        public int? NumCardInBadged { get => _numCardInBadged; set { _numCardInBadged = value; OnPropertyChanged(); } }

        private int? _numCardInBadged;

        public object _creatNewNotificationViewModel;
        public object _showDetailNotificationViewModel;
        #endregion

        #region Icommand
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
        #endregion

        public AdminNotificationViewModel()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            Instance = this;
            Type = NotificationTypeServices.Instance.GetListNotificationType();
            TypeInMain = new ObservableCollection<string>(Type);
            TypeInMain.Add("Tất cả");
            SearchInfo = "";
            SearchType = "Tất cả";
            SearchDate = null;
            //Cards = new ObservableCollection<NotificationCard>() {
            //    new NotificationCard(Guid.NewGuid(),"Nguyễn Tấn Toàn","Thông báo chung","Chào các bạn sinh viên! Trung tâm Khảo thí và Đánh giá chất lượng đào tạo - ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Tổ chức thi chứng chỉ tiếng Anh VNU-OPT", DateTime.Now),
            //    new NotificationCard(Guid.NewGuid(),"Nguyễn Thị Quý","Thông báo sinh viên","ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Tổ chức thi chứng chỉ tiếng Anh VNU-OPT", DateTime.Now),
            //    new NotificationCard(Guid.NewGuid(),"Nguyễn Thị Quý","Thông báo giáo viên","Chào các bạn sinh viên! Trung tâm Khảo thí và Đánh giá chất lượng đào tạo - ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Tổ chức thi chứng chỉ tiếng Anh VNU-OPT", DateTime.Now),
            //    new NotificationCard(Guid.NewGuid(),"Nguyễn Tấn Toàn","Thông báo chung","Chào các bạn sinh viên! Trung tâm Khảo thí và Đánh giá chất lượng đào tạo - ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Tổ chức thi chứng chỉ tiếng Anh VNU-OPT", DateTime.Now),
            //    new NotificationCard(Guid.NewGuid(),"Nguyễn Tấn Toàn","Thông báo chung","Chào các bạn sinh viên! Trung tâm Khảo thí và Đánh giá chất lượng đào tạo - ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Cường chức thi chứng chỉ tiếng Anh VNU-OPT", DateTime.Now)

            //};
            IdUser = DataProvider.Instance.Database.Users.FirstOrDefault(user=>user.UserRole.Role.Contains("Giáo viên")).Id;
            Cards = NotificationServices.Instance.LoadNotificationCardByUserId(IdUser);
            Cards = new ObservableCollection<NotificationCard>(Cards.OrderByDescending(card => card.Time).ThenBy(card => card.Time.TimeOfDay));
            RealCards = new ObservableCollection<NotificationCard>(Cards.Select(card=>card));
            CardsInBadge = NotificationServices.Instance.LoadNotificationInBadgeByIdUser(IdUser);
            NumCardInBadged = CardsInBadge.Where(notificationInfo => notificationInfo.Status == false).ToList().Count;
            IsOpen = false;
            InitIcommand();
        }
        #region method
        public void InitIcommand()
        {
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
        //public void ReLoadCardInBadge()
        //{
        //    CardsInBadge = new ObservableCollection<NotificationCard>();
        //    if (UserServices.Instance.GetUserById(IdUser).UserRole.Role.Contains("Admin"))
        //        Cards.Where(card => card.Type.Contains("Thông báo Admin") || card.Type.Contains("Thông báo chung")).ToList().ForEach(card => CardsInBadge.Add(card));
        //    else
        //        CardsInBadge = Cards;
        //}
        public void DeleteNotificationCardInBadge(UserControl p)
        {
            if (p.DataContext == null)
                return;
            var card = p.DataContext as NotificationCard;
            if(!UserServices.Instance.GetUserById(IdUser).UserRole.Role.Contains("Admin"))
            {
                var tmp = Cards.FirstOrDefault(tmpCard => tmpCard.Id == card.Id);
                Cards.Remove(tmp);
                RealCards.Remove(tmp);
            }
            NotificationServices.Instance.DeleteNotificationInfoByNotificationCardAndIdUser(card, IdUser); 
            CardsInBadge.Remove(card);
        }
        public void MarkAsRead(UserControl p)
        {
            if (p.DataContext == null)
                return;
            var card = p.DataContext as NotificationCard;
            card.Status = true;
            NotificationServices.Instance.MarkAsReadNotificationInfoByNotificationCardAndIdUser(card,IdUser);
        }
        public void MarkAsUnread(UserControl p)
        {
            if (p.DataContext == null)
                return;
            var card = p.DataContext as NotificationCard;
            card.Status = false;
            NotificationServices.Instance.MarkAsUnReadNotificationInfoByNotificationCardAndIdUser(card, IdUser);
            //NumCardInBadged += 1;
        }
        public void MarkAllAsRead()
        {
            Cards.ToList().ForEach(card => card.Status = true);
            NotificationServices.Instance.MarkAllAsReadNotificationInfoByIdUser(IdUser);
        }
        public void SeenNotification()
        {
            NumCardInBadged = 0;
        }

        public void ShowDetailNotification(UserControl p)
        {
            if (p.DataContext == null)
                return;
            var card = p.DataContext as NotificationCard;
            IsOpen = true;
            this._showDetailNotificationViewModel = new ShowDetailNotificationViewModel(card);
            this.DialogItemViewModel = this._showDetailNotificationViewModel;
            card.Status = true;
            for(int i=0;i<CardsInBadge.Count;i++)
            {
                if(CardsInBadge[i].Id == card.Id)
                {
                    CardsInBadge[i].Status = true;
                    break;
                }
            }    
            NotificationServices.Instance.MarkAsReadNotificationInfoByNotificationCardAndIdUser(card, IdUser);
        }
     
        public void Search()
        {
            //RealCards = Cards;
            var tmp = Cards.Where(x => vietnameseStringNormalizer.Normalize(x.Topic).Contains(vietnameseStringNormalizer.Normalize(SearchInfo.ToLower())));
            if (SearchDate != null)
            {
                tmp = tmp.Where(x => x.Time.Date == _searchDate);
            }
            if(!SearchType.Equals("Tất cả"))
            {
                tmp = tmp.Where(x => x.Type.Contains(SearchType));
            }    
            RealCards = new ObservableCollection<NotificationCard>(tmp);
        }
        public void UpdateNotification()
        {
            var AdminNotificationRightSideBarVM = AdminNotificationRightSideBarViewModel.Instance;
            NotificationCard card = (AdminNotificationRightSideBarVM._adminNotificationRightSideBarEditViewModel as AdminNotificationRightSideBarEditViewModel).CurrentCard;
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
            CardsInBadge.Remove(tmp);
            if (UserServices.Instance.GetUserById(IdUser).UserRole.Role.Contains("Admin"))
                NotificationServices.Instance.DeleteNotificationByNotificationCard(tmp);
            else
                NotificationServices.Instance.DeleteNotificationInfoByNotificationCardAndIdUser(tmp, DataProvider.Instance.Database.Users.FirstOrDefault().Id);
        }

        public void CreateNewNotification()
        {
            var card = new NotificationCard(Guid.NewGuid(), IdUser, "", "", "", DateTime.Now);
            this._creatNewNotificationViewModel = new CreateNewNotificationViewModel(card);
            this.DialogItemViewModel = this._creatNewNotificationViewModel;
        }  
    }
    #endregion
}
