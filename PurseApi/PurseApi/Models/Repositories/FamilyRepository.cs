using PurseApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Repositories
{
    public class FamilyRepository : GenericRepository<Family>
    {
        private int _code;

        public FamilyRepository()
        {
        }

        public FamilyRepository(int code)
        {
            _code = code;
            SelectData();
        }

        public bool DeleteFamily()
        {
            return DeleteData();
        }

        protected override string TableWhere
        {
            get
            {
                return string.Format("[CODE] = {0}", _code);
            }
        }

        private readonly Dictionary<string, string> fieldSelect = new Dictionary<string, string>()
        {
            {"", "CODE"},
            {"OwnerCode", "OWNER_CODE" },
        };

        private readonly Dictionary<string, string> fieldInsert = new Dictionary<string, string>()
        {
            {"OwnerCode", "OWNER_CODE" },
        };

        protected override string TableName
        {
            get
            {
                return "[FAMILY]";
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