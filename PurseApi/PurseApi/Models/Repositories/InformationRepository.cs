using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Repositories
{
    public class InformationRepository : GenericRepository<Information>
    {
        private int _code;
        private int _action;
        private bool _withPlan;

        public InformationRepository(bool withPlan = false)
        {
            _withPlan = withPlan;
        }

        public InformationRepository(int code, int action, bool empty = false) : base(empty)
        {
            _code = code;
            _action = action;
        }

        protected override string TableWhere
        {
            get
            {
                switch (_action)
                {
                    case (int)Constants.InformationAction.Select:
                        return string.Format("[CODE] = {0} AND [DATE_ACTION] >= {1} ORDER BY [DATE_ACTION] DESC", _code, Constants.TotalMillisecondsTwoWeeksOld);
                    case (int)Constants.InformationAction.Delete:
                        return string.Format("[DATE_ACTION] < {0}", Constants.TotalMillisecondsTwoWeeksOld);
                }
                return string.Empty;
            }
        }

        public void DeleteInformationData()
        {
            DeleteData();
        }

        private Dictionary<string, string> fieldInsert = new Dictionary<string, string>()
        {
            {"UserData", "USER_CODE" },
            {"Info", "INFO" },
            {"DateAction", "DATE_ACTION" }
        };

        private Dictionary<string, string> fieldInsertPlan = new Dictionary<string, string>()
        {
            {"UserData", "USER_CODE" },
            {"Info", "INFO" },
            {"PlanCode", "PLAN_CODE" },
            {"DateAction", "DATE_ACTION" }
        };

        private Dictionary<string, string> fieldSelect = new Dictionary<string, string>()
        {
            {"", "CODE"},
            {"UserData", "USER_CODE" },
            {"Info", "INFO" },
            {"PlanCode", "PLAN_CODE" },
            {"DateAction", "DATE_ACTION" }
        };

        protected override string TableName
        {
            get
            {
               return "USER_INFORMATION";
            }
        }

        protected override Dictionary<string, string> GetFieldsConformity(int action)
        {
            switch (action)
            {
                case (int)Action.Select:
                    return fieldSelect;
                case (int)Action.Insert:
                    return _withPlan ? fieldInsertPlan : fieldInsert;
                default:
                    return new Dictionary<string, string>();
            }
        }
    }
}