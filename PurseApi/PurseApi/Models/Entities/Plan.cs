using Newtonsoft.Json;
using PurseApi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Entities
{
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

        public string Name { get; set; }
        public long CreateDate { get; set; }
        public long LastUpdate { get; set; }
        public int OwnerCode { get; set; } 
        public int ExecutorCode { get; set; }

        [JsonIgnore]
        public UserData Owner { get; }
        [JsonIgnore]
        public UserData Executor { get; }

        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public decimal PlannedBudget { get; set; }
        public decimal ActualBudget { get; set; }
        public int FamilyCode { get; set; }
        public int Status { get; set; }
   /*     public int CurrencyCode { get; set; }*/
        public bool IsPrivate { get; set; }
        public int CountFlight { get; set; }
        public int CategoryCode { get; set; }
        public int ServiceCode { get; set; }

        public Currency CurrencyPlan
        {
            get;
        }

        public Plan()
        {

        }

        public Plan(int code)
        {
            _code = code;
        }

        public Plan(int code, Plan plan)
        {
            _code = code;
            this.CreateDate = plan.CreateDate;
            this.LastUpdate = plan.LastUpdate;
            this.OwnerCode = plan.OwnerCode;
            this.ExecutorCode = plan.ExecutorCode;
            this.StartDate = plan.StartDate;
            this.EndDate = plan.EndDate;
            this.PlannedBudget = plan.PlannedBudget;
            this.ActualBudget = plan.ActualBudget;
            this.FamilyCode = plan.FamilyCode;
            this.Status = plan.Status;
       /*     this.CurrencyCode = plan.CurrencyCode;*/
            this.IsPrivate = plan.IsPrivate;
            this.CountFlight = plan.CountFlight;
        }
    }
}