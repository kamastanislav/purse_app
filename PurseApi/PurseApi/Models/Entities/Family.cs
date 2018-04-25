using PurseApi.Models.Entity;
using System;
using System.Collections.Generic;

namespace PurseApi.Models.Entities
{
    public class Family : IEntity
    {
        private readonly int _code;

        public int Code
        {
            get
            {
                return _code;
            }
        }

        public int OwnerCode { get; }

        public Family(int code)
        {
            _code = code;
        }
    }
}