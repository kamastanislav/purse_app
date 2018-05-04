using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PurseApi.Models.Helpers;
using System.Threading.Tasks;

namespace PurseApi.Models.Managers
{
    public class PlanManager
    {
        public static Plan GetPlan(int code)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                PlanRepository repo = new PlanRepository(false, (int)Constants.PlanAction.Code, code);
                if (repo.List.Any())
                    return repo.List.FirstOrDefault();
            }
            throw new Exception();
        }

        public static List<Plan> GetPlans(FilterData filter)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                if (filter.Executor == null && filter.Owner == null)
                {
                    filter.Executor = new List<int>() { user.Code };
                    filter.Owner = new List<int>() { user.Code };
                }
                PlanRepository repo = new PlanRepository(false, (int)Constants.PlanAction.List, filter: filter);
                return repo.List;
            }
            throw new NotImplementedException();
        }

        public static List<CategoryService> GetCategories()
        {
            var repo = new CategoryServiceRepository(false);
            return repo.List;
           
        }

        public static bool CreatePlan(Plan plan)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                plan.OwnerCode = user.Code;
                plan.CreateDate = Constants.TotalMilliseconds;
                plan.LastUpdate = Constants.TotalMilliseconds;
                plan.Status = (int)Constants.WorkflowStatus.InPlanned;
                var repo = new PlanRepository();
                var code = repo.InsertData(plan);
                
                if(code > 0)
                {
                    var task = new Task(() => InformationManager.CreatePlan(new Plan(code, plan), user.FamilyCode));
                    task.Start();
                    return true;
                }

            }
            throw new Exception();
        }

        public static bool UpdatePlan(Plan plan)
        {
            var userData = UserSession.Current.User;
            if (userData != null && userData.Code == plan.OwnerCode)
            {
                var result = GetPlan(plan.Code);

                var fields = CheckFieldsForUpdate(plan, result);
                if (fields.Count == 1)
                    return false;
                Logger.Logger.WriteInfo("Category " + plan.CategoryCode);
                var repo = new PlanRepository(false, (int)Constants.PlanAction.Code);
                plan.LastUpdate = Constants.TotalMilliseconds;
                return repo.UpdatePlan(plan, fields) != null;

            }
            throw new Exception();
        }

        private static List<int> CheckFieldsForUpdate(Plan plan, Plan result)
        {
            var fields = new List<int>() {(int)Constants.PlanField.LastUpdate };
            if (plan.Name != result.Name)
                fields.Add((int)Constants.PlanField.Name);
            if (plan.ExecutorCode != result.ExecutorCode)
                fields.Add((int)Constants.PlanField.ExecutorCode);
            if (plan.StartDate != result.StartDate)
                fields.Add((int)Constants.PlanField.StartDate);
            if (plan.EndDate != result.EndDate)
                fields.Add((int)Constants.PlanField.EndDate);
            if (plan.PlannedBudget != result.PlannedBudget)
                fields.Add((int)Constants.PlanField.PlannedBudget);
            if (plan.ActualBudget != result.ActualBudget)
                fields.Add((int)Constants.PlanField.ActualBudget);
            if (plan.CategoryCode != result.CategoryCode)
                fields.Add((int)Constants.PlanField.CategoryCode);
            if (plan.ServiceCode != result.ServiceCode)
                fields.Add((int)Constants.PlanField.ServiceCode);

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
                        if(UpdateStatus(plan, repo, (int)Constants.WorkflowStatus.Approved) != null)
                        {
                            var task = new Task(() => InformationManager.PlanApprove(plan, user.FamilyCode));
                            task.Start();
                            var task2 = new Task(() => TemplateManager.UpdatePlan(plan));
                            task2.Start();
                            return true;
                        }
                }
            }
            throw new Exception();
        }

        public static bool DeletePlan(int code)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                var repo = new PlanRepository(false, (int)Constants.PlanAction.Code, code);
                if (repo.List.Any())
                {
                    var plan = repo.List.FirstOrDefault();
                    if (plan.OwnerCode == user.Code)
                        if (UpdateStatus(plan, repo, (int)Constants.WorkflowStatus.Deleted) != null)
                        {
                            var task = new Task(() => InformationManager.DeletePlan(plan, user.FamilyCode));
                            task.Start();
                            return true;
                        }
                }
            }
            throw new Exception();
        }

        public static bool UndeletePlan(int code)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                var repo = new PlanRepository(false, (int)Constants.PlanAction.Code, code);
                if (repo.List.Any())
                {
                    var plan = repo.List.FirstOrDefault();
                    if (plan.OwnerCode == user.Code)
                        if( UpdateStatus(plan, repo, (int)Constants.WorkflowStatus.InPlanned) != null)
                        {
                            var task = new Task(() => InformationManager.UnDeletePlan(plan, user.FamilyCode));
                            task.Start();
                            return true;
                        }
                }
            }
            throw new Exception();
        }

        private static Plan UpdateStatus(Plan plan, PlanRepository repo, int status)
        {
            plan.LastUpdate = Constants.TotalMilliseconds;
            plan.Status = status;
            plan = repo.UpdatePlan(plan, new List<int>() { (int)Constants.PlanField.LastUpdate, (int)Constants.PlanField.Status });
            if (plan != null && (status == (int)Constants.WorkflowStatus.Deleted || status == (int)Constants.WorkflowStatus.InPlanned))
                FlightManager.UpdateStatus(plan.Flights, status);
            return plan;
        }

       // public static void Update 

    }
}