using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Stripe.BillingPortal;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [CustomAuthorized]
    public class AdminController : MybaseController
    {
       
        //private NiovarJobEntities db = new NiovarJobEntities();
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.nbreClient = db.Inscrire.Where(p => p.archived == 1 && p.type.Equals("client")).ToList().Count;

            ViewBag.nbreCandidat = db.Inscrire.Where(p => p.archived == 1 && p.type.Equals("candidat")).ToList().Count;

            ViewBag.nbreOffreActif = db.Postuler.Where(p => p.Job.archived == 1 && p.status == 1 && p.Inscrire.type.Equals("client")).ToList().Count;

            ViewBag.nbreOffreInactif = db.Postuler.Where(p => p.Job.archived == 1 && p.status == 0 && p.Inscrire.type.Equals("client")).ToList().Count;

            ViewBag.nbreApplication= db.Postuler.Where(p => p.archived == 1 && p.status == 1 && p.Inscrire.type.Equals("candidat")).ToList().Count;

            ViewBag.nbreCategorie = db.Categorie.Where(p => p.archived == 1).ToList().Count;

            ViewBag.nbreType = db.Types.Where(p => p.archived == 1).ToList().Count;

            ViewBag.nbreAnneExp = db.AnneeExp.Where(p => p.archived == 1).ToList().Count;

            ViewBag.nbreUtilisateur = db.Utilisateur.Where(p => p.archived == 1).ToList().Count;

            ViewBag.nbreListOffreExpirer = db.Postuler.Where(p => p.Job.archived == 1 && p.Inscrire.type == "client"  && p.Job.status == 1 && p.approbation.Equals("1") && DateTime.Compare((DateTime)p.Job.dateFinOffre, DateTime.Now) < 0).ToList().Count;

            ViewBag.nbreListOffreEnCour = db.Postuler.Where(p => p.Job.archived == 1 && p.Inscrire.type == "client" && p.Job.status == 1 && p.approbation.Equals("1") && DateTime.Compare((DateTime)p.Job.dateFinOffre, DateTime.Now) > 0).ToList().Count;

            DateTime addDate = DateTime.Now.AddDays(1);

            ViewBag.nbreListOffreExpire24h = db.Postuler.Where(p => p.Job.archived == 1 && p.Inscrire.type == "client" && p.Job.status == 1 && p.approbation.Equals("1") && DateTime.Compare((DateTime)p.Job.dateFinOffre, DateTime.Now) > 0 && DateTime.Compare((DateTime)p.Job.dateFinOffre, addDate) < 0).ToList().Count;

            ViewBag.nbreEmploye = db.Postuler.Where(p => p.archived == 1 && p.Inscrire.archived == 1 &&  p.signatures.Equals("2") &&  p.situationTravail.Equals("2") && p.Inscrire.type.Equals("candidat") && p.status==1).ToList().Count;

            ViewBag.nbreNewEmploye = db.Postuler.Where(p => p.archived == 1 && p.Inscrire.archived == 1 &&  p.signatures.Equals("2") &&  p.situationTravail.Equals("1") && p.Inscrire.type.Equals("candidat") && p.status==1).ToList().Count;
           



            return View();
        }

        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN, Constante.roleGESTIONNAIRE })]
        public ActionResult ManageOffreAdmin()
        {
           
            List<Postuler> listPostuler = db.Postuler.Where(p => p.Job.archived == 1 && p.Inscrire.type.Equals("client") && p.Job.immediat.Equals("true")).OrderByDescending(p => p.Job.created).ToList();


            List<Postuler> listResult = new List<Postuler>();

            int nbreInteresser = 0;
            foreach (Postuler postuler in listPostuler)
            {
                postuler.nbreApply = db.Postuler.Where(p => p.Job_id == postuler.Job_id && p.archived == 1  && p.Inscrire.type.Equals("candidat")).ToList().Count;
                listResult.Add(postuler);

                nbreInteresser = nbreInteresser + postuler.nbreApply;
            }

            ViewBag.listResult = listResult;

            ViewBag.nbreOffre = listPostuler.Count;
            ViewBag.nbreactive = listPostuler.Where(p => p.Job.status == 1).Count();
            ViewBag.OffreNonResolus = listPostuler.Where(p => p.etatAdmin.Equals("0")).Count();
            ViewBag.OffreResolus = listPostuler.Where(p => p.etatAdmin.Equals("2")).Count();
            ViewBag.nbreInteresser = nbreInteresser;

            if (TempData["myData"]!=null)
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

        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN, Constante.roleGESTIONNAIRE })]
        public ActionResult ManageOffrePoster()
        {
            List<Postuler> listPostuler = db.Postuler.Where(p => p.Job.archived == 1 && p.Inscrire.type.Equals("client") && p.Job.immediat.Equals("false")).OrderByDescending(p => p.Job.created).ToList();


            List<Postuler> listResult = new List<Postuler>();

            int nbreInteresser = 0;
            foreach (Postuler postuler in listPostuler)
            {
                postuler.nbreApply = db.Postuler.Where(p => p.Job_id == postuler.Job_id && p.archived == 1 && p.Inscrire.type.Equals("candidat")).ToList().Count;
                listResult.Add(postuler);

                nbreInteresser = nbreInteresser + postuler.nbreApply;
            }

            ViewBag.listResult = listResult;

            ViewBag.nbreOffre = listPostuler.Count;
            ViewBag.nbreactive = listPostuler.Where(p => p.Job.status == 1).Count();
            ViewBag.OffreNonResolus = listPostuler.Where(p => p.etatAdmin.Equals("0")).Count();
            ViewBag.OffreResolus = listPostuler.Where(p => p.etatAdmin.Equals("2")).Count();
            ViewBag.nbreInteresser = nbreInteresser;
            return View();
        }


        public ActionResult listPostulantAdmin( decimal id)
        {
            var listPostulant = db.Postuler.Where(p => p.Job_id == id && p.archived == 1 && p.Inscrire.type.Equals("candidat")).OrderByDescending(p => p.created).ToList();
            Job job = db.Job.Where(p => p.id == id && p.archived == 1).FirstOrDefault();
            ViewBag.titreJob = job.titre;

            ViewBag.attente = listPostulant.Where(p => p.etatCandidat.Equals("0")).Count();
            ViewBag.evaluation = listPostulant.Where(p => p.etatCandidat.Equals("1")).Count();
            ViewBag.aprouver = listPostulant.Where(p => p.etatCandidat.Equals("2")).Count();
            ViewBag.refuser = listPostulant.Where(p => p.etatCandidat.Equals("3")).Count();

            int talonNotexist = 0, talonExist = 0;

            foreach (var item in listPostulant )
            {
                if (item.etatCandidat.Equals("2"))
                {
                    if(item.Document.Count> 0)
                    {
                        talonExist++;
                    }
                    else
                    {
                        talonNotexist++;
                    }
                }
            }


            ViewBag.talonExist = talonExist;
            ViewBag.talonNotexist = talonNotexist;

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

            return View(listPostulant);
        }



        public ActionResult listPostulantOffrePoster(decimal id)
        {
            var listPostulant = db.Postuler.Where(p => p.Job_id == id && p.archived == 1 && p.Inscrire.type.Equals("candidat")).OrderByDescending(p => p.created).ToList();
            Job job = db.Job.Where(p => p.id == id && p.archived == 1).FirstOrDefault();
            ViewBag.titreJob = job.titre;

            ViewBag.attente = listPostulant.Where(p => p.etatCandidat.Equals("0")).Count();
            ViewBag.evaluation = listPostulant.Where(p => p.etatCandidat.Equals("1")).Count();
            ViewBag.aprouver = listPostulant.Where(p => p.etatCandidat.Equals("2")).Count();
            ViewBag.refuser = listPostulant.Where(p => p.etatCandidat.Equals("3")).Count();

            int talonNotexist = 0, talonExist = 0;

            foreach (var item in listPostulant)
            {
                if (item.etatCandidat.Equals("2"))
                {
                    if (item.Document.Count > 0)
                    {
                        talonExist++;
                    }
                    else
                    {
                        talonNotexist++;
                    }
                }
            }


            ViewBag.talonExist = talonExist;
            ViewBag.talonNotexist = talonNotexist;

            return View(listPostulant);
        }

        public ActionResult cvResumeAdmin(decimal id)
        {
            if (id == null)
            {
                String message = "Une erreur c'est produit durant l'opération veuillez reessayer !";
                return RedirectToAction("ErrorPage", "Home", new { sms = message });
            }

            Information information = db.Information.FirstOrDefault(p => p.Ins_id == id);

            Inscrire inscrire = db.Inscrire.FirstOrDefault(p => p.id == id);

            if (information == null) information = new Information();

            List<Education> education = db.Education.Where(p => p.Ins_id == id).ToList();
            if (education.Count > 0)
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
            model.education = new Education();
            model.experience = new Experience();
            model.autre = new Autre();
            model.inscrire = inscrire;


            return View(model);
        }


        public ActionResult detailsOffreAdmin(decimal id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Postuler postuler = db.Postuler.Where(p => p.Job.archived == 1 && p.Inscrire.type == "client" && p.Job.status == 1 && p.Job_id == id).FirstOrDefault();
           
            List<DiplomeJob> listDiplomeJob = db.DiplomeJob.Where(p => p.archived == 1 && p.Job_id == id).ToList();
            ViewBag.listDiplomeJob = listDiplomeJob;

            List<AvantageSociauxJob> listAvantageSociauxJob = db.AvantageSociauxJob.Where(p => p.archived == 1 && p.Job_id == id).ToList();
            ViewBag.listAvantageSociauxJob = listAvantageSociauxJob;

            return View(postuler);
        }

     
        public async Task<ActionResult> EditEtatCandidat(decimal id, decimal idJob, String status, int salFinal)
        {

            try
            {
                Postuler currentPostuler = db.Postuler.FirstOrDefault(p => p.id == id);
                if (currentPostuler == null)
                {
                    ViewBag.Message = "Votre compte n'existe pas !";
                    TempData["myData"] = "0";
                    return RedirectToAction("listPostulantAdmin", new { id=idJob });
                }

                if (status.Equals("2"))
                {
                    currentPostuler.remuneration = salFinal;
                    Postuler currentEmployeur = db.Postuler.Where(p => p.Job_id == idJob && p.Inscrire.type.Equals("client") && p.archived == 1).FirstOrDefault();

                    var map = new Dictionary<String, String>();
                    map.Add("@ViewBag.titre", "Un candidat est intérésser par votre offre de " + currentEmployeur.Job.titre);
                    map.Add("@ViewBag.login", "");
                    map.Add("@ViewBag.content", "Vous avez reçus une  candidature pour le poste de :" + currentEmployeur.Job.titre + ".veuillez suivre la candidature de ce candidat sur NioVarJobs");
                    string body = MsMail.BuldBodyTemplate("~/EmailTemplate/CustomEmail.cshtml", map);
                    MsMail mail = new MsMail();

                    await mail.Send(currentEmployeur.Inscrire.email, "Candidat intérésser", body);
                }

                currentPostuler.etatCandidat = status;

                db.Entry(currentPostuler).State = EntityState.Modified;
                db.SaveChanges();

                TempData["myData"] = "1";
                TempData["message"] = "Status modifié avec succès";

                return RedirectToAction("listPostulantAdmin", new { id = idJob });

            }
            catch (Exception)
            {
                TempData["myData"] = "0";
                return RedirectToAction("listPostulantAdmin", new { id = idJob });

            }
        }

        public ActionResult RepublierOffre( decimal idJob, DateTime dateFinOffre, string actionName)
        {

            try
            {
                Job job = db.Job.FirstOrDefault(p => p.id == idJob);
                job.dateFinOffre = dateFinOffre;
                job.created = DateTime.Now;
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction(actionName);
            }
            catch (Exception)
            {
                return RedirectToAction(actionName);
            }
        }


        public ActionResult documentsCandidat(decimal id) {


            List<WebApplication1.Models.File> files = db.File.Where(p => p.Ins_id == id).ToList();

            if (files.Count > 0)
            {
                ViewBag.listFile = files;
            }
            else
            {
                ViewBag.listFile = null;
            }
            return View();
        }


        public ActionResult talonPaieAdmin(decimal idPost, decimal idIns, decimal idJob)
        {


            List<Document> documents = db.Document.Where(p => p.Pos_id== idPost && p.Ins_id==idIns && p.Job_id== idJob).ToList();

            ViewBag.nbre = documents.Count;

            if (documents.Count > 0)
            {
                ViewBag.listDocument = documents;
            }
            else
            {
                ViewBag.listDocument = null;
            }

            ViewBag.idPost = idPost;
            ViewBag.idIns = idIns;
            ViewBag.idJob = idJob;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> addTalonPaie( Document document )
        {
            Inscrire inscrire = db.Inscrire.Where(p => p.archived == 1 && p.id == document.Ins_id).FirstOrDefault();
            if (inscrire == null) { return HttpNotFound(); }

            List<string> listFichier = new List<string>();

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        var filename = Path.GetFileName(file.FileName);

                        //verification de l'extention du fichiers
                        var supportedTypes = new[] { ".png", ".jpg", ".jpeg", ".gif", ".pdf", ".docx" };
                        var extention = Path.GetExtension(filename);
                        if (!supportedTypes.Contains(extention))
                        {
                            String message = "L'extention du fichier n'est pas valide ! Veuillez selectionnez une extention parmis les suivants  .png, .jpg, .jpeg, .gif, .pdf, .docx";
                            return RedirectToAction("ErrorPage", "Home", new { sms = message });
                        }
                        //end verification

                        var formattedFilename = string.Format("{0}-{1}{2}"
                            , Path.GetFileNameWithoutExtension(filename)
                            , Guid.NewGuid().ToString("N")
                            , Path.GetExtension(filename));
                        string path = Path.Combine(Server.MapPath("~/Fichier"),
                                                   Path.GetFileName(formattedFilename));
                        file.SaveAs(path);
                        listFichier.Add(path);

                        document.chemin = formattedFilename;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        return RedirectToAction("talonPaieAdmin", new {idPost = document.Pos_id, idIns = document.Ins_id, idJob = document.Job_id });
                    }
                }
            }

            try
            {

                document.archived = 1;
                document.status = 1;
                document.etat = 1;
                document.created = DateTime.Now;
                document.type = Constante.typeTalonPaie;
                db.Document.Add(document);

                var map = new Dictionary<String, String>();
                map.Add("@ViewBag.titre", "Confirmation reception  Talon de paie ");
                map.Add("@ViewBag.login", "");
                map.Add("@ViewBag.content", "Vous avez reçus un talon de paie veuillez consulter ce dernier ci-dessous oubien dans votre compte sur NiovarJobs");
                string body = MsMail.BuldBodyTemplate("~/EmailTemplate/CustomEmail.cshtml", map);
                MsMail mail = new MsMail();

                await mail.Send(inscrire.email, "Nouveau talon de paie pour le candida : " + inscrire.login, body, null, null, listFichier);
                db.SaveChanges();
                return RedirectToAction("talonPaieAdmin", new { idPost = document.Pos_id, idIns = document.Ins_id, idJob = document.Job_id });

            }
            catch (DbEntityValidationException ex)
            {
                return HttpNotFound();
              /*  foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }*/
            }


            return RedirectToAction("talonPaieAdmin", new { idPost = document.Pos_id, idIns = document.Ins_id, idJob = document.Job_id });
        }

        public ActionResult deleteTalonPaie(decimal id)
        {

            Document document = db.Document.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }

            db.Document.Remove(document);
            db.SaveChanges();

            return RedirectToAction("talonPaieAdmin", new { idPost = document.Pos_id, idIns = document.Ins_id, idJob = document.Job_id });

        }

        public ActionResult changeApprobation(decimal id,  String remunarationFinal,int masqueEmpla, string eqtEmp, string json)
        {
            try
            {
                var currentPostuler = db.Postuler.FirstOrDefault(p => p.id == id);
                if (currentPostuler == null)
                {
                    TempData["myData"] = "0";
                   

                    return RedirectToAction("ManageOffreAdmin");
                }

                var currentJob = db.Job.Where(p => p.id == currentPostuler.Job_id).FirstOrDefault();
                if (currentJob == null)
                {
                    TempData["myData"] = "0";
                    return RedirectToAction("ManageOffreAdmin");
                }

                if (Convert.ToDouble(remunarationFinal) > currentPostuler.Job.remuneration)
                {
                    TempData["myData"] = "1";
                    TempData["message"] = "Echec ! Le montant saisie doit être inférieur au salaire proposé ";
                    return RedirectToAction("ManageOffreAdmin");
                }

                if (currentPostuler.approbation.Equals("1"))
                {
                    currentPostuler.approbation = "0";
                    TempData["message"] = "Approbation annuler avec succès";
                }
                else
                {

                    string[] arr = json.Split(',');

                    if (arr.Length > 0)
                    {
                       for (int i = 0; i < arr.Length; i++)
                        {
                            AvantageSociauxJob avantageSociauxJob = new AvantageSociauxJob();
                            avantageSociauxJob.archived = 1;
                            avantageSociauxJob.created = DateTime.Now;
                            avantageSociauxJob.Job_id = currentJob.id;
                            avantageSociauxJob.Ava_id = Convert.ToDecimal(arr[i]);
                            db.AvantageSociauxJob.Add(avantageSociauxJob);
                        }
                    }

                    currentPostuler.Job.remunerationN = Convert.ToDouble(remunarationFinal);
                    currentPostuler.approbation = "1";
                    currentJob.equipeEmploi = eqtEmp;
                    currentJob.masquerEmplacement = masqueEmpla;

                    TempData["message"] = "Approbation confirmé avec succès";

                    db.Entry(currentPostuler).State = EntityState.Modified;

                    db.Entry(currentJob).State = EntityState.Modified;

                    db.SaveChanges();
                }

                TempData["myData"] = "1";
               
                return RedirectToAction("ManageOffreAdmin");

            }
            catch (Exception)
            {
                TempData["myData"] = "0";
              
                return RedirectToAction("ManageOffreAdmin");

            }  
        }

        public ActionResult DeleteOffre(decimal id, string page)
        {

            Job job = db.Job.Find(id);
            if (job == null)
            {
                return RedirectToAction(page,"Admin");
            }
            job.archived = 0;
            db.Entry(job).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction(page, "Admin");
        }


        public ActionResult contratTravailAdmin(decimal id, decimal idIns, decimal idJob)
        {

            Postuler postuler = db.Postuler.FirstOrDefault(p => p.id == id);
            if (postuler == null)
            {
                return HttpNotFound();
            }
            
            return View(postuler);
        }
        

        public async Task<ActionResult> changeStatutSignatures(decimal id, string val, int remuneration)
        {

            Postuler postuler = db.Postuler.FirstOrDefault(p => p.id == id);
            if (postuler == null)
            {
                return HttpNotFound();
            }

            postuler.signatures = val;
            postuler.remuneration = remuneration;
            db.Entry(postuler).State = EntityState.Modified;

            var map = new Dictionary<String, String>();
            map.Add("@ViewBag.titre",postuler.Inscrire.login +" Nouvelle opportunité pour vous ");
            map.Add("@ViewBag.login", "");
            map.Add("@ViewBag.content", "Votre candidature au poste de "+postuler.Job.titre+" à été accepté vous êtes convié à signer votre contrat recus via le dashbaord  sur NioVarJobs");
            string body = MsMail.BuldBodyTemplate("~/EmailTemplate/CustomEmail.cshtml", map);
            MsMail mail = new MsMail();

            await mail.Send(postuler.Inscrire.email, "Candidature accepté", body);
            db.SaveChanges();
            return RedirectToAction("listPostulantAdmin", new { id = postuler.Job_id });
        }

        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN })]
        public ActionResult ManageEmploye()
        {
            List<Postuler> listEmployes = db.Postuler.Where(p => p.archived == 1 && p.Inscrire.archived == 1 &&  p.signatures.Equals("2") && p.situationTravail.Equals("1") || p.situationTravail.Equals("2") && p.Inscrire.type.Equals("candidat") && p.status==1).ToList();
            ViewBag.listEmployes = listEmployes;
            return View();
        }


        public ActionResult changeStatusPoste(decimal id, String val)
        {
            Postuler postuler = db.Postuler.FirstOrDefault(p => p.id == id);
            if (postuler == null)
            {
                return HttpNotFound();
            }
            postuler.situationTravail= val;
            db.Entry(postuler).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ManageEmploye");
        }

        
        [CustomAuthorized]
        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN, Constante.roleJOB_MANAGER })]

        public ActionResult offreNiovarJobs()
        {
            
            List<Postuler> listPostuler = db.Postuler.Where(p => p.Job.archived == 1 && p.Inscrire.type.Equals("client") && p.Job.immediat.Equals("direct")).OrderByDescending(p => p.created).ToList();


            List<Postuler> listResult = new List<Postuler>();

            int nbreInteresser = 0;
            foreach (Postuler postuler in listPostuler)
            {
                postuler.nbreApply = db.Postuler.Where(p => p.Job_id == postuler.Job_id && p.archived == 1 && p.Inscrire.type.Equals("candidat")).ToList().Count;
                listResult.Add(postuler);

                nbreInteresser = nbreInteresser + postuler.nbreApply;
            }

            ViewBag.listResult = listResult.OrderByDescending(p => p.created).ToList();

            return View();
        }


        public ActionResult listPaiementAdmin()
        {
            List<Paiement> listPaiement = db.Paiement.Where(p => p.archived == 1).OrderByDescending(p => p.created).ToList();
            ViewBag.listPaiement = listPaiement;
            return View();
        }

        public ActionResult listFraisLocationAdmin()
        {
            List<FraisLocation> listFraisLocation = db.FraisLocation.Where(p => p.archived == 1).OrderByDescending(p => p.created).ToList();
            ViewBag.listFraisLocation = listFraisLocation;
            return View();
        }

    }
}