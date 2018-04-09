using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using PurseApi.Models.Helpers;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Managers
{
    public class UserManager
    {
        public static List<UserData> GetUsers(int? famalyCode)
        {
            UserRepository userRepo;
            if (famalyCode.HasValue)
            {
                userRepo = new UserRepository((int)Constants.UserAction.Family);
                return userRepo.GetList(famalyCode.Value);
            }
            else
            {
                userRepo = new UserRepository((int)Constants.UserAction.List);
                return userRepo.GetList();
            }
        }

        public static UserData GetUser(int userCode)
        {
            UserRepository userRepo = new UserRepository((int)Constants.UserAction.Code);
            var user = userRepo.GetUser(userCode);
            return user;
        }

        public static int CreateNewUser(UserData user)
        {
            try
            {
                user.CreateDate = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
                user.LastLogin = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
                UserRepository userRepo = new UserRepository();
                var code = userRepo.InsertData(user);
                return code;
            }
            catch (Exception)
            {
                return Constants.DEFAULT_CODE;
            }
        }

        public static UserData UpdateUser(UserData user, List<int> fields)
        {
            UserRepository userRepo = new UserRepository((int)Constants.UserAction.Code);

            user = userRepo.UpdateUserData(user, fields);
            return user;
        }

        public static bool LogoutUser()
        {
            try
            {
                return UserSession.DestroyIfExpired();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static int LoginUser(UserLogin userLogin)
        {
            var userSession = UserSession.Create(userLogin.NickName, userLogin.Password);
            return userSession != null ? userSession.User.Code : Constants.DEFAULT_CODE;
        }

        public static UserData UpdateUserData(int id, UserData user)
        {
            var userData = UserSession.Current.User;
            if (userData != null && user.Code == id)
            {
                var userRepo = new UserRepository((int)Constants.UserAction.Code);
                var fields = CheckFieldsForUpdate(user, userData);
                if (fields.Any())
                {
                    user.LastLogin = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
                    user = userRepo.UpdateUserData(user, fields);
                    var userSession = UserSession.UpdateSession(user);
                    return userSession != null ? userSession.User : null;
                }
            }
            throw new Exception();
        }

        private static List<int> CheckFieldsForUpdate(UserData user, UserData userData)
        {
            List<int> fields = new List<int>();

            if (user.FirstName != userData.FirstName)
                fields.Add((int)Constants.UserField.FirstName);

            if (user.LastName != userData.LastName)
                fields.Add((int)Constants.UserField.LastName);

            if (user.NickName != userData.NickName)
                fields.Add((int)Constants.UserField.NickName);

            if (user.Email != userData.Email)
                fields.Add((int)Constants.UserField.Email);

            if (user.Phone != userData.Phone)
                fields.Add((int)Constants.UserField.Phone);

            if (user.Birthday != userData.Birthday)
                fields.Add((int)Constants.UserField.Birthday);
            
            if (fields.Any())
                fields.Add((int)Constants.UserField.LastLogin);
            return fields;
        }

        public static bool UpdateUserData(int id, string password)
        {
            var user = UserSession.Current.User;
            if (user != null && user.Code == id)
            {
                var userRepo = new UserRepository((int)Constants.UserAction.Code);
                user.Password = password;
                user.LastLogin = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
                userRepo.UpdateUserData(user, new List<int>() { (int)Constants.UserField.Password, (int)Constants.UserField.LastLogin });
                return true;
            }
            throw new Exception();
        }

        public static bool CheckPassword(int id, string password)
        {
            var user = UserSession.Current.User;
            if (user != null && user.Code == id)
            {
                var userRepo = new UserRepository();
                return userRepo.CheckUserPassword(id, password);
            }
            throw new Exception();
        }

        public static bool IsUnique(int field, string value)
        {
            UserRepository userRepo = new UserRepository();
            return !userRepo.IsUnique((Constants.UserField)field, value);
        }

        public static bool DeleteUser(int code)
        {
            UserRepository userRepo = new UserRepository((int)Constants.UserAction.Code);
            return userRepo.DeleteUser(code);
        }
    }
}