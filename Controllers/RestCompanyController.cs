using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using X.PagedList;

namespace WebApplication1.Controllers
{
    public class RestCompanyController : MybaseController
    { 
        public JsonResult AproposHome(decimal id)
        {
            Page page = db.Page.Where(p => p.Ins_id == id && p.archived == 1).FirstOrDefault();
            if (page != null)
            {
                Inscrire inscrire = db.Inscrire.Where(p => p.id == page.Ins_id && p.archived == 1).FirstOrDefault(); 
                if(inscrire == null) return Json(new { result_code = -1, message = "Company not found" }, JsonRequestBehavior.AllowGet);
                return Json(new { result_code = 1, Page = getJson(page) }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result_code = -1, message= "Company page not found" }, JsonRequestBehavior.AllowGet);
         }

        public JsonResult PourquoiPostulerHome(decimal id)
        {
            Page page = db.Page.Where(p => p.Ins_id == id && p.archived == 1).FirstOrDefault();
            if (page != null)
            {
                Inscrire inscrire = db.Inscrire.Where(p => p.id == page.Ins_id && p.archived == 1).FirstOrDefault();
                if (inscrire == null) return Json(new { result_code = -1, message = "Company not found" }, JsonRequestBehavior.AllowGet);
                return Json(new { result_code = 1, Page = getJson(page) }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result_code = -1, message = "Company page not found" }, JsonRequestBehavior.AllowGet);
         }

        public JsonResult MaGalerieHome(decimal id)
        {

            Page page = db.Page.Where(p => p.Ins_id == id && p.archived == 1).FirstOrDefault();
            if (page != null)
            {
                int pages = !string.IsNullOrEmpty(Request["page"]) ? Request["page"].AsInt() : 1;
                int limit = 10;
                Inscrire inscrire = db.Inscrire.Where(p => p.id == page.Ins_id && p.archived == 1).FirstOrDefault();
                if (inscrire == null) return Json(new { result_code = -1, message = "Company not found" }, JsonRequestBehavior.AllowGet);
                List<Galerie> listGalerie = db.Galerie.Where(p => p.Pag_id == page.id && p.archived == 1).OrderByDescending(p => p.created).ToList();
                var count = listGalerie.Count();
                List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
                foreach (var item in listGalerie.OrderByDescending(p => p.created).ToPagedList(pages, limit)) datas.Add(getJson(item));
                return Json(new { result_code = 1, page = pages, count = count, limit = limit, Page = getJson(page), galeries = datas }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result_code = -1, message = "Company page not found" }, JsonRequestBehavior.AllowGet);
         }


        public JsonResult MesAvisHome(decimal id)
        {


            Page page = db.Page.Where(p => p.Ins_id == id && p.archived == 1).FirstOrDefault();
            if (page != null)
            {
                Inscrire inscrire = db.Inscrire.Where(p => p.id == page.Ins_id && p.archived == 1).FirstOrDefault();

                if (inscrire == null) return Json(new { result_code = -1, message = "Company not found" }, JsonRequestBehavior.AllowGet);
                int pages = !string.IsNullOrEmpty(Request["page"]) ? Request["page"].AsInt() : 1;
                int limit = 10;

                List<Avis> listAvis = db.Avis.Where(p => p.Pag_id == page.id && p.archived == 1 && p.status == 1).OrderByDescending(p => p.created).ToList();
                var count = listAvis.Count(); 
                List<Dictionary<string, Object>> avis = new List<Dictionary<string, Object>>();
                foreach (var item in listAvis.OrderByDescending(p => p.created).ToPagedList(pages, limit)) 
                { 
                    Inscrire insc = db.Inscrire.Where(p => p.id == item.iduser).FirstOrDefault();
                    if (insc != null)
                    {
                        item.Pseudo = insc.login;
                        item.Profil = insc.profil;
                    }
                    avis.Add(getJson(item)); 
                }

                  
                return Json(new { result_code = 1, page = pages, count = count, limit = limit, Page = getJson(page), avis = avis }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result_code = -1, message = "Company page not found" }, JsonRequestBehavior.AllowGet);

        }

        [Authorize] 
        [HttpPost] 
        public JsonResult addAvis(Avis avis)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            Page currentPage = db.Page.Where(p => p.id == avis.Pag_id && p.archived == 1).FirstOrDefault();
            if (currentPage != null)
            {
                avis.iduser = Convert.ToInt32(_me.id);
                avis.archived = 1;
                avis.created = DateTime.Now;
                avis.status = 0;
                avis.Pseudo = _me.login;
                avis.Profil = _me.profil;
                avis.Pag_id = currentPage.id;
                db.Avis.Add(avis);
                db.SaveChanges();  
                return Json(new { result_code = 1, item = getJson(avis) }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result_code = -1, message = "Company page not found" }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult MyPageOffre(decimal id)
        {
            Page page = db.Page.Where(p => p.Ins_id == id && p.archived == 1).FirstOrDefault();
            if (page != null)
            {
                Inscrire inscrire = db.Inscrire.Where(p => p.id == page.Ins_id && p.archived == 1).FirstOrDefault();

                if (inscrire == null) return Json(new { result_code = -1, message = "Company not found" }, JsonRequestBehavior.AllowGet);
                int pages = !string.IsNullOrEmpty(Request["page"]) ? Request["page"].AsInt() : 1;
                int limit = 10;

                List<Postuler> listNosOffres = db.Postuler.Where(p => p.Job.archived == 1 && p.Inscrire.type == "client" && p.Job.status == 1 && p.Ins_id == inscrire.id && p.approbation.Equals("1") && p.Job.immediat.Equals("false")).OrderByDescending(p => p.Job.created).ToList();
                var count = listNosOffres.Count();
                List<Dictionary<string, Object>> offres = new List<Dictionary<string, Object>>();
                foreach (var item in listNosOffres.OrderByDescending(p => p.created).ToPagedList(pages, limit)) offres.Add(getJson(item));

                return Json(new { result_code = 1, page = pages, count = count, limit = limit, Page = getJson(page), offres = offres }, JsonRequestBehavior.AllowGet);


            }
            return Json(new { result_code = -1, message = "Company page not found" }, JsonRequestBehavior.AllowGet);

        }
    }
}
