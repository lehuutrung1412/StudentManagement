using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public class TrainingFormCard : BaseObjectWithBaseViewModel, IBaseCard
    {
        private string _tenHeDaoTao;
        private int _soLuongKhoa;
        private int _soLuongSinhVien;

        public TrainingFormCard() { }

        public TrainingFormCard(string tenHeDaoTao, int soLuongKhoa, int soLuongSinhVien)
        {
            TenHeDaoTao = tenHeDaoTao;
            SoLuongKhoa = soLuongKhoa;
            SoLuongSinhVien = soLuongSinhVien;
        }

        public string TenHeDaoTao { get => _tenHeDaoTao; set => _tenHeDaoTao = value; }
        public int SoLuongKhoa { get => _soLuongKhoa; set => _soLuongKhoa = value; }
        public int SoLuongSinhVien { get => _soLuongSinhVien; set => _soLuongSinhVien = value; }
    }
}
