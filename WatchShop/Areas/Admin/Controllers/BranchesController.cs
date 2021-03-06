﻿using System.Net;
using System.Web.Mvc;
using Models.EF;
using Models.DAO;
using WatchShop.Areas.Admin.Models;

namespace WatchShop.Areas.Admin.Controllers
{
    [Authority]
    public class BranchesController : Controller
    {
        // GET: Admin/Branches
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            return View(BranchDAO.Instance.GetAll(page, pageSize));
        }

        // GET: Admin/Branches/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = BranchDAO.Instance.GetById(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        // GET: Admin/Branches/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Branches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                BranchDAO.Instance.Insert(branch);
                return RedirectToAction("Index");
            }

            return View(branch);
        }

        // GET: Admin/Branches/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = BranchDAO.Instance.GetById(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        // POST: Admin/Branches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                BranchDAO.Instance.Update(branch);
                return RedirectToAction("Index");
            }
            return View(branch);
        }

        // GET: Admin/Branches/Delete/5
        public ActionResult Delete(string id)
        {
            BranchDAO.Instance.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
