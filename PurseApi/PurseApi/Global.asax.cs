using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using PurseApi.Models.Repositories;
using PurseApi.Models.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace PurseApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ConnectionStringSettings css = ConfigurationManager.ConnectionStrings[Constants.MAIN_CONNECTION];
            Connection.SetConnectionString(Constants.MAIN_CONNECTION, css.ConnectionString);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
