using PurseApi.Models.Entities;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace PurseApi.Models
{
    public class UserSession
    {
        public const string SESSION_NAME = "_user_session_";
        
        public UserData User { get; set; }
        private DateTime CreatedDate;
        private static int _expiredHours = 12;

        public object this[string key]
        {
            get
            {
                return HttpContext.Current.Session[key];
            }
            set
            {
                HttpContext.Current.Session[key] = value;
            }
        }

        public static UserSession Current
        {
            [System.Diagnostics.DebuggerStepThrough]
            get
            {
                HttpContext ctx = HttpContext.Current;
                if (ctx != null && ctx.Session != null)
                    return (ctx.Session[SESSION_NAME] as UserSession);
                else
                    return null;
            }
        }

        public static UserSession Create(string userName = "", string password = "")
        {
            UserSession ret = null;
           
            if (userName != string.Empty && password != string.Empty)
            {
                var userRepo = new UserRepository((int)UserAction.Login);
                var user = userRepo.GetUser(userName, password);
                if (user != null)
                {
                    
                    userRepo.SetActionCode((int)UserAction.Id);

                    user.LastLogin = DateTime.Now;
                    var userLogin = userRepo.UpdateUserData(user, new List<int>() { (int)UserField.LastLogin });

                    ret = new UserSession() {
                        User = userLogin != null ? userLogin : user,
                        CreatedDate = DateTime.Now                       
                    };
                    HttpContext.Current.Session.Add(SESSION_NAME, ret);
                }
            }

            return ret;
        }

        public static UserSession UpdateSession(UserData user)
        {
            UserSession ret = null;
            if (Current != null)
            {
                ret = Current;
                ret.User = user;
                HttpContext.Current.Session[SESSION_NAME] = ret;
            }

            return ret;
        }

        public static bool DestroyIfExpired()
        {
            if (Current != null)
            {
                if (Current.CreatedDate.AddHours(_expiredHours) <= DateTime.UtcNow)
                {
                    HttpContext.Current.Session.Remove(SESSION_NAME);
                    return true;
                }
            }
            return false;
        }

    }
}