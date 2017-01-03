using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WatchShop.Common;

namespace WatchShop.Areas.Client.Controllers
{
    public class ProductsController : Controller
    {

        public ActionResult Index(IEnumerable<Product> products, int page = 1, int pageSize = 15)
        {
            if (products == null)
            {
                string branchFilter = GetAllBranchFilter();
                string priceFilter = GetPriceFilter();
                string discountFilter = GetDiscountFilter();
                int typeFilter = GetTypeFilter();
                products = ProductDAO.Instance
                    .GetAllByFilter(branchFilter, priceFilter, discountFilter, typeFilter, page, pageSize);
            }
            if (products == null || products.Count() == 0) return RedirectToAction("NoProduct");
            ViewData["branches"] = BranchDAO.Instance.GetAll();
            return View(products);
        }

        public ActionResult NoProduct()
        {
            ViewData["branches"] = BranchDAO.Instance.GetAll();
            return View();
        }

        public string Filter(string branchFilter, string priceFilter, string discountFilter)
        {
            if (string.IsNullOrEmpty(branchFilter))
            {
                Session[Constants.SESSION_FILTER_BRANCH] = null;
            }
            else
            {
                Session[Constants.SESSION_FILTER_BRANCH] = branchFilter;
            }
            Session[Constants.SESSION_FILTER_PRICE] = priceFilter;
            Session[Constants.SESSION_FILTER_DISCOUNT] = discountFilter;
            return "success";
        }

        public ActionResult TypeFilter(int typeFilter)
        {
            Session[Constants.SESSION_FILTER_TYPE] = typeFilter;
            return RedirectToAction("Index");
        }

        public ActionResult Detail(string id)
        {
            Product product = ProductDAO.Instance.GetProductById(id);
            return View(product);
        }

        public string GetAllBranchFilter()
        {
            if (Session[Constants.SESSION_FILTER_BRANCH] == null) return null;
            return Session[Constants.SESSION_FILTER_BRANCH].ToString();
        }

        public string GetPriceFilter()
        {
            if (Session[Constants.SESSION_FILTER_PRICE] == null) return "price0";
            return Session[Constants.SESSION_FILTER_PRICE].ToString();
        }

        public string GetDiscountFilter()
        {
            if (Session[Constants.SESSION_FILTER_DISCOUNT] == null) return "discount0";
            return Session[Constants.SESSION_FILTER_DISCOUNT].ToString();
        }

        public int GetTypeFilter()
        {
            if (Session[Constants.SESSION_FILTER_TYPE] == null) return ProductDAO.FILTER_TYPE_ALL;
            return Int16.Parse(Session[Constants.SESSION_FILTER_TYPE].ToString());
        }
    }
}