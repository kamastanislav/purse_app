using System;
using System.Data;

namespace PurseApi.Models.Utils
{
    public class DbReader : IDisposable
    {    
        private readonly IDataReader _reader;

        internal DbReader(IDataReader reader)
        {
            _reader = reader;
        }

        public string GetString(int pos)
        {
            try
            {
                if (_reader.IsDBNull(pos))
                    return null;
                return Convert.ToString(_reader.GetValue(pos));
            }
            catch (SystemException e)
            {
                throw e;
            }
        }

        public bool GetBoolean(int pos)
        {
            try
            {
                object value = _reader.GetValue(pos);
                if (value == DBNull.Value)
                    return false;
                return Convert.ToBoolean(value);
            }
            catch (SystemException e)
            {
                throw e;
            }
        }

        public int GetInt(int pos)
        {
            try
            {
                object value = _reader.GetValue(pos);
                if (value == DBNull.Value)
                    return 0;
                return Convert.ToInt32(value);
            }
            catch (SystemException e)
            {
                throw e;
            }
        }

        public DateTime GetDateTime(int pos)
        {
            try
            {
                if (_reader.IsDBNull(pos))
                    return DateTime.MinValue;

                return Convert.ToDateTime(_reader.GetValue(pos));
            }
            catch (SystemException e)
            {
                throw e;
            }
        }

        public string GetName(int pos)
        {
            try
            {
                return _reader.GetName(pos);
            }
            catch (SystemException e)
            {
                throw e;
            }
        }

        public double GetDouble(int pos)
        {
            try
            {
                if (_reader.IsDBNull(pos))
                    return 0;

                double value = Convert.ToDouble(_reader.GetValue(pos));

                if (value == double.MinValue)
                    return double.NaN;

                return value;
            }
            catch (SystemException e)
            {
                throw e;
            }
        }

        public object GetValue(int pos)
        {
            try
            {
                return _reader.GetValue(pos);
            }
            catch (SystemException e)
            {
                throw e;
            }
        }

        public bool Read()
        {
            try
            {
                return _reader.Read();
            }
            catch (SystemException e)
            {
                throw e;
            }
        }

        public void Close()
        {
            try
            {
                _reader.Close();
            }
            catch (SystemException e)
            {
                throw e;
            }
        }
        
        public void Dispose()
        {
            _reader.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}