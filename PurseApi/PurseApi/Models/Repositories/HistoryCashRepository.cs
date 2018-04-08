using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Repositories
{
    public class HistoryCashRepository : GenericRepository<HistoryCash>
    {
        private int _actionCode;
        private int _code;

        public HistoryCashRepository()
        {
        }

        public HistoryCashRepository(int code, int action)
        {
            _code = code;
            _actionCode = action; 
            SelectData();
        }

        protected override string TableWhere
        {
            get
            {
                switch ((Constants.HistoryCashAction)_actionCode)
                {
                    case Constants.HistoryCashAction.Family:
                        return string.Format("WHERE [FAMILY_CODE] = {0}", _code);
                    case Constants.HistoryCashAction.UserCode:
                        return string.Format("WHERE [USER_CODE] = {0}", _code);
                }
                return string.Empty;
            }
        }
       

        private readonly Dictionary<string, string> fieldSelect = new Dictionary<string, string>()
        {
            {"", "CODE"},
            {"UserCode", "USER_CODE" },
            {"DateAction", "DATE_ACTION" },
            {"Cash", "CASH" },
            {"CategoryCode", "CATEGORY_CODE" },
            {"PlanCode", "PLAN_CODE" },
            {"FamilyCode", "FAMILY_CODE" }
        };

        private readonly Dictionary<string, string> fieldInsert = new Dictionary<string, string>()
        {
            {"UserCode", "USER_CODE" },
            {"DateAction", "DATE_ACTION" },
            {"Cash", "CASH" },
            {"CategoryCode", "CATEGORY_CODE" },
            {"PlanCode", "PLAN_CODE" },
            {"FamilyCode", "FAMILY_CODE" }
        };

        protected override string TableName
        {
            get
            {
                return "[USER_HISTORY_CASH]";
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
                default:
                    return new Dictionary<string, string>();
            }
        }
    }
}