using PurseApi.Models.Entities;
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
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [Route("list")]
        public IEnumerable<UserData> GetUsers()
        {
            return UserManager.GetUsers();
        }

        [Route("{id}")]
        [ResponseType(typeof(UserData))]
        public IHttpActionResult GetUser(int id)
        {
            var user = UserManager.GetUsers().FirstOrDefault(x => x.Code == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Route("update")]
        [ResponseType(typeof(UserData))]
        public UserData PutUser(UserData user)
        {
            
            return user;
        }
    }
}
