using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using System.Data.Entity;

namespace Models.DAO
{
    public class RoleDAO
    {
        private WatchShopEntities db;

        private static RoleDAO instance;
        public static RoleDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new RoleDAO();
                return instance;
            }
        }

        private RoleDAO()
        {
            db = CommonDAO.Instance.Database;
        }

        public void Create(Role role)
        {
            db.Roles.Add(role);
            db.SaveChanges();
        }

        public void Update(Role role)
        {
            Role foundRole = db.Roles.Find(role.Id);
            foundRole.Name = role.Name;
            db.Entry(foundRole).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(string id)
        {
            Role role = db.Roles.Find(id);
            db.Roles.Remove(role);
            db.SaveChanges();
        }

        public List<Role> GetAll()
        {
            return db.Roles.ToList();
        }

        public IEnumerable<Role> GetAll(int page, int pageSize)
        {
            return db.Roles.OrderBy(r => r.Id).ToPagedList(page, pageSize);
        }

        public Role GetById(string id)
        {
            return db.Roles.Find(id);
        }

        public bool IsRoleExisted(string id)
        {
            Role role = db.Roles.Find(id);
            return role != null;
        }

        public List<UserGroup> GetAllGroupsByRoleId(string roleId)
        {
            var userGroups = from ug in db.UserGroups
                             join gp in db.GroupPermissions on ug.Id equals gp.UserGroupId
                             where gp.RoleId.Equals(roleId)
                             select ug;
            return userGroups.ToList();
        }

        public List<User> GetAllUsersByRoleId(string roleId)
        {
            var users = from u in db.Users
                        join up in db.UserPermissions on u.Username equals up.Username
                        where up.RoleId.Equals(roleId)
                        select u;
            return users.ToList();
        }

        public void GrantUserPermission(string username, string roleId)
        {
            UserPermission userPermission = new UserPermission();
            userPermission.RoleId = roleId;
            userPermission.Username = username;
            db.UserPermissions.Add(userPermission);
            db.SaveChanges();
        }

        public void GrantGroupPermission(string userGroupId, string roleId)
        {
            GroupPermission groupPermission = new GroupPermission();
            groupPermission.UserGroupId = userGroupId;
            groupPermission.RoleId = roleId;
            db.GroupPermissions.Add(groupPermission);
            db.SaveChanges();
        }

        public List<UserGroup> GetAllRemainingGroupsByRoleId(string roleId)
        {
            List<UserGroup> groups = db.UserGroups.ToList();
            List<string> existedGroups = db.GroupPermissions.Where(gp => gp.RoleId.Equals(roleId))
                .Select(gp => gp.UserGroupId).ToList();
            for (int i = 0; i < existedGroups.Count; i++)
            {
                for (int j = 0; j < groups.Count; j++)
                {
                    if (groups[j].Id.Equals(existedGroups[i]))
                    {
                        groups.RemoveAt(j);
                        break;
                    }
                }
            }
            return groups;
        }

        public List<User> GetAllRemainingUsersByRoleId(string roleId)
        {
            List<User> users = db.Users.ToList();
            List<string> existedGroups = db.UserPermissions.Where(up => up.RoleId.Equals(roleId))
                .Select(up => up.Username).ToList();
            for (int i = 0; i < existedGroups.Count; i++)
            {
                for (int j = 0; j < users.Count; j++)
                {
                    if (users[j].Username.Equals(existedGroups[i]))
                    {
                        users.RemoveAt(j);
                        break;
                    }
                }
            }
            return users;
        }

        public List<Role> GetAllPersonalPermissions(string username)
        {
            User user = db.Users.Find(username);

            var userPermissions = from up in db.UserPermissions
                                  join r in db.Roles on up.RoleId equals r.Id
                                  where up.Username.Equals(username)
                                  select r;

            var groupPermissions = from gp in db.GroupPermissions
                                   join r in db.Roles on gp.RoleId equals r.Id
                                   where gp.UserGroupId.Equals(user.UserGroupId)
                                   select r;

            return groupPermissions.Union(userPermissions).ToList();
        }

        public IEnumerable<Role> GetAllPersonalPermissions(string username, int page, int pageSize)
        {
            User user = db.Users.Find(username);

            var userPermissions = from up in db.UserPermissions
                                  join r in db.Roles on up.RoleId equals r.Id
                                  where up.Username.Equals(username)
                                  select r;

            var groupPermissions = from gp in db.GroupPermissions
                                   join r in db.Roles on gp.RoleId equals r.Id
                                   where gp.UserGroupId.Equals(user.UserGroupId)
                                   select r;

            return groupPermissions.Union(userPermissions).OrderBy(r => r.Id).ToPagedList(page, pageSize);
        }

        public void DeleteGroupPermission(string roleId, string groupId)
        {
            GroupPermission groupPermission = db.GroupPermissions
                .Where(gp => gp.RoleId.Equals(roleId))
                .Where(gp => gp.UserGroupId.Equals(groupId))
                .First();
            db.GroupPermissions.Remove(groupPermission);
            db.SaveChanges();
        }

        public void DeletePersonPermission(string roleId, string username)
        {
            UserPermission personPermission = db.UserPermissions
                .Where(gp => gp.RoleId.Equals(roleId))
                .Where(gp => gp.Username.Equals(username))
                .First();
            db.UserPermissions.Remove(personPermission);
            db.SaveChanges();
        }
    }
}
