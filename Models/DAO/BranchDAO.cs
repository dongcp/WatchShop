using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class BranchDAO
    {
        private const string ID_PREFIX_BRANCH = "BR";

        private WatchShopEntities db;

        private static BranchDAO instance;
        public static BranchDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new BranchDAO();
                return instance;
            }
        }

        private BranchDAO()
        {
            db = CommonDAO.Instance.Database;
        }

        public void Insert(Branch branch)
        {
            branch.Id = GetNewId();
            db.Branches.Add(branch);
            db.SaveChanges();
        }

        public void Update(Branch branch)
        {
            Branch foundBranch = db.Branches.Find(branch.Id);
            foundBranch.Name = branch.Name;
            db.Entry(foundBranch).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(string id)
        {
            Branch branch = db.Branches.Find(id);
            db.Branches.Remove(branch);
            db.SaveChanges();
        }

        public List<Branch> GetAll()
        {
            return db.Branches.ToList();
        }

        public Branch GetById(string id)
        {
            return db.Branches.SingleOrDefault(br => br.Id.Equals(id));
        }

        private string GetNewId()
        {
            string oldId = db.Branches.Max(p => p.Id);
            if (oldId == null || oldId.Equals("")) return ID_PREFIX_BRANCH + "1";
            int newIdSuffix = Int16.Parse(oldId.Substring(ID_PREFIX_BRANCH.Length, oldId.Length - ID_PREFIX_BRANCH.Length)) + 1;
            return ID_PREFIX_BRANCH + newIdSuffix;
        }
    }
}
