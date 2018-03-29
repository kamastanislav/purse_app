using PurseApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Repositories
{
    public enum HistoryCashAction
    {
        Id = 1,
        Family = 2,
        Category = 3
    }
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
                switch ((HistoryCashAction)_actionCode)
                {
                    case HistoryCashAction.Id:
                        return string.Format("WHERE [CODE] = {0}", _code);
                    case HistoryCashAction.Family:
                        return string.Format("WHERE [PLAN_CODE] = {0}", _code);
                    case HistoryCashAction.Category:
                        return string.Format("WHERE [PLAN_CODE] = {0}", _code);
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