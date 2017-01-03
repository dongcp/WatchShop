using Models.DAO;
using Models.EF;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using WatchShop.Common;

namespace WatchShop.Areas.Admin.Models
{
    public class Authority : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            User user = (User)HttpContext.Current.Session[Constants.SESSION_USER];
            if (user == null)
                filterContext.Result = new RedirectResult("~/Admin/State/NoPermission");
            else
            {
                List<Role> roles = RoleDAO.Instance.GetAllPersonalPermissions(user.Username);
                string actionName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "-" +
                    filterContext.ActionDescriptor.ActionName;
                if (!IsIncluded(roles, actionName))
                    filterContext.Result = new RedirectResult("~/Admin/State/NoPermission");
            }
        }

        private bool IsIncluded(List<Role> roles, string actionName)
        {
            for (int i = 0; i < roles.Count; i++)
            {
                if (roles[i].Id.Equals(actionName))
                    return true;
            }
            return false;
        }
    }
}