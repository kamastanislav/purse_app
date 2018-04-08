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
        public static List<Plan> GetPlans(int code, int action)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                if (action == (int)Constants.PlanAction.Family && user.FamilyCode != code)
                {
                    throw new Exception();
                }
                var repo = new PlanRepository(false, action, code);
                return repo.List;
            }
            throw new Exception();
        }

        public static Plan CreatePlan(Plan plan)
        {
            var user = UserSession.Current.User;
            if (user != null && user.Code == plan.OwnerCode)
            {
                var repo = new PlanRepository();
                var code = repo.InsertData(plan);
                return new Plan(code, plan);
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
                var result = PlanManager.GetPlans(plan.Code, (int)Constants.PlanAction.Code).FirstOrDefault();
                if (result!=null)
                {
                    var fields = CheckFieldsForUpdate(plan, result);
                    if (fields.Count == 1)
                        return null;
                    var repo = new PlanRepository();
                    plan.LastUpdate = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
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

        public static Plan ApprovePlan(int code)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                var repo = new PlanRepository(false, (int)Constants.PlanAction.Code, code);
                if (repo.List.Any())
                {
                    var plan = repo.List.FirstOrDefault();
                    if (plan.ExecutorCode == user.Code)
                        return UpdateStatus(plan, repo, (int)Constants.WorkflowStatus.Approved);
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
            plan.LastUpdate = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
            plan.Status = status;
            plan = repo.UpdatePlan(plan, new List<int>() { (int)Constants.PlanField.LastUpdate, (int)Constants.PlanField.Status });
            return plan;
        }

       // public static void Update 

    }
}