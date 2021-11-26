using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Forms;

namespace StudentManagement.ViewModels
{
    public class UserInfoViewModel : BaseViewModel
    {
        public class InfoItem : BaseViewModel
        {
            private Guid _id;
            private string _labelName;
            private int _type;
            private ObservableCollection<string> _itemSource;
            private object _value;
            private int _sTT;
            private bool _isEnable;

            public Guid Id { get => _id; set { _id = value; OnPropertyChanged(); } }
            public string LabelName { get => _labelName; set { _labelName = value; OnPropertyChanged(); } }
            public int Type { get => _type; set { _type = value; OnPropertyChanged(); } }
            public ObservableCollection<string> ItemSource { get => _itemSource; set { _itemSource = value; OnPropertyChanged(); } }
            public object Value { get => _value; set { _value = value; OnPropertyChanged(); } }

            public int STT { get => _sTT; set { _sTT = value; OnPropertyChanged(); } }

            public bool IsEnable { get => _isEnable; set => _isEnable = value; }
            

            public InfoItem()
            {
                Id = Guid.NewGuid();
                IsEnable = true;
            }

            public InfoItem(Guid id, string labelName, int type, ObservableCollection<string> itemSource, object value, int sTT, bool isEnable)
            {
                Id = id;
                LabelName = labelName;
                Type = type;
                ItemSource = itemSource;
                Value = value;
                STT = sTT;
                IsEnable = isEnable;
            }
            public void  CopyItem(InfoItem item)
            {
                Id = item.Id;
                LabelName = item.LabelName;
                Type = item.Type;
                ItemSource = item.ItemSource;
                Value = item.Value;
                STT = item.STT;
                IsEnable = item.IsEnable;
            }
            public InfoItem(InfoItem item)
            {
                Id = item.Id;
                LabelName = item.LabelName;
                Type = item.Type;
                ItemSource = item.ItemSource;
                Value = item.Value;
                STT = item.STT;
                IsEnable = item.IsEnable;
            }
        }

        private static UserInfoViewModel s_instance;
        public static UserInfoViewModel Instance
        {
            get => s_instance ?? (s_instance = new UserInfoViewModel());

            private set => s_instance = value;
        }

        private ObservableCollection<InfoItem> _infoSource;

        public ObservableCollection<InfoItem> InfoSource { get => _infoSource; set { _infoSource = value; OnPropertyChanged(); } }

        public bool IsChangeAvatar { get => _isChangeAvatar; set { _isChangeAvatar = value; OnPropertyChanged(); } }

        private bool _isChangeAvatar;

