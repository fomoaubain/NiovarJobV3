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
    public class NiveauScolaritesController : MybaseController
    {
        private NiovarJobEntities db = new NiovarJobEntities();

        // GET: NiveauScolarites
        public ActionResult Index()
        {
            return View(db.NiveauScolarite.Where(m => m.archived == 1).ToList());
        }

        // GET: NiveauScolarites/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NiveauScolarite niveauScolarite = db.NiveauScolarite.Find(id);
            if (niveauScolarite == null)
            {
                return HttpNotFound();
            }
            return View(niveauScolarite);
        }

        // GET: NiveauScolarites/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NiveauScolarites/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( NiveauScolarite niveauScolarite)
        {
            if (ModelState.IsValid)
            {
                niveauScolarite.archived = 1;
                niveauScolarite.status = 1;
                niveauScolarite.created = DateTime.Now;
                db.NiveauScolarite.Add(niveauScolarite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(niveauScolarite);
        }

        // GET: NiveauScolarites/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NiveauScolarite niveauScolarite = db.NiveauScolarite.Find(id);
            if (niveauScolarite == null)
            {
                return HttpNotFound();
            }
            return View(niveauScolarite);
        }

        // POST: NiveauScolarites/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( NiveauScolarite niveauScolarite)
        {
            var currentNiveauScolarite = db.NiveauScolarite.FirstOrDefault(p => p.id == niveauScolarite.id);
            if (currentNiveauScolarite == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                currentNiveauScolarite.libelle = niveauScolarite.libelle;
                db.Entry(currentNiveauScolarite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(niveauScolarite);
        }

        // GET: NiveauScolarites/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NiveauScolarite niveauScolarite = db.NiveauScolarite.Find(id);
            if (niveauScolarite == null)
            {
                return HttpNotFound();
            }
            return View(niveauScolarite);
        }

        // POST: NiveauScolarites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            NiveauScolarite niveauScolarite = db.NiveauScolarite.Find(id);
            niveauScolarite.archived = 0;
            db.Entry(niveauScolarite).State = EntityState.Modified;
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
