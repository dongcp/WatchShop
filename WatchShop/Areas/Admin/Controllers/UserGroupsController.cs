using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Web.Mvc;
using Models.EF;
using Models.DAO;
using WatchShop.Areas.Admin.Models;

namespace WatchShop.Areas.Admin.Controllers
{
    [Authority]
    public class UserGroupsController : Controller
    {
        // GET: Admin/UserGroups
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            return View(UserGroupDAO.Instance.GetAll(page, pageSize));
        }

        // GET: Admin/UserGroups/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserGroup userGroup = UserGroupDAO.Instance.GetById(id);
            if (userGroup == null)
            {
                return HttpNotFound();
            }
            return View(userGroup);
        }

        // GET: Admin/UserGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/UserGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] UserGroup userGroup)
        {
            if (ModelState.IsValid)
            {
                UserGroupDAO.Instance.Insert(userGroup);
                return RedirectToAction("Index");
            }

            return View(userGroup);
        }

        // GET: Admin/UserGroups/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserGroup userGroup = UserGroupDAO.Instance.GetById(id);
            if (userGroup == null)
            {
                return HttpNotFound();
            }
            return View(userGroup);
        }

        // POST: Admin/UserGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] UserGroup userGroup)
        {
            if (ModelState.IsValid)
            {
                UserGroupDAO.Instance.Update(userGroup);
                return RedirectToAction("Index");
            }
            return View(userGroup);
        }

        // GET: Admin/UserGroups/Delete/5
        public ActionResult Delete(string id)
        {
            UserGroupDAO.Instance.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
