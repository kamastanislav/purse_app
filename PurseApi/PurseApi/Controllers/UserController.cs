using PurseApi.Models;
using PurseApi.Models.Entities;
using PurseApi.Models.Helpers;
using PurseApi.Models.Logger;
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

        [Route("session")]
        [ResponseType(typeof(UserData))]
        public IHttpActionResult PostSessionUser()
        {
            var user = UserSession.Current.User;


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

        [Route("logout")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult PostLogout()
        {
            var result = UserManager.LogoutUser();
            return Ok(result);
        }

        [Route("update")]
        public IHttpActionResult PutUser(UserData user)
        {
            try
            {
                Logger.WriteInfo("PutUser");
                var updateUser = UserManager.UpdateUserData(user);
                return Ok(updateUser);
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("update_password")]
        public IHttpActionResult PutUpdatePassword(string password)
        {
            try
            {
                var updateUser = UserManager.UpdateUserData(password);
                return Ok(true);
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("check_password")]
        public IHttpActionResult PostCheckPassword(string password)
        {
            try
            {
                bool isUserPassword = UserManager.CheckPassword(password);
                return Ok(isUserPassword);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
