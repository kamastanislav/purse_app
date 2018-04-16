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
        public static object GetUsersFamily()
        {
            var user = UserSession.Current.User;
            if(user != null && user.FamilyCode > Constants.DEFAULT_CODE)
            {
                var userRepo = new UserRepository((int)Constants.UserAction.Family);
                return userRepo.GetList(user.FamilyCode);
            }
            throw new NotImplementedException();
        }
    }
}