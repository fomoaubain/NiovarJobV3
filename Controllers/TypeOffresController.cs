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
    public class TypeOffresController : MybaseController
    {
        private NiovarJobEntities db = new NiovarJobEntities();

        // GET: TypeOffres
        public ActionResult Index()
        {
            return View(db.TypeOffre.Where(m => m.archived == 1).ToList());
        }

        // GET: TypeOffres/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeOffre typeOffre = db.TypeOffre.Find(id);
            if (typeOffre == null)
            {
                return HttpNotFound();
            }
            return View(typeOffre);
        }

        // GET: TypeOffres/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeOffres/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TypeOffre typeOffre)
        {
            if (ModelState.IsValid)
            {
                typeOffre.archived = 1;
                typeOffre.status = 1;
                typeOffre.created = DateTime.Now;

                db.TypeOffre.Add(typeOffre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(typeOffre);
        }

        // GET: TypeOffres/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeOffre typeOffre = db.TypeOffre.Find(id);
            if (typeOffre == null)
            {
                return HttpNotFound();
            }
            return View(typeOffre);
        }

        // POST: TypeOffres/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( TypeOffre typeOffre)
        {
            var currentTypeOffre = db.TypeOffre.FirstOrDefault(p => p.id == typeOffre.id);
            if (currentTypeOffre == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                currentTypeOffre.libelle = typeOffre.libelle;
                db.Entry(currentTypeOffre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(typeOffre);
        }

        // GET: TypeOffres/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeOffre typeOffre = db.TypeOffre.Find(id);
            if (typeOffre == null)
            {
                return HttpNotFound();
            }
            return View(typeOffre);
        }

        // POST: TypeOffres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            TypeOffre typeOffre = db.TypeOffre.Find(id);
            typeOffre.archived = 0;
            db.Entry(typeOffre).State = EntityState.Modified;
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
