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
    public class DiplomesController : MybaseController
    {
        private NiovarJobEntities db = new NiovarJobEntities();

        // GET: Diplomes
        public ActionResult Index()
        {
            return View(db.Diplome.Where(m => m.archived == 1).ToList());
        }

        // GET: Diplomes/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diplome diplome = db.Diplome.Find(id);
            if (diplome == null)
            {
                return HttpNotFound();
            }
            return View(diplome);
        }

        // GET: Diplomes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Diplomes/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Diplome diplome)
        {
            if (ModelState.IsValid)
            {
                diplome.archived = 1;
                diplome.status = 1;
                diplome.created = DateTime.Now;
                db.Diplome.Add(diplome);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(diplome);
        }

        // GET: Diplomes/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diplome diplome = db.Diplome.Find(id);
            if (diplome == null)
            {
                return HttpNotFound();
            }
            return View(diplome);
        }

        // POST: Diplomes/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Diplome diplome)
        {
            var currentDiplome = db.Diplome.FirstOrDefault(p => p.id == diplome.id);
            if (currentDiplome == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                currentDiplome.libelle = diplome.libelle;
                db.Entry(currentDiplome).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(diplome);
        }

        // GET: Diplomes/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diplome diplome = db.Diplome.Find(id);
            if (diplome == null)
            {
                return HttpNotFound();
            }
            return View(diplome);
        }

        // POST: Diplomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Diplome diplome = db.Diplome.Find(id);
            diplome.archived = 0;
            db.Entry(diplome).State = EntityState.Modified;
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
