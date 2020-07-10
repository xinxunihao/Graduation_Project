using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CYESW.Filter
{
    public class LoginAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (HttpContext.Current.Session["user"] ==null)
            {
                filterContext.Result = new RedirectResult("/Home/Login");
            }
        }
    }
}