using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Managers
{
    public class InformationManager
    {
        public static void MoneyTransfer(UserData userFrom, UserData userTo, decimal budget)
        {
            var infoRepo = new InformationRepository(withPlan: false);
            var historyRepo = new HistoryCashRepository();

            var nameFirst = string.Format("{0} {1}({2})", userFrom.FirstName, userFrom.LastName, userFrom.NickName);
            var nameLast = string.Format("{0} {1}({2})", userTo.FirstName, userTo.LastName, userTo.NickName);

            var information = new Information()
            {
                UserData = userFrom.Code,
                DateAction = Constants.TotalMilliseconds,
                Info = string.Format(Constants.Transfer, budget, nameFirst, nameLast)
            };
            infoRepo.InsertData(information);
            information.UserData = userTo.Code;
            infoRepo.InsertData(information);

            var historyCash = new HistoryCash();
            historyCash.Name = string.Format(Constants.Transfer, budget, nameFirst, nameLast);
            historyCash.DateAction = Constants.TotalMilliseconds;

            historyCash.Cash = budget;
            historyCash.UserCode = userTo.Code;
            historyRepo.InsertData(historyCash);

            historyCash.Cash = -budget;
            historyCash.UserCode = userFrom.Code;
            historyRepo.InsertData(historyCash);
        }

        public static void BudgetReplenishment(UserData user, decimal budget)
        {
            var infoRepo = new InformationRepository(withPlan: false);
            var historyRepo = new HistoryCashRepository();

            var information = new Information()
            {
                UserData = user.Code,
                DateAction = Constants.TotalMilliseconds,
                Info = string.Format(Constants.BudgetReplenishment, budget)
            };
            infoRepo.InsertData(information);

            var historyCash = new HistoryCash();
            historyCash.Cash = budget;
            historyCash.Name = string.Format(Constants.BudgetReplenishment, budget);
            historyCash.DateAction = Constants.TotalMilliseconds;
            historyCash.UserCode = user.Code;
            historyRepo.InsertData(historyCash);
        }

        public static void CreatePlan(Plan plan, int family)
        {
            SaveInfoForPlan(plan, family, Constants.CreatePlan);
        }

        public static void CreateFlight(Plan plan, Flight flight)
        {
            SaveInfoForFlight(plan, flight, Constants.CreateFlight);
        }

        public static void PlanApprove(Plan plan, int family)
        {
            SaveInfoForPlan(plan, family, Constants.ApprovePlan);
        }

        public static void FlightApprove(Plan plan, Flight flight)
        {
            SaveInfoForFlight(plan, flight, Constants.ApproveFlight);
            InformationUserCash(plan, flight);
        }

        public static void UnDeletePlan(Plan plan, int family)
        {
            SaveInfoForPlan(plan, family, Constants.UnDeletePlan);
        }

        public static void DeletePlan(Plan plan, int family)
        {
            SaveInfoForPlan(plan, family, Constants.DeletePlan);
        }

        public static void DeleteFlight(Plan plan, Flight flight)
        {
            SaveInfoForFlight(plan, flight, Constants.DeleteFlight);
        }

        public static void AddUserToFamily(Family family, string userName)
        {
            var infoRepo = new InformationRepository(withPlan: false);

            var users = family.Users;
            if (users == null)
                return;
            foreach (var user in users)
            {
                var information = new Information()
                {
                    UserData = user.Code,
                    DateAction = Constants.TotalMilliseconds,
                    Info = string.Format(Constants.AddNewUser, userName)
                };
                infoRepo.InsertData(information);
            }
        }

        private static void SaveInfoForPlan(Plan plan, int family, string message)
        {
            var infoRepo = new InformationRepository(true);

            if (family > Constants.DEFAULT_CODE)
            {
                var userRepo = new UserRepository((int)Constants.UserAction.Family);
                var users = userRepo.GetList(family);
                if (users == null)
                    return;
                foreach (var user in users)
                {
                    var information = new Information()
                    {
                        UserData = user.Code,
                        DateAction = Constants.TotalMilliseconds,
                        Info = string.Format(message, plan.Name),
                        PlanCode = plan.Code
                    };
                    infoRepo.InsertData(information);
                }
            }
            else
            {
                var information = new Information()
                {
                    UserData = plan.OwnerCode,
                    DateAction = Constants.TotalMilliseconds,
                    Info = string.Format(message, plan.Name),
                    PlanCode = plan.Code
                };
                infoRepo.InsertData(information);
            }
        }

        private static void SaveInfoForFlight(Plan plan, Flight flight, string message)
        {
            var infoRepo = new InformationRepository(true);

            var codes = new List<int>();

            codes.Add(flight.OwnerCode);
            if (!codes.Contains(plan.OwnerCode))
                codes.Add(plan.OwnerCode);
            if (!codes.Contains(plan.ExecutorCode))
                codes.Add(plan.ExecutorCode);

            foreach (var code in codes)
            {
                var information = new Information()
                {
                    UserData = code,
                    DateAction = Constants.TotalMilliseconds,
                    Info = string.Format(message, plan.Name, flight.Comment),
                    PlanCode = plan.Code
                };
                infoRepo.InsertData(information);
            }
        }

        private static void InformationUserCash(Plan plan, Flight flight)
        {
            var historyRepo = new HistoryCashRepository(true);
            var historyCash = new HistoryCash();
            historyCash.Name = string.Format(Constants.ApproveFlight, plan.Name, flight.Comment);
            historyCash.DateAction = Constants.TotalMilliseconds;
            historyCash.Cash = -flight.ActualBudget;
            historyCash.UserCode = plan.ExecutorCode;
            historyCash.PlanCode = plan.Code;
            historyCash.CategoryCode = plan.CategoryCode;
            historyRepo.InsertData(historyCash);
        }
    }
}