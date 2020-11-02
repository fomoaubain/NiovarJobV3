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
    public class AbonnementsController : MybaseController
    {
        //private NiovarJobEntities db = new NiovarJobEntities();

        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN, Constante.roleGESTIONNAIRE })]
        // GET: Abonnements
        public ActionResult Index()
        {
            return View(db.Abonnement.Where(a => a.archived == 1).ToList());
        }
         

        // GET: Abonnements/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonnement abonnement = db.Abonnement.Find(id);
            if (abonnement == null)
            {
                return HttpNotFound();
            }
            return View(abonnement);
        }

        // GET: Abonnements/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Abonnements/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,type,titre,description,amount,nbrePost,illimite")] Abonnement abonnement)
        {
            if (ModelState.IsValid)
            {
                abonnement.status = 0;
                abonnement.montant = (Double) abonnement.amount;
                abonnement.archived = 1;
                abonnement.created = DateTime.Now;
                db.Abonnement.Add(abonnement);
                db.SaveChanges();
                TempData["result_code"] = 1;
                TempData["message"] = "Ajout effectué avec succès.";
                TempData.Keep();
                return RedirectToAction("Index");
            }

            TempData["result_code"] = -1;
            TempData["message"] = "Données non valide.";
            TempData.Keep();
            return View(abonnement);
        }

        // GET: Abonnements/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonnement abonnement = db.Abonnement.Find(id);
            if (abonnement == null)
            {
                return HttpNotFound();
            }
            return View(abonnement);
        }

        // POST: Abonnements/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,type,titre,description,amount,nbrePost,illimite")] Abonnement abonnement)
        {

            try
            {
                Abonnement item = db.Abonnement.SingleOrDefault(m => m.id == abonnement.id);
                //InsAbonne insAbonne = db.InsAbonne.Find(id);
                if (item == null) return HttpNotFound(); 
                item.type = abonnement.type;
                item.titre = abonnement.titre;
                item.description = abonnement.description;
                item.montant = (Double) abonnement.amount;
                //item.montant = abonnement.montant;
                item.nbrePost = abonnement.nbrePost;
                item.illimite = abonnement.illimite;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                TempData["result_code"] = 1;
                TempData["message"] = "Modifications effectué avec succès.";
                TempData.Keep();
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                TempData["result_code"] = -1;
                TempData["message"] = "Données non valide.";
                TempData.Keep();
                return View(abonnement);

            } 
        }

        // GET: Abonnements/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abonnement abonnement = db.Abonnement.Find(id);
            if (abonnement == null)
            {
                return HttpNotFound();
            }
            return View(abonnement);
        }

        // POST: Abonnements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Abonnement abonnement = db.Abonnement.Find(id);
            abonnement.archived = 0;
            db.Entry(abonnement).State = EntityState.Modified;
            db.SaveChanges();
            TempData["result_code"] = 1;
            TempData["message"] = "type d'abonnement supprimé avec succès.";
            TempData.Keep();
            return RedirectToAction("Index");
        }

        [CustomAuthorized]
        public ActionResult EditStatus(decimal id)
        {

            try
            {
                var item = db.Abonnement.FirstOrDefault(p => p.id == id);
                if (item == null)
                {
                    TempData["result_code"] = -1;
                    TempData["message"] = "Ce type d'abonnement n'existe pas !";
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
