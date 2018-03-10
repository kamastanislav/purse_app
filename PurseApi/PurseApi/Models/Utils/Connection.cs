using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Utils
{
    public static class Connection
    {
        private static Dictionary<string, string> ConnectionStrings = new Dictionary<string, string>();

        public static void SetConnectionString(string key, string value)
        {
            if (!ConnectionStrings.ContainsKey(key))
                ConnectionStrings.Add(key, value);
        }

        public static DbConnection GetConnection(string connectionId)
        {
            if (ConnectionStrings.Count == 0 || !ConnectionStrings.ContainsKey(connectionId))
                throw new Exception("Connection with name '" + connectionId + "' doesn't exist!");
            var conn = new SqlConnection(ConnectionStrings[connectionId]);
            conn.Open();
           
            return new DbConnection(conn);
        }
    }
}