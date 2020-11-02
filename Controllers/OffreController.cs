
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Data.Entity;
using System.Net;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using Stripe;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    public class OffreController : MybaseController
    {


        //private NiovarJobEntities db = new NiovarJobEntities();
        // GET: Offre
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult PostOffre()
        {
            if (Session.Count > 0 && Session["status"] != null)
            {
                decimal status = Convert.ToDecimal(Session["status"]);

                if (Session["type"] != null && Session["type"].Equals("client"))
                {
                    if (status == 0)
                    {
                        String message = "Impossible de publier une offre. Vérifier que vos informations sont bien renseignez dans votre profil  et attendez l'activation de votre profil par les administrateurs NiovarJobs. Sinon contactez nous ";

                        return RedirectToAction("ErrorPage", "Home", new { sms = message });
                    }

                    if (needAbonnement()) return RedirectToAction("Abonnement", "Inscrires");
                    decimal id = Convert.ToDecimal(Session["id"]);
                    List<Postuler> listPostuler = db.Postuler.Where(p => p.Ins_id == id && p.Job.archived == 1 && p.Inscrire.type.Equals("client")).OrderByDescending(p => p.Job.created).ToList();

                    ViewBag.nbreOffre = listPostuler.Count;
                    ViewBag.nbreactive = listPostuler.Where(p => p.Job.status == 1).Count();



                    return View();

                }
                else
                {
                    String message = "Vous n'avez pas accès cette page. Cette page est reservé aux entreprises ayant besoin de main d'oeuvre";

                    return RedirectToAction("ErrorPage", "Home", new { sms = message });
                }
            }
            else
            {
                return RedirectToAction("Login", "Inscrires");
            }

        }

        [Authorize]
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostOffre(Job job)
        {
            if (job.immediat.Equals("false") && needAbonnement()) return RedirectToAction("Abonnement", "Inscrires");

            

            if (ModelState.IsValid)
            {
                try
                {
                    string daysWork = "";
                    for (int i = 0; i < job.workDay.Length; i++)
                    {
                        daysWork += job.workDay[i].ToString() + ",";
                    }
                    daysWork = daysWork.TrimEnd(',');

                    job.jourTravail = daysWork;

                    job.status = 1;
                    job.archived = 1;
                    job.etat = "1";
                    job.created = DateTime.Now;

                    db.Job.Add(job);
                    db.SaveChanges();

                    for (int i = 0; i < job.listDip.Length; i++)
                    {
                        DiplomeJob diplomeJob = new DiplomeJob();
                        diplomeJob.archived = 1;
                        diplomeJob.created = DateTime.Now;
                        diplomeJob.Job_id = job.id;
                        diplomeJob.Dip_id = Convert.ToDecimal(job.listDip[i]);
                        db.DiplomeJob.Add(diplomeJob);
                    }

                    if (job.listAvantage.Length > 0)
                    {
                        for (int i = 0; i < job.listAvantage.Length; i++)
                        {
                            AvantageSociauxJob avantageSociauxJob = new AvantageSociauxJob();
                            avantageSociauxJob.archived = 1;
                            avantageSociauxJob.created = DateTime.Now;
                            avantageSociauxJob.Job_id = job.id;
                            avantageSociauxJob.Ava_id = Convert.ToDecimal(job.listAvantage[i]);
                            db.AvantageSociauxJob.Add(avantageSociauxJob);
                        }

                    }

                    db.SaveChanges();



                    Postuler postuler = new Postuler();
                    postuler.Job_id = job.id;
                    postuler.Ins_id = Convert.ToDecimal(Session["id"]);
                    postuler.created = DateTime.Now;
                    postuler.etatAdmin = "0";
                    postuler.etatClient = "0";
                    postuler.etatCandidat = "0";
                    postuler.etat = "0";
                    postuler.signatures = "0";
                    postuler.signatureClient = 0;
                    postuler.approbation = "0";
                    postuler.situationTravail = "0";

                    if (job.immediat.Equals("true"))
                    {
                        postuler.approbation = "0";

                    }
                    else
                    {
                        postuler.approbation = "1";

                        if (lastAsbonnement != null)
                        {
                            InsAbonne currentInsAbonnement = db.InsAbonne.Where(p => p.id == lastAsbonnement.id).First();
                            currentInsAbonnement.etat -= 1;
                            currentInsAbonnement.nbrePost -= 1;
                            db.Entry(currentInsAbonnement).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            return RedirectToAction("Abonnement", "Inscrires");
                        }


                    }

                    postuler.situationTravail = "0";
                    postuler.archived = 1;


                    db.Postuler.Add(postuler);
                    db.SaveChanges();

                    TempData["result_code"] = 1;
                    TempData["message"] = "Offre d'emploi publié avec succès.";
                    TempData.Keep();

                    return RedirectToAction("ManagerOffre");
                }
                catch (Exception e)
                {
                    ViewBag.Error = e.Message.ToString();

                    TempData["result_code"] = -1;
                    TempData["message"] = "Données non valide. veuillez renseignez tous les champs obligatoire (*)";
                    TempData.Keep();
                    return View(job);


                }
            }

            TempData["result_code"] = -1;
            TempData["message"] = "Données non valide. veuillez renseignez tous les champs";
            TempData.Keep();
            return View(job);

        }


        [Authorize]
        [AuthorizedClient]
        public ActionResult ManagerOffre()
        {
            decimal id = Convert.ToDecimal(Session["id"]);

            ViewBag.Message = "Your PostJob page.";

            List<Postuler> listPostuler = db.Postuler.Where(p => p.Ins_id == id && p.Job.archived == 1 && p.Inscrire.type.Equals("client") && p.Job.immediat.Equals("false") ).OrderByDescending(p => p.Job.created).ToList();

            List<Postuler> listResult = new List<Postuler>();

            int nbreInteresser = 0;
            foreach (Postuler postuler in listPostuler)
            {
                postuler.nbreApply = db.Postuler.Where(p => p.Job_id == postuler.Job_id && p.archived == 1 && p.Inscrire.type.Equals("candidat")).ToList().Count;
                postuler.nbreAccept = db.Postuler.Where(p => p.Job_id == postuler.Job_id && p.archived == 1 && p.Inscrire.type.Equals("candidat") && p.etatCandidat.Equals("2")).ToList().Count;
                listResult.Add(postuler);

                nbreInteresser = nbreInteresser + postuler.nbreApply;
            }

            ViewBag.listResult = listResult;

            ViewBag.nbreOffre = listPostuler.Count;
            ViewBag.nbreOffreActive = listPostuler.Where(p => p.Job.status == 1 && p.Job.immediat.Equals("false")).Count();
            ViewBag.nbreOffreInactive = listPostuler.Where(p => p.Job.status == 0 && p.Job.immediat.Equals("false")).Count(); 
            ViewBag.nbreInteresser = nbreInteresser;

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

        [Authorize]
        [AuthorizedClient]
        public ActionResult ManageMainOeuvre()
        {
            decimal id = Convert.ToDecimal(Session["id"]);

            ViewBag.Message = "Your PostJob page.";

            List<Postuler> listPostuler = db.Postuler.Where(p => p.Ins_id == id && p.Job.archived == 1 && p.Inscrire.type.Equals("client") && p.Job.immediat.Equals("true")).OrderByDescending(p => p.Job.created).ToList();

            List<Postuler> listResult = new List<Postuler>();

            int nbreInteresser = 0;
            foreach (Postuler postuler in listPostuler)
            {
                postuler.nbreApply = db.Postuler.Where(p => p.Job_id == postuler.Job_id && p.archived == 1 && p.Inscrire.type.Equals("candidat")).ToList().Count;
                postuler.nbreAccept = db.Postuler.Where(p => p.Job_id == postuler.Job_id && p.archived == 1 && p.Inscrire.type.Equals("candidat") && p.etatCandidat.Equals("2")).ToList().Count;
                listResult.Add(postuler);

                nbreInteresser = nbreInteresser + postuler.nbreApply;
            }

            ViewBag.listResult = listResult;

            ViewBag.nbreOffre = listPostuler.Count;
            ViewBag.nbreLocationActive = listPostuler.Where(p => p.Job.status == 1 && p.Job.immediat.Equals("true")).Count();
            ViewBag.nbreLocationInactive = listPostuler.Where(p => p.Job.status == 0 && p.Job.immediat.Equals("true")).Count();
            ViewBag.nbreInteresser = nbreInteresser;

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


        [HttpGet]
        public JsonResult GetAllAnneeExperience(decimal id)
        {

            List<CatAnneeExp> catAnneeExps = db.CatAnneeExp.Where(p => p.Cat_id == id && p.prixHoraire != null).ToList();
            var subCategoryToReturn = catAnneeExps.Select(S => new {
                Cat_id = S.Cat_id,
                Ann_id = S.Ann_id,
                Last_Name = S.id,
                prixHoraire = S.prixHoraire,
                archived = S.archived,
                status = S.status
            });
            return Json(subCategoryToReturn, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public JsonResult GetAllTitreEmploi(decimal id)
        {

            List<Categorie> listCategorie = db.Categorie.Where(p => p.Typ_id == id && p.archived == 1).ToList();
            var subCategoryToReturn = listCategorie.Select(S => new {
                id = S.id,
                Typ_id = S.Typ_id,
                libelle = S.libelle,
                image = S.image,
                archived = S.archived,
                status = S.status
            });
            return Json(subCategoryToReturn, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAllInfosInscrire(decimal id)
        {
            List<Inscrire> listInscrire = db.Inscrire.Where(p => p.archived == 1 && p.status == 1 && p.id == id).ToList();
            var subCategoryToReturn = listInscrire.Select(S => new {
                id = S.id,
                email = S.email,
                adresse = S.adresse,
                phone = S.phone,
                nom = S.nom
            });
            return Json(subCategoryToReturn, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetAll(decimal id)
        {
            //  db.Configuration.ProxyCreationEnabled = false;
            List<AnneeExp> anneeExps = db.AnneeExp.Where(p => p.libelle != null).ToList();
            var subCategoryToReturn = anneeExps.Select(S => new {
                id = S.id,
                libelle = S.libelle,
                archived = S.archived,
                status = S.status
            });
            return Json(subCategoryToReturn, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public ActionResult deleteOffre(decimal id, string actionName)
        {

            try
            {
                var currentJob = db.Job.FirstOrDefault(p => p.id == id);
                if (currentJob == null)
                {
                    ViewBag.Message = "Votre compte n'existe pas !";

                    return RedirectToAction(actionName);
                }

                currentJob.archived = 0;

                db.Entry(currentJob).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.Message = "modification éffectuer avec succès avec succès !";

                return RedirectToAction(actionName);

            }
            catch (Exception)
            {
                return RedirectToAction(actionName);

            }
        }

        [Authorize]
        public ActionResult EditStatus(decimal id, string actionName)
        {
            var currentJob = db.Job.FirstOrDefault(p => p.id == id);
            try
            {

                if (currentJob == null)
                {
                    ViewBag.Message = "Votre compte n'existe pas !";

                    return RedirectToAction(actionName);
                }

                if (currentJob.status == 1)
                {
                    currentJob.status = 0;
                    TempData["message"] = "Vous avez annulé une offre publié avec succès !";
                }
                else
                {

                    currentJob.status = 1;
                    TempData["message"] = "Offre activé avec succès !";
                }

                db.Entry(currentJob).State = EntityState.Modified;
                db.SaveChanges();
                TempData["myData"] = "1";


                if (currentJob.immediat.Equals("direct")) {
                    return RedirectToAction("offreNiovarJobs", "Admin");
                }
                else {
                    return RedirectToAction(actionName);
                }


            }
            catch (Exception)
            {
                TempData["myData"] = "0";

                if (currentJob.immediat.Equals("direct"))
                {
                    return RedirectToAction("offreNiovarJobs", "Admin");
                }
                else {
                    return RedirectToAction(actionName);
                }

            }
        }

        [Authorize]
        public ActionResult DetailsOffre(decimal id)
        {
            Job job = db.Job.Find(id);
            List<DiplomeJob> listDiplomeJob = db.DiplomeJob.Where(p => p.archived == 1 && p.Job_id == id).ToList();
            ViewBag.listDiplomeJob = listDiplomeJob;

            List<AvantageSociauxJob> listAvantageSociauxJob = db.AvantageSociauxJob.Where(p => p.archived == 1 && p.Job_id == id).ToList();
            ViewBag.listAvantageSociauxJob = listAvantageSociauxJob;
            return View(job);
        }

        [Authorize]
        [AuthorizedCandidat]
        public ActionResult MesCandidatures()
        {
            decimal id = Convert.ToDecimal(Session["id"]);

            ViewBag.Message = "Your PostJob page.";

            var listPostuler = db.Postuler.Where(p => p.Ins_id == id && p.Job.archived == 1 && p.archived == 1 && p.Inscrire.type.Equals("candidat")).OrderByDescending(p => p.created).ToList();

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

            return View(listPostuler);
        }

        [Authorize]
        public ActionResult DeleteCandidatures(decimal id)
        {
            try
            {
                var current = db.Postuler.FirstOrDefault(p => p.id == id);
                if (current == null)
                {
                    String message = "Votre compte n'existe pas ! veuillez vous connectez ou  créer un compte";
                    return RedirectToAction("ErrorPage", "Home", new { sms = message });
                }

                current.archived = 0;

                db.Entry(current).State = EntityState.Modified;
                db.SaveChanges();


                return RedirectToAction("MesCandidatures");

            }
            catch (Exception)
            {
                String message = "Une erreur c'est produit durant l'opération veuillez reessayer !";
                return RedirectToAction("ErrorPage", "Home", new { sms = message });

            }
        }

        [Authorize]
        public ActionResult ListPostulant(decimal id)
        {
            Job job = db.Job.Where(p => p.id == id && p.archived == 1).FirstOrDefault();
            ViewBag.titreJob = job.titre;

            if (job.immediat.Equals("true"))
            {
                var listPostulant = db.Postuler.Where(p => p.Job_id == id && p.archived == 1 && p.Inscrire.type.Equals("candidat") && p.etatCandidat.Equals("2")).OrderByDescending(p => p.created).ToList();
                return View(listPostulant);
            }
            else
            {
                var listPostulant = db.Postuler.Where(p => p.Job_id == id && p.archived == 1 && p.Inscrire.type.Equals("candidat")).OrderByDescending(p => p.created).ToList();
                return View(listPostulant);
            }

        }

        [Authorize]
        public ActionResult CvResumePostulant(decimal id, decimal id_job)
        {
            if (id == null)
            {
                String message = "Une erreur c'est produit durant l'opération veuillez reessayer !";
                return RedirectToAction("ErrorPage", "Home", new { sms = message });
            }
            Postuler postuler = db.Postuler.Where(p => p.Ins_id == id && p.Job_id == id_job).FirstOrDefault();
            postuler.etat = "1";
            ViewBag.etatClient = postuler.etatClient;
            ViewBag.immediat = postuler.Job.immediat;
            db.Entry(postuler).State = EntityState.Modified;
            db.SaveChanges();

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

        [Authorize]
        public ActionResult TrouverEmployer()
        {
            if (Session.Count > 0)
            {
                decimal status = Convert.ToDecimal(Session["status"]);

                if (Session["type"].Equals("client"))
                {
                    if (status == 0)
                    {
                        String message = "Impossible de publier une offre. Vérifier que vos informations sont bien renseignez dans votre profil  et attendez l'activation de votre profil par les administrateurs NiovarJobs. Sinon contactez nous ";

                        return RedirectToAction("ErrorPage", "Home", new { sms = message });
                    }

                    List<Types> listTypes = db.Types.ToList();
                    List<Categorie> listCategorie = db.Categorie.ToList();

                    ViewBag.listTypes = listTypes;
                    ViewBag.listCategorie = listCategorie;

                    return View();

                }
                else
                {
                    String message = "Vous n'avez pas accès cette page. Cette page est reservé aux entreprise ayant besoin de main d'oeuvre";

                    return RedirectToAction("ErrorPage", "Home", new { sms = message });
                }
            }
            else
            {
                return RedirectToAction("Login", "Inscrires");
            }

        }

        [Authorize]
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TrouverEmployer(Job job)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    string daysWork = "";
                    for (int i = 0; i < job.workDay.Length; i++)
                    {
                        daysWork += job.workDay[i].ToString() + ",";
                    }
                    daysWork = daysWork.TrimEnd(',');

                    job.jourTravail = daysWork;
                    
                    job.status = 1;
                    job.archived = 1;
                    job.etat = "1";
                    job.created = DateTime.Now;
                    job.dateFinOffre = job.dateFin;

                    db.Job.Add(job);
                    db.SaveChanges();

                    if (job.listDip.Length > 0)
                    {
                        for (int i = 0; i < job.listDip.Length; i++)
                        {
                            DiplomeJob diplomeJob = new DiplomeJob();
                            diplomeJob.archived = 1;
                            diplomeJob.created = DateTime.Now;
                            diplomeJob.Job_id = job.id;
                            diplomeJob.Dip_id = Convert.ToDecimal(job.listDip[i]);
                            db.DiplomeJob.Add(diplomeJob);
                        }
                    }



                    db.SaveChanges();



                    Postuler postuler = new Postuler();
                    postuler.Job_id = job.id;
                    postuler.Ins_id = Convert.ToDecimal(Session["id"]);
                    postuler.created = DateTime.Now;
                    postuler.etatAdmin = "0";
                    postuler.etatClient = "0";
                    postuler.etatCandidat = "0";
                    postuler.etat = "0";
                    postuler.signatures = "0";
                    postuler.signatureClient = 0;
                    postuler.approbation = "0";


                    postuler.situationTravail = "0";

                    postuler.approbation = "0";



                    postuler.situationTravail = "0";
                    postuler.archived = 1;


                    db.Postuler.Add(postuler);
                    db.SaveChanges();

                    TempData["result_code"] = 1;
                    TempData["message"] = "Votre Demande a été envoyé succès.";
                    TempData.Keep();

                    return RedirectToAction("ManageMainOeuvre");
                }
                catch (Exception e)
                {
                    ViewBag.Error = e.Message.ToString();

                    TempData["result_code"] = -1;
                    TempData["message"] = "Données non valide. veuillez renseignez tous les champs obligatoire (*)";
                    TempData.Keep();
                    return View(job);


                }
            }

            TempData["result_code"] = -1;
            TempData["message"] = "Données non valide. veuillez renseignez tous les champs";
            TempData.Keep();
            return View(job);

        }

        [Authorize]
        [AuthorizedClient]
        public ActionResult TakeDecision(decimal idPostuler, String val)
        {
            try
            {
                var currentPostuler = db.Postuler.FirstOrDefault(p => p.id == idPostuler);
                if (currentPostuler == null)
                {

                    return HttpNotFound();
                }

                if (currentPostuler.Job.immediat.Equals("true"))
                {
                    currentPostuler.etatClient = val;
                    currentPostuler.signatureClient = 0;
                }
                else
                {
                    currentPostuler.etatCandidat = val;
                }

                db.Entry(currentPostuler).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.Message = "modification éffectuer avec succès  !";

                return RedirectToAction("ListPostulant", new { id = currentPostuler.Job_id });

            }
            catch (Exception)
            {
                return HttpNotFound();

            }
        }

        [Authorize]
        [AuthorizedCandidat]
        public ActionResult contratTravailCandidat(decimal id, decimal idIns, decimal idJob)
        {

            Postuler postuler = db.Postuler.FirstOrDefault(p => p.id == id);
            if (postuler == null)
            {
                return HttpNotFound();
            }

            return View(postuler);
        }

        [Authorize]
        public async Task<ActionResult> confirmContratCandidat(decimal id)
        {

            Postuler postuler = db.Postuler.FirstOrDefault(p => p.id == id);
            if (postuler == null)
            {
                return HttpNotFound();
            }

            postuler.signatures = "2";
            postuler.situationTravail = "1";
            postuler.dateSignatures = DateTime.Now;
            db.Entry(postuler).State = EntityState.Modified;

            var map = new Dictionary<String, String>();
            map.Add("@ViewBag.titre", "Validation engagement de travail " );
            map.Add("@ViewBag.login", "");
            map.Add("@ViewBag.content", "Le candidat " + postuler.Inscrire.login+ " à lus et confirmer la signature de son engagement de travail pour le poste de " + postuler.Job.titre + ".veuillez suivre la candidature de ce candidat sur NioVarJobs");
            string body = MsMail.BuldBodyTemplate("~/EmailTemplate/CustomEmail.cshtml", map);
            MsMail mail = new MsMail();

            await mail.Send(Constante.mailCompagny, "Contrat de travail signé et valider", body);
            db.SaveChanges();
            return RedirectToAction("MesCandidatures");
        }

        [Authorize]
        public ActionResult addEntrevue(decimal idPostuler, String dateEntrevue, String heure, String duree, String details, String outils)
        {

            try
            {
                var currentPostuler = db.Postuler.FirstOrDefault(p => p.id == idPostuler);
                if (currentPostuler == null)
                {

                    return HttpNotFound();
                }

                currentPostuler.dateEntrevue = Convert.ToDateTime(dateEntrevue);
                currentPostuler.heure = heure;
                currentPostuler.duree = duree;
                currentPostuler.responsableEntrevue = details;
                currentPostuler.outils = outils;
                currentPostuler.etatCandidat = "4";

                db.Entry(currentPostuler).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.Message = "modification éffectuer avec succès  !";

                return RedirectToAction("ListPostulant", new { id = currentPostuler.Job_id });

            }
            catch (Exception)
            {
                return HttpNotFound();

            }
        }

        [Authorize]
        [AuthorizedClient]
        public ActionResult CvDocumentPostulant(decimal id)
        {

            if (Session["id"] == null)
            {
                return HttpNotFound();
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
        [AuthorizedClient]
        public ActionResult contratClient(decimal idPostuler)
        {
            if (Session["id"] != null)
            {
                decimal idCurrent = (decimal)Session["id"];
                Inscrire inscritCurrent = db.Inscrire.FirstOrDefault(p => p.id == idCurrent);
                if (inscritCurrent == null)
                {
                    return HttpNotFound();
                }

                ViewBag.inscritCurrent = inscritCurrent;

                Postuler postuler = db.Postuler.FirstOrDefault(p => p.id == idPostuler);
                if (postuler == null)
                {
                    return HttpNotFound();
                }

                Session.Add("postuler", postuler);
                return View(postuler);
            }
            else
            {
                return HttpNotFound();
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> contratClient(String stripeToken)
        {
            Postuler currentPostuler = (Postuler)Session["postuler"];
            if (currentPostuler == null || _me == null || _me.type != "client")
            {
                TempData["result_code"] = -1;
                TempData["message"] = "Temps fourni pour le payment est expiré veuillez réessayer.";
                TempData.Keep();
                //TODO rediriger a la liste des candidatures
                return RedirectToAction("Index", "Home");
            }
            if (String.IsNullOrEmpty(stripeToken)) return HttpNotFound();
            StripeConfiguration.SetApiKey(STRIPE_API_KEY);

            var stripeOptions = new ChargeCreateOptions
            {

                Amount = (long)Math.Ceiling(currentPostuler.Job.remuneration.Value) * 25 / 100, // 1 dollar is equal to 100 cent. 
                Currency = "USD",
                Description = "Charge for payment of 25% of" + currentPostuler.libelle,
                Source = stripeToken,
                //Customer = customer.Id
            };
            var service = new ChargeService();

            if (Session["id"] == null)
            {
                return HttpNotFound();
            }
            int idUserSession = Convert.ToInt32(Session["id"]);

            Postuler postuler = db.Postuler.FirstOrDefault(p => p.id == currentPostuler.id);
            if (postuler == null) { return HttpNotFound(); }
            postuler.etatClient = "2";
            postuler.signatureClient = 1;
            postuler.dateSignClient = DateTime.Now;
            db.Entry(postuler).State = EntityState.Modified;

            double amount = postuler.Job.remuneration.Value * 25 / 100;
            Paiement paiement = new Paiement();
            paiement.archived = 1;
            paiement.avance = amount;
            paiement.created = DateTime.Now;
            paiement.etat = 0; //TODO mettre le bon etat
            paiement.Ins_id = postuler.Ins_id;
            paiement.Job_id = postuler.Job_id;
            paiement.libelle = "Paiement de 25% de l'offre ";
            paiement.montant = postuler.Job.remuneration.Value;
            paiement.Pos_id = postuler.id;
            paiement.userId = idUserSession;
            paiement.reste = postuler.Job.remuneration - amount;
            paiement.status = 0; //TODO mettre le bon status je suppose o veut dire non achevé  

            db.Paiement.Add(paiement);

            try
            {
                Charge charge = service.Create(stripeOptions);

                var map = new Dictionary<String, String>();
                map.Add("@ViewBag.titre", "Paiement de 25% du salaire via stripe " + currentPostuler.libelle);
                map.Add("@ViewBag.login", _me.login);
                map.Add("@ViewBag.content", "Votre paiement a bien été pris en compte. Le montant à été prélever de votre compte avec succès. Reste à payer :"+ paiement.reste +". Bénéficiez du meilleur de nos service.");
                string body = MsMail.BuldBodyTemplate("~/EmailTemplate/CustomEmail.cshtml", map);
                MsMail mail = new MsMail();

                db.SaveChanges();
                List<string> mylistmail = new List<string>(new string[] { _me.email, Constante.mailCompagny });
                //TODO retraviller le mail avec du HTML 
                await mail.Send(mylistmail, "Paiement frais location candidat", body);
                //TODO rediriger a la liste des candidatures
                return RedirectToAction("ListPostulant", new { id = postuler.Job_id });
            }
            catch (StripeException e)
            {

            }
            ViewBag.inscritCurrent = _me;
            return View(currentPostuler);
        }


        [Authorize]
        public ActionResult EditPublicationOffre(decimal id)
        {
            Job job = db.Job.FirstOrDefault(p => p.id == id);
            if (job == null)
            {
                return HttpNotFound();
            }
            ViewBag.pays = job.pays;
            ViewBag.province = job.province;
            ViewBag.ville = job.ville;
            return View(job);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPublicationOffre(Job job)
        {
            Job currentJob = db.Job.FirstOrDefault(p => p.id == job.id);

            if (currentJob == null)
            {
                return HttpNotFound();
            }

            try
            {
                currentJob.titre = job.titre;
                currentJob.description = job.description;
                currentJob.responsabilite = job.responsabilite;
                currentJob.exigence = job.exigence;
                currentJob.pays = job.pays;
                currentJob.province = job.province;
                currentJob.ville = job.ville;
                currentJob.typeJob = job.typeJob;
                currentJob.niveauEtude = job.niveauEtude;
                currentJob.dateEntre = job.dateEntre;
                currentJob.nbreEmploye = job.nbreEmploye;

                db.Entry(currentJob).State = EntityState.Modified;
                db.SaveChanges();

                TempData["result_code"] = 1;
                TempData["message"] = "Modification éffectué avec succès.";
                TempData.Keep();

                return RedirectToAction("ManagerOffre");
            }
            catch (Exception e)
            {
                TempData["result_code"] = -1;
                TempData["message"] = "Echec de la modification. veuillez renseignez tous les champs";
                TempData.Keep();
                return RedirectToAction("EditPublicationOffre", new { id = job.id });
            }

        }


        // GET: ReadJson  
        public JsonResult ListeVilleProvince()
        {
            string id = !string.IsNullOrEmpty(Request["id"]) ? Convert.ToString(Request["id"]) : null;

            string json = System.IO.File.ReadAllText(Server.MapPath("~/Json/states_cities.json"));
            List<State> list = JsonConvert.DeserializeObject<List<State>>(json);
            if (id == null)
            {
                var jsonResult = Json(new { result_code = 1, datas = list }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            foreach (var item in list)
            {
                if (item.state_code == id)
                {
                    var jsonResult = Json(new { result_code = 1, datas = item.cities }, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
            }
            return Json(new { id = id, count = list.Count, result_code = -1, message = "An error occured" }, JsonRequestBehavior.AllowGet);

        }


        [CustomAuthorized]
        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN, Constante.roleJOB_MANAGER })]

        public ActionResult publierOffreAdmin()
        {

            return View();
        }

        [CustomAuthorized]
        [AuthorizedAccessAction(roleAccess = new string[] {Constante.roleADMIN,Constante.roleJOB_MANAGER })]
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult publierOffreAdmin(Job job)
        {
           Inscrire currentUser = db.Inscrire.Where(x => x.email == job.courriel || x.nom == job.nomEntreprise && x.archived == 1).FirstOrDefault();

           

            if (ModelState.IsValid)
            {
                try
                {
                    string daysWork = "";
                    for (int i = 0; i < job.workDay.Length; i++)
                    {
                        daysWork += job.workDay[i].ToString() + ",";
                    }
                    daysWork = daysWork.TrimEnd(',');

                    job.jourTravail = daysWork;

                    job.status = 1;
                    job.archived = 1;
                    job.etat = "1";
                    job.created = DateTime.Now;
                    job.immediat = "direct";

                    db.Job.Add(job);
                    db.SaveChanges();

                    for (int i = 0; i < job.listDip.Length; i++)
                    {
                        DiplomeJob diplomeJob = new DiplomeJob();
                        diplomeJob.archived = 1;
                        diplomeJob.created = DateTime.Now;
                        diplomeJob.Job_id = job.id;
                        diplomeJob.Dip_id = Convert.ToDecimal(job.listDip[i]);
                        db.DiplomeJob.Add(diplomeJob);
                    }

                    if (job.listAvantage.Length > 0)
                    {
                        for (int i = 0; i < job.listAvantage.Length; i++)
                        {
                            AvantageSociauxJob avantageSociauxJob = new AvantageSociauxJob();
                            avantageSociauxJob.archived = 1;
                            avantageSociauxJob.created = DateTime.Now;
                            avantageSociauxJob.Job_id = job.id;
                            avantageSociauxJob.Ava_id = Convert.ToDecimal(job.listAvantage[i]);
                            db.AvantageSociauxJob.Add(avantageSociauxJob);
                        }

                    }

                    db.SaveChanges();

                    decimal id;

                    if (currentUser != null)
                    {
                        id = currentUser.id;
                      
                    }
                    else
                    {
                        Inscrire inscrire = new Inscrire();
                        inscrire.type = Constante.typeclient;
                        inscrire.nom = job.nomEntreprise;
                        inscrire.login = job.nomEntreprise;
                        inscrire.adresse = job.adresse;
                        inscrire.phone = job.telephone;
                        inscrire.email = job.courriel;
                        inscrire.emailProf = job.courriel;
                        inscrire.password = Constante.defaultPwd;
                        inscrire.cpassword = inscrire.password;
                        inscrire.etat = 1;
                        inscrire.status = 1;
                        inscrire.archived = 1;
                        db.Inscrire.Add(inscrire);
                        db.SaveChanges();

                        id = inscrire.id;
                    }

                    Postuler postuler = new Postuler();
                    postuler.Job_id = job.id;
                    postuler.Ins_id = id;
                    postuler.created = DateTime.Now;
                    postuler.etatAdmin = "0";
                    postuler.etatClient = "0";
                    postuler.etatCandidat = "0";
                    postuler.etat = "0";
                    postuler.signatures = "0";
                    postuler.signatureClient = 0;
                    postuler.approbation = "1";
                    postuler.situationTravail = "0";
                    

                    postuler.archived = 1;


                    db.Postuler.Add(postuler);
                    db.SaveChanges();

                    TempData["result_code"] = 1;
                    TempData["message"] = "Offre d'emploi publié avec succès.";
                    TempData.Keep();

                    return RedirectToAction("offreNiovarJobs","Admin");
                }
                catch (Exception e)
                {
                    ViewBag.Error = e.Message.ToString();

                    TempData["result_code"] = -1;
                    TempData["message"] = "Données non valide. veuillez renseignez tous les champs obligatoire (*)";
                    TempData.Keep();
                    return View(job);
                }
            }

            TempData["result_code"] = -1;
            TempData["message"] = "Données non valide. veuillez renseignez tous les champs";
            TempData.Keep();
            return View(job);

        }
    }
}