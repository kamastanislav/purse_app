using PurseApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Repositories
{
    public enum PlanAction
    {
        Id = 1,
        Executor = 2,
        Owner = 3,
        Family = 4 
    }
    public enum PlanField
    {
        OwnerCode = 1,
        ExecutorCode = 2,
        StartDate = 3,
        EndDate = 4,
        PlannedBudget = 5,
        ActualBudget = 6,
        Status = 7,
        CurrencyCode = 8,
        IsPrivate = 9
    }
    public class PlanRepository : GenericRepository<Plan>
    {
        private const string SQL_WHERE = " WHERE {0}";
        private int _actionCode;
        private int _code;
        private List<int> _fields = new List<int>();

        public PlanRepository(bool empty = true, int actionCode = 0, int code = 0)
        {
            _actionCode = actionCode;
            _code = code;
            if (!empty)
                SelectData();
        }
        

        public Plan UpdatePlan(Plan plan, List<int> fields)
        {
            if (_actionCode == (int)PlanAction.Id)
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
            if (_actionCode == (int)PlanAction.Id)
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
                switch ((PlanAction)_actionCode)
                {
                    case PlanAction.Family:
                        parametr = string.Format("[FAMILY_CODE] = {0}", _code);
                        break;
                    case PlanAction.Id:
                        parametr = string.Format("[CODE] = {0}", _code);
                        break;
                    case PlanAction.Owner:
                        parametr = string.Format("[OWNER] = {0}", _code);
                        break;
                    case PlanAction.Executor:
                        parametr = string.Format("[EXECUTOR] = {0}", _code);
                        break;
                }
                return parametr != string.Empty ? string.Format(SQL_WHERE, parametr) : string.Empty;
            }
        }
  
        private readonly Dictionary<string, string> fieldSelect = new Dictionary<string, string>()
        {
            {"", "CODE"},
            {"OwnerCode", "OWNER" },
            {"ExecutorCode", "EXECUTOR" },
            {"StartDate", "START_DATE" },
            {"EndDate", "END_DATE" },
            {"PlannedBudget", "PLANNED_BUDGET" },
            {"ActualBudget", "ACTUAL_BUDGET" },
            {"FamilyCode", "FAMILY_CODE" },
            {"Status", "STATUS" },
            {"CurrencyCode", "CURRCODE" },
            {"IsPrivate", "IS_PRIVATE" }
        };

        private readonly Dictionary<string, string> fieldInsert = new Dictionary<string, string>()
        {
            {"OwnerCode", "OWNER" },
            {"ExecutorCode", "EXECUTOR" },
            {"StartDate", "START_DATE" },
            {"EndDate", "END_DATE" },
            {"PlannedBudget", "PLANNED_BUDGET" },
            {"ActualBudget", "ACTUAL_BUDGET" },
            {"FamilyCode", "FAMILY_CODE" },
            {"Status", "STATUS" },
            {"CurrencyCode", "CURRCODE" },
            {"IsPrivate", "IS_PRIVATE" }
        };

        private readonly Dictionary<string, string> fieldUpdate = new Dictionary<string, string>()
        {
            {"OwnerCode", "OWNER" },
            {"ExecutorCode", "EXECUTOR" },
            {"StartDate", "START_DATE" },
            {"EndDate", "END_DATE" },
            {"PlannedBudget", "PLANNED_BUDGET" },
            {"ActualBudget", "ACTUAL_BUDGET" },
            {"Status", "STATUS" },
            {"CurrencyCode", "CURRCODE" },
            {"IsPrivate", "IS_PRIVATE" }
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
                    return fieldUpdate.Where(x => _fields.Any(y => ((PlanField)y).ToString() == x.Key)).ToDictionary(x => x.Key, x => x.Value);
                default:
                    return new Dictionary<string, string>();
            }
        }
    }
}