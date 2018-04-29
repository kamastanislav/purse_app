using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Managers
{
    public class HistoryManager
    {
        public static List<HistoryCash> GetHistoryCash(int? code)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
               // HistoryCashRepository repo = null;
                /*  if (action == (int)Constants.HistoryCashAction.Family && user.FamilyCode > Constants.DEFAULT_CODE)
                      repo = new HistoryCashRepository(user.FamilyCode, action);
                  else if (action == (int)Constants.HistoryCashAction.UserCode)*/
                HistoryCashRepository repo = new HistoryCashRepository(code == null ? user.Code : code.Value);
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
                var repo = new HistoryCashRepository();
                var infoRepo = new InformationRepository();
                var userRepo = new UserRepository((int)Constants.UserAction.Code);
                if (code == null)
                {
                    /* var name = string.Format("{0} {1}({2})", user.FirstName, user.LastName, user.NickName);
                     try
                     {
                         var information = new Information()
                         {
                             UserData = user.Code,
                             DateAction = Constants.TotalMilliseconds,
                             Info = string.Format(Constants.BudgetReplenishment, name)
                         };
                         infoRepo.InsertData(information);
                     }
                     catch (Exception ex)
                     {
                         Logger.Logger.WriteError(ex);
                     }
                     user = userRepo.GetUser(user.Code);*/
                    user.Cash += budget;
                return    UserManager.UpdateUserData(user) != null;
               //     return WriteInformation(user, budget, string.Format(Constants.BudgetReplenishment, name), repo);
                   

                   
                }
                else
                {
                    var userData = userRepo.GetUser(code.Value);
                    if (userData != null)
                    {
                        userData.Cash += budget;
                        userRepo.UpdateUserData(userData.Code, userData, new List<int>() { (int)Constants.UserField.Cash });
                        user.Cash -= budget;
                        user.LastLogin = Constants.TotalMilliseconds;
                        userRepo.UpdateUserData(user.Code, user, new List<int>() { (int)Constants.UserField.Cash, (int)Constants.UserField.LastLogin });

                        var historyCash = new HistoryCash();
                        historyCash.Cash = budget;
                        var nameFirst = string.Format("{0} {1}({2})", user.FirstName, user.LastName, user.NickName);
                        var nameLast = string.Format("{0} {1}({2})", userData.FirstName, userData.LastName, userData.NickName);
                        historyCash.Name = string.Format(Constants.Transfer, nameFirst, nameLast);
                        historyCash.DateAction = Constants.TotalMilliseconds;
                        historyCash.UserCode = userData.Code;
                      /*  if (!userData.IsNoneFamily)
                        {
                            repo.SetActionInsert((int)Constants.HistoryCashInsertAction.Family);
                            historyCash.FamilyCode = userData.FamilyCode;
                        }*/
                        repo.InsertData(historyCash);

                        historyCash.Cash = -budget;
                        historyCash.UserCode = user.Code;
                        repo.SetActionInsert((int)Constants.HistoryCashInsertAction.Empty);
                      /*  if (!userData.IsNoneFamily)
                        {
                            repo.SetActionInsert((int)Constants.HistoryCashInsertAction.Family);
                            historyCash.FamilyCode = userData.FamilyCode;
                        }*/
                        repo.InsertData(historyCash);

                        try
                        {
                            var information = new Information()
                            {
                                UserData = user.Code,
                                DateAction = Constants.TotalMilliseconds,
                                Info = string.Format(Constants.Transfer, nameFirst, nameLast)
                            };
                            infoRepo.InsertData(information);
                            information.UserData = userData.Code;
                            infoRepo.InsertData(information);
                        }
                        catch (Exception ex)
                        {
                            Logger.Logger.WriteError(ex);
                        }

                        return true;
                    }

                }
        
                return false;
            }
            throw new NotImplementedException();
        }

        public static bool WriteInformation(UserData user, decimal budget, string name, HistoryCashRepository repo)
        {
            user.Cash += budget;
            UserManager.UpdateUserData(user);

            var historyCash = new HistoryCash();
            historyCash.Cash = budget;
            historyCash.Name = name;
            historyCash.DateAction = Constants.TotalMilliseconds;
            historyCash.UserCode = user.Code;
          /*  if (!user.IsNoneFamily)
            {
                historyCash.FamilyCode = user.FamilyCode;
                repo.SetActionInsert((int)Constants.HistoryCashInsertAction.Family);
            }*/
            var codeHistory = repo.InsertData(historyCash);


            return codeHistory > Constants.DEFAULT_CODE;
        }

        public static List<Information> GetHistoryInformation()
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                var infoRepo = new InformationRepository(user.Code, (int)Constants.InformationAction.Select);
                return infoRepo.List;
            }
            throw new NotImplementedException();
        }
    }
}