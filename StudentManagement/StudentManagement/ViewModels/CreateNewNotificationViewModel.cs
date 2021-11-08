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
    public class CreateNewNotificationViewModel
    {
        public CardNotification CurrentCard { get => _currentCard; set => _currentCard = value; }
        private CardNotification _currentCard;

        public ICommand CreateNotificationCommand { get => _createNotificationCommand; set => _createNotificationCommand = value; }
        private ICommand _createNotificationCommand;

        public CreateNewNotificationViewModel()
        {
            this.CurrentCard = null;
           
        }
        public CreateNewNotificationViewModel(CardNotification card)
        {
            this.CurrentCard = card;
            InitCommand();
        }
        public void InitCommand()
        {
            CreateNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => CreateNewNotification());
        }
        public void CreateNewNotification()
        {
            var AdminNotificationVM = AdminNotificationViewModel.Instance;
            AdminNotificationVM.Cards.Add(CurrentCard);
        }
    }
}
