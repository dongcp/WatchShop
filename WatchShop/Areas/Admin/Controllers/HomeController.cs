using System.Web.Mvc;

namespace WatchShop.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Products", "Admin");
        }
    }
}