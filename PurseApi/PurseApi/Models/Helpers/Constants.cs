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
            Name = 1,
            LastUpdate = 2,
            ExecutorCode = 3,
            StartDate = 4,
            EndDate = 5,
            PlannedBudget = 6,
            ActualBudget = 7,
            Status = 8,
            CurrencyCode = 9,
            IsPrivate = 10,
            CountFlight = 11,
            CategoryCode = 12,
            ServiceCode = 13
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

        public enum HistoryCashInsertAction
        {
            All = 1,
            Empty = 2,
            Plan = 3,
            Family = 4
        }

        public enum InformationAction
        {
            Select = 1,
            Delete = 2
        }

        public static long TotalMilliseconds
        {
            get
            {
                return (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
            }
        }

        public static long TotalMillisecondsTwoWeeksOld
        {
            get
            {
                return (long)(DateTime.Now.AddDays(-14) - new DateTime(1970, 1, 1)).TotalMilliseconds;
            }
        }

        public static readonly string BudgetReplenishment = "Пополнение бюджета {0}";
        public static readonly string Transfer = "Перевод от {0} к {1}";
        public static readonly string Plan = "План {0}";
        public static readonly string CreatePlanOwner = "Вы создатель плана {0}";
        public static readonly string CreatePlanExecutor = "Вы исполнитель план {0}";
        public static readonly string DeletedPlanExecutor = "План {0} удален";
        public static readonly string ExecutedPlan = "План {0} выполнен";
    }
}