using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudentManagement.ViewModels.UserInfoViewModel;

namespace StudentManagement.ViewModels
{
    public class UserInfoItemViewModel: BaseViewModel
    {
        private InfoItem _currendInfo;

        public InfoItem CurrendInfo { get => _currendInfo; set => _currendInfo = value; }

        public string TypeControl { get => _typeControl; set { _typeControl = value; OnPropertyChanged(); } }
        public string TypeUser { get => _typeUser; set { _typeUser = value; OnPropertyChanged(); } }

        private string _typeControl;
        private string _typeUser;



        public UserInfoItemViewModel()
        {
            CurrendInfo = new InfoItem();
            CurrendInfo.ItemSource = new ObservableCollection<string> { "" };
            ObservableCollection<string> listTypeControl = new ObservableCollection<string>() { "Combobox", "Textbox", "Datepicker" };
            ObservableCollection<string> listTypeUser = new ObservableCollection<string>() { "Tất cả", "Admin", "Học sinh","Sinh viên" };
        }
    }
}
