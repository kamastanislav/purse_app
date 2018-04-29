using PurseApi.Models;
using PurseApi.Models.Entities;
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

        [Route("is_admin_family")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult PostIsAdminFamily()
        {
            var user = UserSession.Current.User;
            if (user != null)
                return Ok(user.IsAdmin);
            else
                return NotFound();
        }

        [Route("create_family")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult PostCreateFamily()
        {
            try
            {
                return Ok(FamilyManager.CreateFamily());
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("users")]
        [ResponseType(typeof(List<UserData>))]
        public IHttpActionResult PostUser()
        {
            try
            {
                var users = FamilyManager.GetUsersFamily();
                return Ok(users);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
