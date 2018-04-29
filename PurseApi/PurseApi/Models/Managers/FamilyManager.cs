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

        public static bool CreateFamily()
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
                    return true;
                }
                return false;
                
            }
            throw new NotImplementedException();
        }
    }
}