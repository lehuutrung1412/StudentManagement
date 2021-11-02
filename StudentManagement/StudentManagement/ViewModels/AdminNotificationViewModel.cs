using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class AdminNotificationViewModel
    {
        public class CardNotification
        {
            private string _nguoiDang;
            private string _noiDung;
            private string _chuDe;
            private DateTime _ngayDang;
            private string _loaiBaiDang;

            public CardNotification(string nguoiDang, string loaiBaiDang, string noiDung, string chuDe, DateTime ngayDang)
            {
                NguoiDang = nguoiDang;
                LoaiBaiDang = loaiBaiDang;
                NoiDung = noiDung;
                ChuDe = chuDe;
                NgayDang = ngayDang;
            }

            public string NguoiDang { get => _nguoiDang; set => _nguoiDang = value; }
            public string NoiDung { get => _noiDung; set => _noiDung = value; }
            public string ChuDe { get => _chuDe; set => _chuDe = value; }
            public DateTime NgayDang { get => _ngayDang; set => _ngayDang = value; }
            public string LoaiBaiDang { get => _loaiBaiDang; set => _loaiBaiDang = value; }
        }

        public ObservableCollection<CardNotification> _cards;
        private ObservableCollection<string> type;
        public ObservableCollection<CardNotification> Cards { get => _cards; set => _cards = value; }
        private ICommand popUpNotification;
        public ICommand PopUpNotification { get => popUpNotification; set => popUpNotification = value; }
        public ObservableCollection<string> Type { get => type; set => type = value; }

        public AdminNotificationViewModel()
        {
            Type = new ObservableCollection<string>() { "Thông báo chung", "Thông báo sinh viên", "Thông báo giáo viên" };
            Cards = new ObservableCollection<CardNotification>() {
                new CardNotification("Nguyễn Tấn Toàn","Thông báo chung","Chào các bạn sinh viên! Trung tâm Khảo thí và Đánh giá chất lượng đào tạo - ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Tổ chức thi chứng chỉ tiếng Anh VNU-OPT", DateTime.Now),
                new CardNotification( "Nguyễn Thị Quý","Thông báo chung","ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Tổ chức thi chứng chỉ tiếng Anh VNU-OPT", DateTime.Now),
                new CardNotification("Nguyễn Thị Quý","Thông báo chung","Chào các bạn sinh viên! Trung tâm Khảo thí và Đánh giá chất lượng đào tạo - ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Tổ chức thi chứng chỉ tiếng Anh VNU-OPT", DateTime.Now),
                new CardNotification("Nguyễn Tấn Toàn","Thông báo chung","Chào các bạn sinh viên! Trung tâm Khảo thí và Đánh giá chất lượng đào tạo - ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Tổ chức thi chứng chỉ tiếng Anh VNU-OPT", DateTime.Now),
                new CardNotification("Nguyễn Tấn Toàn","Thông báo chung","Chào các bạn sinh viên! Trung tâm Khảo thí và Đánh giá chất lượng đào tạo - ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Tổ chức thi chứng chỉ tiếng Anh VNU-OPT", DateTime.Now)

            };
            PopUpNotification = new RelayCommand<object>(
            (p) => { return true; },
            (p) =>
            {
                        
            });
        }
    }
}
