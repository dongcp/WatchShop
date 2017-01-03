using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using Models.EF;
using Models.DAO;
using WatchShop.Areas.Admin.Models;

namespace WatchShop.Areas.Admin.Controllers
{
    [Authority]
    public class OrdersController : Controller
    {
        private WatchShopEntities db = new WatchShopEntities();

        // GET: Admin/Orders
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            return View(OrderDAO.Instance.GetAllOrders(page, pageSize));
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(string id)
        {
            return View(OrderDAO.Instance.GetOrderDetail(id));
        }

        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreatedDate,TotalCost,Status,CustomerId,CustomerName,Address,PhoneNumber")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public ActionResult Delete(string id)
        {
            OrderDAO.Instance.Delete(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
