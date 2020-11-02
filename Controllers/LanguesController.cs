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
    public class LanguesController : MybaseController
    {
        private NiovarJobEntities db = new NiovarJobEntities();

        // GET: Langues
        public ActionResult Index()
        {
            return View(db.Langue.Where(m => m.archived ==1).ToList());
        }

        // GET: Langues/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Langue langue = db.Langue.Find(id);
            if (langue == null)
            {
                return HttpNotFound();
            }
            return View(langue);
        }

        // GET: Langues/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Langues/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Langue langue)
        {
            if (ModelState.IsValid)
            {
                langue.archived = 1;
                langue.status = 1;
                langue.created = DateTime.Now;
                db.Langue.Add(langue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(langue);
        }

        // GET: Langues/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Langue langue = db.Langue.Find(id);
            if (langue == null)
            {
                return HttpNotFound();
            }
            return View(langue);
        }

        // POST: Langues/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Langue langue)
        {
            var currentLangue = db.Langue.FirstOrDefault(p => p.id == langue.id);
            if (currentLangue == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                currentLangue.libelle = langue.libelle ;
                

                db.Entry(currentLangue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(langue);
        }

        // GET: Langues/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Langue langue = db.Langue.Find(id);
            if (langue == null)
            {
                return HttpNotFound();
            }
            return View(langue);
        }

        // POST: Langues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Langue langue = db.Langue.Find(id);
            langue.archived = 0;
            db.Entry(langue).State = EntityState.Modified;
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
