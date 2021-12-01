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

        public List<User_UserRole_UserInfo> LoadInfoItemByUserId(Guid userId)
        {
            List<User_UserRole_UserInfo> db = DataProvider.Instance.Database.User_UserRole_UserInfo.Where(infoItem => infoItem.IdUser == userId && infoItem.UserRole_UserInfo.IsDeleted == false).ToList();
            return db;
        }
        public ObservableCollection<InfoItem> GetInfoSourceByUserId(Guid userId)
        {
            ObservableCollection<InfoItem> InfoSource = new ObservableCollection<InfoItem>();
            List<User_UserRole_UserInfo> db = DataProvider.Instance.Database.User_UserRole_UserInfo.Where(infoItem => infoItem.IdUser == userId && infoItem.UserRole_UserInfo.IsDeleted == false).ToList();
            foreach (User_UserRole_UserInfo infoItem in db)
            {
                InfoItem info = new InfoItem();
                info.Id = infoItem.Id;
                info.LabelName = infoItem.UserRole_UserInfo.InfoName;
                info.Type = Convert.ToInt32(infoItem.UserRole_UserInfo.Type);
                if (info.Type == 2)
                {
                    ObservableCollection<string> itemSources = new ObservableCollection<string>();
                    List<UserRole_UserInfoItem> userInfoItems = DataProvider.Instance.Database.UserRole_UserInfoItem.Where(userInfoItem => userInfoItem.UserRole_UserInfo == infoItem.UserRole_UserInfo).ToList();
                    userInfoItems.ForEach(userInfoItem => itemSources.Add(userInfoItem.Content));
                    info.ItemSource = itemSources;
                }
                info.Value = infoItem.Content;
                info.IsEnable = Convert.ToBoolean(infoItem.UserRole_UserInfo.IsEnable);
                InfoSource.Add(info);
            }
            return InfoSource;
        }
        public Guid FindUserRoleByRole(string role)
        {
            return DataProvider.Instance.Database.UserRoles.Where(userRoles => userRoles.Role == role).FirstOrDefault().Id;
        }
        public ObservableCollection<InfoItem> GetInfoSourceInSettingByRole(string role)
        {
            ObservableCollection<InfoItem> InfoSourece = new ObservableCollection<InfoItem>();
            List<UserRole_UserInfo> db = DataProvider.Instance.Database.UserRole_UserInfo.Where(userRole_UserInfo => userRole_UserInfo.UserRole.Role == role && userRole_UserInfo.IsDeleted == false).ToList();
            foreach (UserRole_UserInfo item in db)
            {
                InfoItem info = new InfoItem();
                info.Id = item.Id;
                info.LabelName = item.InfoName;
                info.Type = Convert.ToInt32(item.Type);
                if (info.Type == 2)
                {
                    ObservableCollection<string> itemSources = new ObservableCollection<string>();
                    List<UserRole_UserInfoItem> userInfoItems = DataProvider.Instance.Database.UserRole_UserInfoItem.Where(userInfoItem => userInfoItem.UserRole_UserInfo == item).ToList();
                    userInfoItems.ForEach(userInfoItem => itemSources.Add(userInfoItem.Content));
                    info.ItemSource = itemSources;
                }
                info.IsEnable = Convert.ToBoolean(item.IsEnable);
                InfoSourece.Add(info);
            }
            return InfoSourece;
        }
        //public UserInfo ConverInfoItemToUserInfo(InfoItem infoItem)
        //{
        //    UserInfo userInfo = new UserInfo()
        //    {
        //        Id = infoItem.Id,
        //        InfoName = infoItem.LabelName,
        //        Type = infoItem.Type,
        //    };
        //    return userInfo;
        //}
        //public void AddUserInfoByInfoItem(InfoItem infoItem)
        //{
        //    UserInfo userInfo = ConverInfoItemToUserInfo(infoItem);
        //    DataProvider.Instance.Database.UserInfoes.Add(userInfo);
        //    DataProvider.Instance.Database.SaveChanges();
        //}
        //public UserInfo FindUserInfoByInfoItemId(Guid id)
        //{
        //    return DataProvider.Instance.Database.UserInfoes.Where(userInfo => userInfo.Id == id).FirstOrDefault();
        //}
        public void AddUserRole_UserInfoByRoleAndInfoItem(InfoItem infoItem, string role)
        {
            Guid userRoleId = DataProvider.Instance.Database.UserRoles.Where(userRole => userRole.Role == role).FirstOrDefault().Id;
            UserRole_UserInfo userRole_UserInfo = new UserRole_UserInfo()
            {
                Id = Guid.NewGuid(),
                IdRole = userRoleId,
                InfoName = infoItem.LabelName,
                Type = infoItem.Type,
                IsEnable = infoItem.IsEnable,
                IsDeleted = false,
            };
            DataProvider.Instance.Database.UserRole_UserInfo.Add(userRole_UserInfo);
            DataProvider.Instance.Database.SaveChanges();
            AddUser_UserRole_UserInfoByUserRole_UserInfo(userRole_UserInfo);
            if(infoItem.Type==2)
                AddUserRole_UserInfoItemByInfoItem(infoItem, role);
        }
        public void AddUser_UserRole_UserInfoByUserRole_UserInfo(UserRole_UserInfo userRole_UserInfo)
        {
            List<User> users = DataProvider.Instance.Database.Users.Where(user => user.IdUserRole == userRole_UserInfo.IdRole).ToList();
            if (users.Count == 0)
                return;
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
            }
            DataProvider.Instance.Database.SaveChanges();
        }
        public UserRole_UserInfo FindUserRole_UserInfoByInfoItemAndRole(InfoItem infoItem, string Roles)
        {
            return DataProvider.Instance.Database.UserRole_UserInfo.Where(userRole_UserInfo=> userRole_UserInfo.UserRole.Role == Roles && userRole_UserInfo.InfoName == infoItem.LabelName).FirstOrDefault();
        }
        public void AddUserRole_UserInfoItemByInfoItem(InfoItem infoItem, string Roles)
        {
            UserRole_UserInfo userRole_UserInfo = FindUserRole_UserInfoByInfoItemAndRole(infoItem, Roles);
            foreach (var itemInCombobox in infoItem.ItemSource)
            {
                UserRole_UserInfoItem userInfoItem = new UserRole_UserInfoItem()
                {
                    Id = Guid.NewGuid(),
                    IdUserRole_Info = userRole_UserInfo.Id,
                    Content = itemInCombobox,
                };
                DataProvider.Instance.Database.UserRole_UserInfoItem.Add(userInfoItem);
            }
            DataProvider.Instance.Database.SaveChanges();
        }
        public void UpdateUser_UserRole_UserInfoByInfoItem(InfoItem infoItem)
        {
            User_UserRole_UserInfo user_UserRole_UserInfo = DataProvider.Instance.Database.User_UserRole_UserInfo.Where(item => item.Id == infoItem.Id).FirstOrDefault();
            if (user_UserRole_UserInfo == null)
                return;
            user_UserRole_UserInfo.Content = Convert.ToString(infoItem.Value);
            DataProvider.Instance.Database.SaveChanges();
        }
    }
}
