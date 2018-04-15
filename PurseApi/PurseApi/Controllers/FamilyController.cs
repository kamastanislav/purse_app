using PurseApi.Models;
using PurseApi.Models.Helper;
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
    [RoutePrefix("purse/family")]
    public class FamilyController : ApiController
    {
        [Route("having_family")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult PostHavingFamily()
        {
            var user = UserSession.Current.User;
            if (user != null)
                return Ok(user.FamilyCode != Constants.DEFAULT_CODE);
            else
                return NotFound();
        }

        [Route("create_famly")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult PostCreateFamily()
        {
            var user = UserSession.Current.User;
            if (user != null)
                return Ok(user.FamilyCode != Constants.DEFAULT_CODE);
            else
                return NotFound();
        }
    }
}
