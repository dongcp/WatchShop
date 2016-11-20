using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WatchShop.Common;

namespace WatchShop.Areas.Client.Controllers
{
    public class ProductsController : Controller
    {
        public ActionResult Index(IEnumerable<Product> products, int page = 1, int pageSize = 15)
        {
            if (products == null)
            {
                string branchFilter = Session[Constants.SESSION_FILTER_BRANCH] == null
                    ? "" : Session[Constants.SESSION_FILTER_BRANCH].ToString();
                string priceFilter = Session[Constants.SESSION_FILTER_PRICE] == null ?
                    "price0" : Session[Constants.SESSION_FILTER_PRICE].ToString();
                string discountFilter = Session[Constants.SESSION_FILTER_DISCOUNT] == null ?
                    "discount0" : Session[Constants.SESSION_FILTER_DISCOUNT].ToString();
                products = ProductDAO.Instance
                    .GetAllByFilter(branchFilter, priceFilter, discountFilter, page, pageSize);
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
    }
}