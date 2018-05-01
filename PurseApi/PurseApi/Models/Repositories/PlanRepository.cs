using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using PurseApi.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Repositories
{
    public class PlanRepository : GenericRepository<Plan>
    {
        private const string SQL_WHERE = " WHERE {0}";

        private int _actionCode;
        private int _code;
        private FilterData _filter;
        private List<int> _fields = new List<int>();

        public PlanRepository(bool empty = true, int actionCode = 0, int code = 0,  FilterData filter = null)
        {
            _actionCode = actionCode;
            _code = code;
            _filter = filter;
            if (!empty)
                SelectData();
        }
        
        public string GetFilter()
        {
            var sqlWhere = "WHERE"; 
            if(_filter != null)
            {
                if (_filter.DateInterval != null) {
                    var start = _filter.DateInterval[0];
                    var end = _filter.DateInterval[1];
                    sqlWhere = string.Format("{0} {1} <= [END_DATE_PLAN] AND {2} >= [START_DATE_PLAN]", sqlWhere, start, end);
                }
                if (_filter.Executor != null)
                {
                    if (sqlWhere != "WHERE")
                        sqlWhere += " AND ";
                    sqlWhere = string.Format("{0} [EXECUTOR_CODE] IN({1})", sqlWhere, string.Join(", ", _filter.Executor));
                }
                if (_filter.Owner != null)
                {
                    if (sqlWhere != "WHERE")
                        sqlWhere += " AND ";
                    sqlWhere = string.Format("{0} [OWNER_CODE] IN({1})", sqlWhere, string.Join(", ", _filter.Owner));
                }
                if (_filter.Category != null)
                {
                    if (sqlWhere != "WHERE")
                        sqlWhere += " AND ";
                    sqlWhere = string.Format("{0} [CATEGORY_CODE] IN({1})", sqlWhere, string.Join(", ", _filter.Category));
                }
                if (_filter.Status != null)
                {
                    if (sqlWhere != "WHERE")
                        sqlWhere += " AND ";
                    sqlWhere = string.Format("{0} [STATUS] IN({1})", sqlWhere, string.Join(", ", _filter.Status));
                }
            }
            return sqlWhere == "WHERE" ? string.Empty : sqlWhere;
        }

        public Plan UpdatePlan(Plan plan, List<int> fields)
        {
            if (_actionCode == (int)Constants.PlanAction.Code)
            {
                _code = plan.Code;
                _fields = fields;
                if (UpdateData(plan))
                    return plan;
            }
            return null;
        }

        public bool DeletePlan(int code)
        {
            if (_actionCode == (int)Constants.PlanAction.Code)
            {
                _code = code;
                return DeleteData();
            }
            return false;
        }

        protected override string TableWhere
        {
            get
            {
                string parametr = string.Empty;
                switch ((Constants.PlanAction)_actionCode)
                {
                    case Constants.PlanAction.Code:
                        parametr = string.Format("WHERE [CODE] = {0}", _code);
                        break;
                    case Constants.PlanAction.List:
                        parametr = GetFilter();
                        break;
                }
                return parametr;
            }
        }
  
        private readonly Dictionary<string, string> fieldSelect = new Dictionary<string, string>()
        {
            {"", "CODE"},
            {"Name", "NAME" },
            {"CreateDate", "DATE_CREATE" },
            {"LastUpdate", "LAST_UPDATE_PLAN" },
            {"OwnerCode", "OWNER_CODE" },
            {"ExecutorCode", "EXECUTOR_CODE" },
            {"StartDate", "START_DATE_PLAN" },
            {"EndDate", "END_DATE_PLAN" },
            {"PlannedBudget", "PLANNED_BUDGET_PLAN" },
            {"ActualBudget", "ACTUAL_BUDGET_PLAN" },
            {"Status", "STATUS" },
            {"CountFlight", "COUNT_FLIGHT"},
            {"CategoryCode", "CATEGORY_CODE"},
            {"ServiceCode", "SERVICE_CODE"}
        };

        private readonly Dictionary<string, string> fieldInsert = new Dictionary<string, string>()
        {
            {"Name", "NAME" },
            {"CreateDate", "DATE_CREATE" },
            {"LastUpdate", "LAST_UPDATE_PLAN" },
            {"OwnerCode", "OWNER_CODE" },
            {"ExecutorCode", "EXECUTOR_CODE" },
            {"StartDate", "START_DATE_PLAN" },
            {"EndDate", "END_DATE_PLAN" },
            {"PlannedBudget", "PLANNED_BUDGET_PLAN" },
            {"ActualBudget", "ACTUAL_BUDGET_PLAN" },
            {"Status", "STATUS" },
            {"CountFlight", "COUNT_FLIGHT"},
            {"CategoryCode", "CATEGORY_CODE"},
            {"ServiceCode", "SERVICE_CODE"}
        };

        private readonly Dictionary<string, string> fieldUpdate = new Dictionary<string, string>()
        {
            {"Name", "NAME" },
            {"CreateDate", "DATE_CREATE" },
            {"LastUpdate", "LAST_UPDATE_PLAN" },
            {"OwnerCode", "OWNER_CODE" },
            {"ExecutorCode", "EXECUTOR_CODE" },
            {"StartDate", "START_DATE_PLAN" },
            {"EndDate", "END_DATE_PLAN" },
            {"PlannedBudget", "PLANNED_BUDGET_PLAN" },
            {"ActualBudget", "ACTUAL_BUDGET_PLAN" },
            {"Status", "STATUS" },
            {"CountFlight", "COUNT_FLIGHT"},
            {"CategoryCode", "CATEGORY_CODE"},
            {"ServiceCode", "SERVICE_CODE"}
        };

        protected override string TableName
        {
            get
            {
                return "[PLAN]";
            }
        }

        protected override Dictionary<string, string> GetFieldsConformity(int action)
        {
            switch (action)
            {
                case (int)Action.Select:
                    return fieldSelect;
                case (int)Action.Insert:
                    return fieldInsert;
                case (int)Action.Update:
                    return fieldUpdate.Where(x => _fields.Any(y => ((Constants.PlanField)y).ToString() == x.Key)).ToDictionary(x => x.Key, x => x.Value);
                default:
                    return new Dictionary<string, string>();
            }
        }
    }
}