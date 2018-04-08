using PurseApi.Models.Entity;
using System;
using System.Collections.Generic;

namespace PurseApi.Models.Entities
{
    public class Family : IEntity
    {
        private readonly int _code;
        private Currency _currency;

        public int Code
        {
            get
            {
                return _code;
            }
        }

        public long CalendarPeriodStart { get; set; }
        public long CalendarPeriodEnd { get; set; }
        public int OwnerCode { get; }
        public int CurrencyCode { get; set; }
        public Currency CurrencyFamily
        {
            get
            {
                if (_currency == null)
                {

                }
                return _currency;
            }
        }

        public Family(int code)
        {
            _code = code;
        }
    }
}