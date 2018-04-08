using PurseApi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Entities
{
    public class HistoryCash: IEntity
    {
        private readonly int _code;

        public int Code
        {
            get
            {
                return _code;
            }
        }
        
        public int UserCode { get; set; }
        public long DateAction { get; set; }
        public double Cash { get; set; }
        public int CategoryCode { get; set; }
        public int PlanCode { get; set; }
        public int FamilyCode { get; set; }

        public HistoryCash()
        {

        }
        
        public HistoryCash(int code)
        {
            _code = code;
        }

        public HistoryCash(HistoryCash historyCash, int code)
        {
            this._code = code;
            this.UserCode = historyCash.UserCode;
            this.DateAction = historyCash.DateAction;
            this.Cash = historyCash.Cash;
            this.CategoryCode = historyCash.CategoryCode;
            this.PlanCode = historyCash.PlanCode;
            this.FamilyCode = historyCash.FamilyCode;
        }
    }
}

