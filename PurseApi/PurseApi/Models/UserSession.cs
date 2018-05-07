using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
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
            get
            {
                HttpContext ctx = HttpContext.Current;
                if (ctx != null && ctx.Session != null)
                    return (ctx.Session[SESSION_NAME] as UserSession);
                else
                    return null;
            }
        }

        public static UserSession Create(string userName = "", string password = "", UserData userRegistration = null)
        {
            UserSession ret = null;
           
            if (userName != string.Empty && password != string.Empty)
            {
                var userRepo = new UserRepository((int)Constants.UserAction.Login);
                var user = userRepo.GetUser(userName, password);
                if (user != null)
                {
                    
                    userRepo.SetActionCode((int)Constants.UserAction.Code);

                    user.LastLogin = Constants.TotalMilliseconds;
                    var userLogin = userRepo.UpdateUserData(user.Code, user, new List<int>() { (int)Constants.UserField.LastLogin });

                    ret = new UserSession() {
                        User = userLogin != null ? userLogin : user,
                        CreatedDate = DateTime.Now                       
                    };
                    Logger.Logger.WriteInfo(user.NickName);
                    HttpContext.Current.Session.Add(SESSION_NAME, ret);
                }
            }
            if (userRegistration != null)
            {
                userRegistration.CreateDate = Constants.TotalMilliseconds;
                userRegistration.LastLogin = Constants.TotalMilliseconds;
                UserRepository userRepo = new UserRepository();
                var code = userRepo.InsertData(userRegistration);
                if (code > Constants.DEFAULT_CODE)
                {
                    ret = new UserSession()
                    {
                        User = userRegistration,
                        CreatedDate = DateTime.Now
                    };
                    HttpContext.Current.Session.Add(SESSION_NAME, ret);
                }
            }

            return ret;
        }

        public static UserSession UpdateSession()
        {
            UserSession ret = null;
            if (Current != null)
            {
                ret = Current;
                var userRepo = new UserRepository((int)Constants.UserAction.Code);
                ret.User = userRepo.GetUser(ret.User.Code);
                HttpContext.Current.Session[SESSION_NAME] = ret;
            }

            return ret;
        }

        public static bool DestroyIfExpired()
        {
            if (Current != null)
            {
                HttpContext.Current.Session.Remove(SESSION_NAME);
                return true;
            }
            return false;
        }

    }
}