using PurseApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Repositories
{
    public class TotalLogRepository : GenericRepository<TotalLog>
    {
        protected override string TableName
        {
            get
            {
                return "TOTALLOG";
            }
        }

        private readonly Dictionary<string, string> fieldInsert = new Dictionary<string, string>()
        {
            {"Descripton", "DESCRIPTION" }
        };

        protected override Dictionary<string, string> GetFieldsConformity(int action)
        {
            switch (action)
            {
                case (int)Action.Insert:
                    return fieldInsert;
               default:
                    return new Dictionary<string, string>();
            }
        }
    }
}