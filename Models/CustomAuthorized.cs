using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1.Models
{
    public class CustomAuthorized : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["emailAdmin"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"Controller","Utilisateurs" },  {"Action","LoginAdmin"}
                });
                base.OnActionExecuting(filterContext);
             }
         }
    }
}