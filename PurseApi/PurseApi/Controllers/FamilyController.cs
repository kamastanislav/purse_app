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
        [Route("info")]
        public IHttpActionResult PostInfoFamily()
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                List<bool> info = new List<bool>() { user.FamilyCode != Constants.DEFAULT_CODE, user.IsAdmin };
                return Ok(info);
            }
            else
                return NotFound();
        }

        [Route("add_user")]
        public IHttpActionResult PostAddUserInFamily(int code)
        {
            try
            {
                return Ok(FamilyManager.AddUser(code));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Route("create_family")]
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

        [Route("search_users")]
        public IHttpActionResult PostSearchUser(string name)
        {
            try
            {
                var users = FamilyManager.GetSearchUsers(name);
                return Ok(users);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
