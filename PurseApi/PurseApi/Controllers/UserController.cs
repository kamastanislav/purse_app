using PurseApi.Models;
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
        [ResponseType(typeof(List<UserData>))]
        public IHttpActionResult GetUsers()
        {
            return Ok(UserManager.GetUsers(null));
        }

        [Route("{id}")]
        [ResponseType(typeof(UserData))]
        public IHttpActionResult GetUser(int id)
        {
            var user = UserManager.GetUser(id);
            if (user == null)
            {
                return Ok(user);
            }

            return Ok(user);
        }

        [Route("unique")]
        public IHttpActionResult PostUnique(int field, string value)
        {
            var result = UserManager.IsUnique(field, value);
            return Ok(result);
        }
        
        [Route("login")]
        public IHttpActionResult PostLogin(string login, string password)
        {
            var user = UserManager.LoginUser(login, password);

            return Ok(true);
        }

        [Route("logout")]
        public IHttpActionResult PostLogout()
        {

            return Ok(true);
        }

        [Route("update")]
        [ResponseType(typeof(UserData))]
        public UserData PutUser(UserData user)
        {
            
            return user;
        }
    }
}
