using Newtonsoft.Json;
using PurseApi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Entities
{
    public class TemplatePlan : IEntity
    {
        private readonly int _code;

        public int Code
        {
            get
            {
                return _code;
            }
        }

        public int PlanCode { get; set; }
        public bool IsUpdate { get; set; }
        public string AllPlan { get; set; }
        public int UserCode { get; set; }

        [JsonIgnore]
        public List<int> GetPlanCode
        {
            get
            {
                var codes = AllPlan.Split(',').Select(x => Int32.Parse(x)).ToList();
                return codes;
            }
        }

        public void SetPlanCode(int code)
        {
            var codes = GetPlanCode;
            codes.Add(code);
            AllPlan = string.Format(",", codes);
        }

        public TemplatePlan()
        {

        }

        public TemplatePlan(int code)
        {
            _code = code;


        }
    }
}