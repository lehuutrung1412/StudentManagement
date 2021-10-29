using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class AdminSubjectClassViewModel : BaseViewModel
    {
        public class CardInfo
        {
            private int _siSo;
            private string _giaoVien;
            private string _maMon;
            private string _tenMon;

            public CardInfo(int siSo, string giaoVien, string maMon, string tenMon)
            {
                SiSo = siSo;
                GiaoVien = giaoVien;
                MaMon = maMon;
                TenMon = tenMon;
            }

            public int SiSo { get => _siSo; set => _siSo = value; }
            public string GiaoVien { get => _giaoVien; set => _giaoVien = value; }
            public string MaMon { get => _maMon; set => _maMon = value; }
            public string TenMon { get => _tenMon; set => _tenMon = value; }
        }

        public ObservableCollection<CardInfo> _cards;

        public ObservableCollection<CardInfo> Cards { get => _cards; set => _cards = value; }

        public AdminSubjectClassViewModel()
        {
            Cards = new ObservableCollection<CardInfo>() {
                new CardInfo(50, "Nguyễn Tấn Toàn", "IT008", "Lập trình trực quan"),
                new CardInfo(150, "Nguyễn Thị Quý", "SE104", "Nhập môn CNPM"),
                new CardInfo(20, "Nguyễn Thị Quý", "IT009", "Mạng máy tính"),
                new CardInfo(30, "Nguyễn Tấn Toàn", "IT007", "Cơ sở dữ liệu"),
                new CardInfo(40, "Nguyễn Tấn Toàn", "CS231", "Xử lý ngôn ngữ tự nhiên")
            };
        }
    }
}
