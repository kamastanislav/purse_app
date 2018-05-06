using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Repositories
{
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
            if (_actionCode == (int)Constants.FlightAction.Code)
            {
                _code = flight.Code;
                _fields = fields;
                if (UpdateData(flight))
                    return flight;
            }
            return null;
        }

        public bool DeleteFlight()
        {
            if (_actionCode == (int)Constants.FlightAction.Code)
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
       //     { "CurrencyCode", "CURRCODE" },
            { "Comment", "COMMENT" },
            { "OwnerCode", "OWNER" },
            { "Status", "STATUS" }, 
            { "DateCreate", "DATE_CREATE" }
        };

        protected override string TableName
        {
            get
            {
                return "[FLIGHT]";
            }
        }

        private readonly Dictionary<string, string> fieldInsertUpdate = new Dictionary<string, string>()
        {
            { "PlanCode", "PLAN_CODE" },
            { "PlannedBudget", "PLANNED_BUDGET" },
            { "ActualBudget", "ACTUAL_BUDGET" },
            { "Comment", "COMMENT" },
            { "OwnerCode", "OWNER" },
            { "Status", "STATUS" },
            { "DateCreate", "DATE_CREATE" }
        };

        protected override Dictionary<string, string> GetFieldsConformity(int action)
        {
            switch (action)
            {
                case (int)Action.Select:
                    return fieldSelect;
                case (int)Action.Insert:
                    return fieldInsertUpdate;
                case (int)Action.Update:
                    return fieldInsertUpdate.Where(x => _fields.Any(y => ((Constants.FlightField)y).ToString() == x.Key)).ToDictionary(x => x.Key, x => x.Value);
                default:
                    return new Dictionary<string, string>();
            }
        }

        protected override string TableWhere
        {
            get
            {
                switch ((Constants.FlightAction)_actionCode)
                {
                    case Constants.FlightAction.Code:
                        return string.Format("WHERE [CODE] = {0}", _code);
                    case Constants.FlightAction.Plan:
                        return string.Format("WHERE [PLAN_CODE] = {0}", _code);
                   
                }
                return string.Empty;
            }
        }
    }
}