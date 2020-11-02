using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProfileController : MybaseController
    {
        //private NiovarJobEntities db = new NiovarJobEntities();
        // GET: Profile


        public ActionResult Index()
        {
            return View();
        }
     

        [Authorize]
        [AuthorizedCandidat]
        public ActionResult CvResume()
        {
          decimal id = Convert.ToDecimal(Session["id"]);
            if (Session["id"]==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Information information = db.Information.FirstOrDefault(p => p.Ins_id ==id);
            if (information == null) information = new Information();

            List<Education> education  = db.Education.Where(p => p.Ins_id == id).ToList();
            if(education.Count >0)
            {
                ViewBag.listEducation = education;
            }
            else
            {
                ViewBag.listEducation = null;
            }

            List<Experience> experiences = db.Experience.Where(p => p.Ins_id == id).ToList();
            if (experiences.Count > 0)
            {
                ViewBag.listExperience = experiences;
            }
            else
            {
                ViewBag.listExperience = null;
            }

            List<Autre> autres = db.Autre.Where(p => p.Ins_id == id).ToList();
            if (autres.Count > 0)
            {
                ViewBag.listAutre = autres;
            }
            else
            {
                ViewBag.listAutre = null;
            }

            CvListObject model = new CvListObject();
            model.information = information;
            model.education = new  Education();
            model.experience = new Experience();
            model.autre = new Autre();
           

            return View(model);
        }


        [Authorize]
        [AuthorizedClient]
        public ActionResult ClientProfile()
        {
            decimal id = Convert.ToDecimal(Session["id"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inscrire inscrire = db.Inscrire.Find(id);
            if (inscrire == null)
            {
                return HttpNotFound();
            }

            List<Types> listTypes = db.Types.ToList();
            List<Categorie> listCategorie = db.Categorie.ToList();

            ViewBag.listTypes = listTypes;
            ViewBag.listCategorie = listCategorie;

            return View(inscrire);
        }

        [Authorize]
        [AuthorizedCandidat]
        public ActionResult CandidatProfile()
        {
            decimal id = Convert.ToDecimal(Session["id"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inscrire inscrire = db.Inscrire.Find(id);
            if (inscrire == null)
            {
                return HttpNotFound();
            }

            List<Types> listTypes = db.Types.ToList();
            List<Categorie> listCategorie = db.Categorie.ToList();

            ViewBag.listTypes = listTypes;
            ViewBag.listCategorie = listCategorie;

            List<AnneeExp> listAnneeExp = db.AnneeExp.ToList();
            ViewBag.listAnneeExp = listAnneeExp;

            return View(inscrire);
        }

        // POST: Inscrires/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClientProfile(Inscrire inscrire)
        {
            List<Types> listTypes = db.Types.ToList();
            List<Categorie> listCategorie = db.Categorie.ToList();
            try {   
                var currentInscrire = db.Inscrire.FirstOrDefault(p => p.id == inscrire.id);
                if (currentInscrire == null)
                    return HttpNotFound();

                if(Request.Files.Count > 0) {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0) {
                    try
                    {   
                        var filename = Path.GetFileName(file.FileName);

                            //verification de l'extention du fichiers
                            var supportedTypes = new[] { ".png", ".jpg" };
                            var extention = Path.GetExtension(filename);
                            if (!supportedTypes.Contains(extention))
                            {
                                ViewBag.ErrorFile = "L'extention du fichier n'est pas valide !";
                                ViewBag.listTypes = listTypes;
                                ViewBag.listCategorie = listCategorie;
                                return View(inscrire);
                            }
                            //end verification

                            var formattedFilename = string.Format("{0}-{1}{2}"
                            ,Path.GetFileNameWithoutExtension(filename)
                            ,Guid.NewGuid().ToString("N")
                            ,Path.GetExtension(filename));
                        string path = Path.Combine(Server.MapPath("~/Fichier"),
                                                   Path.GetFileName(formattedFilename));
                        file.SaveAs(path);
                        currentInscrire.profil = formattedFilename;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                            ViewBag.listTypes = listTypes;
                            ViewBag.listCategorie = listCategorie;
                            return View(inscrire);
                    }
                }
                }

                currentInscrire.nom = inscrire.nom;
                currentInscrire.phone = inscrire.phone;
                currentInscrire.website = inscrire.website;
                currentInscrire.description = inscrire.website;
                currentInscrire.emailProf = inscrire.emailProf;
                currentInscrire.facebook = inscrire.facebook;
                currentInscrire.linkedin = inscrire.linkedin;
                currentInscrire.adresse = inscrire.adresse;
                currentInscrire.longitude = inscrire.longitude;
                currentInscrire.lat = inscrire.lat;
                currentInscrire.categorie = inscrire.categorie;
                currentInscrire.domaine = inscrire.domaine;
                currentInscrire.nomRepresentant = inscrire.nomRepresentant;
                currentInscrire.prenomRepresentant = inscrire.prenomRepresentant;
                currentInscrire.numeroPoste = inscrire.numeroPoste;
                currentInscrire.province = inscrire.province;
                currentInscrire.codePostal = inscrire.codePostal;
                currentInscrire.login = inscrire.nom;
                currentInscrire.pays = inscrire.pays;
                currentInscrire.ville = inscrire.ville;



                db.Entry(currentInscrire).State = EntityState.Modified;
                db.SaveChanges();
                Session["profil"] = currentInscrire.profil;
                Session["nom"] = currentInscrire.nom;
                Session["ville"] = currentInscrire.ville;
                Session["pays"] = currentInscrire.pays;
                Session["nom_representant"] = currentInscrire.nomRepresentant;
                Session["login"] = currentInscrire.login;
                Session["adresse"] = currentInscrire.adresse;



                ViewBag.listTypes = listTypes;
                ViewBag.listCategorie = listCategorie;

                return View(currentInscrire);
              
            }
            catch(DbEntityValidationException ex)
            {
               /* foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }*/
                //Console.WriteLine(e.Message);
                return View(inscrire);
                
            }
            
        }

        // POST: Inscrires/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CandidatProfile(Inscrire inscrire)
        {
            List<Types> listTypes = db.Types.ToList();
            List<Categorie> listCategorie = db.Categorie.ToList();

            try
            {
                var currentInscrire = db.Inscrire.FirstOrDefault(p => p.id == inscrire.id);
                if (currentInscrire == null)
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
                            var supportedTypes = new[] { ".png", ".jpg" };
                            var extention = Path.GetExtension(filename);
                            if (!supportedTypes.Contains(extention))
                            {
                                ViewBag.ErrorFile = "L'extention du fichier n'est pas valide !";
                                ViewBag.listTypes = listTypes;
                                ViewBag.listCategorie = listCategorie;
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
                            currentInscrire.profil = formattedFilename;
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                            ViewBag.listTypes = listTypes;
                            ViewBag.listCategorie = listCategorie;
                            return View(inscrire);
                        }
                    }
                }

                currentInscrire.nom = inscrire.nom;
                currentInscrire.phone = inscrire.phone;
                currentInscrire.website = inscrire.website;
                currentInscrire.description = inscrire.website;
                currentInscrire.emailProf = inscrire.emailProf;
                currentInscrire.facebook = inscrire.facebook;
                currentInscrire.linkedin = inscrire.linkedin;
                currentInscrire.adresse = inscrire.adresse;
                currentInscrire.longitude = inscrire.longitude;
                currentInscrire.lat = inscrire.lat;
                currentInscrire.categorie = inscrire.categorie;
                currentInscrire.domaine = inscrire.domaine;
                currentInscrire.province = inscrire.province;
                currentInscrire.codePostal = inscrire.codePostal;

                if (inscrire.pays==null)
                {  currentInscrire.pays = currentInscrire.pays;}
                else {currentInscrire.pays = inscrire.pays;}

                if (inscrire.ville == null )
                { currentInscrire.ville = currentInscrire.ville; }
                else{ currentInscrire.ville = inscrire.ville; }
              

                currentInscrire.titreEmploi = inscrire.titreEmploi;
                currentInscrire.anneeExperience = inscrire.anneeExperience;
                currentInscrire.salaireSouhaiter = inscrire.salaireSouhaiter;
                currentInscrire.salaireNegocier = inscrire.salaireNegocier;


                db.Entry(currentInscrire).State = EntityState.Modified;
                db.SaveChanges();
                Session["profil"] = currentInscrire.profil;
                Session["nom"] = currentInscrire.nom;
                Session["ville"] = currentInscrire.ville;
                Session["pays"] = currentInscrire.pays;
                Session["adresse"] = currentInscrire.adresse;

                //ViewBag.listAnneeExp = db.AnneeExp.ToList();
                List<AnneeExp> listAnneeExp = db.AnneeExp.ToList();
                ViewBag.listAnneeExp = listAnneeExp;

               

                ViewBag.listTypes = listTypes;
                ViewBag.listCategorie = listCategorie;

                return View(currentInscrire);

            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
                //Console.WriteLine(e.Message);
                //ViewBag.listAnneeExp = db.AnneeExp.ToList();
                List<AnneeExp> listAnneeExp = db.AnneeExp.ToList();
                ViewBag.listAnneeExp = listAnneeExp;
                return View(inscrire);

            }

        }
         

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEducation(Education education)
        {
            try {
               
                    education.archived = 1;
                    education.status = 1;
                    education.Ins_id = Convert.ToDecimal(Session["id"]);
                    db.Education.Add(education);
                    db.SaveChanges();
                return RedirectToAction("CvResume");

            } catch(DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }

            return RedirectToAction("CvResume");
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveExperience(Experience experience)
        {
            try
            {

                experience.archived = 1;
                experience.status = 1;
                experience.Ins_id = Convert.ToDecimal(Session["id"]);
                db.Experience.Add(experience);
                db.SaveChanges();
                return RedirectToAction("CvResume");

            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }

            return RedirectToAction("CvResume");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAutre(Autre autre)
        {
            try
            {

                autre.archived = 1;
                autre.status = 1;
                autre.Ins_id = Convert.ToDecimal(Session["id"]);
                db.Autre.Add(autre);
                db.SaveChanges();
                return RedirectToAction("CvResume");

            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }

            return RedirectToAction("CvResume");
        }

        [Authorize]
        public ActionResult deleteEducation( decimal id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Education education = db.Education.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }

            db.Education.Remove(education);
            db.SaveChanges();

            return RedirectToAction("CvResume");
        }

        [Authorize]
        public ActionResult deleteExperience(decimal id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Experience experience = db.Experience.Find(id);
            if (experience == null)
            {
                return HttpNotFound();
            }

            db.Experience.Remove(experience);
            db.SaveChanges();

            return RedirectToAction("CvResume");
        }


        [Authorize]
        public ActionResult deleteAutre(decimal id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autre autre = db.Autre.Find(id);
            if (autre == null)
            {
                return HttpNotFound();
            }

            db.Autre.Remove(autre);
            db.SaveChanges();

            return RedirectToAction("CvResume");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveInformation(Information information)
        {

           decimal id = Convert.ToDecimal(Session["id"]);
            try
            {
                Information currentexist = db.Information.Where(p => p.Ins_id == id).FirstOrDefault();

                if(currentexist== null)
                {
                    information.archived = 1;
                    information.status = 1;
                    information.Ins_id = Convert.ToDecimal(Session["id"]);
                    db.Information.Add(information);
                    db.SaveChanges();
                    return RedirectToAction("CvResume");
                }
                else
                {
                    currentexist.competence = information.competence;
                    currentexist.lettre = information.lettre;

                    db.Entry(currentexist).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("CvResume");
                }

               

            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }

            return RedirectToAction("CvResume");
        }

        [Authorize]
        [AuthorizedCandidat]
        public ActionResult CvDocument()
        {
            decimal id = Convert.ToDecimal(Session["id"]);
            if (Session["id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<WebApplication1.Models.File> files = db.File.Where(p => p.Ins_id == id).ToList();

            ViewBag.listDiplome = db.File.Where(p => p.type.Equals("diplome") && p.Ins_id == id).ToList();

            ViewBag.listCv = db.File.Where(p => p.type.Equals("cv") && p.Ins_id == id).ToList();

            ViewBag.listAutre = db.File.Where(p => p.type.Equals("autre") && p.Ins_id == id).ToList();

            if (files.Count > 0)
            {
                ViewBag.listFile = files;
            }
            else
            {
                ViewBag.listFile = null;
            }

            ViewBag.ErrorFile = Request["smsError"];

            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CvDocument(WebApplication1.Models.File fileObject)
        {
            decimal id = Convert.ToDecimal(Session["id"]);
            if (Session["id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
                        var supportedTypes = new[] { ".png", ".jpg", ".txt", ".doc", ".docx", ".pdf", ".xls", ".xlsx" };
                        var extention = Path.GetExtension(filename);
                        if (!supportedTypes.Contains(extention))
                        {
                            var ErrorFile = "Echec de l'upload du documents. Extention du fichier n'est pas valide veuillez reessayer !";
                            return RedirectToAction("CvDocument", new { smsError= ErrorFile });
                        }
                        //end verification

                        var formattedFilename = string.Format("{0}-{1}{2}"
                            , Path.GetFileNameWithoutExtension(filename)
                            , Guid.NewGuid().ToString("N")
                            , Path.GetExtension(filename));
                        string path = Path.Combine(Server.MapPath("~/Fichier"),
                                                   Path.GetFileName(formattedFilename));
                        file.SaveAs(path);
                        fileObject.chemin = formattedFilename;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        return RedirectToAction("CvDocument");
                    }
                }
            }

            try
            {

                fileObject.archived = 1;
                fileObject.status = 1;
                fileObject.created = DateTime.Now;
                fileObject.Ins_id = Convert.ToDecimal(Session["id"]);
                db.File.Add(fileObject);
                db.SaveChanges();
                return RedirectToAction("CvDocument");

            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }


            return RedirectToAction("CvDocument");
        }

        [Authorize]
        public ActionResult deleteCvDocument(decimal id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WebApplication1.Models.File  file = db.File.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }

            db.File.Remove(file);
            db.SaveChanges();

            return RedirectToAction("CvDocument");
        }

        [Authorize]
        public ActionResult EditPassword()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            { 
                decimal id = (decimal) Session["id"];
                if (_me == null || id == null) return RedirectToAction("Login", "Inscrires");
                if (_me.password != model.oldPassword)
                {
                    ModelState.AddModelError("oldPassword", "Votre ancien mot de passe n'est pas valide");
                    return View();
                }
                try
                {

                    Inscrire inscrire = db.Inscrire.Find(id);

                    inscrire.password = model.password;
                    inscrire.cpassword = model.password;
                    db.Entry(inscrire).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["result_code"] = 1;
                    TempData["message"] = "Mot de passe modifié avec succès.";
                    TempData.Keep();
                    Session.Add("inscrire", inscrire);
                    if (inscrire.type == "client") return RedirectToAction("ClientProfile", "Profile");
                    return RedirectToAction("CandidatProfile", "Profile");
                }
                catch (Exception e)
                { 
                    TempData["result_code"] = -1;
                    TempData["message"] = "Une erreur inconnu est survenue.";
                    TempData.Keep();
                    return View(model);
                }

            }
            TempData["result_code"] = -1;
            TempData["message"] = "Données non valide.";
            TempData.Keep();
            return View(model);
        }

        [Authorize] 
        public ActionResult AbonnementList()
        {
            if (Session["id"] == null) return RedirectToAction("Login", "Inscrires");
            Decimal id = (Decimal)Session["id"]; 
            var datas = db.InsAbonne.Where(a => a.Ins_id == id && a.archived == 1).OrderByDescending(a => a.id).Include(f => f.Abonnement).Include(l => l.Inscrire);
            return View(datas.ToList());
        }

        [Authorize]
        public ActionResult MesTalonPaie()
        {
            decimal id = (decimal)Session["id"];
            List<Document> documents = db.Document.Where(p =>p.Ins_id == id && p.archived == 1 && p.type.Equals(Constante.typeTalonPaie)).OrderByDescending(p => p.created).ToList();
            ViewBag.listDocuments = documents;

            return View();
        }

       public ActionResult marqueLu(decimal id)
        {
            Document documents = db.Document.Where(p => p.id == id && p.archived == 1).FirstOrDefault();

            if(documents!= null)
            {
                documents.etat = 0;
                db.Entry(documents).State = EntityState.Modified;
                db.SaveChanges();

                string filePath = "~/Fichier/" + documents.chemin;
                return File(filePath, "application/pdf");
            }

            return HttpNotFound();
            
            
        }

    }
}