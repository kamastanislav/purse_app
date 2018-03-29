using PurseApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Repositories
{
    public class ServiceRepository : GenericRepository<Service>
    {
        public ServiceRepository(bool empty = true) : base(empty)
        {

        }

        private readonly Dictionary<string, string> fieldSelect = new Dictionary<string, string>()
        {
            {"", "CODE"},
            {"Name", "NAME"},
            {"Color", "COLOR"},
            {"CategoryCode", "CATEGORY_CODE"}
        };

        protected override string TableName
        {
            get
            {
                return "SERVICE";
            }
        }

        protected override Dictionary<string, string> GetFieldsConformity(int action)
        {
            switch (action)
            {
                case (int)Action.Select:
                    return fieldSelect;
                default:
                    return new Dictionary<string, string>();
            }
        }
    }
}