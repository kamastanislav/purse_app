using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using PurseApi.Models.Helpers;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PurseApi.Models.Managers
{
    public class HistoryManager
    {
        public static List<HistoryCash> GetHistoryCash(FilterData filter)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                HistoryCashRepository repo = new HistoryCashRepository(filter);
                if(repo != null)
                    return repo.List;
            }
            return new List<HistoryCash>();
        }

        public static bool BudgetReplenishment(int? code, decimal budget)
        {
            var user = UserSession.Current.User;
            if(user != null)
            {
                var userRepo = new UserRepository((int)Constants.UserAction.Code);
                if (code == null)
                {
                    user = userRepo.GetUser(user.Code);
                    user.Cash += budget;
                    var task = new Task(() => InformationManager.BudgetReplenishment(user, budget));
                    task.Start();
                    return UserManager.UpdateUserData(user) != null;

                }
                else
                {
                    var userData = userRepo.GetUser(code.Value);
                    if (userData != null)
                    {
                        userData.Cash += budget;
                        userRepo.UpdateUserData(userData.Code, userData, new List<int>() { (int)Constants.UserField.Cash });

                        user = userRepo.GetUser(user.Code);
                        user.Cash -= budget;
                        user.LastLogin = Constants.TotalMilliseconds;
                        UserManager.UpdateUserData(user);
                       
                        var task = new Task(() => InformationManager.MoneyTransfer(user, userData, budget));
                        task.Start();
                        return true;
                    }

                }
        
                return false;
            }
            throw new NotImplementedException();
        }

        public static void DeleteInformation()
        {
            var repo = new InformationRepository(0, (int)Constants.InformationAction.Delete, empty: true);
            repo.DeleteInformationData();
        }

        public static List<Information> GetHistoryInformation()
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                Logger.Logger.WriteInfo(Constants.TotalMillisecondsTwoWeeksOld.ToString());
                var infoRepo = new InformationRepository(1, (int)Constants.InformationAction.Select);
                return infoRepo.List;
            }
            throw new NotImplementedException();
        }
    }
}