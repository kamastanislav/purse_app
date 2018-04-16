using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Managers
{
    public class FlightManager
    {
        public static bool CreateFlight(Flight flight)
        {
            var user = UserSession.Current.User;
            var planRepo = new PlanRepository(false, (int)Constants.PlanAction.Code, flight.PlanCode);
            var plan = planRepo.List.FirstOrDefault();
            if (user != null && plan != null)
            {
                flight.OwnerCode = user.Code;
                flight.DateCreate = Constants.TotalMilliseconds;
                flight.Status = (int)Constants.WorkflowStatus.InPlanned;
                var repo = new FlightRepository();
                var code = repo.InsertData(flight);

                if (code > Constants.DEFAULT_CODE)
                {
                    plan.PlannedBudget += flight.PlannedBudget;
                    plan.CountFlight++;
                    plan.LastUpdate = Constants.TotalMilliseconds;
                    plan = planRepo.UpdatePlan(plan, new List<int> { (int)Constants.PlanField.LastUpdate, (int)Constants.PlanField.PlannedBudget, (int)Constants.PlanField.CountFlight });
                }


                return code > Constants.DEFAULT_CODE && plan != null;
            }
            throw new Exception();
        }

        public static List<Flight> GetFlightsPlan(int code)
        {
            var user = UserSession.Current.User;
            var repo = new FlightRepository(false, (int)Constants.FlightAction.Plan, code);
            return repo.List;
        }

        public static Flight GetFlightPlan(int code)
        {
            var user = UserSession.Current.User;
            var repo = new FlightRepository(false, (int)Constants.FlightAction.Code, code);
            if (repo.List.Any())
                return repo.List.FirstOrDefault();
            return null;
        }

        public static bool DeleteFlight(int code)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                var repo = new FlightRepository(actionCode:  (int)Constants.FlightAction.Code, code: code);
                var result = repo.DeleteFlight();
                if (result)
                {

                }
                return result;
            }
            throw new Exception();
        }

    }
}