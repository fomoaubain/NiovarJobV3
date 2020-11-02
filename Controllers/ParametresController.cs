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
    public class ParametresController : MybaseController
    {
        //private NiovarJobEntities db = new NiovarJobEntities();

        // GET: Parametres
        public ActionResult Index()
        {
            return View(db.Parametre.ToList());
        }

        // GET: Parametres/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parametre parametre = db.Parametre.Find(id);
            if (parametre == null)
            {
                return HttpNotFound();
            }
            return View(parametre);
        }

        // GET: Parametres/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Parametres/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,libelle,valeur,description")] Parametre parametre)
        {
            if (ModelState.IsValid)
            {
                parametre.created = DateTime.Now;
                parametre.status = 1;
                db.Parametre.Add(parametre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parametre);
        }

        // GET: Parametres/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parametre parametre = db.Parametre.Find(id);
            if (parametre == null)
            {
                return HttpNotFound();
            }
            return View(parametre);
        }

        // POST: Parametres/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,libelle,valeur,description")] Parametre parametre)
        {
            Parametre currentParametre = db.Parametre.Find(parametre.id);
            if (currentParametre!=null)
            {
                currentParametre.created = DateTime.Now;
                db.Entry(parametre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else {
                return HttpNotFound();
            }
            return View(parametre);
        }

        // GET: Parametres/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parametre parametre = db.Parametre.Find(id);
            if (parametre == null)
            {
                return HttpNotFound();
            }
            return View(parametre);
        }

        // POST: Parametres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Parametre parametre = db.Parametre.Find(id);
            db.Parametre.Remove(parametre);
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
