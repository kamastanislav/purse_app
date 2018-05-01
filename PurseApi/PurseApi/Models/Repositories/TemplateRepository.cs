using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Repositories
{
    public class TemplateRepository : GenericRepository<TemplatePlan>
    {
        private int _code;
        private int _action;

        public TemplateRepository(int action, int code, bool empty = true) 
        {
            _code = code;
            _action = action;
            if (!empty)
                SelectData();
        }

        protected override string TableWhere
        {
            get
            {
                switch (_action)
                {
                    case (int)Constants.TemplateAction.Plan:
                        return string.Format("WHERE [PLAN_CODE] = {0}", _code);
                    case (int)Constants.TemplateAction.List:
                        return string.Format("WHERE [USER_CODE] = {0}", _code);
                }
                return string.Empty;
            }
        }

        public bool UpdateTemplate(TemplatePlan plan)
        {
            return UpdateData(plan);
        }

        public bool DeleteTemplate()
        {

            return DeleteData();

        }
  
        protected override string TableName
        {
            get
            {
                return "[TEMPLATE_PLAN]";
            }
        }

        private readonly Dictionary<string, string> fieldSelect = new Dictionary<string, string>()
        {
           {"","CODE" },
           {"PlanCode", "PLAN_CODE"},
           {"IsUpdate", "IS_UPDATE"},
           {"AllPlan", "ALL_PLAN"},
           {"UserCode", "USER_CODE"}
        };

        private readonly Dictionary<string, string> fieldInsert = new Dictionary<string, string>()
        {
           {"PlanCode", "PLAN_CODE"},
           {"IsUpdate", "IS_UPDATE"},
           {"AllPlan", "ALL_PLAN"},
           {"UserCode", "USER_CODE"}
        };

        private readonly Dictionary<string, string> fieldUpdate = new Dictionary<string, string>()
        {
           {"IsUpdate", "IS_UPDATE"},
           {"AllPlan", "ALL_PLAN"}
        };

        protected override Dictionary<string, string> GetFieldsConformity(int action)
        {
            switch (action)
            {
                case (int)Action.Update:
                    return fieldUpdate;
                case (int)Action.Select:
                    return fieldSelect;
                case (int)Action.Insert:
                    return fieldInsert;
            }
            return new Dictionary<string, string>();
        }
    }


}