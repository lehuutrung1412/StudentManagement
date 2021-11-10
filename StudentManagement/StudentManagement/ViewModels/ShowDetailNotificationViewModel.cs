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
    public class ShowDetailNotificationViewModel: BaseViewModel
    {
        public CardNotification CurrentCard { get => _currentCard; set => _currentCard = value; }
        private CardNotification _currentCard;

        public ICommand IsEditNotificationCommand { get => _isEditNotificationCommand; set => _isEditNotificationCommand = value; }
        private ICommand _isEditNotificationCommand;
        public ICommand CancelEditCommand { get => _cancelEditCommand; set => _cancelEditCommand = value; }
        private ICommand _cancelEditCommand;
        public ICommand EditNotificationCommand { get => _editNotificationCommand; set => _editNotificationCommand = value; }
        private ICommand _editNotificationCommand;
        public ICommand DeleteNotificationCommand { get => _deleteNotification; set => _deleteNotification = value; }
        private ICommand _deleteNotification;

        public bool IsEnable { get => _isEnable; set { _isEnable = value; OnPropertyChanged(); } }
        private bool _isEnable;

        public ShowDetailNotificationViewModel(CardNotification card)
        {
            this.CurrentCard = card;
            IsEnable = false;
            InitCommand();
        }
        public void InitCommand()
        {
            IsEditNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => IsEditNotification());
            EditNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => EditNotification());
            CancelEditCommand = new RelayCommand<object>((p) => { return true; }, (p) => CancelEdit());
            DeleteNotificationCommand = new RelayCommand<object>((p) => { return true; }, (p) => DeleteNotification());
        }
        public void DeleteNotification()
        {
            var AdminNotificationVM = AdminNotificationViewModel.Instance;
            AdminNotificationVM.Cards.Remove(CurrentCard);
            AdminNotificationVM.RealCards.Remove(CurrentCard);
        }
        public void CancelEdit()
        {
            IsEnable = false;
        }
        public void IsEditNotification()
        {
            IsEnable = true;
        }
        public void EditNotification()
        {
            var AdminNotificationVM = AdminNotificationViewModel.Instance;
            for(int i=0; i<AdminNotificationVM.Cards.Count;i++)
            {
                if (AdminNotificationVM.Cards[i].Id == CurrentCard.Id)
                {
                    AdminNotificationVM.Cards[i] = CurrentCard;
                    break;
                }    
            }
            for (int i = 0; i < AdminNotificationVM.RealCards.Count; i++)
            {
                if (AdminNotificationVM.RealCards[i].Id == CurrentCard.Id)
                {
                    AdminNotificationVM.RealCards[i] = CurrentCard;
                    break;
                }
            }
            IsEnable = false;
        }
    }
}