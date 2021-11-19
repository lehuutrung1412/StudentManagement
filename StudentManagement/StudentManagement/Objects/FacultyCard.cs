using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public class FacultyCard : BaseObjectWithBaseViewModel, IBaseCard
    {
        private string _tenKhoa;
        private DateTime _ngayThanhLap;
        private int _soLuongSinhVien;
        private string _cacHeDaoTao;

        public FacultyCard() { }
        public FacultyCard(string tenKhoa, DateTime ngayThanhLap, int soLuongSinhVien, string cacHeDaoTao)
        {
            _tenKhoa = tenKhoa;
            _ngayThanhLap = ngayThanhLap;
            _soLuongSinhVien = soLuongSinhVien;
            _cacHeDaoTao = cacHeDaoTao;
        }

        public string TenKhoa { get => _tenKhoa; set => _tenKhoa = value; }
        public DateTime NgayThanhLap { get => _ngayThanhLap; set => _ngayThanhLap = value; }
        public int SoLuongSinhVien { get => _soLuongSinhVien; set => _soLuongSinhVien = value; }
        public string CacHeDaoTao { get => _cacHeDaoTao; set => _cacHeDaoTao = value; }
    }
}
