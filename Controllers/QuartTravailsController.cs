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
    public class QuartTravailsController : MybaseController
    {
        private NiovarJobEntities db = new NiovarJobEntities();

        // GET: QuartTravails
        public ActionResult Index()
        {
            return View(db.QuartTravail.Where(m => m.archived == 1).ToList());
        }

        // GET: QuartTravails/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuartTravail quartTravail = db.QuartTravail.Find(id);
            if (quartTravail == null)
            {
                return HttpNotFound();
            }
            return View(quartTravail);
        }

        // GET: QuartTravails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuartTravails/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuartTravail quartTravail)
        {
            if (ModelState.IsValid)
            {
                quartTravail.archived = 1;
                quartTravail.status = 1;
                quartTravail.created = DateTime.Now;
                db.QuartTravail.Add(quartTravail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(quartTravail);
        }

        // GET: QuartTravails/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuartTravail quartTravail = db.QuartTravail.Find(id);
            if (quartTravail == null)
            {
                return HttpNotFound();
            }
            return View(quartTravail);
        }

        // POST: QuartTravails/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuartTravail quartTravail)
        {
            var currentQuartTravail = db.QuartTravail.FirstOrDefault(p => p.id == quartTravail.id);
            if (currentQuartTravail == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                currentQuartTravail.description = quartTravail.description;
                currentQuartTravail.libelle = quartTravail.libelle;
                db.Entry(quartTravail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(quartTravail);
        }

        // GET: QuartTravails/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuartTravail quartTravail = db.QuartTravail.Find(id);
            if (quartTravail == null)
            {
                return HttpNotFound();
            }
            return View(quartTravail);
        }

        // POST: QuartTravails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            QuartTravail quartTravail = db.QuartTravail.Find(id);
            quartTravail.archived = 0;
            db.Entry(quartTravail).State = EntityState.Modified;
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
