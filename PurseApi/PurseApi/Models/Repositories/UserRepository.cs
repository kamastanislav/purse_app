using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using PurseApi.Models.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Repositories
{
    public class UserRepository : GenericRepository<UserData>
    {
        private const string SQL_UNIQUE = "SELECT [CODE] FROM [USER] WHERE {0} LIKE '{1}'";
        private const string SQL_WHERE = " WHERE {0}";
        private const string SQL_CHECK_USER_PASSWORD = "SELECT [CODE] FROM [USER] WHERE [CODE] = {0} AND [PASSWORD] LIKE '{1}'";

        private int _actionCode;
        private int _code;
        private string _login;
        private string _password;

        private List<int> _fields = new List<int>();
        
        private Dictionary<Constants.UserField, string> _uniqueFields = new Dictionary<Constants.UserField, string>()
        {
            { Constants.UserField.NickName, "[NAME]"},
            { Constants.UserField.Email, "[EMAIL]"},
            { Constants.UserField.Phone, "[PHONE]"}
        };

        public UserRepository()
        {

        }

        public UserRepository(int action)
        {
            _actionCode = action;
        }

        public void SetActionCode(int action)
        {
            _actionCode = action;
        }

        public UserData GetUser(int code)
        {
            if (_actionCode == (int)Constants.UserAction.Code)
            {
                _code = code;
                SelectData();
                return data.FirstOrDefault();
            }

            return null;
        }

        public UserData GetUser(string nick, string password)
        {
            if (_actionCode == (int)Constants.UserAction.Login)
            {
                _login = nick;
                _password = password;
                SelectData();
                return data.FirstOrDefault();
            }
            return null;
        }

        public bool DeleteUser(int code)
        {
            if (_actionCode == (int)Constants.UserAction.Code)
            {
                _code = code;
                return DeleteData();
            }
            return false;
        }

        public List<UserData> GetList(int familyCode)
        {
            if (_actionCode == (int)Constants.UserAction.Family)
            {
                _code = familyCode;
                SelectData();
                return data;
            }
            return null;
        }

        public List<UserData> GetList()
        {
            if (_actionCode == (int)Constants.UserAction.List)
            {
                SelectData();
                return data;
            }
            return null;
        }

        protected override string TableWhere
        {
            get
            {
                string parametr = string.Empty;
                switch ((Constants.UserAction)_actionCode)
                {
                    case Constants.UserAction.Family:
                        parametr = string.Format("[FAMILY_CODE] = {0}", _code);
                        return string.Format(SQL_WHERE, parametr);
                    case Constants.UserAction.Code:
                        parametr = string.Format("[CODE] = {0}", _code);
                        return string.Format(SQL_WHERE, parametr);
                    case Constants.UserAction.Login:
                        parametr = string.Format("[NAME] LIKE '{0}' AND [PASSWORD] LIKE '{1}'", _login, _password);
                        return string.Format(SQL_WHERE, parametr);
                }
                return string.Empty;
            }
        }

        public UserData UpdateUserData(UserData user, List<int> fields)
        {
            if (_actionCode == (int)Constants.UserAction.Code)
            {
                _code = user.Code;
                _fields = fields;
                if (UpdateData(user))
                    return user;
            }
            return null;
        }

        public bool CheckUserPassword(int code, string password)
        {
            try
            {
                using (var conn = Connection.GetConnection(Constants.MAIN_CONNECTION))
                {

                    var cmd = conn.CreateCommand(string.Format(SQL_CHECK_USER_PASSWORD, code, password));
                    using (var rdr = cmd.ExecuteReader())
                    {
                        return rdr.Read();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsUnique(Constants.UserField field, string value)
        {
            try
            {
                if (!_uniqueFields.ContainsKey(field))
                    throw new Exception();
                var column = _uniqueFields[field];

                using (var conn = Connection.GetConnection(Constants.MAIN_CONNECTION))
                {

                    var cmd = conn.CreateCommand(string.Format(SQL_UNIQUE, column, value));
                    using (var rdr = cmd.ExecuteReader())
                    {
                        return rdr.Read();
                        //throw new Exception();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private readonly Dictionary<string, string> fieldSelect = new Dictionary<string, string>()
        {
            {"", "CODE"},
            {"FirstName", "FIRST_NAME"},
            {"LastName", "LAST_NAME"},
            {"NickName", "NAME"},
            {"Email", "EMAIL" },
            {"Phone", "PHONE" },
            {"Cash", "CASH" },
            {"LastLogin", "LAST_LOGIN" },
            {"CreateDate", "CREATE_DATE" },
            {"Birthday", "BIRTHDAY"},
            {"FamilyCode", "FAMILY_CODE"},
            {"StatusCode", "STATUS_USER" }, 
        };

        private readonly Dictionary<string, string> fieldInsert = new Dictionary<string, string>()
        {
            {"FirstName", "FIRST_NAME"},
            {"LastName", "LAST_NAME"},
            {"NickName", "NAME"},
            {"Email", "EMAIL" },
            {"Phone", "PHONE" },
            {"Password", "PASSWORD" },
            {"LastLogin", "LAST_LOGIN" },
            {"CreateDate", "CREATE_DATE" },
            {"Birthday", "BIRTHDAY"}
        };

        private readonly Dictionary<string, string> fieldUpdate = new Dictionary<string, string>()
        {
            {"FirstName","FIRST_NAME"},
            {"LastName","LAST_NAME"},
            {"NickName","NAME"},
            {"Email","EMAIL"},
            {"Phone","PHONE"},
            {"Password","PASSWORD"},
            {"Cash","CASH"},
            {"LastLogin","LAST_LOGIN"},
            {"Birthday","BIRTHDAY"},
            {"FamilyCode","FAMILY_CODE"},
            {"StatusCode","STATUS_USER"}
        };

        protected override string TableName
        {
            get
            {
                return "[USER]";
            }
        }

        protected override Dictionary<string, string> GetFieldsConformity(int action)
        {
            switch(action)
            {
                case (int)Action.Select:
                    return fieldSelect;
                case (int)Action.Insert:
                    return fieldInsert;
                case (int)Action.Update:
                    return fieldUpdate.Where(x => _fields.Any(y => ((Constants.UserField)y).ToString() == x.Key)).ToDictionary(x => x.Key, x => x.Value);
                default:
                    return new Dictionary<string, string>();
            }
        }
    }
}