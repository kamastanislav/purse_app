using System;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace PurseApi.Models.Utils
{
    public class DbConnection : IDbConnection
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        
        public const int DbProviderMsSql = 0;

        internal DbConnection(IDbConnection connection)
        {
            _connection = connection;
        }

        public DbCommand CreateCommand(string text)
        {
            var sb = new StringBuilder();
            var paramNum = 0;
            var begin = 0;
            while (begin < text.Length)
            {
                var end = text.IndexOf('?', begin);
                if (end == -1)
                {
                    end = text.Length - 1;
                    sb.Append(text, begin, end - begin + 1);
                }
                else
                {
                    sb.Append(text, begin, end - begin);
                    sb.Append(GetParamName(paramNum));
                    paramNum++;
                }
                begin = end + 1;
            }
            IDbCommand cmd;
            try
            {
                cmd = _connection.CreateCommand();
                cmd.CommandText = sb.ToString();
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = _transaction;
                cmd.CommandTimeout = 300;
                cmd.Connection = this._connection;
            }
            catch (SystemException e)
            {
                throw e;
            }

            return new DbCommand(cmd);
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            throw new NotImplementedException();
        }

        IDbTransaction IDbConnection.BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            try
            {
                _connection.Close();
            }
            catch (SystemException e)
            {
                throw e;
            }
        }

        public void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public IDbCommand CreateCommand()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public string ConnectionString
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int ConnectionTimeout
        {
            get { throw new NotImplementedException(); }
        }

        public string Database
        {
            get { throw new NotImplementedException(); }
        }

        public ConnectionState State
        {
            get { throw new NotImplementedException(); }
        }

        public virtual void Dispose()
        {
            _connection.Dispose();
        }

        public static string GetParamName(int pos)
        {
            return "@pr" + pos;
        }
    }
}