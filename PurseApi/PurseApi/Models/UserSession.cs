using PurseApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models
{
    public class UserSession
    {
        public const string SESSION_NAME = "_user_session_";
        
        public UserData User { get; set; }
        public DateTime LastRequestTime { get; set; }
        private DateTime _createdDate;
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

        public static UserSession Create(bool withException, string userName = "", string password = "", string email = "", int? userCode = null)
        {
            UserSession ret = null;
            //@TODO
            return ret;
        }

        public static bool DestroyIfExpired()
        {
            if (Current != null)
            {
                if (Current._createdDate.AddHours(_expiredHours) <= DateTime.UtcNow)
                {
                    HttpContext.Current.Session.Remove(SESSION_NAME);
                    return true;
                }
            }
            return false;
        }

    }
}