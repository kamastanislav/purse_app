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

        public static bool CreateUser(UserData user)
        {
            return true; 
        }

        public static UserData LoginUser(string login, string password)
        {
            var userSession = UserSession.Create(login, password);
            return userSession != null ? userSession.User : null;
        }

        public static bool IsUnique(int field, string value)
        {
            UserRepository userRepo = new UserRepository();
            return userRepo.IsUnique((UserField)field, value);
        }
    }
}