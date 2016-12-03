using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchShop.Areas.Client.Models;
using Models.DAO;
using Models.EF;
using WatchShop.Common;

namespace WatchShop.Areas.Client.Controllers
{
    public class HomeController : Controller
    {
        // GET: Client/Home
        public ActionResult Index()
        {
            ViewData["branches"] = BranchDAO.Instance.GetAll();
            ViewData["products"] = ProductDAO.Instance.GetEightProductsWithMaxView();
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult CommonLogin(int type)
        {
            @ViewBag.Type = type;
            return View();
        }

        public ActionResult SignIn()
        {
            return RedirectToAction("CommonLogin", new { @type = 1 });
        }

        public ActionResult SignUp()
        {
            return RedirectToAction("CommonLogin", new { @type = 2 });
        }

        public ActionResult SignOut()
        {
            Session[Constants.SESSION_USER] = null;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult IsValidUser(string username, string password)
        {
            User user = UserDAO.Instance.GetByUsername(username);
            LoginJson json = new LoginJson();
            if (user == null)
            {
                json.Error = "fail";
                json.Message = "Tài khoản không tồn tại";
            }
            else if (!user.Password.Equals(password))
            {
                json.Error = "fail";
                json.Message = "Mật khẩu không đúng";
            }
            else
            {
                Session[Constants.SESSION_USER] = user;
                json.Error = "success";
                if (user.UserGroupId.Equals("CUSTOMER"))
                    json.Message = "/Client/Home/Index";
                else
                    json.Message = "/Admin/Home/Index";
            }
            return Json(json);
        }

        [ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            List<CartItem> cart = (List<CartItem>)Session[Constants.SESSION_CART];
            if (cart == null) cart = new List<CartItem>();
            return PartialView(cart);
        }
    }
}