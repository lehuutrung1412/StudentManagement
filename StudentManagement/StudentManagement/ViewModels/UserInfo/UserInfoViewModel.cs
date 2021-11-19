using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class UserInfoViewModel: BaseViewModel
    {
        public class InfoItem : BaseViewModel
        {
            private string _labelName;
            private int _type;
            private ObservableCollection<string> _itemSource;
            private object _value;
            private int _sTT;
            private bool _isEnable;

            public string LabelName { get => _labelName; set { _labelName = value; OnPropertyChanged(); } }
            public int Type { get => _type; set { _type = value; OnPropertyChanged(); } }
            public ObservableCollection<string> ItemSource { get => _itemSource; set { _itemSource = value; OnPropertyChanged(); } }
            public object Value { get => _value; set { _value = value; OnPropertyChanged(); } }

            public int STT { get => _sTT; set { _sTT = value; OnPropertyChanged(); } }

            public bool IsEnable { get => _isEnable; set => _isEnable = value; }

            public InfoItem(string labelName, int type, ObservableCollection<string> itemSource, object value, int sTT, bool isEnable)
            {
                LabelName = labelName;
                Type = type;
                ItemSource = itemSource;
                Value = value;
                STT = sTT;
                IsEnable = isEnable;
            }
        }
        private ObservableCollection<InfoItem> _infoSource;

        public ObservableCollection<InfoItem> InfoSource { get => _infoSource; set { _infoSource = value; OnPropertyChanged(); } }

        public bool IsChangeAvatar { get => _isChangeAvatar; set { _isChangeAvatar = value; OnPropertyChanged(); } }

        private bool _isChangeAvatar;

        public string Avatar { get => _avatar; set { _avatar = value; OnPropertyChanged(); } }
        private string _avatar;

        public ICommand ClickImageCommand { get => clickImageCommand; set => clickImageCommand = value; }
        public ICommand ClickChangeImageCommand { get => clickChangeImageCommand; set => clickChangeImageCommand = value; }

        private ICommand clickImageCommand;
        private ICommand clickChangeImageCommand;

        public UserInfoViewModel()
        {
            ObservableCollection<string> Faculty = new ObservableCollection<string> {"KHMT","KTPM" };
            ObservableCollection<string> Sex = new ObservableCollection<string> { "Nam", "Nữ" };
            ObservableCollection<string> Class = new ObservableCollection<string> { "KHMT2019", "KHTN2019" };
            ObservableCollection<string> TrainingForm = new ObservableCollection<string> { "CNTN", "CQDT" };
            IsChangeAvatar = false;
            Avatar = "https://picsum.photos/200";
            InfoSource = new ObservableCollection<InfoItem>()
            {
                new InfoItem("Giới tính",2,Sex,"Nam",5,false),
                new InfoItem("Địa chỉ mail",0,null,"cuongnguyen14022001",6,true),
                new InfoItem("Hệ",2,TrainingForm,"CNTN",7,false),
                new InfoItem("Lớp sinh hoạt",2,Class,"KHTN2019",8,false),

                new InfoItem("Họ và tên",0,null,"Nguyễn Đỗ Mạnh Cường",0,false),
                new InfoItem("Ngày sinh",1,null,"14/02/2001",1,false),
                new InfoItem("Địa chỉ",0,null,"02B1, chợ mới Ninh Hoà",2,true),
                new InfoItem("Khoa",2,Faculty,"KHMT",3,false),
                new InfoItem("Số điện thoại",0,null,"0937418670",4,true),

            };
            InfoSource = new ObservableCollection<InfoItem>(InfoSource.OrderBy(x => x.STT));

            ClickImageCommand = new RelayCommand<object>((p) => { return true; }, (p) => ClickImage());
            ClickChangeImageCommand = new RelayCommand<object>((p) => { return true; }, (p) => ClickChangeImage());
        }
        public void ClickImage()
        {
            IsChangeAvatar = true;
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
        }
      
    }
}
