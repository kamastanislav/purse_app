using PurseApi.Models;
using PurseApi.Models.Entities;
using PurseApi.Models.Helpers;
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
    [RoutePrefix("purse/user")]
    public class UserController : ApiController
    {
        [Route("data/{code}")]
        [ResponseType(typeof(UserData))]
        public IHttpActionResult PostUser(int code)
        {
            var user = UserManager.GetUser(code);
      

            return Ok(user);
        }

        [Route("unique_field")]
        public IHttpActionResult PostUniqueField(int field, string value)
        {
            var result = UserManager.IsUnique(field, value);
            return Ok(result);
        }
        
        [Route("login")]
        [ResponseType(typeof(int))]
        public IHttpActionResult PostLogin(UserLogin userLogin)
        {
            var code = UserManager.LoginUser(userLogin);

            return Ok(code);
        }

        [Route("registration")]
        public IHttpActionResult PostRegistration(UserData user)
        {
            var code = UserManager.CreateNewUser(user);
            return Ok(code);
        }

        [Route("logout/{code}")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult PostLogout(int code)
        {
            var result = UserManager.LogoutUser(code);
            return Ok(result);
        }

        [Route("update/{code}")]
        public IHttpActionResult PutUser(int code, UserData user)
        {
            try
            {
                var updateUser = UserManager.UpdateUserData(code, user);
                return Ok(updateUser);
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("update_password/{code}")]
        public IHttpActionResult PutUpdatePassword(int code, string password)
        {
            try
            {
                var updateUser = UserManager.UpdateUserData(code, password);
                return Ok(true);
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("check_password/{code}")]
        public IHttpActionResult PostCheckPassword(int code, string password)
        {
            try
            {
                bool isUserPassword = UserManager.CheckPassword(code, password);
                return Ok(isUserPassword);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
