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
        public static bool CreateFlight(int planCode, decimal plannedBudget, string comment)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {

                Flight flight = new Flight()
                {
                    PlanCode = planCode,
                    PlannedBudget = plannedBudget,
                    Comment = comment,
                    OwnerCode = user.Code,
                    DateCreate = Constants.TotalMilliseconds,
                    Status = (int)Constants.WorkflowStatus.InPlanned,
                    ActualBudget = default(decimal)
                };
                var repo = new FlightRepository();
                var code = repo.InsertData(flight);
                if (code > Constants.DEFAULT_CODE)
                {
                    UpdatePlan(flight.Plan, true);
                }


                return code > Constants.DEFAULT_CODE;
            }
            throw new Exception();
        }

        public static bool UpdateFlight(int code, decimal plannedBudget, string comment)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                var repo = new FlightRepository(false, (int)Constants.FlightAction.Code, code);
                var flight = repo.List.FirstOrDefault();
                return UpdateFlight(flight, plannedBudget, comment, repo);
            }
            throw new Exception();
        }

        private static bool UpdateFlight(Flight flight, decimal plannedBudget, string comment, FlightRepository repo)
        {
            var fields = new List<int>();
            if(flight.PlannedBudget != plannedBudget)
            {
                fields.Add((int)Constants.FlightField.PlannedBudge);
                flight.PlannedBudget = plannedBudget;
            }
            if(flight.Comment != comment)
            {
                fields.Add((int)Constants.FlightField.Comment);
                flight.Comment = comment;
            }
            if (fields.Any())
            {
                flight = repo.UpdateFlight(flight, fields);
                if (flight == null)
                    return false;
                if(fields.Contains((int)Constants.FlightField.PlannedBudge))
                    UpdatePlan(flight.Plan, true);
            }
            return true;
        }

        private static void UpdatePlan(Plan plan, bool isPlanned = false)
        {
            try
            {
                var flights = plan.Flights;
                if (isPlanned)
                    plan.PlannedBudget = flights.Sum(x => x.PlannedBudget);
                else
                    plan.ActualBudget = flights.Sum(x => x.ActualBudget);

                plan.LastUpdate = Constants.TotalMilliseconds;
                var planRepo = new PlanRepository(actionCode: (int)Constants.PlanAction.Code, code: plan.Code);
                planRepo.UpdatePlan(plan, new List<int>() { (int)Constants.PlanField.LastUpdate, isPlanned ? (int)Constants.PlanField.PlannedBudget : (int)Constants.PlanField.ActualBudget });
            }
            catch(Exception ex)
            {
                Logger.Logger.WriteError(ex);
            }
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
                var repo = new FlightRepository(false, actionCode: (int)Constants.FlightAction.Code, code: code);
                var flight = repo.List.FirstOrDefault() ?? new Flight();
                var result = repo.DeleteFlight();
                if (result)
                {
                    UpdatePlan(flight.Plan, true);
                }
                return result;
            }
            throw new Exception();
        }

        public static void UpdateStatus(List<Flight> flights, int status)
        {
            try
            {
                var repo = new FlightRepository(actionCode: (int)Constants.FlightAction.Code);

                foreach (var flight in flights)
                {
                    flight.Status = status;
                    flight.DateCreate = Constants.TotalMilliseconds;
                    repo.UpdateFlight(flight, new List<int>() { (int)Constants.FlightField.Status, (int)Constants.FlightField.DateCreate });
                }

            }
            catch (Exception e)
            {
                Logger.Logger.WriteError(e);
            }
        }

        public static bool ApproveFlight(int code, decimal actualBudget)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                var repo = new FlightRepository(empty: false, actionCode: (int)Constants.FlightAction.Code, code: code);
                var flight = repo.List.FirstOrDefault();
                if (flight != null)
                {
                    flight.Status = (int)Constants.WorkflowStatus.Approved;
                    flight.ActualBudget = actualBudget;
                    flight.DateCreate = Constants.TotalMilliseconds;
                    flight = repo.UpdateFlight(flight, new List<int>() { (int)Constants.FlightField.ActualBudget, (int)Constants.FlightField.Status, (int)Constants.FlightField.DateCreate });
                    if (flight == null)
                        return false;

                    /*    var userRepo = new UserRepository((int)Constants.UserAction.Code);
                        user = userRepo.GetUser(user.Code);

                        var name = string.Format("{0} {1}({2})", user.FirstName, user.LastName, user.NickName);
                        HistoryManager.WriteInformation(user, -actualBudget, string.Format(Constants.ApproveFlight, name, plan.Name), new HistoryCashRepository());
                       */
                    var plan = flight.Plan;
                    UpdatePlan(plan);

                    return true;
                }
                return false;
            }
            throw new Exception();
        }

    }
}