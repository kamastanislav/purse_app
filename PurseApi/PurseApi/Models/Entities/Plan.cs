using PurseApi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Entities
{
    public enum StatusPlan
    {
        InPlanned = 1,
        Actually = 2,
        Executed = 3
    }

    public class Plan : IEntity
    {
        private readonly int _code;

        public int Code
        {
            get
            {
                return _code;
            }
        }

        public DateTime CreateDate { get; set; }
        public int OwnerCode { get; set; } 
        public int ExecutorCode { get; set; }
        public UserData Owner { get; }
        public UserData Executor { get; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double PlannedBudget { get; set; }
        public double ActualBudget { get; set; }
        public int FamilyCode { get; set; }
        public int Status { get; set; }
        public int CurrencyCode { get; set; }
        public bool IsPrivate { get; set; }

        public Currency CurrencyPlan
        {
            get;
        }

        public Plan(int code)
        {
            _code = code;
        }
    }
}