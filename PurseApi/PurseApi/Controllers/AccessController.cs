using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PurseApi.Controllers
{
    [RoutePrefix("api/access")]
    public class AccessController : ApiController
    {
        public string Post()
        {
            return "test";
        }
    }
}
