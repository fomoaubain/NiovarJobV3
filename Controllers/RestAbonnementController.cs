using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.WebPages;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class RestAbonnementController : MybaseController
    {

        [Authorize]
        public JsonResult AbonnementList() 
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
             
            var list = db.InsAbonne.Where(a => a.Ins_id == _me.id && a.archived == 1).OrderByDescending(a => a.id).Include(f => f.Abonnement).Include(l => l.Inscrire);

            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in list) datas.Add(getJson(item)); 
            return Json(new { result_code = 1, datas = datas }, JsonRequestBehavior.AllowGet);
        }

    }
}
