using PurseApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Repositories
{
    public enum FlightAction
    {
        One = 1,
        Plan = 2
    }

    public class FlightRepository : GenericRepository<UserData>
    {
        private int _code;
        private int _actionCode = 0;

        private readonly Dictionary<string, string> field = new Dictionary<string, string>()
        {
            { "", "CODE" },
            { "PlanCode", "PLAN_CODE" },
            { "PlannedBudget", "PLANNED_BUDGET" },
            { "ActualBudget", "ACTUAL_BUDGET" },
            { "CurrencyCode", "CURRCODE" },
            { "Comment", "COMMENT" },
            { "OwnerCode", "OWNER" }
        };

        protected override string TableName
        {
            get
            {
                return "[FLIGHT]";
            }
        }

        protected override Dictionary<string, string> GetFieldsConformity()
        {
            return field;
        }

        protected override string TableWhere
        {
            get
            {
                switch ((FlightAction)_actionCode)
                {
                    case FlightAction.One:
                        return string.Format("WHERE [CODE] = {0}", _code);
                    case FlightAction.Plan:
                        return string.Format("WHERE [PLAN_CODE] = {0}", _code);
                   
                }
                return string.Empty;
            }
        }
    }
}