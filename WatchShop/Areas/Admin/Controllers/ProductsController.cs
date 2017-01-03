using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models.EF;
using Models.DAO;
using System.IO;
using WatchShop.Common;
using WatchShop.Areas.Admin.Models;

namespace WatchShop.Areas.Admin.Controllers
{
    [Authority]
    public class ProductsController : Controller
    {
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            return View(ProductDAO.Instance.GetAllProducts(page, pageSize));
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = ProductDAO.Instance.GetProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            List<Branch> branches = BranchDAO.Instance.GetAll();
            ViewData["branches"] = branches;
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product product, HttpPostedFileBase image, IEnumerable<HttpPostedFileBase> images)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.ContentLength > 0)
                {
                    image.SaveAs(Path.Combine(Server.MapPath(Constants.IMAGE_PATH), image.FileName));
                    product.Image = image.FileName;
                }
                ProductDAO.Instance.Insert(product, ((User)Session[Constants.SESSION_USER]).Username);
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = ProductDAO.Instance.GetProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            if (!product.Username.Equals(((User)Session[Constants.SESSION_USER]).Username))
                return RedirectToAction("Warning", "Home", "Admin");
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Discount,Description,Image,Images,View,Username")] Product product)
        {
            if (ModelState.IsValid)
            {
                ProductDAO.Instance.Update(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(string id)
        {
            ProductDAO.Instance.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
