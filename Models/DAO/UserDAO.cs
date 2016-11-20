using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.DAO
{
    public class UserDAO
    {
        private WatchShopEntities db;

        private static UserDAO instance;
        public static UserDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new UserDAO();
                return instance;
            }
        }

        private UserDAO()
        {
            db = CommonDAO.Instance.Database;
        }

        public void Insert(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }

        public User GetUserByUsername(string username)
        {
            return db.Users.SingleOrDefault(u => u.Username.Equals(username));
        }
    }
}
