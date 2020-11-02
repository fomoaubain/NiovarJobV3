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
    public class StatutEmploisController : MybaseController
    {
        private NiovarJobEntities db = new NiovarJobEntities();

        // GET: StatutEmplois
        public ActionResult Index()
        {
            return View(db.StatutEmploi.Where(m => m.archived == 1).ToList());
        }

        // GET: StatutEmplois/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StatutEmploi statutEmploi = db.StatutEmploi.Find(id);
            if (statutEmploi == null)
            {
                return HttpNotFound();
            }
            return View(statutEmploi);
        }

        // GET: StatutEmplois/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StatutEmplois/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StatutEmploi statutEmploi)
        {
            if (ModelState.IsValid)
            {
                statutEmploi.archived = 1;
                statutEmploi.status = 1;
                statutEmploi.created = DateTime.Now;
                db.StatutEmploi.Add(statutEmploi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(statutEmploi);
        }

        // GET: StatutEmplois/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StatutEmploi statutEmploi = db.StatutEmploi.Find(id);
            if (statutEmploi == null)
            {
                return HttpNotFound();
            }
            return View(statutEmploi);
        }

        // POST: StatutEmplois/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( StatutEmploi statutEmploi)
        {
            var currentStatutEmploi = db.StatutEmploi.FirstOrDefault(p => p.id == statutEmploi.id);
            if (currentStatutEmploi == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                currentStatutEmploi.libelle = statutEmploi.libelle;
                db.Entry(currentStatutEmploi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(statutEmploi);
        }

        // GET: StatutEmplois/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StatutEmploi statutEmploi = db.StatutEmploi.Find(id);
            if (statutEmploi == null)
            {
                return HttpNotFound();
            }
            return View(statutEmploi);
        }

        // POST: StatutEmplois/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            StatutEmploi statutEmploi = db.StatutEmploi.Find(id);
            statutEmploi.archived = 0;
            db.Entry(statutEmploi).State = EntityState.Modified;
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
