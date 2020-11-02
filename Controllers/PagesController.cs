using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

using System.Web.Mvc;
using WebApplication1.Models;
using System.IO;
using System.Net;

namespace WebApplication1.Controllers
{
    public class PagesController : MybaseController
    {
        private NiovarJobEntities db = new NiovarJobEntities();

        // GET: Pages
        public ActionResult Index()
        {
            var page = db.Page.Include(p => p.Inscrire);
            return View(page.ToList());
        }

        [Authorize]
     
        public ActionResult pageApropos()
        {
            decimal idCurrent;
            if (Session["id"] != null)
            {
                 idCurrent = Convert.ToDecimal(Session["id"]);
            }
            else
            {
                Inscrire value = db.Inscrire.Where(p => p.type.Equals("compagnie") && p.archived == 1).FirstOrDefault();
                 idCurrent = value.id;
            }
         
            Page page = db.Page.Where(p => p.Ins_id== idCurrent && p.archived==1).FirstOrDefault();
            if (page!=null)
            {
                Inscrire inscrire = db.Inscrire.Where(p => p.id == idCurrent && p.archived == 1).FirstOrDefault();
                ViewBag.inscrire = inscrire;

                if (TempData["myData"] != null)
                {
                    String errsms = TempData["myData"].ToString();
                    if (errsms.Equals("1"))
                    {
                        TempData["result_code"] = 1;
                        TempData["myData"] = null;
                        TempData.Keep();
                    }
                    else
                    {
                        TempData["result_code"] = -1;
                        TempData["message"] = "Echec de l'opération veuillez réessayer.";
                        TempData["myData"] = null;
                        TempData.Keep();
                    }
                }
                return View(page);
            }
            else { return HttpNotFound(); }
           // return View();
        }

        [Authorize]
        
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult pageApropos(Page page)
        {
            Page currentPage = db.Page.Where(p => p.id== page.id && p.archived == 1).FirstOrDefault();
            if (currentPage != null)
            {
                currentPage.aPropos = page.aPropos;
                db.Entry(currentPage).State = EntityState.Modified;
                db.SaveChanges();
                TempData["myData"] = "1";
                TempData["result_code"] = 1;
                TempData["message"] = "Sauvegarde éffectué avec succès.";
                TempData.Keep();
            }
            else
            {
                TempData["myData"] = "0";
                TempData["result_code"] = -1;
                TempData["message"] = "Echec de la saugarde.";
                TempData.Keep();
            }

            return RedirectToAction("pageApropos");

        }

        [Authorize]
        
        public ActionResult pagePourquoiPostuler()
        {
            decimal idCurrent;
            if (Session["id"] != null)
            {
                idCurrent = Convert.ToDecimal(Session["id"]);
            }
            else
            {
                Inscrire value = db.Inscrire.Where(p => p.type.Equals("compagnie") && p.archived == 1).FirstOrDefault();
                idCurrent = value.id;
            }
            Page page = db.Page.Where(p => p.Ins_id == idCurrent && p.archived == 1).FirstOrDefault();
            if (page != null)
            {
                Inscrire inscrire = db.Inscrire.Where(p => p.id == idCurrent && p.archived == 1).FirstOrDefault();
                ViewBag.inscrire = inscrire;

                if (TempData["myData"] != null)
                {
                    String errsms = TempData["myData"].ToString();
                    if (errsms.Equals("1"))
                    {
                        TempData["result_code"] = 1;
                        TempData["myData"] = null;
                        TempData.Keep();
                    }
                    else
                    {
                        TempData["result_code"] = -1;
                        TempData["message"] = "Echec de l'opération veuillez réessayer.";
                        TempData["myData"] = null;
                        TempData.Keep();
                    }
                }

                return View(page);
            }
            else { return HttpNotFound(); }
            // return View();
        }

        [Authorize]
        
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult pagePourquoiPostuler(Page page)
        {
            Page currentPage = db.Page.Where(p => p.id == page.id && p.archived == 1).FirstOrDefault();
            if (currentPage != null)
            {
                currentPage.pourquoiPostuler = page.pourquoiPostuler;
                db.Entry(currentPage).State = EntityState.Modified;
                db.SaveChanges();
                TempData["myData"] = "1";
                TempData["result_code"] = 1;
                TempData["message"] = "Sauvegarde éffectué avec succès.";
                TempData.Keep();

            }
            else
            {
                TempData["myData"] = "0";
                TempData["result_code"] = -1;
                TempData["message"] = "Echec de la saugarde.";
                TempData.Keep();
            }

            return RedirectToAction("pagePourquoiPostuler");

        }

        [Authorize]
        
        public ActionResult pageQuestionReponse()
        {
   
          return View();
        }

