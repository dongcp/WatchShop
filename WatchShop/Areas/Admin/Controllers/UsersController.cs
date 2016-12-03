using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models.EF;
using Models.DAO;
using WatchShop.Common;

namespace WatchShop.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private WatchShopEntities db = new WatchShopEntities();

        // GET: Admin/Users
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            return View(UserDAO.Instance.GetAll(page, pageSize));
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details(string username)
        {
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = UserDAO.Instance.GetByUsername(username);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            List<UserGroup> userGroups = UserGroupDAO.Instance.GetAll();
            ViewBag.UserGroups = userGroups;
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password,Name,Birthday,Address,PhoneNumber,UserGroupId")] User user)
        {
            if (ModelState.IsValid)
            {
                UserDAO.Instance.Insert(user);
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(string username)
        {
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = UserDAO.Instance.GetByUsername(username);
            if (user == null)
            {
                return HttpNotFound();
            }
            List<UserGroup> userGroups = UserGroupDAO.Instance.GetAll();
            ViewBag.UserGroups = userGroups;
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Username,Password,Name,Birthday,Address,PhoneNumber,UserGroupId")] User user)
        {
            if (ModelState.IsValid)
            {
                UserDAO.Instance.Update(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(string username)
        {
            UserDAO.Instance.Delete(username);
            return RedirectToAction("Index");
        }

        public ActionResult UserInfo()
        {
            if (Session[Constants.SESSION_USER] != null)
            {
                User user = (User)Session[Constants.SESSION_USER];
                return View(user);
            }
            return RedirectToAction("Index", "Home", "Client");
        }

        public ActionResult PersonalPermission(int page = 1, int pageSize = 10)
        {
            if (Session[Constants.SESSION_USER] != null)
            {
                User user = (User)Session[Constants.SESSION_USER];
                return View(RoleDAO.Instance.GetAllPersonalPermissions(user.Username, page, pageSize));
            }
            return RedirectToAction("Index", "Home", "Client");
        }
    }
}
