using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Managers
{
    public class TemplateManager
    {
        public static object UpdateTemplate(int code, bool use, bool data)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                var repo = new TemplateRepository((int)Constants.TemplateAction.Plan, code, false);
                var template = repo.List.FirstOrDefault();
                if (template != null)
                {
                    template.IsUpdate = data;
                    return use ? repo.UpdateTemplate(template) : repo.DeleteTemplate();
                }
                else
                {
                    var plan = PlanManager.GetPlan(code);
                    if (plan == null)
                        throw new Exception();

                    template = new TemplatePlan()
                    {
                        UserCode = user.Code,
                        AllPlan = code.ToString(),
                        IsUpdate = data,
                        PlanCode = code
                    };
                    return repo.InsertData(template) > Constants.DEFAULT_CODE;
                }
            }
            throw new NotImplementedException();
        }

        public static List<Plan> GetList()
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                var repo = new TemplateRepository((int)Constants.TemplateAction.List, user.Code, false);
                var codes = repo.List.Select(x => x.PlanCode).ToList();
                if (codes.Any())
                {
                    var planRepo = new PlanRepository(false, (int)Constants.PlanAction.Codes, codes: codes);
                    return planRepo.List;
                }
                return new List<Plan>();
            }
            throw new NotImplementedException();
        }

        public static int GetCreatePlanUseTemplate(int code)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                var repo = new TemplateRepository((int)Constants.TemplateAction.Plan, code, false);
                var template = repo.List.FirstOrDefault();
                if (template == null)
                    throw new Exception();

                var planRepo = new PlanRepository(false, (int)Constants.PlanAction.Code, code);
                var plan = planRepo.List.FirstOrDefault();
                if (plan != null)
                {
                    var flights = plan.Flights;
                    plan.CreateDate = Constants.TotalMilliseconds;
                    plan.LastUpdate = Constants.TotalMilliseconds;
                    plan.StartDate = Constants.TotalMilliseconds;
                    plan.EndDate = Constants.TotalMilliseconds;
                    plan.Name = plan.Name.Substring(0, plan.Name.Length - 12);
                    plan.ActualBudget = 0;
                    plan.PlannedBudget = flights.Sum(x => x.ActualBudget);
                    plan.Status = (int)Constants.WorkflowStatus.InPlanned;

                    code = planRepo.InsertData(plan);
                    if (code == Constants.DEFAULT_CODE)
                        throw new Exception();

                    template.SetPlanCode(code);
                    repo.UpdateTemplate(template);

                    var flightRepo = new FlightRepository();
                    foreach(var flight in flights)
                    {
                        flight.PlanCode = code;
                        flight.PlannedBudget = flight.ActualBudget;
                        flight.ActualBudget = 0;
                        flight.OwnerCode = user.Code;
                        flight.Status = (int)Constants.WorkflowStatus.InPlanned;
                        flight.DateCreate = Constants.TotalMilliseconds;
                        flightRepo.InsertData(flight);
                    }

                    return code;

                }

            }
            throw new NotImplementedException();
        }

        public static List<bool> GetInfoTemplate(int code)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                var repo = new TemplateRepository((int)Constants.TemplateAction.Plan, code, false);
                var template = repo.List.FirstOrDefault();
                var haveTemplate = template != null;
                var info = new List<bool>() { haveTemplate, haveTemplate ? template.IsUpdate : false };

                return info;
            }
            throw new NotImplementedException();
        }

        public static void UpdatePlan(Plan plan)
        {
            var repo = new TemplateRepository((int)Constants.TemplateAction.LastCode, plan.Code, false);
            var template = repo.List.FirstOrDefault();
            if (template != null)
            {
                var planRepo = new PlanRepository(false, (int)Constants.PlanAction.Code, plan.Code);
                var planTemp = planRepo.List.FirstOrDefault();

                if (template.IsUpdate && planTemp != null && planTemp.CategoryCode == plan.CategoryCode && planTemp.ServiceCode == plan.ServiceCode)
                {
                    template.PlanCode = plan.Code;
                    repo.UpdateTemplate(template);
                }
            }
        }
    }
}