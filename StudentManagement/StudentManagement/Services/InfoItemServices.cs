using StudentManagement.Models;
using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class InfoItemServices
    {
        private static InfoItemServices s_instance;

        public static InfoItemServices Instance => s_instance ?? (s_instance = new InfoItemServices());

        public InfoItemServices() { }

        //public List<User_UserRole_UserInfo> LoadInfoItemByUserId(Guid userId)
        //{
        //    List<User_UserRole_UserInfo> db = DataProvider.Instance.Database.User_UserRole_UserInfo.Where(infoItem => infoItem.IdUser == userId && infoItem.UserRole_UserInfo.IsDeleted == false).ToList();
        //    return db;
        //}
        public ObservableCollection<InfoItem> GetInfoSourceByUserId(Guid userId)
        {
            ObservableCollection<InfoItem> InfoSource = new ObservableCollection<InfoItem>();
            List<User_UserRole_UserInfo> db = DataProvider.Instance.Database.User_UserRole_UserInfo.Where(infoItem => infoItem.IdUser == userId && infoItem.UserRole_UserInfo.IsDeleted == false).ToList();
            foreach(User_UserRole_UserInfo infoItem in db)
            {
                InfoItem info = new InfoItem();
                info.Id = infoItem.Id;
                info.LabelName = infoItem.UserRole_UserInfo.UserInfo.InfoName;
                info.Type = Convert.ToInt32(infoItem.UserRole_UserInfo.UserInfo.Type);
                if(info.Type == 2)
                {
                    ObservableCollection<string> itemSources = new ObservableCollection<string>();
                    List<UserInfoItem> userInfoItems = DataProvider.Instance.Database.UserInfoItems.Where(userInfoItem => userInfoItem.IdUserInfo == infoItem.UserRole_UserInfo.IdUserInfo).ToList();
                    userInfoItems.ForEach(userInfoItem => itemSources.Add(userInfoItem.Content));
                }
                info.Value = infoItem.Content;
                info.IsEnable = Convert.ToBoolean(infoItem.UserRole_UserInfo.IsEnable);
                InfoSource.Add(info);
            }
            return InfoSource;
        }

        public UserInfo ConverInfoItemToUserInfo(InfoItem infoItem)
        {
            UserInfo userInfo = new UserInfo()
            {
                Id = infoItem.Id,
                InfoName = infoItem.LabelName,
                Type = infoItem.Type,
            };
            return userInfo;
        }
        public void AddUserInfoByInfoItem(InfoItem infoItem)
        {
            UserInfo userInfo = ConverInfoItemToUserInfo(infoItem);
            DataProvider.Instance.Database.UserInfoes.Add(userInfo);
            DataProvider.Instance.Database.SaveChanges();
        }
        public UserInfo FindUserInfoByInfoItemId(Guid id)
        {
            return DataProvider.Instance.Database.UserInfoes.Where(userInfo => userInfo.Id == id).FirstOrDefault();
        }
        public void AddUserRole_UserInfoByRoleAndInfoItem(InfoItem infoItem,string role)
        {
            Guid userRoleId = DataProvider.Instance.Database.UserRoles.Where(userRole=>userRole.Role == role).FirstOrDefault().Id;
            UserInfo userInfo = FindUserInfoByInfoItemId(infoItem.Id);
            UserRole_UserInfo userRole_UserInfo = new UserRole_UserInfo()
            {
                Id = Guid.NewGuid(),
                IdRole = userRoleId,
                IdUserInfo = userInfo.Id,
                IsEnable = infoItem.IsEnable,
                IsDeleted = false,
            };
            DataProvider.Instance.Database.UserRole_UserInfo.Add(userRole_UserInfo);
            DataProvider.Instance.Database.SaveChanges();
            AddUser_UserRole_UserInfoBy(userRole_UserInfo);
        }
        public void AddUser_UserRole_UserInfoBy(UserRole_UserInfo userRole_UserInfo)
        {
            List<User> users = DataProvider.Instance.Database.Users.Where(user=>user.UserRole.Id == userRole_UserInfo.IdRole).ToList();
            foreach (User user in users)
            {
                User_UserRole_UserInfo user_UserRole_UserInfo = new User_UserRole_UserInfo()
                {
                    Id = Guid.NewGuid(),
                    IdUser = user.Id,
                    IdUserRole_Info = userRole_UserInfo.Id,
                    Content = "",
                };
                DataProvider.Instance.Database.User_UserRole_UserInfo.Add(user_UserRole_UserInfo);
                DataProvider.Instance.Database.SaveChanges();
            }      
        }
    }
}
