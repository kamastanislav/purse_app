using System;
using System.Collections.Generic;

namespace PurseApi.Models.Helper
{
    public class Constants
    {
        public static readonly string MAIN_CONNECTION = "PurseConnection";

        public static readonly int PERIOD_DELETED = 2;

        public static readonly int DEFAULT_CODE = 0;

        public enum StatusUser
        {
            NoneFamily = 0,
            Admin = 1,
            User = 2
        }

        public enum WorkflowStatus
        {
            InPlanned = 1,
            Approved = 2,
            Deleted = 100
        }
        
        public enum FlightField
        {
            PlannedBudge = 1,
            ActualBudget = 2,
            CurrencyCode = 3,
            Comment = 4,
            Status = 5,
            DateCreate = 6
        }

        public enum FlightAction
        {
            Code = 1,
            Plan = 2
        }

        public enum PlanAction
        {
            Code = 1,
            Executor = 2,
            Owner = 3,
            Family = 4
        }

        public enum PlanField
        {
            LastUpdate = 1,
            ExecutorCode = 2,
            StartDate = 3,
            EndDate = 4,
            PlannedBudget = 5,
            ActualBudget = 6,
            Status = 7,
            CurrencyCode = 8,
            IsPrivate = 9,
            CountFlight = 10,
            CategoryCode = 11,
            ServiceCode = 12
        }

        public enum UserField
        {
            FirstName = 1,
            LastName = 2,
            NickName = 3,
            Email = 4,
            Phone = 5,
            Cash = 6,
            LastLogin = 7,
            Birthday = 8,
            FamilyCode = 9,
            StatusCode = 10,
            Password = 11
        }

        public enum UserAction
        {
            Empty = 0,
            Code = 1,
            Login = 2,
            List = 3,
            Family = 4,
            Nick = 5
        }

        public enum HistoryCashAction
        {
            Family = 1,
            UserCode = 2
        }
    }
}