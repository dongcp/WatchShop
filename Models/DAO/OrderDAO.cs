using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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

        public string Insert(string recipient, string address, string phone)
        {
            Order order = new Order();
            order.Id = GetNewId();
            order.CustomerName = recipient;
            order.Address = address;
            order.PhoneNumber = phone;
            order.CreatedDate = DateTime.Now;
            order.Status = 0;

            db.Orders.Add(order);
            db.SaveChanges();
            return order.Id;
        }

        public void InsertOrderDetail(string orderId, string productId, int quantity)
        {
            OrderDetail detail = new OrderDetail();
            detail.OrderId = orderId;
            detail.Status = 0;
            detail.ProductId = productId;
            detail.Quantity = quantity;

            // Update product quantity
            Product product = db.Products.Find(productId);
            product.Quantity -= quantity;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();

            // Update order total cost
            Order order = db.Orders.Find(orderId);
            double price = product.Discount.Value > 0 ?
                product.Price * product.Discount.Value / 100 : product.Price;
            order.TotalCost += detail.Quantity * price;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            detail.Price = product.Price;
            db.OrderDetails.Add(detail);
            db.SaveChanges();
        }

        public IPagedList<Order> GetAllOrders(int page, int pageSize)
        {
            return db.Orders.OrderBy(o => o.Id).ToPagedList(page, pageSize);
        }

        public List<OrderDetail> GetOrderDetail(string orderId)
        {
            return db.OrderDetails.Where(od => od.OrderId.Equals(orderId))
                .OrderBy(od => od.ProductId).ToList() ;
        }

        public void Delete(string orderId)
        {
            Order order = db.Orders.Find(orderId);
            db.Orders.Remove(order);

            var details = db.OrderDetails.Where(od => od.OrderId.Equals(orderId));
            db.OrderDetails.RemoveRange(details);
            db.SaveChanges();
        }

        private string GetNewId()
        {
            string oldId = db.Orders.Max(o => o.Id);
            if (string.IsNullOrEmpty(oldId)) return ID_PREFIX_BRANCH + "1";
            int newIdSuffix = Int16.Parse(oldId.Substring(ID_PREFIX_BRANCH.Length, oldId.Length - ID_PREFIX_BRANCH.Length)) + 1;
            return ID_PREFIX_BRANCH + newIdSuffix;
        }
    }
}
