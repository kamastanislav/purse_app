using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PurseApi.Models.Utils
{
    public class DbCommand : IDbCommand
    {
        private readonly SqlCommand _command;
        private int _nCurentBindArraySize;

        private readonly DataTable _table = new DataTable();
        
        public DbCommand(IDbCommand command)
        {
            BindArraySize = 0;
            _command = (SqlCommand)command;
        }

        public int BindArraySize { get; set; }

        public String DestinationTableName { get; set; }

        public DbReader ExecuteReader()
        {
            try
            {
                return new DbReader(_command.ExecuteReader());
            }
            catch (SystemException e)
            {
                throw e;
            }
        }

        public int Execute()
        {
            try
            {
                if (BindArraySize == 0)
                    return _command.ExecuteNonQuery();
                return 0;
            }
            catch (SystemException e)
            {
                throw e;
            }
        }

        public object ExecuteScalar()
        {
            try
            {
                return _command.ExecuteScalar();
            }
            catch (SystemException e)
            {
                throw e;
            }
        }

        public virtual void AddParam(int pos, object val)
        {
            try
            {
                var param = _command.CreateParameter();
                param.ParameterName = DbConnection.GetParamName(pos);
                param.Value = val ?? string.Empty;
                _command.Parameters.Add(param);
            }
            catch (SystemException e)
            {
                throw e;
            }
        }

        public virtual void AddParam(string paramName, object val)
        {
            try
            {
                var param = _command.CreateParameter();
                param.ParameterName = paramName;
                param.Value = val ?? string.Empty;
                _command.Parameters.Add(param);
            }
            catch (SystemException e)
            {
                throw e;
            }
        }

        public virtual void AddDateTimeParam(int pos, DateTime val)
        {
            try
            {
                var param = _command.CreateParameter();
                param.ParameterName = DbConnection.GetParamName(pos);
                param.DbType = DbType.DateTime;
                if (val == DateTime.MinValue)
                {
                    param.Value = DBNull.Value;
                }
                else
                {
                    param.Value = val;
                }
                _command.Parameters.Add(param);
            }
            catch (SystemException e)
            {
                throw e;
            }
        }

        public void SetParam(int pos, object val)
        {
            try
            {
                IDbDataParameter param;
                if (_command.Parameters.Count > pos)
                {
                    param = _command.Parameters[DbConnection.GetParamName(pos)];
                    param.Value = val ?? string.Empty;
                }
                else
                {
                    param = _command.CreateParameter();
                    param.ParameterName = DbConnection.GetParamName(pos);
                    param.Value = val ?? string.Empty;
                    _command.Parameters.Add(param);
                }
            }
            catch (SystemException e)
            {
                throw e;
            }
        }

        public void AddStringParam(int pos, string val)
        {
            AddParam(pos, val);
        }

        public void AddIntParam(int pos, int val)
        {
            AddParam(pos, val);
        }

        public void SetIntParam(int pos, int val)
        {
            SetParam(pos, val);
        }

        public void AddDoubleParam(int pos, double val)
        {
            AddParam(pos, val);
        }

        public void SetDoubleParam(int pos, double val)
        {
            SetParam(pos, val);
        }

        public void SetStringParam(int pos, string val)
        {
            SetParam(pos, val);
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public string CommandText
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int CommandTimeout
        {
            get
            {
                return _command.CommandTimeout;
            }
            set
            {
                _command.CommandTimeout = value;
            }
        }

        public CommandType CommandType
        {
            get
            {
                return _command.CommandType;
            }
            set
            {
                _command.CommandType = value;
            }
        }

        public IDbConnection Connection
        {
            get { return _command.Connection; }
            set { _command.Connection = value as SqlConnection; }
        }

        public IDbDataParameter
        CreateParameter()
        {
            return _command.CreateParameter();
        }

        public int ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            throw new NotImplementedException();
        }

        IDataReader IDbCommand.ExecuteReader()
        {
            throw new NotImplementedException();
        }

        public IDataParameterCollection Parameters
        {
            get { return _command.Parameters; }
        }

        public void Prepare()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction Transaction
        {
            get
            {
                return _command.Transaction;
            }
            set
            {
                _command.Transaction = value as SqlTransaction;
            }
        }

        public UpdateRowSource UpdatedRowSource
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            _command.Dispose();
        }
    }
}