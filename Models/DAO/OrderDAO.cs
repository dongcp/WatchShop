using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class OrderDAO
    {
        private const string ID_PREFIX_BRANCH = "ORD";

        private WatchShopEntities db;

        private static OrderDAO instance;
        public static OrderDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new OrderDAO();
                return instance;
            }
        }

        private OrderDAO()
        {
            db = CommonDAO.Instance.Database;
        }

        public void Insert(Order order)
        {
            order.Id = GetNewId();
            order.CreatedDate = DateTime.Now;
            db.Orders.Add(order);
            db.SaveChanges();
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
