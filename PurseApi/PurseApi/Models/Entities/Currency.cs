using PurseApi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Entities
{
    public class Currency : IEntity
    {
        private readonly int _code;

        public int Code
        {
            get
            {
                return _code;
            }
        }

        public string Name { get; set; }
        public string Symbol { get; set; }

        public Currency(int code)
        {
            _code = code;
        }
    }
}