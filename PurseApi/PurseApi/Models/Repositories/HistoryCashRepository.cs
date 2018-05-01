using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using PurseApi.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Repositories
{
    public class HistoryCashRepository : GenericRepository<HistoryCash>
    {
        private FilterData _filter;
        private int _actionInset = (int)Constants.HistoryCashInsertAction.Empty;

        public HistoryCashRepository()
        {
        }

        public HistoryCashRepository(FilterData filter)
        {
            _filter = filter;
            SelectData();
        }

        protected override string TableWhere
        {
            get
            {
                return GetFilterForData();//string.Format("WHERE [USER_CODE] = {0} ORDER BY [DATE_ACTION] DESC", _code);
            }
        }

        private string GetFilterForData()
        {
            var sqlWhere = "WHERE ";
            if (_filter.DateInterval != null)
            {
                var start = _filter.DateInterval[0];
                var end = _filter.DateInterval[1];
                sqlWhere = string.Format("{0} {1} <= [DATE_ACTION] AND [DATE_ACTION] <= {2}", sqlWhere, start, end);
            }
            if (_filter.Owner != null)
            {
                if (sqlWhere != "WHERE")
                    sqlWhere += " AND ";
                sqlWhere = string.Format("{0} [USER_CODE] IN({1})", sqlWhere, string.Join(", ", _filter.Owner));
            }
            if (_filter.Category != null)
            {
                if (sqlWhere != "WHERE" && _filter.Category.Any())
                    sqlWhere += " AND ";
                if (_filter.Category.Any(x => x == 0) && _filter.Category.Count == 1)
                    sqlWhere = string.Format("{0} [CATEGORY_CODE] IS NULL", sqlWhere);
                else if (_filter.Category.Any() && _filter.Category.Count > 0 && _filter.Category.All(x => x != 0))
                    sqlWhere = string.Format("{0} [CATEGORY_CODE] IN({1})", sqlWhere, string.Join(", ", _filter.Category));
                else
                {
                    sqlWhere = string.Format("{0} ([CATEGORY_CODE] IN({1})", sqlWhere, string.Join(", ", _filter.Category.Where(x => x != 0)));
                    sqlWhere = string.Format("{0} OR [CATEGORY_CODE] IS NULL)", sqlWhere);
                }
            }
            return sqlWhere;
        }

        private readonly Dictionary<string, string> fieldSelect = new Dictionary<string, string>()
        {
            {"", "CODE"},
            {"UserCode", "USER_CODE" },
            {"DateAction", "DATE_ACTION" },
            {"Cash", "CASH" },
            {"CategoryCode", "CATEGORY_CODE" },
            {"Name", "NAME" },
            {"PlanCode", "PLAN_CODE" }
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
                        case (int)Constants.HistoryCashInsertAction.Plan:
                            return fieldInsertPlan;
                        case (int)Constants.HistoryCashInsertAction.Empty:
                            return fieldInserEmpty;
                    }
                    break;
            }
            return new Dictionary<string, string>();
        }
    }
}