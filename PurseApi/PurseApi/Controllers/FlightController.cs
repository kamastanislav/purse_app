using PurseApi.Models.Entities;
using PurseApi.Models.Logger;
using PurseApi.Models.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace PurseApi.Controllers
{
    [RoutePrefix("purse/flight")]
    public class FlightController : ApiController
    {
        [Route("plan/{code}")]
        public IHttpActionResult PostFlightsPlan(int code)
        {
            try
            {
                var result = FlightManager.GetFlightsPlan(code);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("data/{code}")]
        public IHttpActionResult PostFlightPlan(int code)
        {
            try
            {
                var result = FlightManager.GetFlightPlan(code);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("create_flight")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult PostCreateFlight(int planCode, decimal plannedBudget, string comment)
        {
            Logger.WriteInfo("create");
            try
            {
                var result = FlightManager.CreateFlight(planCode, plannedBudget, comment);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("delete/{code}")]
        public IHttpActionResult DeleteFlight(int code)
        {
            try
            {
                var result = FlightManager.DeleteFlight(code);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("update_flight/{code}")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult PutFlight(int code, decimal plannedBudget, string comment)
        {
            try
            {
                var result = FlightManager.UpdateFlight(code, plannedBudget, comment);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("approve/{code}")]
        public IHttpActionResult PutApproveFlight(int code, decimal actualBudget)
        {
            try
            {
                var result = FlightManager.ApproveFlight(code, actualBudget);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