        [Authorize]
        
        public ActionResult pageAvis()
        {
            decimal idCurrent;
            if (Session["id"] != null)
            {
                idCurrent = Convert.ToDecimal(Session["id"]);
            }
            else
            {
                Inscrire value = db.Inscrire.Where(p => p.type.Equals("compagnie") && p.archived == 1).FirstOrDefault();
                idCurrent = value.id;
            }

            Page page = db.Page.Where(p => p.Ins_id == idCurrent && p.archived == 1).FirstOrDefault();
            if (page != null)
            {
                Inscrire inscrire = db.Inscrire.Where(p => p.id == idCurrent && p.archived == 1).FirstOrDefault();
                ViewBag.inscrire = inscrire;

                List<Avis>  listAvis = db.Avis.Where(p =>p.Pag_id==page.id && p.archived == 1).OrderByDescending(p => p.created).ToList();
                listAvis = listAvis.Take(15).ToList();

                List<Avis> newListAvis = new List<Avis>();
                foreach (Avis item in listAvis)
                {
                    Avis newAvis = new Avis();
                    newAvis = item;

                    Inscrire insc = db.Inscrire.Where(p => p.id == item.iduser).FirstOrDefault();
                    if (insc != null)
                    {
                        newAvis.Pseudo = insc.login;
                        newAvis.Profil = insc.profil;
                    }
                    newListAvis.Add(newAvis);
                }
                ViewBag.newListAvis = newListAvis;
                ViewBag.page= page;

                if (TempData["myData"] != null)
                {
                    String errsms = TempData["myData"].ToString();
                    if (errsms.Equals("1"))
                    {
                        TempData["result_code"] = 1;
                        TempData["myData"] = null;
                        TempData.Keep();
                    }
                    else
                    {
                        TempData["result_code"] = -1;
                        TempData["message"] = "Echec de l'opération veuillez réessayer.";
                        TempData["myData"] = null;
                        TempData.Keep();
                    }
                }
                return View();
            }

            ViewBag.page = page;
            return View();
        }

        [Authorize]
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult pageAvis(Avis avis)
        {
            decimal idCurrent;
            if (Session["id"] != null)
            {
                idCurrent = Convert.ToDecimal(Session["id"]);
            }
            else
            {
                Inscrire value = db.Inscrire.Where(p => p.type.Equals("compagnie") && p.archived == 1).FirstOrDefault();
                idCurrent = value.id;
            }

            Page currentPage = db.Page.Where(p => p.id == avis.Pag_id && p.archived == 1).FirstOrDefault();
            if (currentPage != null)
            {
                avis.iduser =(int) idCurrent;
                avis.archived = 1;
                avis.created = DateTime.Now;
                avis.status = 0;
                db.Avis.Add(avis);
                db.SaveChanges();
                TempData["myData"] = "1";
                TempData["result_code"] = 1;
                TempData["message"] = "Avis envoyer avec succès.";
                TempData.Keep();

            }
            else
            {
                TempData["myData"] = "0";
                TempData["result_code"] = -1;
                TempData["message"] = "Echec de l'opération.";
                TempData.Keep();
            }

            return RedirectToAction("pageAvis");
        }

        [Authorize]
        
        public ActionResult pageGalerie()
        {

            decimal idCurrent;
            if (Session["id"] != null)
            {
                idCurrent = Convert.ToDecimal(Session["id"]);
            }
            else
            {
                Inscrire value = db.Inscrire.Where(p => p.type.Equals("compagnie") && p.archived == 1).FirstOrDefault();
                idCurrent = value.id;
            }

            Page page = db.Page.Where(p => p.Ins_id == idCurrent && p.archived == 1).FirstOrDefault();
            if (page != null)
            {
                Inscrire inscrire = db.Inscrire.Where(p => p.id == idCurrent && p.archived == 1).FirstOrDefault();
                ViewBag.inscrire = inscrire;

                List<Galerie> listGalerie = db.Galerie.Where(p => p.Pag_id == page.id && p.archived == 1).OrderByDescending(p => p.created).ToList();
                ViewBag.listGalerie = listGalerie.Take(12).ToList();

                ViewBag.page = page;

                if (TempData["myData"] != null)
                {
                    String errsms = TempData["myData"].ToString();
                    if (errsms.Equals("1"))
                    {
                        TempData["result_code"] = 1;
                        TempData["myData"] = null;
                        TempData.Keep();
                    }
                    else
                    {
                        TempData["result_code"] = -1;
                        TempData["message"] = "Echec de l'opération veuillez réessayer.";
                        TempData["myData"] = null;
                        TempData.Keep();
                    }
                }
                return View();
            }

            ViewBag.page = page;
            return View();
        }

