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
    [CustomAuthorized]
    public class NiveauLanguesController : MybaseController
    {
        private NiovarJobEntities db = new NiovarJobEntities();

        // GET: NiveauLangues
        public ActionResult Index()
        {
            return View(db.NiveauLangue.Where(m => m.archived == 1).ToList());
        }

        // GET: NiveauLangues/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NiveauLangue niveauLangue = db.NiveauLangue.Find(id);
            if (niveauLangue == null)
            {
                return HttpNotFound();
            }
            return View(niveauLangue);
        }

        // GET: NiveauLangues/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NiveauLangues/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( NiveauLangue niveauLangue)
        {
            if (ModelState.IsValid)
            {
                niveauLangue.archived = 1;
                niveauLangue.status = 1;
                niveauLangue.created = DateTime.Now;
                db.NiveauLangue.Add(niveauLangue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(niveauLangue);
        }

        // GET: NiveauLangues/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NiveauLangue niveauLangue = db.NiveauLangue.Find(id);
            if (niveauLangue == null)
            {
                return HttpNotFound();
            }
            return View(niveauLangue);
        }

        // POST: NiveauLangues/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NiveauLangue niveauLangue)
        {
            var currentNiveauLangue = db.NiveauLangue.FirstOrDefault(p => p.id == niveauLangue.id);
            if (currentNiveauLangue == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                currentNiveauLangue.libelle = niveauLangue.libelle;
                db.Entry(currentNiveauLangue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(niveauLangue);
        }

        // GET: NiveauLangues/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NiveauLangue niveauLangue = db.NiveauLangue.Find(id);
            if (niveauLangue == null)
            {
                return HttpNotFound();
            }
            return View(niveauLangue);
        }

        // POST: NiveauLangues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            NiveauLangue niveauLangue = db.NiveauLangue.Find(id);
            niveauLangue.archived = 0;
            db.Entry(niveauLangue).State = EntityState.Modified;
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
    }
}
