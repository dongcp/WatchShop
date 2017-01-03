using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PagedList;

namespace Models.DAO
{
    public class ProductDAO
    {
        public const int FILTER_TYPE_ALL = -1;
        public const int FILTER_TYPE_MEN = 1;
        public const int FILTER_TYPE_WOMEN = 2;

        private const string ID_PREFIX_PRODUCT = "PRO";

        private WatchShopEntities db;

        private static ProductDAO instance;
        public static ProductDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new ProductDAO();
                return instance;
            }
        }

        private ProductDAO()
        {
            db = CommonDAO.Instance.Database;
        }

        public void Insert(Product product, string username)
        {
            product.Id = GetNewId(ID_PREFIX_PRODUCT);
            product.View = 0;
            product.Username = username;
            db.Products.Add(product);
            db.SaveChanges();
        }

        public bool Update(Product product)
        {
            Product foundProduct = db.Products.SingleOrDefault(p => p.Id.Equals(product.Id));
            if (foundProduct == null) throw new Exception("Không tìm thấy sản phẩm!");
            db.Entry(foundProduct).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }

        public bool Delete(string id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return true;
        }

        public List<Product> GetAllProducts()
        {
            return db.Products.OrderBy(p => p.Id).ToList();
        }

        public IPagedList<Product> GetAllProducts(int page, int pageSize)
        {
            return db.Products.OrderBy(p => p.Id).ToPagedList(page, pageSize);
        }

        public Product GetProductById(string id)
        {
            return db.Products.SingleOrDefault(p => p.Id.Equals(id));
        }

        public List<Product> GetEightProductsWithMaxView()
        {
            return db.Products.OrderByDescending(p => p.View).Take(8).ToList();
        }

        public IEnumerable<Product> GetAllByFilter(
            string branchFilter, string priceFilter, string discountFilter, int typeFilter, int page, int pageSize)
        {
            if (string.IsNullOrEmpty(branchFilter))
            {
                branchFilter = "";
            }
            string[] branchesId = branchFilter.Trim().Split(' ');
            double minPrice;
            double maxPrice;
            int discount;
            switch (priceFilter)
            {
                case "price1":
                    minPrice = 0f;
                    maxPrice = 1000000f;
                    break;
                case "price2":
                    minPrice = 1000000f;
                    maxPrice = 3000000f;
                    break;
                case "price3":
                    minPrice = 3000000f;
                    maxPrice = 5000000f;
                    break;
                case "price4":
                    minPrice = 5000000f;
                    maxPrice = 0f;
                    break;
                default:
                    minPrice = maxPrice = 0f;
                    break;
            }
            switch (discountFilter)
            {
                case "discount1":
                    discount = 10;
                    break;
                case "discount2":
                    discount = 20;
                    break;
                case "discount3":
                    discount = 30;
                    break;
                case "discount4":
                    discount = 40;
                    break;
                default:
                    discount = 0;
                    break;
            }
            List<Product> products = new List<Product>();
            for (int i = 0; i < branchesId.Length; i++)
            {
                string branchId = branchesId[i];
                List<Product> tmp;
                if (minPrice >= maxPrice)
                {
                    if (string.IsNullOrEmpty(branchId))
                    {
                        if (typeFilter == FILTER_TYPE_ALL)
                            tmp = db.Products
                                .Where(p => p.Price >= minPrice)
                                .Where(p => p.Discount >= discount)
                                .ToList();
                        else
                            tmp = db.Products
                            .Where(p => p.Price >= minPrice)
                            .Where(p => p.Discount >= discount)
                            .Where(p => p.Type == typeFilter)
                            .ToList();
                    }
                    else
                    {
                        if (typeFilter == FILTER_TYPE_ALL)
                            tmp = db.Products
                            .Where(p => p.BranchId.Equals(branchId))
                            .Where(p => p.Price >= minPrice)
                            .Where(p => p.Discount >= discount)
                            .ToList();
                        else
                            tmp = db.Products
                            .Where(p => p.BranchId.Equals(branchId))
                            .Where(p => p.Price >= minPrice)
                            .Where(p => p.Discount >= discount)
                            .Where(p => p.Type == typeFilter)
                            .ToList();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(branchId))
                    {
                        if (typeFilter == FILTER_TYPE_ALL)
                            tmp = db.Products
                                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                                .Where(p => p.Discount >= discount)
                                .ToList();
                        else
                            tmp = db.Products
                            .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                            .Where(p => p.Discount >= discount)
                            .Where(p => p.Type == typeFilter)
                            .ToList();
                    }
                    else
                    {
                        if (typeFilter == FILTER_TYPE_ALL)
                            tmp = db.Products
                                .Where(p => p.BranchId.Equals(branchId))
                                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                                .Where(p => p.Discount >= discount)
                                .ToList();
                        else
                            tmp = db.Products
                           .Where(p => p.BranchId.Equals(branchId))
                           .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                           .Where(p => p.Discount >= discount)
                            .Where(p => p.Type == typeFilter)
                           .ToList();
                    }
                }
                products.AddRange(tmp);
            }
            return products.ToPagedList(page, pageSize);
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
