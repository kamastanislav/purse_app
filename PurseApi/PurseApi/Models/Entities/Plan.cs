using Newtonsoft.Json;
using PurseApi.Models.Entity;
using PurseApi.Models.Helper;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Entities
{
    public class Plan : IEntity
    {
        private readonly int _code;
        private List<Flight> _flights;

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
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public decimal PlannedBudget { get; set; }
        public decimal ActualBudget { get; set; }
        public int Status { get; set; }
        public int CountFlight { get; set; }
        public int CategoryCode { get; set; }
        public int ServiceCode { get; set; }

        [JsonIgnore]
        public List<Flight> Flights
        {
            get
            {
                if (_flights == null)
                {
                    var repo = new FlightRepository(false, (int)Constants.FlightAction.Plan, _code);
                    _flights = repo.List;
                }
                return _flights;
            }
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
            this.Name = plan.Name;
            this.CreateDate = plan.CreateDate;
            this.LastUpdate = plan.LastUpdate;
            this.OwnerCode = plan.OwnerCode;
            this.ExecutorCode = plan.ExecutorCode;
            this.StartDate = plan.StartDate;
            this.EndDate = plan.EndDate;
            this.PlannedBudget = plan.PlannedBudget;
            this.ActualBudget = plan.ActualBudget;
            this.Status = plan.Status;
            this.CategoryCode = plan.CategoryCode;
            this.ServiceCode = plan.ServiceCode;
       /*     this.CurrencyCode = plan.CurrencyCode;*/
            this.CountFlight = plan.CountFlight;
        }
    }
}