using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PaiementsController : MybaseController
    {
        //private NiovarJobEntities db = new NiovarJobEntities();

        // GET: Paiements
        public ActionResult Index()
        {
            var paiement = db.Paiement.Include(p => p.Postuler.Job).Include(p => p.Postuler);
            return View(paiement.ToList());
        }

        // GET: Paiements/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paiement paiement = db.Paiement.Find(id);
            if (paiement == null)
            {
                return HttpNotFound();
            }
            return View(paiement);
        }

        // GET: Paiements/Create
        public ActionResult Create()
        {
            ViewBag.Job_id = new SelectList(db.Job, "id", "titre");
            ViewBag.Ins_id = new SelectList(db.Postuler, "Ins_id", "libelle");
            return View();
        }

        // POST: Paiements/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Job_id,libelle,montant,avance,reste,type,etat,archived,status,created,Ins_id,Pos_id")] Paiement paiement)
        {
            if (ModelState.IsValid)
            {
                db.Paiement.Add(paiement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Job_id = new SelectList(db.Job, "id", "titre", paiement.Job_id);
            ViewBag.Ins_id = new SelectList(db.Postuler, "Ins_id", "libelle", paiement.Ins_id);
            return View(paiement);
        }

        // GET: Paiements/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paiement paiement = db.Paiement.Find(id);
            if (paiement == null)
            {
                return HttpNotFound();
            }
            ViewBag.Job_id = new SelectList(db.Job, "id", "titre", paiement.Job_id);
            ViewBag.Ins_id = new SelectList(db.Postuler, "Ins_id", "libelle", paiement.Ins_id);
            return View(paiement);
        }

        // POST: Paiements/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Job_id,libelle,montant,avance,reste,type,etat,archived,status,created,Ins_id,Pos_id")] Paiement paiement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paiement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Job_id = new SelectList(db.Job, "id", "titre", paiement.Job_id);
            ViewBag.Ins_id = new SelectList(db.Postuler, "Ins_id", "libelle", paiement.Ins_id);
            return View(paiement);
        }

        // GET: Paiements/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paiement paiement = db.Paiement.Find(id);
            if (paiement == null)
            {
                return HttpNotFound();
            }
            return View(paiement);
        }

        // POST: Paiements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Paiement paiement = db.Paiement.Find(id);
            db.Paiement.Remove(paiement);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize]
        [AuthorizedClient]
        public ActionResult ListPaiementUser()
        {

            if (Session["id"] != null)
            {
                decimal id = Convert.ToDecimal(Session["id"]);

                List<Paiement> listPaiement = db.Paiement.Where(p => p.userId == id).OrderByDescending(p => p.created).ToList();
                ViewBag.listPaiement = listPaiement;

                List<FraisLocation> listFraisLocation = db.FraisLocation.Where(p => p.userId == id).OrderByDescending(p => p.created).ToList();
                ViewBag.listFraisLocation = listFraisLocation;

                return View();
            }
            else
            {
                return HttpNotFound();
            }
          
        }
    }
}
