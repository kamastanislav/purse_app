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
    [RoutePrefix("user")]
    public class UserController : ApiController
    {
        [Route("list")]
        [ResponseType(typeof(List<UserData>))]
        public IHttpActionResult GetUsers()
        {
            return Ok(UserManager.GetUsers(null));
        }

        [Route("data/{id}")]
        public IHttpActionResult PostUser(int id)
        {
            var user = UserManager.GetUser(id);
      

            return Ok(user);
        }

        [Route("unique_field")]
        public IHttpActionResult PostUniqueField(int field, string value)
        {
            var result = UserManager.IsUnique(field, value);
            return Ok(result);
        }
        
        [Route("login")]
        [ResponseType(typeof(UserData))]
        public IHttpActionResult PostLogin(string login, string password)
        {
            var user = UserManager.LoginUser(login, password);

            return Ok(user);
        }

        [Route("registration")]
        public IHttpActionResult PostRegistration(UserData user, string password)
        {
            var newUser = UserManager.CreateNewUser(user, password);
            if (newUser != null)
            {
                return Ok(newUser);
            }
            else
            {
                return NotFound();
            }
        }

        [Route("logout/{id}")]
        public IHttpActionResult PostLogout(int id)
        {
            var result = UserManager.LogoutUser(id);
            return Ok(result);
        }

        [Route("update/{id}")]
        public IHttpActionResult PutUser(int id, UserData user)
        {
            try
            {
                var updateUser = UserManager.UpdateUserData(id, user);
                return Ok(updateUser);
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("update_password/{id}")]
        public IHttpActionResult PutUpdatePassword(int id, string password)
        {
            try
            {
                var updateUser = UserManager.UpdateUserData(id, password);
                return Ok(true);
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("check_password/{id}")]
        public IHttpActionResult PostCheckPassword(int id, string password)
        {
            try
            {
                bool isUserPassword = UserManager.CheckPassword(id, password);
                return Ok(isUserPassword);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
