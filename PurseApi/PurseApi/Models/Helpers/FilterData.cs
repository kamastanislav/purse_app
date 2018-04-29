using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Helpers
{
    public class FilterData
    {
        public List<long> DateInterval { get; set; }
        public List<int> Owner { get; set; }
        public List<int> Executor { get; set; }
        public List<int> Status { get; set; }
        public List<int> Category { get; set; }
    }
}