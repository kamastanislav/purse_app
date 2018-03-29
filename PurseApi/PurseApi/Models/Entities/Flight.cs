using Newtonsoft.Json;
using PurseApi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Entities
{
    public class Flight : IEntity
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
        public double PlannedBudget { get; set; }
        public double ActualBudget { get; set; }
        public int CurrencyCode { get; set; }
        public string Comment { get; set; }
        [JsonIgnore]
        public int OwnerCode { get; set; }
        public int Status { get; set; }
        public UserData Owner { get; }

        public Flight(int code)
        {
            _code = code;
        }
    }
}
