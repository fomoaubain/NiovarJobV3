using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.IO;

namespace WebApplication1.Controllers
{
    [CustomAuthorized]
    public class AvantageSociauxesController : MybaseController
    {
        private NiovarJobEntities db = new NiovarJobEntities();

        // GET: AvantageSociauxes
        public ActionResult Index()
        {
            return View(db.AvantageSociaux.Where(m => m.archived == 1).ToList());
        }

        // GET: AvantageSociauxes/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvantageSociaux avantageSociaux = db.AvantageSociaux.Find(id);
            if (avantageSociaux == null)
            {
                return HttpNotFound();
            }
            return View(avantageSociaux);
        }

        // GET: AvantageSociauxes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AvantageSociauxes/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( AvantageSociaux avantageSociaux)
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
                            return View(avantageSociaux);
                        }
                        //end verification

                        var formattedFilename = string.Format("{0}-{1}{2}"
                            , Path.GetFileNameWithoutExtension(filename)
                            , Guid.NewGuid().ToString("N")
                            , Path.GetExtension(filename));
                        string path = Path.Combine(Server.MapPath("~/Fichier"),
                                                   Path.GetFileName(formattedFilename));
                        file.SaveAs(path);
                        avantageSociaux.image = formattedFilename;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        return View(avantageSociaux);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                avantageSociaux.archived = 1;
                avantageSociaux.status = 1;
                avantageSociaux.created = DateTime.Now;
                db.AvantageSociaux.Add(avantageSociaux);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(avantageSociaux);
        }

        // GET: AvantageSociauxes/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvantageSociaux avantageSociaux = db.AvantageSociaux.Find(id);
            if (avantageSociaux == null)
            {
                return HttpNotFound();
            }
            return View(avantageSociaux);
        }

        // POST: AvantageSociauxes/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( AvantageSociaux avantageSociaux)
        {

            var currentAvantageSociaux = db.AvantageSociaux.FirstOrDefault(p => p.id == avantageSociaux.id);
            if (currentAvantageSociaux == null)
                return HttpNotFound();

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
                            return View(avantageSociaux);
                        }
                        //end verification

                        var formattedFilename = string.Format("{0}-{1}{2}"
                            , Path.GetFileNameWithoutExtension(filename)
                            , Guid.NewGuid().ToString("N")
                            , Path.GetExtension(filename));
                        string path = Path.Combine(Server.MapPath("~/Fichier"),
                                                   Path.GetFileName(formattedFilename));
                        file.SaveAs(path);
                        currentAvantageSociaux.image = formattedFilename;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        return View(avantageSociaux);
                    }
                }
            }

                currentAvantageSociaux.image = avantageSociaux.image;
                 currentAvantageSociaux.libelle = avantageSociaux.libelle;
                   currentAvantageSociaux.description = avantageSociaux.description;
                db.Entry(currentAvantageSociaux).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
         
        }

        // GET: AvantageSociauxes/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvantageSociaux avantageSociaux = db.AvantageSociaux.Find(id);
            if (avantageSociaux == null)
            {
                return HttpNotFound();
            }
            return View(avantageSociaux);
        }

        // POST: AvantageSociauxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            AvantageSociaux avantageSociaux = db.AvantageSociaux.Find(id);
            avantageSociaux.archived = 0;
            db.Entry(avantageSociaux).State = EntityState.Modified;
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
