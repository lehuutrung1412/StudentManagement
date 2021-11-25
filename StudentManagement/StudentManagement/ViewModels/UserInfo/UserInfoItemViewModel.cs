using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static StudentManagement.ViewModels.UserInfoViewModel;

namespace StudentManagement.ViewModels
{
    public class UserInfoItemViewModel: BaseViewModel
    {
        public class ItemInCombobox
        {
            public string Value { get; set; }
            public Guid Id { get; set; }
        }
        private ObservableCollection<ItemInCombobox> _listItemInCombobox;
        public ObservableCollection<ItemInCombobox> ListItemInCombobox { get => _listItemInCombobox; set { _listItemInCombobox = value; OnPropertyChanged(); } }

        private InfoItem _currendInfo;

        public InfoItem CurrendInfo { get => _currendInfo; set => _currendInfo = value; }

        public string TypeControl { get => _typeControl; set { _typeControl = value; OnPropertyChanged(); } }
        public string TypeUser { get => _typeUser; set { _typeUser = value; OnPropertyChanged(); } }

        

        private string _typeControl;
        private string _typeUser;


        public ICommand DeleteItemCommand { get => _deleteItemCommand; set => _deleteItemCommand = value; }
        
        private ICommand _deleteItemCommand;
        public ICommand AddItemCommand { get => _addItemCommand; set => _addItemCommand = value; }

        private ICommand _addItemCommand;

        public ICommand AddInfoItemCommand { get => _addInfoItemCommand; set => _addInfoItemCommand = value; }

        private ICommand _addInfoItemCommand;


        public UserInfoItemViewModel()
        {
            CurrendInfo = new InfoItem();
            CurrendInfo.ItemSource = new ObservableCollection<string>();
            ListItemInCombobox =  new ObservableCollection<ItemInCombobox>() 
            { 
                new ItemInCombobox { Id = Guid.NewGuid(), Value = "" }, 
                new ItemInCombobox { Id = Guid.NewGuid(), Value = "" }, 
                new ItemInCombobox { Id = Guid.NewGuid(), Value = "" } 
            };
            AddItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => AddItem());
            DeleteItemCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) => DeleteItem(p));
            AddInfoItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => AddInfoItem());
        }
        public void AddInfoItem()
        {
            if (TypeControl == "Combobox")
            {
                ListItemInCombobox.Where(x => !string.IsNullOrEmpty(x.Value)).ToList().ForEach(s => CurrendInfo.ItemSource.Add(s.Value));
                CurrendInfo.Type = 2;
            }
            else if (TypeControl == "Datepicker")
            {
                CurrendInfo.Type = 1;
            }
            else
                CurrendInfo.Type = 0;
            CurrendInfo.STT = UserInfoViewModel.Instance.InfoSource.LastOrDefault().STT + 1;
            UserInfoViewModel.Instance.InfoSource.Add(CurrendInfo);
            UserInfoViewModel.Instance.InfoSource.OrderBy(x => x.STT).ToList();
            UserInfoViewModel.Instance.IsOpen = false;
        }
        public void DeleteItem(TextBox p)
        {
            if (p.DataContext == null)
                return;
            var item = p.DataContext as ItemInCombobox;
            ListItemInCombobox.Remove(item);
        }
        public void AddItem()
        {
            ListItemInCombobox.Add(new ItemInCombobox { Id = Guid.NewGuid(), Value = "" });
        }
    }
}
