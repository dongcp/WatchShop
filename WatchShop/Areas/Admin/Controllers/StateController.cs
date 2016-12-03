using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WatchShop.Areas.Admin.Controllers
{
    public class StateController : Controller
    {
        // GET: Admin/State
        public ActionResult ShowError(string error)
        {
            return View(error);
        }

        public ActionResult NoRoleLeft()
        {
            return View();
        }
    }
}