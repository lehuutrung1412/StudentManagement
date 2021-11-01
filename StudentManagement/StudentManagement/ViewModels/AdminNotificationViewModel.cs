using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    public class AdminNotificationViewModel
    {
        public class CardNotification
        {
            private string _nguoiDang;
            private string _noiDung;
            private string _chuDe;
            private string _ngayDang;

            public CardNotification(string nguoiDang, string noiDung, string chuDe, string ngayDang)
            {
                NguoiDang = nguoiDang;
                NoiDung = noiDung;
                ChuDe = chuDe;
                NgayDang = ngayDang;
            }

            public string NguoiDang { get => _nguoiDang; set => _nguoiDang = value; }
            public string NoiDung { get => _noiDung; set => _noiDung = value; }
            public string ChuDe { get => _chuDe; set => _chuDe = value; }
            public string NgayDang { get => _ngayDang; set => _ngayDang = value; }
        }

        public ObservableCollection<CardNotification> _cards;

        public ObservableCollection<CardNotification> Cards { get => _cards; set => _cards = value; }

        public AdminNotificationViewModel()
        {
            Cards = new ObservableCollection<CardNotification>() {
                new CardNotification("Nguyễn Tấn Toàn","Chào các bạn sinh viên! Trung tâm Khảo thí và Đánh giá chất lượng đào tạo - ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Tổ chức thi chứng chỉ tiếng Anh VNU-OPT", "25/10/2019"),
                new CardNotification( "Nguyễn Thị Quý","ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Tổ chức thi chứng chỉ tiếng Anh VNU-OPT", "25/10/2019"),
                new CardNotification("Nguyễn Thị Quý","Chào các bạn sinh viên! Trung tâm Khảo thí và Đánh giá chất lượng đào tạo - ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Tổ chức thi chứng chỉ tiếng Anh VNU-OPT", "25/10/2019"),
                new CardNotification("Nguyễn Tấn Toàn","Chào các bạn sinh viên! Trung tâm Khảo thí và Đánh giá chất lượng đào tạo - ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Tổ chức thi chứng chỉ tiếng Anh VNU-OPT", "25/10/2019"),
                new CardNotification("Nguyễn Tấn Toàn","Chào các bạn sinh viên! Trung tâm Khảo thí và Đánh giá chất lượng đào tạo - ĐHQG-HCM thông báo lịch thi chứng chỉ trong các tháng 10, 11, 12  ...", "Tổ chức thi chứng chỉ tiếng Anh VNU-OPT", "25/10/2019")
            };
        }
    }
}
