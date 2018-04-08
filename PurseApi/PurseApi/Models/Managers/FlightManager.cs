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
        public static Flight CreateFlight(Flight flight)
        {
            var user = UserSession.Current.User;
            if (user != null && user.Code == flight.OwnerCode)
            {
                var repo = new FlightRepository();
                var code = repo.InsertData(flight);

                return new Flight(code, flight);
            }
            throw new Exception();
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