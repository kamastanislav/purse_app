using PurseApi.Models.Entities;
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
                userRepo = new UserRepository((int)UserAction.Family);
                return userRepo.GetList(famalyCode.Value);
            }
            else
            {
                userRepo = new UserRepository((int)UserAction.List);
                return userRepo.GetList();
            }
        }

        public static UserData GetUser(int userCode)
        {
            UserRepository userRepo = new UserRepository((int)UserAction.Id);
            var user = userRepo.GetUser(userCode);
            return user;
        }

        public static UserData CreateNewUser(UserData user, string password)
        {
            try
            {
                UserRepository userRepo = new UserRepository();
                user.Password = password;
                var code = userRepo.InsertData(user);
                return new UserData(user, code);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static UserData UpdateUser(UserData user, List<int> fields)
        {
            UserRepository userRepo = new UserRepository((int)UserAction.Id);

            user = userRepo.UpdateUserData(user, fields);
            return user;
        }

        public static int LogoutUser(int id)
        {
            var user = UserSession.Current.User;
            if (user == null)
                return -1;

            if (user.Code != id)
                return -2;

            return UserSession.DestroyIfExpired() ? 1 : 0;
        }

        public static UserData LoginUser(string login, string password)
        {
            var userSession = UserSession.Create(login, password);
            return userSession != null ? userSession.User : null;
        }

        public static UserData UpdateUserData(int id, UserData user)
        {
            var userData = UserSession.Current.User;
            if (userData != null && user.Code == id)
            {
                var userRepo = new UserRepository((int)UserAction.Id);
                var fields = CheckFieldsForUpdate(user, userData);
                if (fields.Any())
                {
                    user.LastLogin = DateTime.Now;
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
                fields.Add((int)UserField.FirstName);

            if (user.LastName != userData.LastName)
                fields.Add((int)UserField.LastName);

            if (user.NickName != userData.NickName)
                fields.Add((int)UserField.NickName);

            if (user.Email != userData.Email)
                fields.Add((int)UserField.Email);

            if (user.Phone != userData.Phone)
                fields.Add((int)UserField.Phone);

            if (user.Birthday != userData.Birthday)
                fields.Add((int)UserField.Birthday);
            
            if (fields.Any())
                fields.Add((int)UserField.LastLogin);
            return fields;
        }

        public static bool UpdateUserData(int id, string password)
        {
            var user = UserSession.Current.User;
            if (user != null && user.Code == id)
            {
                var userRepo = new UserRepository((int)UserAction.Id);
                user.Password = password;
                user.LastLogin = DateTime.Now;
                userRepo.UpdateUserData(user, new List<int>() { (int)UserField.Password, (int)UserField.LastLogin });
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
            return userRepo.IsUnique((UserField)field, value);
        }

        public static bool DeleteUser(int code)
        {
            UserRepository userRepo = new UserRepository((int)UserAction.Id);
            return userRepo.DeleteUser(code);
        }
    }
}