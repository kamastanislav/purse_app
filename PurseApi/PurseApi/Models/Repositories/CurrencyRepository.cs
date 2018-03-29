using PurseApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Repositories
{
    public class CurrencyRepository : GenericRepository<Currency>
    {
        public CurrencyRepository(bool empty = true) : base(empty)
        {

        }

        private readonly Dictionary<string, string> fieldSelect = new Dictionary<string, string>()
        {
            {"", "CODE"},
            {"Name", "NAME"},
            {"Symbol", "SYMBOL"}
        };

        protected override string TableName
        {
            get
            {
                return "CURRENCY";
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