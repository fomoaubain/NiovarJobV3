using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [CustomAuthorized]
    public class InsAbonnesController : MybaseController
    {
        //private NiovarJobEntities db = new NiovarJobEntities();
        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN, Constante.roleGESTIONNAIRE })]
        // GET: InsAbonnes
        public ActionResult Index()
        {
            var from = Request["from"];
            var to = Request["to"];
            var status = String.IsNullOrEmpty(Request["status"])? -1 : Request["status"].AsInt();
            var type = Request["type"].AsInt();
            var insc = Request["insc"].AsInt();
            var datas = db.InsAbonne.Where(a => a.archived == 1);
            if (status >= 0 && status <= 1) datas = datas.Where(a => a.status == status);
            if (type > 0) datas = datas.Where(a => a.Abo_id == type);
            if (insc > 0) datas = datas.Where(a => a.Ins_id == insc);
            if (!String.IsNullOrEmpty(from)) datas = datas.Where(a => DateTime.Parse(a.dateDebut) >= DateTime.Parse(from));
            if (!String.IsNullOrEmpty(to)) datas = datas.Where(a => DateTime.Parse(a.dateFin) <= DateTime.Parse(to));
            datas = datas.OrderByDescending(a => a.id).Include(f => f.Abonnement).Include(l => l.Inscrire);
            
            ViewBag.typeAbonnement = db.Abonnement.Where(a => a.archived == 1).ToList();
            ViewBag.inscrires = db.Inscrire.Where(a => a.archived == 1).ToList();

            return View(datas.ToList());
        }

        // GET: InsAbonnes/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                InsAbonne insAbonne = db.InsAbonne.SingleOrDefault(m => m.id == id);
                //InsAbonne insAbonne = db.InsAbonne.Find(id);
                if (insAbonne == null)
                {
                    return HttpNotFound();
                }
                return View(insAbonne);
            }
            catch (Exception e)
            {
                return HttpNotFound();

            }
        }

        // GET: InsAbonnes/Create
        public ActionResult Create()
        { 
            return View();
        }

        // POST: InsAbonnes/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ins_id,Abo_id,id,libelle,dateDebut,dateFin,etat,archived,status,created")] InsAbonne insAbonne)
        {
            if (ModelState.IsValid)
            {
                db.InsAbonne.Add(insAbonne);
                db.SaveChanges();
                TempData["result_code"] = 1;
                TempData["message"] = "Ajout effectué avec succès.";
                TempData.Keep();
                return RedirectToAction("Index");
            }
            TempData["result_code"] = -1;
            TempData["message"] = "Données non valide.";
            TempData.Keep();
            return View(insAbonne);
        }

        // GET: InsAbonnes/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                InsAbonne insAbonne = db.InsAbonne.SingleOrDefault(m => m.id == id);
                //InsAbonne insAbonne = db.InsAbonne.Find(id);
                if (insAbonne == null)
                {
                    return HttpNotFound();
                }
                return View(insAbonne);
            }
            catch (Exception e)
            {
                return HttpNotFound();

            }
        }

        // POST: InsAbonnes/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,libelle,dateDebut,dateFin")] InsAbonne insAbonne)
        {
            try
            {
                InsAbonne item = db.InsAbonne.SingleOrDefault(m => m.id == insAbonne.id);
                //InsAbonne insAbonne = db.InsAbonne.Find(id);
                if (item == null)
                {
                    return HttpNotFound();
                }
                item.libelle = insAbonne.libelle;
                item.dateDebut = insAbonne.dateDebut;
                item.dateFin = insAbonne.dateFin;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                TempData["result_code"] = 1;
                TempData["message"] = "Modifications effectué avec succès.";
                TempData.Keep();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                throw;
                TempData["result_code"] = -1;
                TempData["message"] = "Données non valide.";
                TempData.Keep();
                return View(insAbonne);

            }
        }

        // GET: InsAbonnes/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                InsAbonne insAbonne = db.InsAbonne.SingleOrDefault(m => m.id == id);
                //InsAbonne insAbonne = db.InsAbonne.Find(id);
                if (insAbonne == null)
                {
                    return HttpNotFound();
                }
                return View(insAbonne);
            }
            catch (Exception e)
            {
                return HttpNotFound();

            } 
        }

        // POST: InsAbonnes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            try
            {
                InsAbonne insAbonne = db.InsAbonne.SingleOrDefault(m => m.id == id);

                //InsAbonne insAbonne = db.InsAbonne.Find(id);
                insAbonne.archived = 0;
                db.Entry(insAbonne).State = EntityState.Modified;
                db.SaveChanges();
                TempData["result_code"] = 1;
                TempData["message"] = "Abonnement supprimé avec succès.";
                TempData.Keep();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return HttpNotFound();

            }
        }

        [CustomAuthorized]
        public ActionResult EditStatus(decimal id)
        {

            try
            {
                var item = db.InsAbonne.FirstOrDefault(p => p.id == id);
                if (item == null)
                {
                    TempData["result_code"] = -1;
                    TempData["message"] = "Cet abonnement n'existe pas !";
                    TempData.Keep();
                    return RedirectToAction("Index");
                }

                if (item.status == 1) item.status = 0;
                else item.status = 1;

                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                TempData["result_code"] = 1;
                TempData["message"] = "Statut modifié avec succès.";
                TempData.Keep();

                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                TempData["result_code"] = -1;
                TempData["message"] = "Une erreur innatendu s'et produite.";
                TempData.Keep();
                return RedirectToAction("Index");

            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
