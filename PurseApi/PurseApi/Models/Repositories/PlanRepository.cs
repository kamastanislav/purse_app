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
        private List<int> _fields = new List<int>();

        public PlanRepository(bool empty = true, int actionCode = 0, int code = 0, bool isPlanned = true)
        {
            _actionCode = actionCode;
            _code = code;
            _isPlanned = isPlanned;
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
                        parametr = string.Format("[OWNER] = {0}", _code);
                        break;
                    case Constants.PlanAction.Executor:
                        parametr = string.Format("[EXECUTOR] = {0}", _code);
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
            {"LastUpdate", "LAST_UPDATE" },
            {"OwnerCode", "OWNER" },
            {"ExecutorCode", "EXECUTOR" },
            {"StartDate", "START_DATE" },
            {"EndDate", "END_DATE" },
            {"PlannedBudget", "PLANNED_BUDGET" },
            {"ActualBudget", "ACTUAL_BUDGET" },
            {"FamilyCode", "FAMILY_CODE" },
            {"Status", "STATUS" },
            {"CurrencyCode", "CURRCODE" },
            {"IsPrivate", "IS_PRIVATE" },
            {"CountFlight", "COUNT_FLIGHT"},
            {"CategoryCode", "CATEGORY_CODE"},
            {"ServiceCode", "SERVICE_CODE"}
        };     

        private readonly Dictionary<string, string> fieldInsert = new Dictionary<string, string>()
        {
            {"Name", "NAME" },
            {"LastUpdate", "LAST_UPDATE" },
            {"OwnerCode", "OWNER" },
            {"ExecutorCode", "EXECUTOR" },
            {"StartDate", "START_DATE" },
            {"EndDate", "END_DATE" },
            {"PlannedBudget", "PLANNED_BUDGET" },
            {"ActualBudget", "ACTUAL_BUDGET" },
            {"FamilyCode", "FAMILY_CODE" },
            {"Status", "STATUS" },
            {"CurrencyCode", "CURRCODE" },
            {"IsPrivate", "IS_PRIVATE" },
            {"CountFlight", "COUNT_FLIGHT"},
            {"CategoryCode", "CATEGORY_CODE"},
            {"ServiceCode", "SERVICE_CODE"}
        };

        private readonly Dictionary<string, string> fieldUpdate = new Dictionary<string, string>()
        {
            {"Name", "NAME" },
            {"LastUpdate", "LAST_UPDATE" },
            {"ExecutorCode", "EXECUTOR" },
            {"StartDate", "START_DATE" },
            {"EndDate", "END_DATE" },
            {"PlannedBudget", "PLANNED_BUDGET" },
            {"ActualBudget", "ACTUAL_BUDGET" },
            {"Status", "STATUS" },
            {"CurrencyCode", "CURRCODE" },
            {"IsPrivate", "IS_PRIVATE" },
            {"CountFlight", "COUNT_FLIGHT"},
            {"CategoryCode", "CATEGORY_CODE"},
            {"ServiceCode", "SERVICE_CODE"}
        };

        protected override string TableName
        {
            get
            {
                return "[USER]";
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