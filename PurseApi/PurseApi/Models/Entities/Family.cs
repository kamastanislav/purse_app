using PurseApi.Models.Entity;
using PurseApi.Models.Helper;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;

namespace PurseApi.Models.Entities
{
    public class Family : IEntity
    {
        private readonly int _code;

        private List<UserData> _users;

        public int Code
        {
            get
            {
                return _code;
            }
        }

        public int OwnerCode { get; set; }

        public List<UserData> Users
        {
            get
            {
                if (_users == null)
                {
                    var repo = new UserRepository((int)Constants.UserAction.Family);
                    _users = repo.GetList(Code);
                }
                return _users;
            }
        }

        public Family()
        {

        }

        public Family(int code)
        {
            _code = code;
        }
    }
}