using Newtonsoft.Json;
using PurseApi.Models.Entity;
using PurseApi.Models.Helper;
using System;
using System.Collections.Generic;

namespace PurseApi.Models.Entities
{
    public class UserData : IEntity
    {
        private readonly int _code;
        private Family _userFamily;

        public int Code
        {
            get
            {
                return _code;
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public decimal Cash { get; set; }
        public long LastLogin { get; set; }
        [JsonIgnore]
        public long CreateDate { get; set; }
        public long Birthday { get; set; }
        
        public int FamilyCode { get; set; }
        public int StatusCode { get; set; }

        [JsonIgnore]
        public bool IsAdmin
        {
            get { return StatusCode == (int)Constants.StatusUser.Admin; }
        }
        [JsonIgnore]
        public bool IsUser
        {
            get { return StatusCode == (int)Constants.StatusUser.User; }
        }
        [JsonIgnore]
        public bool IsNoneFamily
        {
            get { return StatusCode == (int)Constants.StatusUser.NoneFamily; }
        }
        [JsonIgnore]
        public Family UserFamily
        {
            get
            {
                if (_userFamily == null)
                {

                }
                return _userFamily;
            }
        }

        public UserData()
        {
        }

        public UserData(int code)
        {
            _code = code;
        }

        public UserData(UserData user, int code)
        {
            this._code = code;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.NickName = user.NickName;
            this.Email = user.Email;
            this.Phone = user.Phone;
            this.Password = user.Password;
            this.Cash = user.Cash;
            this.LastLogin = user.LastLogin;
            this.CreateDate = user.CreateDate;
            this.Birthday = user.Birthday;
            this.FamilyCode = user.FamilyCode;
            this.StatusCode = user.StatusCode;
        }

    }
}