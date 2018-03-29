using PurseApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Repositories
{
    public enum FlightField
    {
        PlannedBudge = 1,
        ActualBudget = 2,
        CurrencyCode = 3,
        Comment = 4,
        Status = 5
    }

    public enum FlightAction
    {
        Id = 1,
        Plan = 2
    }

    public class FlightRepository : GenericRepository<Flight>
    {
        private int _code;
        private int _actionCode = 0;
        private List<int> _fields = new List<int>();

        public FlightRepository(bool empty = true, int actionCode = 0, int code = 0)
        {
            _actionCode = actionCode;
            _code = code;
            if (!empty)
                SelectData();
        }

        public Flight UpdateFlight(Flight flight, List<int> fields)
        {
            if (_actionCode == (int)FlightAction.Id)
            {
                _fields = fields;
                if (UpdateData(flight))
                    return flight;
            }
            return null;
        }

        public bool DeleteFlight()
        {
            if (_actionCode == (int)FlightAction.Id)
            {
                return DeleteData();
            }
            return false;
        }

        private readonly Dictionary<string, string> fieldSelect = new Dictionary<string, string>()
        {
            { "", "CODE" },
            { "PlanCode", "PLAN_CODE" },
            { "PlannedBudget", "PLANNED_BUDGET" },
            { "ActualBudget", "ACTUAL_BUDGET" },
            { "CurrencyCode", "CURRCODE" },
            { "Comment", "COMMENT" },
            { "OwnerCode", "OWNER" },
            { "Status", "STATUS" }
        };

        protected override string TableName
        {
            get
            {
                return "[FLIGHT]";
            }
        }

        private readonly Dictionary<string, string> fieldInsert = new Dictionary<string, string>()
        {
            { "PlanCode", "PLAN_CODE" },
            { "PlannedBudget", "PLANNED_BUDGET" },
            { "ActualBudget", "ACTUAL_BUDGET" },
            { "CurrencyCode", "CURRCODE" },
            { "Comment", "COMMENT" },
            { "OwnerCode", "OWNER" },
            { "Status", "STATUS" }
        };

        private readonly Dictionary<string, string> fieldUpdate = new Dictionary<string, string>()
        {
            { "PlannedBudget", "PLANNED_BUDGET" },
            { "ActualBudget", "ACTUAL_BUDGET" },
            { "CurrencyCode", "CURRCODE" },
            { "Comment", "COMMENT" },
            { "Status", "STATUS" }
        };

        protected override Dictionary<string, string> GetFieldsConformity(int action)
        {
            switch (action)
            {
                case (int)Action.Select:
                    return fieldSelect;
                case (int)Action.Insert:
                    return fieldInsert;
                case (int)Action.Update:
                    return fieldUpdate.Where(x => _fields.Any(y => ((FlightField)y).ToString() == x.Key)).ToDictionary(x => x.Key, x => x.Value);
                default:
                    return new Dictionary<string, string>();
            }
        }

        protected override string TableWhere
        {
            get
            {
                switch ((FlightAction)_actionCode)
                {
                    case FlightAction.Id:
                        return string.Format("WHERE [CODE] = {0}", _code);
                    case FlightAction.Plan:
                        return string.Format("WHERE [PLAN_CODE] = {0}", _code);
                   
                }
                return string.Empty;
            }
        }
    }
}