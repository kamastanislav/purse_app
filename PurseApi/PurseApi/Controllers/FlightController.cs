using PurseApi.Models.Entities;
using PurseApi.Models.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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

        [Route("create")]
        public IHttpActionResult PostCreateFlight(Flight flight)
        {
            try
            {
                var result = FlightManager.CreateFlight(flight);
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
                var result = 1;
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("update/{code}")]
        public IHttpActionResult PutFlight(int code)
        {
            try
            {
                var result = 1;
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("approve/{code}")]
        public IHttpActionResult PutApproveFlight(int code)
        {
            try
            {
                var result = 1;
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
