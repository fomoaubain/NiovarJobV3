using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace WebApplication1.Models
{
    public class AuthorizedAccessAction  : ActionFilterAttribute
    {
        public string[] roleAccess { get; set; }
       



        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["emailAdmin"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"Controller","Utilisateurs" },  {"Action","LoginAdmin"}
                });
                base.OnActionExecuting(filterContext);
            }
            else
            {
                Boolean exist = false;
                for (int i = 0; i < roleAccess.Length; i++)
                {
                    if (roleAccess[i].Equals(filterContext.HttpContext.Session["role"]))
                    {
                        exist = true;
                        break;
                    }
                    
                }

                if (!exist)
                {
                    var values = new RouteValueDictionary(new
                    {
                        action = "ErrorPage",
                        controller = "Home",
                        sms = "Désolé vous n'avez les privilèges requis pour éffectué cette tâches"
                    });

                    //String message = "Vous n'avez pas les droits d'accès a cette page";
                    filterContext.Result = new RedirectToRouteResult(values);
                   
                }
                base.OnActionExecuting(filterContext);
            }

           
        }
    }
}