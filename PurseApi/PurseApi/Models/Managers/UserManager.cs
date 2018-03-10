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
        public static List<UserData> GetUsers()
        {
            var userRepo = new UserRepository(false);
            return userRepo.List;
        }

        public static bool CreateUser(UserData user)
        {
            return true; 
        }

        public UserData LoginUser(string login, string password)
        {
            return null;
        }
    }
}