using PurseApi.Models.Helper;
using PurseApi.Models.Helpers;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Managers
{
    public class FilterManager
    {
        public static FilterPlan GetFilterPlan()
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                var filterPlan = new FilterPlan();
                var categoryServiceRepo = new CategoryServiceRepository(false);
                filterPlan.CategoryServices = categoryServiceRepo.List;
                filterPlan.OwnerUser = string.Format("{0} {1}({2})", user.FirstName, user.LastName, user.NickName);
                filterPlan.HavingFamily = !user.IsNoneFamily;
                if (filterPlan.HavingFamily)
                {
                    var userRepo = new UserRepository((int)Constants.UserAction.Family);
                    filterPlan.Executors = userRepo.GetList(user.FamilyCode);
                }
                else
                    filterPlan.Executors = new List<Entities.UserData>() { user };
                return filterPlan;
            }
            return null;
        }
    }
}