using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Managers
{
    public class PlanManager
    {
        public static List<Plan> GetPlans(int action, int? code = null)
        {
          //  Logger.Logger.WriteInfo(string.Format("get plans {0} {1}", code.Value, ((Constants.PlanAction)action).ToString()));
            var user = UserSession.Current.User;
            if (user != null)
            {
                PlanRepository repo = null;
                if (action == (int)Constants.PlanAction.Owner)
                    repo = new PlanRepository(false, action, code == null ? user.Code : code.Value);
                else if (action == (int)Constants.PlanAction.Executor)
                    repo = new PlanRepository(false, action, code == null ? user.Code : code.Value);
                else if (action == (int)Constants.PlanAction.Family && user.FamilyCode != Constants.DEFAULT_CODE)
                    repo = new PlanRepository(false, action, user.FamilyCode);
                else if (action == (int)Constants.PlanAction.Code)
                    repo = new PlanRepository(false, action, code.Value);
                return repo != null ? repo.List : new List<Plan>();
            }
            throw new Exception();
        }

        public static bool CreatePlan(Plan plan)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                plan.OwnerCode = user.Code;
                plan.CreateDate = Constants.TotalMilliseconds;
                plan.LastUpdate = Constants.TotalMilliseconds;
                if (!user.IsNoneFamily)
                    plan.FamilyCode = user.FamilyCode;
                plan.Status = (int)Constants.WorkflowStatus.InPlanned;
                var repo = new PlanRepository(all: !user.IsNoneFamily);
                var code = repo.InsertData(plan);
                Logger.Logger.WriteInfo("Create plan");
                return code > 0;
            }
            throw new Exception();
        }

        public static object GetDeletedPlans(int code, int action)
        {
            var user = UserSession.Current.User;
            if (user != null && user.Code == code)
            {
                var repo = new PlanRepository(false, action, code, false);
                return repo.List;
            }
            throw new NotImplementedException();
        }

        public static Plan UpdatePlan(Plan plan)
        {
            var userData = UserSession.Current.User;
            if (userData != null && (userData.Code == plan.ExecutorCode || (userData.Code == plan.OwnerCode)))
            {
                var result = PlanManager.GetPlans((int)Constants.PlanAction.Code, plan.Code).FirstOrDefault();
                if (result!=null)
                {
                    var fields = CheckFieldsForUpdate(plan, result);
                    if (fields.Count == 1)
                        return null;
                    var repo = new PlanRepository();
                    plan.LastUpdate = Constants.TotalMilliseconds;
                    return repo.UpdatePlan(plan, fields);
                }               
            }
            throw new Exception();
        }

        private static List<int> CheckFieldsForUpdate(Plan plan, Plan result)
        {
            var fields = new List<int>() {(int)Constants.PlanField.LastUpdate };
            if (plan.ExecutorCode != result.ExecutorCode)
                fields.Add((int)Constants.PlanField.ExecutorCode);
            if (plan.StartDate != result.StartDate)
                fields.Add((int)Constants.PlanField.StartDate);
            if (plan.EndDate != result.EndDate)
                fields.Add((int)Constants.PlanField.EndDate);
            if (plan.PlannedBudget != result.PlannedBudget)
                fields.Add((int)Constants.PlanField.PlannedBudget);
            if (plan.IsPrivate != result.IsPrivate)
                fields.Add((int)Constants.PlanField.IsPrivate);

            return fields;
        }

        public static bool ApprovePlan(int code)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                var repo = new PlanRepository(false, (int)Constants.PlanAction.Code, code);
                if (repo.List.Any())
                {
                    var plan = repo.List.FirstOrDefault();
                    if (plan.ExecutorCode == user.Code)
                        return UpdateStatus(plan, repo, (int)Constants.WorkflowStatus.Approved) != null;
                }
            }
            throw new Exception();
        }

        public static Plan DeletePlan(int code)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                var repo = new PlanRepository(false, (int)Constants.PlanAction.Code, code);
                if (repo.List.Any())
                {
                    var plan = repo.List.FirstOrDefault();
                    if (plan.OwnerCode == user.Code)
                       return UpdateStatus(plan, repo, (int)Constants.WorkflowStatus.Deleted);
                }
            }
            throw new Exception();
        }

        public static Plan UndeletePlan(Plan plan)
        {
            
            throw new Exception();
        }

        public static Plan UndeletePlan(int code)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                var repo = new PlanRepository(false, (int)Constants.PlanAction.Code, code);
                if (repo.List.Any())
                {
                    var plan = repo.List.FirstOrDefault();
                    if (plan.OwnerCode == user.Code)
                        return UpdateStatus(plan, repo, (int)Constants.WorkflowStatus.InPlanned);
                }
            }
            throw new Exception();
        }

        private static Plan UpdateStatus(Plan plan, PlanRepository repo, int status)
        {
            plan.LastUpdate = Constants.TotalMilliseconds;
            plan.Status = status;
            plan = repo.UpdatePlan(plan, new List<int>() { (int)Constants.PlanField.LastUpdate, (int)Constants.PlanField.Status });
            return plan;
        }

       // public static void Update 

    }
}