using PurseApi.Models.Entities;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Logger
{
    public class Logger
    {
        private static TotalLogRepository totalRepo = new TotalLogRepository();

        public static void WriteError(Exception e)
        {
            TotalLog total = new TotalLog()
            {
                Descripton = string.Format("{0}: {1} \n{2}", e.Data, e.Message, e.StackTrace)
            };
            totalRepo.InsertData(total);
        }

        public static void WriteInfo(string message)
        {
            TotalLog total = new TotalLog()
            {
                Descripton = message
            };
            totalRepo.InsertData(total);
        }
    }
}