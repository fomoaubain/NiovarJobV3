using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace WebApplication1.Models
{
    public class AuthorizedCandidat : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["type"]!= null)
            {
                if (!filterContext.HttpContext.Session["type"].Equals("candidat"))
                {
                    String message = "Vous n'avez pas accès cette page. Cette page est reservés uniquement aux  candidats ou travailleurs (chercheurs d'emploi).";

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"Controller","Home" },  {"Action","ErrorPage"}, {"sms",message}
                });
                base.OnActionExecuting(filterContext);
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"Controller","Inscrires" },  {"Action","Login"}
                });
                base.OnActionExecuting(filterContext);
            }
        }
    }
}