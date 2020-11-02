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
    public class RolesController : MybaseController
    {
        //private NiovarJobEntities db = new NiovarJobEntities();


        // GET: Roles
        [CustomAuthorized]
        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN })]
        public ActionResult Index()
        {
         
            return View(db.Role.Where(p => p.archived == 1).ToList());
        }

        [CustomAuthorized]
        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN })]
        // GET: Roles/Details/5
        public ActionResult Details(decimal id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Role.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // GET: Roles/Create
        [CustomAuthorized]
        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN })]
        public ActionResult Create()
        {
           
            return View();
        }

        // POST: Roles/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,libelle,archived,status,created")] Role role)
        {
            if (ModelState.IsValid)
            {
                role.created = DateTime.Now;
                role.archived = 1;
                db.Role.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: Roles/Edit/5
        [CustomAuthorized]
        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN })]
        public ActionResult Edit(decimal id)
        {
          
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Role.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,libelle,archived,status,created")] Role role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        [CustomAuthorized]
        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN })]
        public ActionResult Delete(decimal id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Role.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Role role = db.Role.Find(id);
            role.archived = 0;
            db.Entry(role).State = EntityState.Modified;
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
