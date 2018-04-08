using PurseApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Helpers
{
    public class FilterPlan
    {
        public List<CategoryService> CategoryServices { get; set; }
        public Dictionary<int, string> Executors { get; set; }
        public string CurrencySymbol { get; set; }
    }
}