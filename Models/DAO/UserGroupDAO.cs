using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using System.Data.Entity;

namespace Models.DAO
{
    public class UserGroupDAO
    {
        private const string ID_USER_GROUP_PREFIX = "UG";

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
            userGroup.Id = GetNewId(ID_USER_GROUP_PREFIX);
            db.UserGroups.Add(userGroup);
            db.SaveChanges();
        }

        public void Update(UserGroup userGroup)
        {
            UserGroup foundUserGroup = db.UserGroups.Find(userGroup.Id);
            foundUserGroup.Name = userGroup.Name;
            db.Entry(foundUserGroup).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(string id)
        {
            UserGroup userGroup = db.UserGroups.Find(id);
            db.UserGroups.Remove(userGroup);
            db.SaveChanges();
        }

        public List<UserGroup> GetAll()
        {
            return db.UserGroups.ToList();
        }

        public IEnumerable<UserGroup> GetAll(int page, int pageSize)
        {
            return db.UserGroups.OrderBy(ug => ug.Id).ToPagedList(page, pageSize);
        }

        public UserGroup GetById(string id)
        {
            return db.UserGroups.Find(id);
        }

        private string GetNewId(string idPrefix)
        {
            string oldId = db.Products.Max(p => p.Id);
            if (oldId == null || oldId.Equals("")) return idPrefix + "1";
            int newIdSuffix = Int16.Parse(oldId.Substring(idPrefix.Length, oldId.Length - idPrefix.Length)) + 1;
            return idPrefix + newIdSuffix;
        }
    }
}
