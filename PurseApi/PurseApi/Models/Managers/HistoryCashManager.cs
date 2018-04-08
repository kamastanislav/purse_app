using PurseApi.Models.Entities;
using PurseApi.Models.Helper;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Managers
{
    public class HistoryCashManager
    {
        public static List<HistoryCash> GetHistoryCash(int id, int action)
        {
            var user = UserSession.Current.User;
            if (user != null)
            {
                if (action == (int)Constants.HistoryCashAction.Family && user.FamilyCode != id)
                {
                    return new List<HistoryCash>();
                }
                var repo = new HistoryCashRepository(id, action);
                return repo.List;
            }
            return new List<HistoryCash>();
        }
    }
}