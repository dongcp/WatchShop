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
using WatchShop.Areas.Admin.Models;

namespace WatchShop.Areas.Admin.Controllers
{
    public class RolesController : Controller
    {
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            return View(RoleDAO.Instance.GetAll(page, pageSize));
        }

        // GET: Admin/Roles/Create
        public ActionResult Create()
        {
            List<String> roles = new List<string>();
            List<Type> controllers = ReflectionController.GetControllers("WatchShop.Areas.Admin");
            int numberOfControllers = controllers.Count;
            for (int i = 0; i < numberOfControllers; i++)
            {
                Type controller = controllers[i];
                if (controller.Name != "StateController")
                {
                    List<string> actions = ReflectionController.GetActions(controller);
                    int numberOfActions = actions.Count;
                    for (int j = 0; j < numberOfActions; j++)
                    {
                        string role = controller.Name.Substring(0, controller.Name.Length - 10) + "-" + actions[j];
                        if (!RoleDAO.Instance.IsRoleExisted(role))
                            roles.Add(role);
                    }
                }
            }
            if (roles.Count == 0) return RedirectToAction("NoRoleLeft", "State");
            ViewBag.Roles = roles;
            return View();
        }

        // POST: Admin/Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Role role)
        {
            if (ModelState.IsValid)
            {
                RoleDAO.Instance.Create(role);
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: Admin/Roles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = RoleDAO.Instance.GetById(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Admin/Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Role role)
        {
            if (ModelState.IsValid)
            {
                RoleDAO.Instance.Update(role);
                return RedirectToAction("Index");
            }
            return View(role);
        }

        // GET: Admin/Roles/Delete/5
        public ActionResult Delete(string id)
        {
            RoleDAO.Instance.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult GrantPermission(string id)
        {
            ViewData["users"] = RoleDAO.Instance.GetAllUsersByRoleId(id);
            ViewData["groups"] = RoleDAO.Instance.GetAllGroupsByRoleId(id);
            Role role = RoleDAO.Instance.GetById(id);
            return View(role);
        }

        public ActionResult Groups(string roleId)
        {
            ViewData["RoleId"] = roleId;
            return View(RoleDAO.Instance.GetAllRemainingGroupsByRoleId(roleId));
        }

        public ActionResult Users(string roleId)
        {
            ViewData["RoleId"] = roleId;
            return View(RoleDAO.Instance.GetAllRemainingUsersByRoleId(roleId));
        }

        public string GrantUserPermission(string username, string roleId)
        {
            RoleDAO.Instance.GrantUserPermission(username, roleId);
            return "success";
        }

        public string GrantGroupPermission(string groupId, string roleId)
        {
            RoleDAO.Instance.GrantGroupPermission(groupId, roleId);
            return "success";
        }
    }
}
