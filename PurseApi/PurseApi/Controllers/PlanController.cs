using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using PurseApi.Models.Logger;
using PurseApi.Models.Managers;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace PurseApi.Controllers
{
    [RoutePrefix("purse/plan")]
    public class PlanController : ApiController
    {
        [Route("family/{code}")]
        public IHttpActionResult GetAllPlanFamily(int code)
        {
            try
            {
                var result = PlanManager.GetPlans(code, (int)Constants.PlanAction.Family);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("deleted_plan/{code}")]
        public IHttpActionResult GetDeletedPlan(int code)
        {
            try
            {
                var result = PlanManager.GetDeletedPlans(code, (int)Constants.PlanAction.Owner);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("owner/{code}")]
        public IHttpActionResult GetAllPlanOwner(int code)
        {
            try
            {
                var result = PlanManager.GetPlans(code, (int)Constants.PlanAction.Owner);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("executor/{code}")]
        public IHttpActionResult GetAllPlanExecutor(int code)
        {
            try
            {
                var result = PlanManager.GetPlans(code, (int)Constants.PlanAction.Executor);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("data/{code}")]
        public IHttpActionResult GetPlan(int code)
        {
            try
            {
                var result = PlanManager.GetPlans(code, (int)Constants.PlanAction.Code);
                if (result.Any())
                    return Ok(result.FirstOrDefault());
                throw new Exception();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("approve/{code}")]
        public IHttpActionResult GetApprovePlan(int code)
        {
            try
            {
                var result = PlanManager.ApprovePlan(code);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("create")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult PostCreatePlan(Plan plan)
        {
            try
            {
                var result = PlanManager.CreatePlan(plan);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("delete/{code}")]
        public IHttpActionResult PutDeletePlan(int code)
        {
            try
            {
                var result = PlanManager.DeletePlan(code);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("update/{code}")]
        public IHttpActionResult PutUpdatePlan(Plan plan)
        {
            try
            {
                var result = PlanManager.UpdatePlan(plan);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("filter_data")]
        public IHttpActionResult PostSendFilterPlan()
        {
            try
            {
                return Ok(FilterManager.GetFilterPlan());
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
