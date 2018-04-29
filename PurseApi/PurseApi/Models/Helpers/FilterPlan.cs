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
        public List<UserData> Executors { get; set; }
        public string OwnerUser { get; set; }
        public int FamilyCode { get; set; }
    }
}