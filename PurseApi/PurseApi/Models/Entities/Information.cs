using PurseApi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Entities
{
    public class Information : IEntity
    {
        private readonly int _code;

        public int Code
        {
            get
            {
                return _code;
            }
        }
        public int UserData { get; set; }
        public string Info { get; set; }
        public int PlanCode { get; set; }
        public long DateAction { get; set; } 

        public Information()
        {

        }

        public Information(int code)
        {
            _code = code;
        }
    }
}

/*[USER_CODE] INT NOT NULL,
	[INFO] VARCHAR(100) NOT NULL,
	[PLAN_CODE] INT NULL,
    [DATE_ACTION] BIGINT NOT NULL*/
