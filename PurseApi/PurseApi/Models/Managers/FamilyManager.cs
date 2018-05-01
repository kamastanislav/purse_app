using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Managers
{
    public class FamilyManager
    {
        public static List<UserData> GetUsersFamily()
        {
            var user = UserSession.Current.User;
            if(user != null && user.FamilyCode > Constants.DEFAULT_CODE)
            {
                var userRepo = new UserRepository((int)Constants.UserAction.Family);
                return userRepo.GetList(user.FamilyCode);
            }
            throw new NotImplementedException();
        }

        public static int CreateFamily()
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                var repo = new FamilyRepository();
                Family family = new Family()
                {
                    OwnerCode = user.Code
                };

                var code = repo.InsertData(family);
                if (code > Constants.DEFAULT_CODE)
                {
                    user.FamilyCode = code;
                    user.StatusCode = (int)Constants.StatusUser.Admin;
                    var userRepo = new UserRepository((int)Constants.UserAction.Code);
                    userRepo.UpdateUserData(user.Code, user, new List<int>() { (int)Constants.UserField.FamilyCode, (int)Constants.UserField.StatusCode });
                    return code;
                }
                
            }
            throw new NotImplementedException();
        }

        public static bool AddUser(int code)
        {
            var user = UserSession.Current.User;
            if (user != null && user.FamilyCode > Constants.DEFAULT_CODE)
            {
                var userRepo = new UserRepository((int)Constants.UserAction.Code);
                var userData = userRepo.GetUser(code);
                if (userData!= null && userData.FamilyCode == Constants.DEFAULT_CODE)
                {
                    userData.FamilyCode = user.FamilyCode;
                    userData = userRepo.UpdateUserData(code, userData, new List<int>() { (int)Constants.UserField.FamilyCode });

                    return userData != null;
                }
            }
            throw new NotImplementedException();
        }

        public static List<UserData> GetSearchUsers(string name)
        {
            var user = UserSession.Current.User;
            if (user != null && user.FamilyCode > Constants.DEFAULT_CODE)
            {
                var userRepo = new UserRepository((int)Constants.UserAction.Nick);
                return userRepo.GetList(name);
            }
            throw new NotImplementedException();
        }
    }
}