using PurseApi.Models.Entity;
using PurseApi.Models.Helper;
using PurseApi.Models.Helpers;
using PurseApi.Models.Repositories;
using PurseApi.Models.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace PurseApi.Models
{
    public enum Action
    {
        Insert = 1,
        Select = 2,
        Update = 3
    }

    public abstract class GenericRepository<T> where T : class, IEntity
    {
        public static string SQL_SCOPE_IDENTITY = "SELECT SCOPE_IDENTITY()";

        protected List<T> data = new List<T>();

        public GenericRepository(bool empty = true)
        {
            if (!empty)
                SelectData();
        }

        private string GenerateSelectQuery()
        {
            StringBuilder query = new StringBuilder();
            if (GetFieldsConformity((int)Action.Select).Count == 0 || String.IsNullOrEmpty(TableName))
                return String.Empty;
            try
            {
                query.Append("SELECT ");
                foreach (KeyValuePair<string, string> entry in GetFieldsConformity((int)Action.Select))
                {
                    query.Append(entry.Value + ", ");
                }
                query.Replace(',', ' ', query.Length - 2, 1);
                query.Append("FROM " + TableName);
                query.Append(TableWhere);
            }
            catch (Exception e)
            {
                Logger.Logger.WriteError(e);
                return null;
            }
            return query.ToString();
        }

        private string GenerateDeleteQuery()
        {
            StringBuilder query = new StringBuilder();
            if (GetFieldsConformity((int)Action.Select).Count == 0 || String.IsNullOrEmpty(TableName))
                return String.Empty;
            try
            {
                query.Append("DELETE ");
                query.Append("FROM " + TableName);
                query.Append(TableWhere);
            }
            catch (Exception e)
            {
                Logger.Logger.WriteError(e);
                return null;
            }
            return query.ToString();
        }

        private string GenerateInsertQuery()
        {
            StringBuilder query = new StringBuilder();
            if (GetFieldsConformity((int)Action.Insert).Count == 0 || String.IsNullOrEmpty(TableName))
                return String.Empty;
            try
            {
                query.Append(string.Format("INSERT INTO {0}(", TableName));
                foreach (KeyValuePair<string, string> entry in GetFieldsConformity((int)Action.Insert))
                {
                    query.Append(entry.Value + ", ");
                }
                query.Replace(',', ')', query.Length - 2, 1);
                query.Append("VALUES (");
                for (var i =0; i<GetFieldsConformity((int)Action.Insert).Count; i++)
                {
                    query.Append("?,");
                }
                query.Replace(',', ')', query.Length - 1, 1);
            }
            catch (Exception e)
            {
                Logger.Logger.WriteError(e);
                return null;
            }
            return query.ToString();
        }

        private string GenerateUpdateQuery()
        {
            StringBuilder query = new StringBuilder();
            if (GetFieldsConformity((int)Action.Insert).Count == 0 || String.IsNullOrEmpty(TableName))
                return String.Empty;
            try
            {
                query.Append(string.Format("UPDATE {0} SET ", TableName));
                foreach (KeyValuePair<string, string> entry in GetFieldsConformity((int)Action.Update))
                {
                    query.Append(entry.Value + " = ?,");
                }
                query.Replace(',', ' ', query.Length - 1, 1);
                query.Append(TableWhere);

            }
            catch (Exception e)
            {
                Logger.Logger.WriteError(e);
                return null;
            }
            return query.ToString();
        }

        public int InsertData(T obj)
        {
            try
            {
                int code = 0;
                string sql = GenerateInsertQuery();
                if (String.IsNullOrEmpty(sql))
                    throw new Exception("Impossible to obtain data from a database for [" + this.ToString() + "] Repository");

                sql += (";" + SQL_SCOPE_IDENTITY);

                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                using (var conn = Connection.GetConnection(Constants.MAIN_CONNECTION))
                {
                    var cmd = conn.CreateCommand(sql);

                    var index = 0;
                    for (int i = 0; i < properties.Length; i++)
                    {
                        PropertyInfo p = properties[i];
                        if (p.CanWrite && GetFieldsConformity((int)Action.Insert).ContainsKey(p.Name))
                        {
                            cmd.SetParam(index, p.GetValue(obj));
                            index++;
                        }
                    }
                    
                    code = cmd.Execute();
                    conn.Close();
                }
                return code;
            }
            catch (Exception ex)
            {
                Logger.Logger.WriteError(ex);
                throw ex;
            }

        }

        protected virtual void SelectData()
        {
            data.Clear();
            try
            {
                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                Action<T, object>[] setterArray = new Action<T, object>[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    PropertyInfo p = properties[i];
                    if (p.CanWrite)
                        setterArray[i] = FastInvoke.BuildUntypedSetter<T>(p);
                }

                Type[] types = new Type[] { typeof(int) };
                var constructorInfo = typeof(T).GetConstructor(types);

                var args = Expression.Parameter(typeof(object[]), "args");
                var body = Expression.New(constructorInfo,
                types.Select((t, i) => Expression.Convert(Expression.ArrayIndex(args, Expression.Constant(i)), t)).ToArray());
                var outer = Expression.Lambda<Func<object[], object>>(body, args);
                var func = outer.Compile();

                string sql = GenerateSelectQuery();
                if (String.IsNullOrEmpty(sql))
                    throw new Exception("Impossible to obtain data from a database for [" + this.ToString() + "] Repository");

                using (var conn = Connection.GetConnection(Constants.MAIN_CONNECTION))
                {
                    var cmd = conn.CreateCommand(sql);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            object instance = func(new object[] { Convert.ToInt32(rdr.GetInt(0)) });
                            int j = 1;

                            for (int i = 0; i < properties.Length; i++)
                            {
                                PropertyInfo p = properties[i];
                                if (p.CanWrite && GetFieldsConformity((int)Action.Select).ContainsKey(p.Name))
                                {
                                    object fieldValue = rdr.GetValue(j); // columns and fields must be in the same order 
                                    if (fieldValue == DBNull.Value)
                                    {
                                        fieldValue = null;
                                        p.SetValue(instance, fieldValue, null);
                                    }
                                    else
                                        setterArray[i]((T)instance, fieldValue);
                                    j++;
                                }
                            }
                            AddData((T)instance);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Logger.WriteError(ex);
                throw ex;
            }
        }

        protected bool DeleteData()
        {
            try
            {
                string sql = GenerateDeleteQuery();
                if (String.IsNullOrEmpty(sql))
                    throw new Exception("Impossible to obtain data from a database for [" + this.ToString() + "] Repository");

                using (var conn = Connection.GetConnection(Constants.MAIN_CONNECTION))
                {
                    var cmd = conn.CreateCommand(sql);
                    cmd.Execute();
                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Logger.WriteError(ex);
                return false;
            }
        }

        protected bool UpdateData(T obj)
        {
            try
            {
                string sql = GenerateUpdateQuery();
                if (String.IsNullOrEmpty(sql))
                    throw new Exception("Impossible to obtain data from a database for [" + this.ToString() + "] Repository");

                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                using (var conn = Connection.GetConnection(Constants.MAIN_CONNECTION))
                {
                    var cmd = conn.CreateCommand(sql);

                    var index = 0;
                    for (int i = 0; i < properties.Length; i++)
                    {
                        PropertyInfo p = properties[i];
                        if (p.CanWrite && GetFieldsConformity((int)Action.Update).ContainsKey(p.Name))
                        {
                            cmd.SetParam(index, p.GetValue(obj));
                            index++;
                        }
                    }
                    cmd.Execute();
                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Logger.WriteError(ex);
                return false;
            }
        }

        protected virtual void AddData(T item)
        {
            data.Add(item);
        }

        public List<T> List
        {
            get
            {
                return data;
            }
        }

        protected abstract string TableName { get; }

        protected virtual string TableWhere { get { return ""; } }

        protected abstract Dictionary<string, string> GetFieldsConformity(int action);
  
    }
}