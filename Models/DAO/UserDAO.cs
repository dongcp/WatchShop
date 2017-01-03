using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using System.Data.Entity;

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

        public void Update(User user)
        {
            User foundUser = db.Users.Find(user.Username);
            foundUser.Copy(user);
            db.Entry(foundUser).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(string username)
        {
            User user = db.Users.Find(username);
            db.Users.Remove(user);
            db.SaveChanges();
        }

        public void CreateUser(string username, string password, string phoneNumber)
        {
            User user = new User();
            user.Username = username;
            user.Password = password;
            user.PhoneNumber = phoneNumber;
            user.UserGroupId = "CUSTOMER";
            db.Users.Add(user);
            db.SaveChanges();
        }

        public IEnumerable<User> GetAll(int page, int pageSize)
        {
            return db.Users.OrderBy(u => u.UserGroupId).ToPagedList(page, pageSize);
        }

        public User GetByUsername(string username)
        {
            return db.Users.SingleOrDefault(u => u.Username.Equals(username));
        }
    }
}
