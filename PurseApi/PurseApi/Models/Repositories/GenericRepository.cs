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
    public abstract class GenericRepository<T>: Repository<T> where T : class, IEntity
    {
        private string GenerateSelectQuery()
        {
            StringBuilder query = new StringBuilder();
            if (GetFieldsConformity().Count == 0 || String.IsNullOrEmpty(TableName))
                return String.Empty;
            try
            {
                query.Append("SELECT ");
                foreach (KeyValuePair<string, string> entry in GetFieldsConformity())
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

        private string GenerateInsertQuery()
        {
            StringBuilder query = new StringBuilder();
            return query.ToString();
        }

        private string GenerateDeleteQuery()
        {
            StringBuilder query = new StringBuilder();
            return query.ToString();
        }

        public GenericRepository(bool empty = true) : base(empty) {
            
        }

        protected override void SelectData()
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
                                if (p.CanWrite && GetFieldsConformity().ContainsKey(p.Name))
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
                throw ex;
            }
        }

        protected abstract string TableName { get; }

        protected virtual string TableWhere { get { return ""; } }

        protected abstract Dictionary<string, string> GetFieldsConformity();
  
    }
}