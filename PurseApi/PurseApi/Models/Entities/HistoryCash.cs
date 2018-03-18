﻿using PurseApi.Models.Entity;
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
        public double Cash { get; set; }
        public DateTime DateAction { get; set; }
        public int UserCode { get; set; }
        public int CategoryCode { get; set; }

        public HistoryCash(int code)
        {
            _code = code;
        }
    }
}