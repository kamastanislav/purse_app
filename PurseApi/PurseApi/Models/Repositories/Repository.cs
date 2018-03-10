using PurseApi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Repositories
{
    public abstract class Repository<T> where T : class, IEntity
    {
        public static string SQL_SCOPE_IDENTITY = "SELECT SCOPE_IDENTITY()";

        protected List<T> data = new List<T>();

        public Repository(bool empty = true)
        {
            if (!empty)
                SelectData();
        }

        protected abstract void SelectData();
        
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
    }
}