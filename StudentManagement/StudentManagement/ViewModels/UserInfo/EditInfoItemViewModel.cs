using StudentManagement.Commands;
using StudentManagement.Objects;
using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static StudentManagement.ViewModels.UserInfoItemViewModel;
using static StudentManagement.ViewModels.UserInfoViewModel;

namespace StudentManagement.ViewModels
{
    public class EditInfoItemViewModel: BaseViewModel
    {
        private InfoItem _currendInfoItem;
        public InfoItem CurrendInfoItem { get => _currendInfoItem; set => _currendInfoItem = value; }

        private InfoItem _displayInfoItem;
        public InfoItem DisplayInfoItem { get => _displayInfoItem; set => _displayInfoItem = value; }
        private ObservableCollection<ItemInCombobox> _listItemInCombobox;
        public ObservableCollection<ItemInCombobox> ListItemInCombobox { get => _listItemInCombobox; set { _listItemInCombobox = value; OnPropertyChanged(); } }

        public string TypeControl { get => _typeControl; set { _typeControl = value; OnPropertyChanged(); } }
        public bool IsEnable { get => _isEnable; set { _isEnable = value; OnPropertyChanged(); } }

        private string _typeControl;
        private bool _isEnable;

        public ICommand DeleteItemCommand { get => _deleteItemCommand; set => _deleteItemCommand = value; }

        private ICommand _deleteItemCommand;
        public ICommand AddItemCommand { get => _addItemCommand; set => _addItemCommand = value; }

        private ICommand _addItemCommand;

        public ICommand UpdateInfoItemCommand { get => _updateInfoItemCommand; set => _updateInfoItemCommand = value; }

        private ICommand _updateInfoItemCommand;

        public EditInfoItemViewModel(InfoItem infoItem)
        {
            CurrendInfoItem = infoItem;
            DisplayInfoItem = new InfoItem(infoItem);
            ListItemInCombobox = new ObservableCollection<ItemInCombobox>();
            if (infoItem.Type == 2)
                ConvertItemSourceToListItemCombobox();
            ConvertTypeToTypeControl();
            
            AddItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => AddItem());
            DeleteItemCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) => DeleteItem(p));
            UpdateInfoItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => UpdateInfoItem());
        }
        public void UpdateInfoItem()
        {
            DisplayInfoItem.ItemSource = new ObservableCollection<string>();
            if (TypeControl == "Combobox")
            {
                ListItemInCombobox.Where(x => !string.IsNullOrEmpty(x.Value)).ToList().ForEach(s => DisplayInfoItem.ItemSource.Add(s.Value));
                DisplayInfoItem.Type = 2;
            }
            else if (TypeControl == "Datepicker")
            {
                DisplayInfoItem.Type = 1;
            }
            else
                DisplayInfoItem.Type = 0;
            InfoItemServices.Instance.UpdateUserRole_UserInfoByInfoItem(DisplayInfoItem);
            UserInfoViewModel.Instance.LoadInfoSource();

            //for (int i =0; i<UserInfoViewModel.Instance.InfoSource.Count; i++)
            //{
            //    if (UserInfoViewModel.Instance.InfoSource[i].LabelName == CurrendInfoItem.LabelName)
            //    {
            //        UserInfoViewModel.Instance.InfoSource[i] = DisplayInfoItem;
            //        break;
            //    }    
                    
            //}            
            //UserInfoViewModel.Instance.IsOpen = false;
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
        public void ConvertItemSourceToListItemCombobox()
        {
            DisplayInfoItem.ItemSource.ToList().ForEach(item=> ListItemInCombobox.Add(new ItemInCombobox() { Value = item , Id = Guid.NewGuid()}));
        }
        public void ConvertTypeToTypeControl()
        {
            switch(DisplayInfoItem.Type)
            {
                case 0:
                    {
                        TypeControl = "Textbox";
                        break;
                    }
                case 1:
                    {
                        TypeControl = "Datepicker";
                        break;
                    }
                case 2:
                    {
                        TypeControl = "Combobox";
                        break;
                    }
            }
        }
    }
}
