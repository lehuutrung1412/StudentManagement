using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static StudentManagement.ViewModels.AdminNotificationViewModel;

namespace StudentManagement.ViewModels
{
    public class CreateNewNotificationViewModel: BaseViewModel
    {
        public CardNotification CurrentCard { get => _currentCard; set => _currentCard = value; }
        private CardNotification _currentCard;

        public ICommand CreateNotificationCommand { get => _createNotificationCommand; set => _createNotificationCommand = value; }
        private ICommand _createNotificationCommand;
        public ICommand IsCreateNotificationCommand { get => _iscreateNotificationCommand; set => _iscreateNotificationCommand = value; }
        private ICommand _iscreateNotificationCommand;
        public bool IsCreateNotification { get => _isCreateNotification; set { _isCreateNotification = value; OnPropertyChanged(); } }
        private bool _isCreateNotification;

        public CreateNewNotificationViewModel()
        {
            this.CurrentCard = null;
           
        }
        public CreateNewNotificationViewModel(CardNotification card)
        {
            this.CurrentCard = card;
            InitCommand();
            IsCreateNotification = false;
        }
        public void InitCommand()
        {
            CreateNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => CreateNewNotification());
            IsCreateNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => CheckIsCreateNotification());
        }
        public void CreateNewNotification()
        {
            //if (string.IsNullOrWhiteSpace(CurrentCard.ChuDe) || string.IsNullOrWhiteSpace(CurrentCard.NoiDung) || string.IsNullOrWhiteSpace(CurrentCard.LoaiBaiDang))
            //{
            //    MyMessageBox.Show("Có lỗi khi tạo thông báo!!! Kiểm tra lại thông tin", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            //    return;
            //}    
                
            var AdminNotificationVM = AdminNotificationViewModel.Instance;
            AdminNotificationVM.Cards.Add(CurrentCard);
            if (string.IsNullOrEmpty(AdminNotificationVM.SearchInfo))
                if(AdminNotificationVM.RealCards.Where(x=>x.Id==CurrentCard.Id).Count()==0)
                    AdminNotificationVM.RealCards.Add(CurrentCard);
            AdminNotificationVM.NumCardInBadged += 1;
            //IsCreateNotification = false;
        }
        public void CheckIsCreateNotification()
        {
            if (string.IsNullOrWhiteSpace(CurrentCard.ChuDe) || string.IsNullOrWhiteSpace(CurrentCard.NoiDung) || string.IsNullOrWhiteSpace(CurrentCard.LoaiBaiDang))
                IsCreateNotification = false;
            else
                IsCreateNotification = true;
        }
    }
}
