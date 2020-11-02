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
    public class CategoriesController : MybaseController
    {
        //private NiovarJobEntities db = new NiovarJobEntities();

        // GET: Categories
        public ActionResult Index(decimal id)
        {
            Types type = db.Types.Where(p => p.archived == 1 && p.id == id).First();

            ViewBag.CurrentType = type;

            return View(db.Categorie.Where(p => p.archived == 1 && p.Typ_id==id ).ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(decimal id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categorie categorie = db.Categorie.Find(id);
            if (categorie == null)
            {
                return HttpNotFound();
            }
            return View(categorie);
        }

        // GET: Categories/Create
        public ActionResult Create(decimal id)
        {
            Types type = db.Types.Where(p => p.archived == 1 && p.id == id).First();

            if (type == null)
            {
                return HttpNotFound();
            }

            ViewBag.CurrentType = type;

            return View();
        }

        // POST: Categories/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Categorie categorie)
        {
            if (Session["emailAdmin"] == null) { return RedirectToAction("LoginAdmin", "Utilisateurs"); }

            if (categorie.Typ_id == null)
            {
                return HttpNotFound();
            }

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
                            return View(categorie);
                        }
                        //end verification

                        var formattedFilename = string.Format("{0}-{1}{2}"
                            , Path.GetFileNameWithoutExtension(filename)
                            , Guid.NewGuid().ToString("N")
                            , Path.GetExtension(filename));
                        string path = Path.Combine(Server.MapPath("~/Fichier"),
                                                   Path.GetFileName(formattedFilename));
                        file.SaveAs(path);
                        categorie.image = formattedFilename;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        return View(categorie);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                categorie.archived = 1;
                categorie.status = 1;
                db.Categorie.Add(categorie);
                db.SaveChanges();
                return RedirectToAction("Index",new { id = categorie.Typ_id} );
            }

            return View(categorie);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(decimal id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categorie categorie = db.Categorie.Find(id);
            if (categorie == null)
            {
                return HttpNotFound();
            }
            return View(categorie);
        }

        // POST: Categories/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Categorie categorie)
        {
           

            var currentCategorie = db.Categorie.FirstOrDefault(p => p.id == categorie.id);
            if (currentCategorie == null)
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
                            return View(categorie);
                        }
                        //end verification

                        var formattedFilename = string.Format("{0}-{1}{2}"
                            , Path.GetFileNameWithoutExtension(filename)
                            , Guid.NewGuid().ToString("N")
                            , Path.GetExtension(filename));
                        string path = Path.Combine(Server.MapPath("~/Fichier"),
                                                   Path.GetFileName(formattedFilename));
                        file.SaveAs(path);
                        currentCategorie.image = formattedFilename;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        return View(categorie);
                    }
                }
            }


                currentCategorie.image = categorie.image;
                currentCategorie.libelle = categorie.libelle;
                db.Entry(currentCategorie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = currentCategorie.Typ_id });
           
            
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(decimal id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categorie categorie = db.Categorie.Find(id);
            if (categorie == null)
            {
                return HttpNotFound();
            }
            return View(categorie);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
           
            Categorie categorie = db.Categorie.Find(id);
            categorie.archived = 0;
            db.Entry(categorie).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = categorie.Typ_id });
        }

        public ActionResult ConfigCategorie(decimal id)
        {
            

            if (id == null) {   return HttpNotFound(); }

            Categorie categorie = db.Categorie.Find(id);

            if (categorie == null) { return HttpNotFound(); }

            ViewBag.idTypeCurrent = categorie.Typ_id;

            List<CatAnneeExp> catAnneeExps = db.CatAnneeExp.Where(p => p.Cat_id == id).ToList();

            ViewBag.catAnneeExps = catAnneeExps;

            ViewBag.idCategorie = id;

            ViewBag.Ann_id = new SelectList(db.AnneeExp, "id", "libelle");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfigCategorie(CatAnneeExp catAnneeExp)
        {
           

           CatAnneeExp ListAnneeExps = db.CatAnneeExp.FirstOrDefault(p => p.Cat_id == catAnneeExp.Cat_id && p.Ann_id == catAnneeExp.Ann_id);
           
            if (ListAnneeExps!=null)
            {
                ListAnneeExps.prixHoraire = catAnneeExp.prixHoraire;

                db.Entry(ListAnneeExps).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ConfigCategorie", new { id = catAnneeExp.Cat_id });
            }

            catAnneeExp.archived = 1;
            catAnneeExp.status = 1;
            catAnneeExp.created = DateTime.Now;
            db.CatAnneeExp.Add(catAnneeExp);
            db.SaveChanges();

            //ViewBag.Ann_id = new SelectList(db.AnneeExp, "id", "libelle", catAnneeExp.Ann_id);

            return RedirectToAction("ConfigCategorie", new { id = catAnneeExp.Cat_id });                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
        }

        public ActionResult deleteCatExp(decimal id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            CatAnneeExp catAnneeExp = db.CatAnneeExp.FirstOrDefault(p => p.id == id );

            if (catAnneeExp == null)
            {
                return HttpNotFound();
            }

            db.CatAnneeExp.Remove(catAnneeExp);
            db.SaveChanges();

            return RedirectToAction("ConfigCategorie", new { id = catAnneeExp.Cat_id });
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
