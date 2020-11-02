using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BibliothequesController : MybaseController
    {
        private NiovarJobEntities db = new NiovarJobEntities();

        // GET: Bibliotheques
        public ActionResult Index()
        {
            return View(db.Bibliotheque.ToList());
        }

        // GET: Bibliotheques/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bibliotheque bibliotheque = db.Bibliotheque.Find(id);
            if (bibliotheque == null)
            {
                return HttpNotFound();
            }
            return View(bibliotheque);
        }

        // GET: Bibliotheques/Create
        public ActionResult Create()
        {
            List<Bibliotheque> listBibliotheque = db.Bibliotheque.Where(p => p.archived == 1).OrderByDescending(p => p.created).ToList();
            ViewBag.listBibliotheque = listBibliotheque;
            return View();
        }

        // POST: Bibliotheques/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Bibliotheque bibliotheque)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        var filename = Path.GetFileName(file.FileName);

                        //verification de l'extention du fichiers
                        var supportedTypes = new[] { ".png", ".jpg", ".jpeg", ".gif" };
                        var extention = Path.GetExtension(filename);
                        if (!supportedTypes.Contains(extention))
                        {
                            ViewBag.ErrorFile = "L'extention du fichier n'est pas valide !";
                            return View(bibliotheque);
                        }
                        //end verification

                        var formattedFilename = string.Format("{0}-{1}{2}"
                            , Path.GetFileNameWithoutExtension(filename)
                            , Guid.NewGuid().ToString("N")
                            , Path.GetExtension(filename));
                        string path = Path.Combine(Server.MapPath("~/Fichier"),
                                                   Path.GetFileName(formattedFilename));
                        file.SaveAs(path);
                        bibliotheque.chemin= formattedFilename;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        return View(bibliotheque);
                    }
                }
            }
            else
            {
                ViewBag.ErrorFile = "Veuillez séletionnez une image !";
                return View(bibliotheque);
            }

                bibliotheque.created = DateTime.Now;
                  bibliotheque.archived = 1;
                 db.Bibliotheque.Add(bibliotheque);
                db.SaveChanges();
                return RedirectToAction("Create");

        }

        // GET: Bibliotheques/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bibliotheque bibliotheque = db.Bibliotheque.Find(id);
            if (bibliotheque == null)
            {
                return HttpNotFound();
            }
            return View(bibliotheque);
        }

        // POST: Bibliotheques/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,libelle,chemin,description,status,created,archived")] Bibliotheque bibliotheque)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bibliotheque).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bibliotheque);
        }

        // GET: Bibliotheques/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bibliotheque bibliotheque = db.Bibliotheque.Find(id);
            if (bibliotheque == null)
            {
                return HttpNotFound();
            }
            return View(bibliotheque);
        }

        public ActionResult DeleteImage(decimal id)
        {
            Bibliotheque bibliotheque = db.Bibliotheque.Find(id);
            db.Bibliotheque.Remove(bibliotheque);
            db.SaveChanges();
            return RedirectToAction("Create");
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
