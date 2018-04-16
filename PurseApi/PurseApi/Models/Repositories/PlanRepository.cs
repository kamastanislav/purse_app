using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Repositories
{
    public class PlanRepository : GenericRepository<Plan>
    {
        private const string SQL_WHERE = " WHERE {0} AND STATUS {1} 100";

        private int _actionCode;
        private int _code;
        private bool _isPlanned;
        private bool _all;
        private List<int> _fields = new List<int>();

        public PlanRepository(bool empty = true, int actionCode = 0, int code = 0, bool isPlanned = true, bool all = true)
        {
            _actionCode = actionCode;
            _code = code;
            _isPlanned = isPlanned;
            _all = all;
            if (!empty)
                SelectData();
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
                    case Constants.PlanAction.Family:
                        parametr = string.Format("[FAMILY_CODE] = {0}", _code);
                        break;
                    case Constants.PlanAction.Code:
                        parametr = string.Format("[CODE] = {0}", _code);
                        break;
                    case Constants.PlanAction.Owner:
                        parametr = string.Format("[OWNER_CODE] = {0}", _code);
                        break;
                    case Constants.PlanAction.Executor:
                        parametr = string.Format("[EXECUTOR_CODE] = {0}", _code);
                        break;
                }
                return parametr != string.Empty ? string.Format(SQL_WHERE, parametr, _isPlanned ? "!=" : "=") : string.Empty;
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
            {"FamilyCode", "FAMILY_CODE" },
            {"Status", "STATUS" },
         //   {"CurrencyCode", "CURRCODE" },
            {"IsPrivate", "IS_PRIVATE" },
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
         //   {"CurrencyCode", "CURRCODE" },
            {"IsPrivate", "IS_PRIVATE" },
            {"CountFlight", "COUNT_FLIGHT"},
            {"CategoryCode", "CATEGORY_CODE"},
            {"ServiceCode", "SERVICE_CODE"}
        };

        private readonly Dictionary<string, string> fieldInsertAll = new Dictionary<string, string>()
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
            {"FamilyCode", "FAMILY_CODE" },
            {"Status", "STATUS" },
         //   {"CurrencyCode", "CURRCODE" },
            {"IsPrivate", "IS_PRIVATE" },
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
            {"FamilyCode", "FAMILY_CODE" },
            {"Status", "STATUS" },
         //   {"CurrencyCode", "CURRCODE" },
            {"IsPrivate", "IS_PRIVATE" },
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
                    return _all ? fieldInsertAll : fieldInsert;
                case (int)Action.Update:
                    return fieldUpdate.Where(x => _fields.Any(y => ((Constants.PlanField)y).ToString() == x.Key)).ToDictionary(x => x.Key, x => x.Value);
                default:
                    return new Dictionary<string, string>();
            }
        }
    }
}