using PurseApi.Models.Helper;
using PurseApi.Models.Managers;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PurseApi.Controllers
{
    [RoutePrefix("purse/history_cash")]
    public class HistoryCashController : ApiController
    {
        [Route("user/{code}")]
        public IHttpActionResult GetHistoryUser(int code)
        {
            try
            {
                var result = HistoryCashManager.GetHistoryCash(code, (int)Constants.HistoryCashAction.UserCode);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("family/{code}")]
        public IHttpActionResult GetHistoryFamily(int code)
        {
            try
            {
                var result = HistoryCashManager.GetHistoryCash(code, (int)Constants.HistoryCashAction.Family);
                return Ok(result);
            }
            catch(Exception)
            {
                return NotFound();
            }
        }

       
    }
}
