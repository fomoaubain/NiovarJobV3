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
    
    public class ContratsController : MybaseController
    {
        private NiovarJobEntities db = new NiovarJobEntities();

        [CustomAuthorized]
        // GET: Contrats
        public ActionResult Index()
        {
            return View(db.Contrat.Where(m => m.archived == 1).ToList());
        }

        [CustomAuthorized]
        // GET: Contrats/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contrat contrat = db.Contrat.Find(id);
            if (contrat == null)
            {
                return HttpNotFound();
            }
            return View(contrat);
        }
        [CustomAuthorized]
        // GET: Contrats/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contrats/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Contrat contrat)
        {
            if (ModelState.IsValid)
            {
                contrat.archived = 1;
                contrat.status = 1;
                contrat.created = DateTime.Now;
                db.Contrat.Add(contrat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contrat);
        }

        // GET: Contrats/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contrat contrat = db.Contrat.Find(id);
            if (contrat == null)
            {
                return HttpNotFound();
            }
            return View(contrat);
        }

        // POST: Contrats/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contrat contrat)
        {
            var currentContrat = db.Contrat.FirstOrDefault(p => p.id == contrat.id);
            if (currentContrat == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                currentContrat.libelle = contrat.libelle;
                db.Entry(currentContrat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contrat);
        }

        // GET: Contrats/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contrat contrat = db.Contrat.Find(id);
            if (contrat == null)
            {
                return HttpNotFound();
            }
            return View(contrat);
        }

        // POST: Contrats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Contrat contrat = db.Contrat.Find(id);
            contrat.archived = 0;
            db.Entry(contrat).State = EntityState.Modified;
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