        public string Avatar { get => _avatar; set { _avatar = value; OnPropertyChanged(); } }
        private string _avatar;
        public ObservableCollection<string> ListTypeControl { get => _listTypeControl; set { _listTypeControl = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _listTypeControl;

        public ObservableCollection<string> ListTypeUser { get => _listTypeUser; set { _listTypeUser = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _listTypeUser;        

        public object _userInfoItemViewModel;
        public object _editInfoItemViewModel;

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

        private object _isUpdate;
        public object IsUpdate
        {
            get { return _isUpdate; }
            set
            {
                _isUpdate = value;
                OnPropertyChanged();
            }
        }

        public ICommand ClickImageCommand { get => _clickImageCommand; set => _clickImageCommand = value; }
        private ICommand _clickImageCommand;

        public ICommand ClickChangeImageCommand { get => _clickChangeImageCommand; set => _clickChangeImageCommand = value; }
        private ICommand _clickChangeImageCommand;

        public ICommand AddNewInfoItemCommand { get => _addNewInfoItemCommand; set => _addNewInfoItemCommand = value; }
        private ICommand _addNewInfoItemCommand;

        public ICommand EditInfoItemCommand { get => _editInfoItemCommand; set => _editInfoItemCommand = value; }
        private ICommand _editInfoItemCommand;

        public ICommand UpdateUserInfoCommand { get => _updateUserInfoCommand; set => _updateUserInfoCommand = value; }
        private ICommand _updateUserInfoCommand;

        public ICommand ConfirmUserInfoCommand { get => _confirmUserInfoCommand; set => _confirmUserInfoCommand = value; }
        private ICommand _confirmUserInfoCommand;

        public UserInfoViewModel()
        {
            Instance = this;
            ObservableCollection<string> Faculty = new ObservableCollection<string> { "KHMT", "KTPM" };
            ObservableCollection<string> Sex = new ObservableCollection<string> { "Nam", "Nữ" };
            ObservableCollection<string> Class = new ObservableCollection<string> { "KHMT2019", "KHTN2019" };
            ObservableCollection<string> TrainingForm = new ObservableCollection<string> { "CNTN", "CQDT" };
            IsChangeAvatar = false;
            Avatar = "https://picsum.photos/200";
            InfoSource = new ObservableCollection<InfoItem>()
            {
                new InfoItem(Guid.NewGuid(),"Giới tính",2,Sex,"Nam",5,false),
                new InfoItem(Guid.NewGuid(),"Địa chỉ mail",0,null,"cuongnguyen14022001",6,true),
                new InfoItem(Guid.NewGuid(),"Hệ",2,TrainingForm,"CNTN",7,false),
                new InfoItem(Guid.NewGuid(),"Lớp sinh hoạt",2,Class,"KHTN2019",8,false),

                new InfoItem(Guid.NewGuid(),"Họ và tên",0,null,"Nguyễn Đỗ Mạnh Cường",0,false),
                new InfoItem(Guid.NewGuid(),"Ngày sinh",1,null,"14/02/2001",1,false),
                new InfoItem(Guid.NewGuid(),"Địa chỉ",0,null,"02B1, chợ mới Ninh Hoà",2,true),
                new InfoItem(Guid.NewGuid(),"Khoa",2,Faculty,"KHMT",3,false),
                new InfoItem(Guid.NewGuid(),"Số điện thoại",0,null,"0937418670",4,true),

            };

            ListTypeControl = new ObservableCollection<string> { "Combobox", "Textbox", "Datepicker" };
            ListTypeUser = new ObservableCollection<string> { "Tất cả", "Admin", "Học sinh", "Sinh viên" };

            InfoSource = new ObservableCollection<InfoItem>(InfoSource.OrderBy(x => x.STT));
            IsOpen = false;
            IsUpdate = false;
            ClickImageCommand = new RelayCommand<object>((p) => { return true; }, (p) => ClickImage());
            ClickChangeImageCommand = new RelayCommand<object>((p) => { return true; }, (p) => ClickChangeImage());
            AddNewInfoItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => AddNewInfoItem());
            EditInfoItemCommand = new RelayCommand<System.Windows.Controls.UserControl>((p) => { return true; }, (p) => EditInfoItem(p));
            UpdateUserInfoCommand = new RelayCommand<object>((p) => { return true; }, (p) => UpdateUserInfo());
            ConfirmUserInfoCommand = new RelayCommand<object>((p) => { return true; }, (p) => ComfirmUserInfo());

        }
        public void ComfirmUserInfo()
        {
            IsUpdate = false;
        }
        public void UpdateUserInfo()
        {
            IsUpdate = true;
        }
        public void EditInfoItem(System.Windows.Controls.UserControl p)
        {
            if(p.DataContext == null)
                return;
            var item = p.DataContext as InfoItem;
            this._editInfoItemViewModel = new EditInfoItemViewModel(item);
            this.DialogItemViewModel = this._editInfoItemViewModel;
            IsOpen = true;
        }
        public void AddNewInfoItem()
        {
            this._userInfoItemViewModel = new UserInfoItemViewModel();
            this.DialogItemViewModel = this._userInfoItemViewModel;

            IsOpen = true;
        }
        public void ClickImage()
        {
            IsChangeAvatar = !IsChangeAvatar;

        }
        public void ClickChangeImage()
        {
            OpenFileDialog op = new OpenFileDialog
            {
                Title = "Select a picture",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png"
            };
            if (op.ShowDialog() == DialogResult.OK)
            {
                Avatar = op.FileName;
                IsChangeAvatar = false;
            }
            IsChangeAvatar = false;
        }

    }
}
