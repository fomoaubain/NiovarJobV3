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
    public class AnneeExpsController : MybaseController
    {
        //private NiovarJobEntities db = new NiovarJobEntities();

        // GET: AnneeExps
        public ActionResult Index()
        {
           
            return View(db.AnneeExp.Where(p => p.archived == 1).ToList());
        }

        // GET: AnneeExps/Details/5
        public ActionResult Details(decimal id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnneeExp anneeExp = db.AnneeExp.Find(id);
            if (anneeExp == null)
            {
                return HttpNotFound();
            }
            return View(anneeExp);
        }

        // GET: AnneeExps/Create
        public ActionResult Create()
        {
           
            return View();
        }

        // POST: AnneeExps/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( AnneeExp anneeExp)
        {
           
            if (ModelState.IsValid)
            {
                anneeExp.archived = 1;
                anneeExp.status = 1;
                anneeExp.created = DateTime.Now;
                db.AnneeExp.Add(anneeExp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(anneeExp);
        }

        // GET: AnneeExps/Edit/5
        public ActionResult Edit(decimal id)
        {
          
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnneeExp anneeExp = db.AnneeExp.Find(id);
            if (anneeExp == null)
            {
                return HttpNotFound();
            }
            return View(anneeExp);
        }

        // POST: AnneeExps/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( AnneeExp anneeExp)
        {

            AnneeExp currentAnneeExp = db.AnneeExp.Find(anneeExp.id);

                 currentAnneeExp.libelle = anneeExp.libelle;
                currentAnneeExp.created = DateTime.Now;
                db.Entry(currentAnneeExp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
           
            return View(anneeExp);
        }

        // GET: AnneeExps/Delete/5
        public ActionResult Delete(decimal id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnneeExp anneeExp = db.AnneeExp.Find(id);
            if (anneeExp == null)
            {
                return HttpNotFound();
            }
            return View(anneeExp);
        }

        // POST: AnneeExps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
           
            AnneeExp anneeExp = db.AnneeExp.Find(id);
            anneeExp.archived = 0;
            db.Entry(anneeExp).State = EntityState.Modified;
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
