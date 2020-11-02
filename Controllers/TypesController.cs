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
    [CustomAuthorized]
    public class TypesController : MybaseController
    {
        //private NiovarJobEntities db = new NiovarJobEntities();

        // GET: Types
        public ActionResult Index()
        {
            
            return View(db.Types.Where(p => p.archived == 1).ToList());
        }

        // GET: Types/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Types types = db.Types.Find(id);
            if (types == null)
            {
                return HttpNotFound();
            }
            return View(types);
        }

        // GET: Types/Create
        public ActionResult Create()
        {
            List<Bibliotheque> listBibliotheque = db.Bibliotheque.Where(p => p.archived == 1).OrderByDescending(p => p.created).ToList();
            ViewBag.listBibliotheque = listBibliotheque;
            return View();
        }

        // POST: Types/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Types types)
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
                            return View(types);
                        }
                        //end verification

                        var formattedFilename = string.Format("{0}-{1}{2}"
                            , Path.GetFileNameWithoutExtension(filename)
                            , Guid.NewGuid().ToString("N")
                            , Path.GetExtension(filename));
                        string path = Path.Combine(Server.MapPath("~/Fichier"),
                                                   Path.GetFileName(formattedFilename));
                        file.SaveAs(path);
                        types.image = formattedFilename;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        return View(types);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                types.created = DateTime.Now;
                types.archived = 1;
                types.status = 1;
                db.Types.Add(types);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(types);
        }

        // GET: Types/Edit/5
        public ActionResult Edit(decimal id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Types types = db.Types.Find(id);
            if (types == null)
            {
                return HttpNotFound();
            }
            return View(types);
        }

        // POST: Types/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Types types)
        {
           

            Types currentTypes = db.Types.FirstOrDefault(p => p.id == types.id);
            if (currentTypes == null)
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
                            return View(types);
                        }
                        //end verification

                        var formattedFilename = string.Format("{0}-{1}{2}"
                            , Path.GetFileNameWithoutExtension(filename)
                            , Guid.NewGuid().ToString("N")
                            , Path.GetExtension(filename));
                        string path = Path.Combine(Server.MapPath("~/Fichier"),
                                                   Path.GetFileName(formattedFilename));
                        file.SaveAs(path);
                        currentTypes.image = formattedFilename;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        return View(types);
                    }
                }
            }

                 currentTypes.image = types.image;
                currentTypes.libelle = types.libelle;
                db.Entry(currentTypes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
         
            return View(types);
        }

        // GET: Types/Delete/5
        public ActionResult Delete(decimal id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Types types = db.Types.Find(id);
            if (types == null)
            {
                return HttpNotFound();
            }
            return View(types);
        }

        // POST: Types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            
            Types types = db.Types.Find(id);
            types.archived = 0;
            db.Entry(types).State = EntityState.Modified;
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