        [Authorize]
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult pageGalerie(Galerie galerie)
        {

         
            Page currentPage = db.Page.Where(p => p.id == galerie.Pag_id && p.archived == 1).FirstOrDefault();
            if (currentPage != null)
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
                                TempData["myData"] = "0";
                                TempData["result_code"] = -1;
                                TempData["message"] = "L'extention du fichier n'est pas valide .";
                                TempData.Keep();
                                return RedirectToAction("pageGalerie");
                            }
                            //end verification

                            var formattedFilename = string.Format("{0}-{1}{2}"
                                , Path.GetFileNameWithoutExtension(filename)
                                , Guid.NewGuid().ToString("N")
                                , Path.GetExtension(filename));
                            string path = Path.Combine(Server.MapPath("~/Fichier"),
                                                       Path.GetFileName(formattedFilename));
                            file.SaveAs(path);
                            galerie.image = formattedFilename;
                        }
                        catch (Exception ex)
                        {
                            TempData["myData"] = "0";
                            TempData["result_code"] = -1;
                            TempData["message"] = "Echec de l'operation .";
                            TempData.Keep();
                            return RedirectToAction("pageGalerie");
                        }
                    }
                }

                galerie.archived = 1;
                galerie.created = DateTime.Now;
                galerie.status = 1;
                db.Galerie.Add(galerie);

                db.SaveChanges();
                TempData["myData"] = "1";
                TempData["result_code"] = 1;
                TempData["message"] = "Avis envoyer avec succès.";
                TempData.Keep();

            }
            else
            {
                TempData["myData"] = "0";
                TempData["result_code"] = -1;
                TempData["message"] = "Echec de l'opération.";
                TempData.Keep();
            }

            return RedirectToAction("pageGalerie");
        }


        // GET: Pages/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = db.Page.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // GET: Pages/Create
        public ActionResult Create()
        {
            Inscrire inscrire  = db.Inscrire.Where(p => p.archived==1 && p.type.Equals("compagnie")).FirstOrDefault();
            return View(inscrire);
        }

        // POST: Pages/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Inscrire inscrire)
        {

            /*   if (Request.Files.Count > 0)
               {
                   var file = Request.Files[0];
                   if (file != null && file.ContentLength > 0)
                   {
                       try
                       {
                           var filename = Path.GetFileName(file.FileName);

                           //verification de l'extention du fichiers
                           var supportedTypes = new[] { ".png", ".jpg" };
                           var extention = Path.GetExtension(filename);
                           if (!supportedTypes.Contains(extention))
                           {
                               ViewBag.ErrorFile = "L'extention du fichier n'est pas valide !";
                               return View(inscrire);
                           }
                           //end verification

                           var formattedFilename = string.Format("{0}-{1}{2}"
                           , Path.GetFileNameWithoutExtension(filename)
                           , Guid.NewGuid().ToString("N")
                           , Path.GetExtension(filename));
                           string path = Path.Combine(Server.MapPath("~/Fichier"),
                                                      Path.GetFileName(formattedFilename));
                           file.SaveAs(path);
                           inscrire.profil = formattedFilename;
                       }
                       catch (Exception ex)
                       {
                           ViewBag.ErrorFile = "ERROR:" + ex.Message.ToString();

                           return View(inscrire);
                       }
                   }
               }*/

       

            try
            {
                Inscrire currentInscrire = db.Inscrire.Where(p => p.archived == 1 && p.type.Equals("compagnie")).FirstOrDefault();

                if (currentInscrire != null)
                {
                    currentInscrire.nom = inscrire.nom;
                    currentInscrire.emailProf = inscrire.emailProf;
                    currentInscrire.phone = inscrire.phone;
                    currentInscrire.codePostal = inscrire.codePostal;
                    currentInscrire.website = inscrire.website;
                    currentInscrire.nomRepresentant = inscrire.nomRepresentant;
                    currentInscrire.prenomRepresentant = inscrire.prenomRepresentant;
                    currentInscrire.facebook = inscrire.facebook;
                    currentInscrire.linkedin = inscrire.linkedin;
                    currentInscrire.login = inscrire.login;
                    currentInscrire.email = inscrire.email;
                    currentInscrire.cpassword = currentInscrire.password;

                    db.Entry(currentInscrire).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("pageApropos");
                }
                else
                {
                    inscrire.profil = "logo2.png";
                    inscrire.type = "compagnie";
                    inscrire.status = 1;
                    inscrire.archived = 1;
                    inscrire.password = "111111";
                    inscrire.cpassword = inscrire.password;
                    inscrire.etat = 1;
                    inscrire.created = DateTime.Now;

                    db.Inscrire.Add(inscrire);
                    db.SaveChanges();

                    Page page = new Page();
                    page.Ins_id = inscrire.id;
                    page.archived = 1;
                    page.created = DateTime.Now;
                    page.status = 1;
                    db.Page.Add(page);
                    db.SaveChanges();

                    return RedirectToAction("pageApropos");
                } 

               
            }
            catch (Exception e)
            {
                return View(inscrire);
            }
              
         
        }

        // GET: Pages/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = db.Page.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            ViewBag.Ins_id = new SelectList(db.Inscrire, "id", "type", page.Ins_id);
            return View(page);
        }

        // POST: Pages/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Ins_id,profilPage,profil,aPropos,pourquoiPostuler,description,nomPage,archived,status,created")] Page page)
        {
            if (ModelState.IsValid)
            {
                db.Entry(page).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Ins_id = new SelectList(db.Inscrire, "id", "type", page.Ins_id);
            return View(page);
        }

        // GET: Pages/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = db.Page.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Page page = db.Page.Find(id);
            db.Page.Remove(page);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult deleteAvis(decimal id)
        {
            try
            {
                var currentAvis = db.Avis.FirstOrDefault(p => p.id == id);
                if (currentAvis == null)
                {
                    return HttpNotFound();
                }

                currentAvis.archived = 0;

                db.Entry(currentAvis).State = EntityState.Modified;
                db.SaveChanges();

                TempData["myData"] = "1";
                TempData["result_code"] = 1;
                TempData["message"] = "suppression éffectué avec succès.";
                TempData.Keep();
                

                return RedirectToAction("pageAvis");

            }
            catch (Exception)
            {
                TempData["myData"] = "0";
                TempData["result_code"] = -1;
                TempData["message"] = "Echec de l'opération.";
                TempData.Keep();

                return RedirectToAction("pageAvis");

            }
        }


        public ActionResult changeStatusAvis(decimal id, int nbreEtoile)
        {
            try
            {
                var currentAvis = db.Avis.FirstOrDefault(p => p.id == id);
                if (currentAvis == null)
                {
                    return HttpNotFound();
                }

                if (currentAvis.status == 1) { currentAvis.status = 0; } else { currentAvis.status = 1; };

                currentAvis.nbreEtoile = nbreEtoile;
                db.Entry(currentAvis).State = EntityState.Modified;
                db.SaveChanges();

                TempData["myData"] = "1";
                TempData["result_code"] = 1;
                TempData["message"] = "Opération avec succès.";
                TempData.Keep();


                return RedirectToAction("pageAvis");

            }
            catch (Exception)
            {
                TempData["myData"] = "0";
                TempData["result_code"] = -1;
                TempData["message"] = "Echec de l'opération.";
                TempData.Keep();

                return RedirectToAction("pageAvis");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult changePhotoFond(Inscrire inscrire)
        {
            String nomFichier=null;
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

                            TempData["myData"] = "0";
                            TempData["result_code"] = -1;
                            TempData["message"] = "L'extention du fichier n'est pas valide !";
                            TempData.Keep();
                            return RedirectToAction("pageApropos");
                        }
                        //end verification

                        var formattedFilename = string.Format("{0}-{1}{2}"
                            , Path.GetFileNameWithoutExtension(filename)
                            , Guid.NewGuid().ToString("N")
                            , Path.GetExtension(filename));
                        string path = Path.Combine(Server.MapPath("~/Fichier"),
                                                   Path.GetFileName(formattedFilename));
                        file.SaveAs(path);
                        nomFichier = formattedFilename;
                    }
                    catch (Exception ex)
                    {
                        TempData["myData"] = "0";
                        TempData["result_code"] = -1;
                        TempData["message"] = "Echec de l'opération !";
                        TempData.Keep();
                        return RedirectToAction("pageApropos");
                    }
                }
            }

            decimal idCurrent;
            if (Session["id"] != null)
            {
                idCurrent = Convert.ToDecimal(Session["id"]);
            }
            else
            {
                Inscrire value = db.Inscrire.Where(p => p.type.Equals("compagnie") && p.archived == 1).FirstOrDefault();
                idCurrent = value.id;
            }

            Page page = db.Page.Where(p => p.Ins_id == idCurrent && p.archived == 1).FirstOrDefault();

            if (page == null )
            {
                TempData["myData"] = "0";
                TempData["result_code"] = -1;
                TempData["message"] = "Echec de l'opération !";
                TempData.Keep();
                return RedirectToAction("pageApropos");
            }

            page.profilPage = nomFichier;
            page.couleurFond = inscrire.couleurFond;
            db.Entry(page).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("pageApropos");
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
