using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.DAO
{
    public class UserGroupDAO
    {
        private WatchShopEntities db;

        private static UserGroupDAO instance;
        public static UserGroupDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new UserGroupDAO();
                return instance;
            }
        }

        private UserGroupDAO()
        {
            db = CommonDAO.Instance.Database;
        }

        public void Insert(UserGroup userGroup)
        {
            db.UserGroups.Add(userGroup);
            db.SaveChanges();
        }

        public List<UserGroup> GetAllUserGroups()
        {
            return db.UserGroups.ToList();
        }
    }
}
