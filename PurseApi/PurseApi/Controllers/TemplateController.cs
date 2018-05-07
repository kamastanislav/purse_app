using PurseApi.Models.Logger;
using PurseApi.Models.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PurseApi.Controllers
{
    [RoutePrefix("purse/template")]
    public class TemplateController : ApiController
    {
        [Route("list")]
        public IHttpActionResult PostListTeplates()
        {
            try
            {
                Logger.WriteInfo("list");
                var result = TemplateManager.GetList();
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("update/{code}")]
        public IHttpActionResult PostUpdateTemplate(int code, bool use, bool data)
        {
            try
            {
                var result = TemplateManager.UpdateTemplate(code, use, data);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("info/{code}")]
        public IHttpActionResult PostInfoTemplate(int code)
        {
            try
            {
                var result = TemplateManager.GetInfoTemplate(code);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("create_plan/{code}")]
        public IHttpActionResult PostCreatePlanUseTemplate(int code)
        {
            try
            {
                var result = TemplateManager.GetCreatePlanUseTemplate(code);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

    }

    [RoutePrefix("purse/template")]
    public class CopyOfTemplateController : ApiController
    {
        [Route("list")]
        public IHttpActionResult PostListTeplates()
        {
            try
            {
                var result = TemplateManager.GetList();
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("update/{code}")]
        public IHttpActionResult PostUpdateTemplate(int code, bool use, bool data)
        {
            try
            {
                var result = TemplateManager.UpdateTemplate(code, use, data);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("info/{code}")]
        public IHttpActionResult PostInfoTemplate(int code)
        {
            try
            {
                var result = TemplateManager.GetInfoTemplate(code);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("create_plan/{code}")]
        public IHttpActionResult PostCreatePlanUseTemplate(int code)
        {
            try
            {
                var result = TemplateManager.GetCreatePlanUseTemplate(code);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

    }
}