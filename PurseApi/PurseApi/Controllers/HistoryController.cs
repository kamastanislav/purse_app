using PurseApi.Models.Helper;
using PurseApi.Models.Helpers;
using PurseApi.Models.Logger;
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
    [RoutePrefix("purse/history")]
    public class HistoryController : ApiController
    {
        [Route("user_cash")]
        public IHttpActionResult PostHistoryUser(FilterData filter)
        {
            try
            {
                var result = HistoryManager.GetHistoryCash(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return NotFound();
            }
        }

        [Route("budget_replenishment")]
        public IHttpActionResult PostBudgetReplenishment(decimal budget)
        {
            try
            {
                var result = HistoryManager.BudgetReplenishment(null, budget);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return NotFound();
            }
        }

        [Route("budget_replenishment_other_user/{code}")]
        public IHttpActionResult PostBudgetReplenishment(int code, decimal budget)
        {
            try
            {
                var result = HistoryManager.BudgetReplenishment(code, budget);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return NotFound();
            }
        }

        [Route("info")]
        public IHttpActionResult PostInformation()
        {
            try
            {
                var result = HistoryManager.GetHistoryInformation();// HistoryCashManager.BudgetReplenishment(code, budget);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return NotFound();
            }
        }

    }

    [RoutePrefix("purse/history")]
    public class CopyOfHistoryController : ApiController
    {
        [Route("user_cash")]
        public IHttpActionResult PostHistoryUser(FilterData filter)
        {
            try
            {
                var result = HistoryManager.GetHistoryCash(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return NotFound();
            }
        }

        [Route("budget_replenishment")]
        public IHttpActionResult PostBudgetReplenishment(decimal budget)
        {
            try
            {
                var result = HistoryManager.BudgetReplenishment(null, budget);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return NotFound();
            }
        }

        [Route("budget_replenishment_other_user/{code}")]
        public IHttpActionResult PostBudgetReplenishment(int code, decimal budget)
        {
            try
            {
                var result = HistoryManager.BudgetReplenishment(code, budget);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return NotFound();
            }
        }

        [Route("info")]
        public IHttpActionResult PostInformation()
        {
            try
            {
                var result = HistoryManager.GetHistoryInformation();// HistoryCashManager.BudgetReplenishment(code, budget);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return NotFound();
            }
        }

    }
}
