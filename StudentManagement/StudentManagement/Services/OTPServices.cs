using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class OTPServices
    {
        private static OTPServices s_instance;

        public static OTPServices Instance => s_instance ?? (s_instance = new OTPServices());

        public OTPServices() { }

        public bool CheckGetOTPFromEmail(string email, string OTP)
        {
            var user = UserServices.Instance.GetUserByGmail(email);
            if (user.Count == 0)
                return false;
            if (user.FirstOrDefault().IdOTP==null)
                return false;
            if(user.FirstOrDefault().OTP.Code.Equals(OTP))
                return true;
            return false;
        }
        public void SaveOTP(string email, string OTP)
        {
            var user = UserServices.Instance.GetUserByGmail(email);
            if (user.Count == 0)
                return;
            var otp = new OTP()
            {
                Id = Guid.NewGuid(),
                Code = OTP,
                Time = DateTime.Now,
            };        
            DataProvider.Instance.Database.OTPs.AddOrUpdate(otp);
            user.FirstOrDefault().OTP = otp;
            DataProvider.Instance.Database.SaveChanges();
        }
        public void DeleteOTPOverTime()
        {
            if (DataProvider.Instance.Database.OTPs.ToList().Count == 0)
                return;
            var listOTP = DataProvider.Instance.Database.OTPs.Where(otp => DbFunctions.DiffSeconds(otp.Time,DateTime.Now)>30).ToList();
            if (listOTP.Count < 0)
                return;
            foreach(var otp in listOTP)
            {
                DataProvider.Instance.Database.OTPs.Remove(otp);
                var user = UserServices.Instance.GetUserByOTP(otp);
                try
                {
                    user.IdOTP = null;
                }
                catch { }
            }                    
            DataProvider.Instance.Database.SaveChanges();
        }
    }
}
