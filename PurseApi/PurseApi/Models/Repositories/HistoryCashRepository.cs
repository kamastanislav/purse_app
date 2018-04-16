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
        private int _actionInset = (int)Constants.HistoryCashInsertAction.Empty;

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
                        return string.Format("WHERE [FAMILY_CODE] = {0} ORDER BY [DATE_ACTION] DESC", _code);
                    case Constants.HistoryCashAction.UserCode:
                        return string.Format("WHERE [USER_CODE] = {0} ORDER BY [DATE_ACTION] DESC", _code);
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
            {"Name", "NAME" },
            {"PlanCode", "PLAN_CODE" },
            {"FamilyCode", "FAMILY_CODE" }
        };

        private readonly Dictionary<string, string> fieldInsertAll = new Dictionary<string, string>()
        {
            {"UserCode", "USER_CODE" },
            {"DateAction", "DATE_ACTION" },
            {"Cash", "CASH" },
            {"CategoryCode", "CATEGORY_CODE" },
            {"Name", "NAME" },
            {"PlanCode", "PLAN_CODE" },
            {"FamilyCode", "FAMILY_CODE" }
        };

        private readonly Dictionary<string, string> fieldInsertPlan = new Dictionary<string, string>()
        {
            {"UserCode", "USER_CODE" },
            {"DateAction", "DATE_ACTION" },
            {"Cash", "CASH" },
            {"CategoryCode", "CATEGORY_CODE" },
            {"Name", "NAME" },
            {"PlanCode", "PLAN_CODE" }
        };

        private readonly Dictionary<string, string> fieldInserFamily = new Dictionary<string, string>()
        {
            {"UserCode", "USER_CODE" },
            {"DateAction", "DATE_ACTION" },
            {"Cash", "CASH" },
            {"Name", "NAME" },
            {"FamilyCode", "FAMILY_CODE" }
        };

        private readonly Dictionary<string, string> fieldInserEmpty = new Dictionary<string, string>()
        {
            {"UserCode", "USER_CODE" },
            {"DateAction", "DATE_ACTION" },
            {"Cash", "CASH" },
            {"Name", "NAME" }
        };


        protected override string TableName
        {
            get
            {
                return "[USER_HISTORY_CASH]";
            }
        }

        public void SetActionInsert(int actinInsert)
        {
            _actionInset = actinInsert;
        }

        protected override Dictionary<string, string> GetFieldsConformity(int action)
        {
            switch (action)
            {
                case (int)Action.Select:
                    return fieldSelect;
                case (int)Action.Insert:
                    switch (_actionInset)
                    {
                        case (int)Constants.HistoryCashInsertAction.All:
                            return fieldInsertAll;
                        case (int)Constants.HistoryCashInsertAction.Plan:
                            return fieldInsertPlan;
                        case (int)Constants.HistoryCashInsertAction.Empty:
                            return fieldInserEmpty;
                        case (int)Constants.HistoryCashInsertAction.Family:
                            return fieldInserFamily;
                    }
                    break;
            }
            return new Dictionary<string, string>();
        }
    }
}