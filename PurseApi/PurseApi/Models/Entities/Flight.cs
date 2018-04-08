﻿using Newtonsoft.Json;
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
        public decimal PlannedBudget { get; set; }
        public decimal ActualBudget { get; set; }
        public int CurrencyCode { get; set; }
        public string Comment { get; set; } 
        public int OwnerCode { get; set; }
        public int Status { get; set; }
        public long DateCreate { get; set; }
        [JsonIgnore]
        public UserData Owner { get; }

        public Flight(int code)
        {
            _code = code;
        }

        public Flight(int code, Flight flight)
        {
            _code = code;
            this.PlanCode = flight.PlanCode;
            this.PlannedBudget = flight.PlannedBudget;
            this.ActualBudget = flight.ActualBudget;
            this.CurrencyCode = flight.CurrencyCode;
            this.Comment = flight.Comment;
            this.OwnerCode = flight.OwnerCode;
            this.Status = flight.Status;
        }
    }
}