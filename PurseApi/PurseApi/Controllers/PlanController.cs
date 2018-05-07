using PurseApi.Models.Entities;
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
using System.Web.Http.Description;

namespace PurseApi.Controllers
{
    [RoutePrefix("purse/plan")]
    public class PlanController : ApiController
    {
        [Route("list")]
        public IHttpActionResult PostListPlan(FilterData filter)
        {
            try
            {
                var result = PlanManager.GetPlans(filter);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("categories")]
        public IHttpActionResult PostCatigories()
        {
            try
            {
                var result = PlanManager.GetCategories();

                return Ok(result);

            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [Route("data/{code}")]
        public IHttpActionResult PostPlan(int code)
        {
            try
            {
                var result = PlanManager.GetPlan(code);

                return Ok(result);

            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("approve/{code}")]
        public IHttpActionResult PostApprovePlan(int code)
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
        public IHttpActionResult PostDeletePlan(int code)
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

        [Route("undelete/{code}")]
        public IHttpActionResult PostUndeletePlan(int code)
        {
            try
            {
                var result = PlanManager.UndeletePlan(code);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("update/{code}")]
        public IHttpActionResult PostUpdatePlan(int code, Plan plan)
        {
            try
            {
                var result = PlanManager.UpdatePlan(new Plan(code, plan));
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
