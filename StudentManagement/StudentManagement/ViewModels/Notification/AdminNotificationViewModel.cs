using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Services;
using StudentManagement.Utils;
using StudentManagement.Views;
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
    public class AdminNotificationViewModel : BaseViewModel
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

        public int? NumCardInBadged { get => _numCardInBadged; set { _numCardInBadged = value; OnPropertyChanged(); } }

        private int? _numCardInBadged;

        public object _createNewNotificationViewModel;
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
            try
            {
                CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
                ci.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
                Thread.CurrentThread.CurrentCulture = ci;

                Instance = this;
                Type = NotificationTypeServices.Instance.GetListNotificationType();
                TypeInMain = new ObservableCollection<string>(Type);
                Type = new ObservableCollection<string> { "Thông báo chung", "Thông báo sinh viên", "Thông báo giáo viên", "Thông báo Admin" };
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
                LoginServices.UpdateCurrentUser += LoginServices_UpdateCurrentUser;

                if (LoginServices.CurrentUser != null)
                {
                    IdUser = LoginServices.CurrentUser.Id;
                    LoadCardNotification();
                }

                InitIcommand();
            }
            catch
            {
                MyMessageBox.Show("Có lỗi trong khởi tạo thông báo", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

        }

        private void LoginServices_UpdateCurrentUser(object sender, LoginServices.LoginEvent e)
        {
            IdUser = LoginServices.CurrentUser.Id;
            LoadCardNotification();
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
        public void LoadCardNotification()
        {
            Cards = NotificationServices.Instance.LoadNotificationCardByUserId(IdUser);
            Cards = new ObservableCollection<NotificationCard>(Cards.OrderByDescending(card => card.Time).ThenBy(card => card.Time.TimeOfDay));
            RealCards = new ObservableCollection<NotificationCard>(Cards.Select(card => card));
            CardsInBadge = NotificationServices.Instance.LoadNotificationInBadgeByIdUser(IdUser);
            CardsInBadge = new ObservableCollection<NotificationCard>(CardsInBadge.OrderByDescending(card => card.Time).ThenBy(card => card.Time.TimeOfDay));
            NumCardInBadged = CardsInBadge.Where(notificationInfo => notificationInfo.Status == false).ToList().Count;
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
            try
            {
                if (p.DataContext == null)
                    return;
                var card = p.DataContext as NotificationCard;
                if (!UserServices.Instance.GetUserById(IdUser).UserRole.Role.Contains("Admin"))
                {
                    var tmp = Cards.FirstOrDefault(tmpCard => tmpCard.Id == card.Id);
                    Cards.Remove(tmp);
                    RealCards.Remove(tmp);
                }
                NotificationServices.Instance.DeleteNotificationInfoByNotificationCardAndIdUser(card, IdUser);
                CardsInBadge.Remove(card);
            }
            catch
            {
                MyMessageBox.Show("Đã có lỗi trong việc tải thông báo", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

        }
        public void MarkAsRead(UserControl p)
        {
            try
            {
                if (p.DataContext == null)
                    return;
                var card = p.DataContext as NotificationCard;
                card.Status = true;
                NotificationServices.Instance.MarkAsReadNotificationInfoByNotificationCardAndIdUser(card, IdUser);
            }
            catch
            {
                MyMessageBox.Show("Đã có lỗi trong việc đánh dấu đã đọc", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            
        }
        public void MarkAsUnread(UserControl p)
        {
            try
            {
                if (p.DataContext == null)
                    return;
                var card = p.DataContext as NotificationCard;
                card.Status = false;
                NotificationServices.Instance.MarkAsUnReadNotificationInfoByNotificationCardAndIdUser(card, IdUser);
                //NumCardInBadged += 1;
            }
            catch
            {
                MyMessageBox.Show("Đã có lỗi trong việc đánh dấu chưa đọc", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
        public void MarkAllAsRead()
        {
            try
            {
                CardsInBadge.ToList().ForEach(card => card.Status = true);
                NotificationServices.Instance.MarkAllAsReadNotificationInfoByIdUser(IdUser);
            }
            catch
            {
                MyMessageBox.Show("Đã có lỗi trong việc đánh dấu tất cả đã đọc", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        
        }
        public void SeenNotification()
        {
            NumCardInBadged = 0;
        }

        public void ShowDetailNotification(UserControl p)
        {
            try
            {
                if (p.DataContext == null)
                    return;
                var card = p.DataContext as NotificationCard;
                if(card.IdSubjectClass==null)
                {
                    this._showDetailNotificationViewModel = new ShowDetailNotificationViewModel(card);
                    MainViewModel.Instance.DialogViewModel = this._showDetailNotificationViewModel;
                    MainViewModel.Instance.IsOpen = true;
                    card.Status = true;
                    for (int i = 0; i < CardsInBadge.Count; i++)
                    {
                        if (CardsInBadge[i].Id == card.Id)
                        {
                            CardsInBadge[i].Status = true;
                            break;
                        }
                    }
                }
                else
                {
                    var subjectClass = SubjectClassServices.Instance.FindSubjectClassBySubjectClassId((Guid)card.IdSubjectClass);
                    var subjectClassCard = SubjectClassServices.Instance.ConvertSubjectClassToSubjectClassCard(subjectClass);
                    SubjectClassDetail subjectClassDetail = new SubjectClassDetail
                    {
                        DataContext = new SubjectClassDetailViewModel(subjectClassCard)
                    };
                    subjectClassDetail.Show();
                }    
                NotificationServices.Instance.MarkAsReadNotificationInfoByNotificationCardAndIdUser(card, IdUser);
            }
            catch
            {
                MyMessageBox.Show("Đã có lỗi trong việc đánh dấu đã đọc", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public void Search()
        {
            //RealCards = Cards;
            var tmp = Cards.Where(x => vietnameseStringNormalizer.Normalize(x.Topic).Contains(vietnameseStringNormalizer.Normalize(SearchInfo.ToLower())));
            if (SearchDate != null)
            {
                tmp = tmp.Where(x => x.Time.Date == _searchDate);
            }
            if (!SearchType.Equals("Tất cả"))
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
            for (int i = 0; i < CardsInBadge.Count; i++)
                if (CardsInBadge[i].Id == card.Id)
                {
                    CardsInBadge[i] = card;
                    break;
                }
        }
        public void DeleteNotification()
        {
            try
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
                    NotificationServices.Instance.DeleteNotificationInfoByNotificationCardAndIdUser(tmp, LoginServices.CurrentUser.Id);
            }
            catch
            {
                MyMessageBox.Show("Đã có lỗi trong việc xoá thông báo", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
          
        }

        public void CreateNewNotification()
        {
            var card = new NotificationCard(Guid.NewGuid(), IdUser, "", "", "", DateTime.Now);
            this._createNewNotificationViewModel = new CreateNewNotificationViewModel(card);
            MainViewModel.Instance.DialogViewModel = this._createNewNotificationViewModel;
            MainViewModel.Instance.IsOpen = true;
        }
    }
    #endregion
}
