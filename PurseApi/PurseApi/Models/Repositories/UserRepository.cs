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
    public enum UserField
    {
        Name = 1,
        Email = 2,
        Phone = 3,
        LastLogin = 4,
        Cash = 5,
        Birthday = 6,
        Color = 7,
        FirstName = 8,
        LastName = 9,
        Password = 10,
        FamalyCode = 11
    }

    public class UserRepository : GenericRepository<UserData>
    {
        private const string SQL_INSERT = "INSERT INTO [USER]([FIRST_NAME],[LAST_NAME],[NAME],[EMAIL],[PASSWORD],[PHONE],[COLOR],[BIRTHDAY],[CREATE_DATE]) VALUES(?,?,?,?,?,?,?,?,?)";
        private string SQL_UNIQUE = "SELECT COUNT(*) FROM [USER] WHERE {0} LIKE ?";


        private Dictionary<UserField, string> _uniqueFields = new Dictionary<UserField, string>()
        {
            { UserField.Name, "[NAME]"},
            { UserField.Email, "[EMAIL]"},
            { UserField.Phone, "[PHONE]"}
        };

        public UserRepository(bool empty = true) : base(empty)
        {

        }
        
        public void UpdateUserData(UserData user)
        {

        }

        public bool IsUnique(UserField field, string value)
        {
            try
            {
                if (!_uniqueFields.ContainsKey(field))
                    throw new Exception();
                var column = _uniqueFields[field];

                using (var conn = Connection.GetConnection(Constants.MAIN_CONNECTION))
                {
                    
                    var cmd = conn.CreateCommand(string.Format(SQL_UNIQUE, column));
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                            return rdr.GetInt(0) == 0;
                        throw new Exception();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateUser(UserData user)
        {
            try
            {
                int code = 0;
                using (var conn = Connection.GetConnection(Constants.MAIN_CONNECTION))
                {
                    var cmd = conn.CreateCommand(SQL_INSERT + ";" + SQL_SCOPE_IDENTITY);

                    cmd.SetStringParam(0, user.FirstName);
                    cmd.SetStringParam(1, user.LastName);
                    cmd.SetStringParam(2, user.NickName);
                    cmd.SetStringParam(3, user.Email);
                    cmd.SetStringParam(4, user.Password);
                    cmd.SetStringParam(5, user.Phone);
                    cmd.SetStringParam(6, user.Color);
                    cmd.SetParam(7, user.Birthday);
                    cmd.SetParam(8, user.CreateDate);

                    code = cmd.Execute();

                    conn.Close();
                }
                return code;
            }  
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIRST_NAME],[LAST_NAME],[NAME],[EMAIL],[PASSWORD],[PHONE],[COLOR],[BIRTHDAY],[CREATE_DATE]) VALUES(?,?,?,?,?,?,?,?,
        private readonly Dictionary<string, string> field = new Dictionary<string, string>()
        {
            {"", "CODE"},
            {"FirstName", "FIRST_NAME"},
            {"LastName", "LAST_NAME"},
            {"Birthday", "BIRTHDAY"},
            {"Color", "COLOR"},
            {"FamilyCode", "FAMILY_CODE"}
        };

        protected override string TableName
        {
            get
            {
                return "[USER]";
            }
        }

        protected override Dictionary<string, string> GetFieldsConformity()
        {
            return field;
        }
    }
}

/*
    [CODE] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,

	[FIRST_NAME] NVARCHAR(100) NOT NULL,

	[LAST_NAME] NVARCHAR(100) NOT NULL,

	[NAME] NVARCHAR(10) NOT NULL,
	[EMAIL] NVARCHAR(100) NOT NULL,

	[PASSWORD] NVARCHAR(MAX) NOT NULL,
	[CASH] NUMERIC(16, 2) DEFAULT 0,

    [CREATE_DATE] DATETIME NOT NULL,
	
    [PHONE] NVARCHAR(100) NOT NULL,
	[FAMILY_CODE] INT NULL,
	[COLOR] NVARCHAR(6) NOT NULL,

	[LAST_LOGIN] DATETIME NULL,
	[STATUS_USER] INT DEFAULT 0, 
*/
