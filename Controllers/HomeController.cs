using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;

using WebApplication1.Models;
using X.PagedList;

namespace WebApplication1.Controllers
{
    public class HomeController : MybaseController
    {
        //NiovarJobEntities db = new NiovarJobEntities();
        public async System.Threading.Tasks.Task<ActionResult> Index()
        { 
            List<Types> listTypes = db.Types.Where(p => p.archived == 1 ).ToList();

            List<Postuler> listJobs = db.Postuler.Where(p => p.Job.archived == 1 && p.Inscrire.type == "client" && p.Job.status == 1 && p.approbation.Equals("1") && DateTime.Compare((DateTime)p.Job.dateFinOffre, DateTime.Now) > 0).Take(100).ToList();

            //List<Job> listJobs = db.Job.Where(p => p.archived == 1 && p.status==1  && DateTime.Compare((DateTime)p.dateFinOffre, DateTime.Now) > 0).Take(100).ToList();

            List<Types> newlistTypes = new List<Types>();

            foreach (Types types in listTypes)
            {
                int i = 0;
                foreach (Postuler job in listJobs)
                {
                    if(job.Job.Categorie.Typ_id == types.id)
                    {
                        i++;
                    }
                }
                    types.nbreOffre = i;
                    newlistTypes.Add(types);

            }

            ViewBag.newlistTypes = newlistTypes.Take(8);

            List<Postuler> listPostuler = db.Postuler.Where(p => p.Job.archived == 1 && p.Inscrire.type == "client" && p.Job.status == 1 && p.approbation.Equals("1") && DateTime.Compare((DateTime) p.Job.dateFinOffre, DateTime.Now) > 0 ).OrderByDescending(p => p.Job.created).ToList();
            ViewBag.listPostuler = listPostuler.Take(4);

            // var listVille = db.Job.Where(p => p.archived == 1 && p.status == 1).GroupBy(p => new { p.ville }).ToList();
            List<Job> listVille = db.Job.Where(p => p.archived == 1 && p.status == 1).ToList();
            ViewBag.listVille = listVille.Take(4);

            return View();

        }

        public ActionResult ErrorPage(String sms)
        {
            ViewBag.message = sms;
            return View();
        }

        [HttpGet]
        public ActionResult ListeOffre()
        {
            String search = Request["search"];
            String searchVille = Request["searchVille"];
            Decimal searchCategorie = 0;
            if (!string.IsNullOrEmpty(Request["categoriesSearch"]))
            {
                 searchCategorie = Convert.ToDecimal(Request["searchCategorie"]);
            }
          
            String searchLocation = Request["searchLocation"];

            var listPostuler = db.Postuler.AsQueryable();

            listPostuler = listPostuler.Where(p => p.Job.archived == 1 && p.Inscrire.type == "client" && p.Job.status == 1 && p.approbation.Equals("1") && DateTime.Compare((DateTime) p.Job.dateFinOffre, DateTime.Now) > 0).OrderByDescending(p => p.Job.created);

            if (!string.IsNullOrEmpty(search)) listPostuler = listPostuler.Where(p => p.Job.titre.Contains(search));
            if (!string.IsNullOrEmpty(searchVille)) listPostuler = listPostuler.Where(p => p.Job.ville.Contains(searchVille));
            if (searchCategorie > 0) listPostuler = listPostuler.Where(p => p.Job.Categorie.Typ_id== searchCategorie);
            if (!string.IsNullOrEmpty(searchLocation)) listPostuler = listPostuler.Where(p => p.Job.ville.Contains(searchLocation));

            int limit = 4;
            ViewBag.limit = limit;
            ViewBag.count = listPostuler.ToList().Count;
            ViewBag.listPostuler = listPostuler.ToList().ToPagedList(1, limit);

            List<Types> listTypes = db.Types.Where(p => p.archived == 1).ToList();
            List<Categorie> listCategorie = db.Categorie.Where(p => p.archived == 1).ToList();
            List<AnneeExp> listAnneeExp = db.AnneeExp.Where(p => p.archived == 1).ToList();

            ViewBag.listTypes = listTypes;
            ViewBag.listCategorie = listCategorie;
            ViewBag.listAnneeExp = listAnneeExp;

            return View();
        }

        [HttpGet]
        public ActionResult ListeOffreJson()
        {
           db.Configuration.ProxyCreationEnabled = false;
            String libelleSearch = Request["libelleSearch"];
            String locationSearch = Request["locationSearch"];
            Decimal categoriesSearch = 0, domaine = 0 ;
            Double timePost = 0;
            if (!string.IsNullOrEmpty(Request["categoriesSearch"]))
            {
                 categoriesSearch = Convert.ToDecimal(Request["categoriesSearch"]);
            }
            if (!string.IsNullOrEmpty(Request["domaine"]))
            {
                domaine = Convert.ToDecimal(Request["domaine"]);
            }

            if (!string.IsNullOrEmpty(Request["timePost"]))
            {
                timePost = Convert.ToDouble(Request["timePost"]);
            }

            String anneeExp = Request["anneeExp"];
            String type = Request["type"];
            String datePostedRadio = Request["datePostedRadio"];
            int page = Request["page"].AsInt();
            int limit = 4;
            Console.WriteLine("libelleSearch=" + libelleSearch + ", locationSearch=" + locationSearch + "+categoriesSearch=" + categoriesSearch);

            var listPostuler = db.Postuler.AsQueryable();
             listPostuler = listPostuler.Where(p => p.Job.archived == 1 && p.Inscrire.type == "client" && p.Job.status == 1 && p.approbation.Equals("1") && DateTime.Compare((DateTime) p.Job.dateFinOffre, DateTime.Now) > 0).OrderByDescending(p => p.Job.created);
           if (!string.IsNullOrEmpty(libelleSearch))  listPostuler = listPostuler.Where(p => p.Job.titre.Contains(libelleSearch));
           if (!string.IsNullOrEmpty(locationSearch)) listPostuler = listPostuler.Where(p =>  p.Job.ville.Contains(locationSearch));
            if (!string.IsNullOrEmpty(anneeExp)) listPostuler = listPostuler.Where(p => p.Job.margeExperience.Contains(anneeExp));
            if (!string.IsNullOrEmpty(type)) listPostuler = listPostuler.Where(p => p.Job.heureTravail.Contains(type));
            if (categoriesSearch!=0) listPostuler = listPostuler.Where(p => p.Job.Cat_id== categoriesSearch);
            if (domaine != 0) listPostuler = listPostuler.Where(p => p.Job.Categorie.Types.id== domaine);
            if (timePost != 0)
            {
                DateTime diff = DateTime.Now.AddDays(-timePost);
                listPostuler = listPostuler.Where(p => p.Job.created <= DateTime.Now && p.Job.created >= diff);
            }
            var count = listPostuler.Count();
            //if (!libelleSearch.Equals(""))  listPostuler = from c in listPostuler where c.Job.titre.Contains(libelleSearch) select c;
            //if (!locationSearch.Equals("")) listPostuler = from c in listPostuler where c.Job.ville.Contains(locationSearch) select c;

            List<CustomJob> listResult = new List<CustomJob>();

            foreach (Postuler item in listPostuler.ToList().ToPagedList(page, limit))
            {
                CustomJob customJob = new CustomJob();
                customJob.id = item.Job.id;
                customJob.titre = item.Job.titre;
                customJob.ville = item.Job.ville;
                customJob.heureTravail = item.Job.heureTravail;
                customJob.remuneration = item.Job.remuneration;
                customJob.dateEntre = item.Job.dateEntre;
                customJob.profil = item.Inscrire.profil;
                customJob.nom = item.Inscrire.nom;
                customJob.immediat = item.Job.immediat;
                customJob.negociable = ""+item.Job.negociable;
                customJob.salaireAnnuel = ""+item.Job.salaireAnnuel;
                customJob.remuneration_n = ""+item.Job.remunerationN;
                listResult.Add(customJob);
            }

            return Json(new { count = count, limit = limit, datas = listResult }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetailsOffreHome(decimal id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Postuler postuler = db.Postuler.Where(p => p.Job.archived == 1 && p.Inscrire.type == "client" && p.Job.status == 1 && p.Job_id == id).FirstOrDefault();

            List<Postuler> listSimilaire = db.Postuler.Where(p => p.Job.archived == 1 && p.Inscrire.type == "client" && p.Job.status == 1 && p.Job.Cat_id == postuler.Job.Cat_id && p.approbation.Equals("1") && DateTime.Compare((DateTime) p.Job.dateFinOffre, DateTime.Now) > 0).OrderByDescending(p => p.Job.created).ToList();
            listSimilaire.Remove(postuler);
            if (listSimilaire.Count <=0)
            {
                ViewBag.listSimilaire = null;
            }
            else
            {
                ViewBag.listSimilaire = listSimilaire.Take(4);
            }

            List<DiplomeJob> listDiplomeJob = db.DiplomeJob.Where(p => p.archived == 1 && p.Job_id == postuler.Job_id).ToList();
            ViewBag.listDiplomeJob = listDiplomeJob;

            List<AvantageSociauxJob> listAvantageSociauxJob = db.AvantageSociauxJob.Where(p => p.archived == 1 && p.Job_id == postuler.Job_id).ToList();
            ViewBag.listAvantageSociauxJob = listAvantageSociauxJob;

            return View(postuler);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }




        public JsonResult SaveData(Inscrire model)
        {
            db.Inscrire.Add(model);
            db.SaveChanges();

            return Json("creation du compte ok", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllOffreLibelle()
        {
           db.Configuration.ProxyCreationEnabled = false;

            List<Job> listJob = db.Job.Where(p => p.archived == 1 && p.status == 1).ToList();
            // List<Job> a = listJob.GroupBy(p => new { p.ville }).ToList();
            var subCategoryToReturn = listJob.Select(S => new {
                id = S.id,
                titre = S.titre
            }).GroupBy(p => new { p.titre });
            return Json(subCategoryToReturn, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAllInscrire()
        {
            db.Configuration.ProxyCreationEnabled = false;

          //  List<Job> listJob = db.Job.Where(p => p.archived == 1 && p.ins.status == 1 && p. && p).ToList();
            List<Postuler> listPostuler = db.Postuler.Where(p => p.Inscrire.archived == 1 && p.Inscrire.status==1 && p.Inscrire.type.Equals("client") && p.Job.immediat.Equals(Constante.direct) && p.approbation.Equals("1")).OrderByDescending(p => p.Job.created).ToList();

            var subCategoryToReturn = listPostuler.Select(S => new {
                id = S.Inscrire.id,
                email = S.Inscrire.email,
                adresse= S.Inscrire.adresse,
                phone = S.Inscrire.phone,
                nom = S.Inscrire.nom
            }).GroupBy(p => new { p.nom });
            return Json(subCategoryToReturn, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAllOffreVille()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Job> listJob = db.Job.Where(p => p.archived == 1 && p.status == 1 && p.ville!=null && !p.ville.Equals("")).ToList();
           // List<Job> a = listJob.GroupBy(p => new { p.ville }).ToList();
            var subCategoryToReturn = listJob.Select(S => new {
                id = S.id,
                ville = S.ville
            }).GroupBy(p => new { p.ville });

           // listJob = listJob.GroupBy(p => new { p.ville });
            return Json(subCategoryToReturn, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public async Task<ActionResult> postuler( decimal id)
        {
            decimal idJob = id;

            if (Session.Count > 0 && Session["status"]!=null)
            {
                if (Session["type"].Equals("client"))
                {
                    String message = "Vous ne pouvez pas postuler à cette offre. Seul les travailleurs intéressés ou ayant un compte candidat  peuvent postuler.";
                    return RedirectToAction("ErrorPage", "Home", new { sms = message });
                }else
                {
                    decimal status = Convert.ToDecimal(Session["status"]);

                    if (status == 0)
                    {
                        String message = "Vous ne pouvez pas pour le moment postulé à une offre. Vérifier que vos informations sont bien renseignez dans votre profil et attendez l'activation de votre profil par les administrateurs NiovarJobs. Sinon contactez nous ";

                        return RedirectToAction("ErrorPage", "Home", new { sms = message });
                    }

                    decimal idPostulant = Convert.ToDecimal( Session["id"]);

                    Postuler result = db.Postuler.Where(p => p.Ins_id== idPostulant && p.Job_id == idJob && p.Inscrire.type.Equals("candidat") && p.archived==1).FirstOrDefault();

                    if (result != null)
                    {
                        String message = "Vous ne pouvez pas postuler a un même offre d'emploi deux fois ";
                        return RedirectToAction("ErrorPage", "Home", new { sms = message });
                    }
                    else
                    {
                        Postuler currentEmployeur = db.Postuler.Where(p => p.Job_id == idJob && p.Inscrire.type.Equals("client") && p.archived == 1).FirstOrDefault();
                        if (currentEmployeur == null)
                        {
                            String message = "La  compagnie propriétaire de cette publication  n'existe pas ";
                            return RedirectToAction("ErrorPage", "Home", new { sms = message });
                        }

                        Postuler postuler = new Postuler();
                        postuler.Job_id = idJob;
                        postuler.Ins_id = idPostulant; 
                        postuler.created = DateTime.Now;
                        postuler.archived = 1;
                        postuler.status = 1;
                        postuler.etat = "0";
                        postuler.etatAdmin = "0";
                        postuler.etatClient = "0";
                        postuler.etatCandidat = "0";
                        postuler.signatures = "0";
                        postuler.signatureClient = 0;
                        postuler.situationTravail = "0";


                        db.Postuler.Add(postuler);

                        TempData["myData"] = "1";
                        TempData["message"] = "Votre candidature a été envoyer avec succès";

                        string emailTo = currentEmployeur.Inscrire.email;
                        if (currentEmployeur.Job.immediat.Equals("true"))
                        {
                            emailTo = Constante.mailCompagny;
                        }
                       

                        var map = new Dictionary<String, String>();
                        map.Add("@ViewBag.titre", "Un candidat est intérésser par votre offre de " + currentEmployeur.Job.titre);
                        map.Add("@ViewBag.login", "");
                        map.Add("@ViewBag.content", "Le candidat " + Session["login"] + " à deposer sa candidature pour le poste de :" + currentEmployeur.Job.titre + ".veuillez suivre la candidature de ce candidat sur NioVarJobs");
                        string body = MsMail.BuldBodyTemplate("~/EmailTemplate/CustomEmail.cshtml", map);
                        MsMail mail = new MsMail();

                        await mail.Send(emailTo, "Candidat intérésser", body);

                        db.SaveChanges();
                    }

                  
                    return RedirectToAction("MesCandidatures", "Offre");
                }

            }
            else
            {
                return RedirectToAction("Login", "Inscrires");
            }

          
        }

        

        [HttpGet]
        public JsonResult GetAllNotification()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Postuler> listResult = new List<Postuler>();

            if (Session.Count > 0 && Session["type"] != null)
            {
                decimal id = Convert.ToDecimal(Session["id"]);
                if (Session["type"] != null)
                {
                    if (Session["type"].Equals("client"))
                    {
                        List<Postuler> listPostuler = db.Postuler.Where(p => p.Ins_id == id && p.Job.archived == 1 && p.archived == 1 && p.Job.status == 1 && p.Inscrire.type.Equals("client")).OrderByDescending(p => p.Job.created).ToList();
                        Job job;
                        foreach (Postuler postuler in listPostuler)
                        {
                            postuler.nbreApply = db.Postuler.Where(p => p.Job_id == postuler.Job_id && p.archived == 1 && p.etat.Equals("0") && p.Inscrire.type.Equals("candidat")).ToList().Count;
                            listResult.Add(postuler);
                        }

                    }
                }

            
                

            }

      
            return Json(listResult.ToList(), JsonRequestBehavior.AllowGet);

        }

        
        public ActionResult AproposHome(decimal id)
        {
            Page page = db.Page.Where(p => p.Ins_id == id && p.archived == 1).FirstOrDefault();
            if (page != null)
            {
                Inscrire inscrire = db.Inscrire.Where(p => p.id== page.Ins_id && p.archived == 1).FirstOrDefault();
                ViewBag.inscrire = inscrire;

                return View(page);
            }
            else { return HttpNotFound(); }
            // return View();
        }

      
        public ActionResult PourquoiPostulerHome(decimal id)
        {
            Page page = db.Page.Where(p => p.Ins_id == id && p.archived == 1).FirstOrDefault();
            if (page != null)
            {
                Inscrire inscrire = db.Inscrire.Where(p => p.id == page.Ins_id && p.archived == 1).FirstOrDefault();
                ViewBag.inscrire = inscrire;

                return View(page);
            }
            else { return HttpNotFound(); }
            // return View();
        }


        public ActionResult MaGalerieHome( decimal id)
        {
    
            Page page = db.Page.Where(p => p.Ins_id == id && p.archived == 1).FirstOrDefault();
            if (page != null)
            {
                Inscrire inscrire = db.Inscrire.Where(p => p.id == page.Ins_id && p.archived == 1).FirstOrDefault();
                ViewBag.inscrire = inscrire;

                List<Galerie> listGalerie = db.Galerie.Where(p => p.Pag_id == page.id && p.archived == 1).OrderByDescending(p => p.created).ToList();
                ViewBag.listGalerie = listGalerie.Take(12).ToList();

                ViewBag.page = page;

                return View();
            }
            else { return HttpNotFound(); }
        }



        public ActionResult MesAvisHome(decimal id)
        {
           

            Page page = db.Page.Where(p => p.Ins_id == id && p.archived == 1).FirstOrDefault();
            if (page != null)
            {
                Inscrire inscrire = db.Inscrire.Where(p => p.id == page.Ins_id && p.archived == 1).FirstOrDefault();
                ViewBag.inscrire = inscrire;

                List<Avis> listAvis = db.Avis.Where(p => p.Pag_id == page.id && p.archived == 1 && p.status==1).OrderByDescending(p => p.created).ToList();
                listAvis = listAvis.Take(15).ToList();

                List<Avis> newListAvis = new List<Avis>();
                foreach (Avis item in listAvis)
                {
                    Avis newAvis = new Avis();
                    newAvis = item;

                    Inscrire insc = db.Inscrire.Where(p => p.id == item.iduser).FirstOrDefault();
                    if (insc!= null)
                    {
                        newAvis.Pseudo = insc.login;
                        newAvis.Profil = insc.profil;
                    }
                    newListAvis.Add(newAvis);
                }

                ViewBag.newListAvis = newListAvis;
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
            else { return HttpNotFound(); }

            ViewBag.page = page;
            return View();
        }


        [Authorize]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MesAvisHome(Avis avis)
        {
            if (Session.Count > 0 )
            { 
                int idCurrent = Convert.ToInt32(Session["id"]);
                Page currentPage = db.Page.Where(p => p.id == avis.Pag_id && p.archived == 1).FirstOrDefault();
                if (currentPage != null)
                {
                    avis.iduser = idCurrent;
                    avis.archived = 1;
                    avis.created = DateTime.Now;
                    avis.status = 0;
                    db.Avis.Add(avis);
                    db.SaveChanges();
                    TempData["myData"] = "1";
                    TempData["result_code"] = 1;
                    TempData["message"] = "Avis envoyer  et en attente de valudation par la compagnie.";
                    TempData.Keep();

                    return RedirectToAction("MesAvisHome", new { id = currentPage.Ins_id });
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else { return RedirectToAction("Login","Inscrires"); }
                 
        }

        public ActionResult MyPageOffre(decimal id)
        {
            Page page = db.Page.Where(p => p.Ins_id == id && p.archived == 1).FirstOrDefault();
            if (page != null)
            {
                Inscrire inscrire = db.Inscrire.Where(p => p.id == page.Ins_id && p.archived == 1).FirstOrDefault();
                ViewBag.inscrire = inscrire;

                List<Postuler> listNosOffres;
                if (inscrire.type.Equals("compagnie"))
                {
                  listNosOffres = db.Postuler.Where(p => p.Job.archived == 1 && p.Inscrire.type == "client" && p.Job.status == 1  && p.approbation.Equals("1") && p.Job.immediat.Equals("true")).OrderByDescending(p => p.Job.created).ToList();

                }
                else
                {
                  listNosOffres = db.Postuler.Where(p => p.Job.archived == 1 && p.Inscrire.type == "client" && p.Job.status == 1 && p.Ins_id == inscrire.id && p.approbation.Equals("1") && p.Job.immediat.Equals("false")).OrderByDescending(p => p.Job.created).ToList();

                }


                if (listNosOffres.Count <= 0)
                {
                    ViewBag.listNosOffres = null;
                }
                else
                {
                    ViewBag.listNosOffres = listNosOffres.Take(10);
                }

                ViewBag.page = page;

                return View();
            }
            else { return HttpNotFound(); } 
        }

        [AuthorizedCandidat]
        public ActionResult postulerDirectement(decimal id, decimal idJob)
        {
            Inscrire inscrire = db.Inscrire.Where(p => p.archived == 1 && p.id==id && p.type.Equals(Constante.typeclient)).FirstOrDefault();
            ViewBag.inscrire = inscrire;
            ViewBag.idJob = idJob;
            return View();
        }

        [AuthorizedCandidat]
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> postulerDirectement(MailsModel mailsModel)
        {

            if (mailsModel.To == null)
            {
                String message = "Veuillez renseignez son l'email";
                return RedirectToAction("ErrorPage", "Home", new { sms = message });
            }

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
                        
                            String message = "L'extention du fichier n'est pas valide !";
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
                    }
                    catch (Exception ex)
                    {

                        String message = "erreur de l'upload du fichier";
                        return RedirectToAction("ErrorPage", "Home", new { sms = message });
                    }
                }
            }


            try
                {
                decimal idPostulant = Convert.ToDecimal(Session["id"]);
                Postuler postuler = new Postuler();
                postuler.Job_id = mailsModel.idJob;
                postuler.Ins_id = idPostulant;
                postuler.created = DateTime.Now;
                postuler.archived = 1;
                postuler.status = 1;
                postuler.etat = "0";
                postuler.etatAdmin = "0";
                postuler.etatClient = "0";
                postuler.etatCandidat = "0";
                postuler.signatures = "0";
                postuler.signatureClient = 0;
                postuler.situationTravail = "0";
                db.Postuler.Add(postuler);

                var map = new Dictionary<String, String>();
                    map.Add("@ViewBag.titre", "Nouvelle candidature");
                    map.Add("@ViewBag.login","");
                    map.Add("@ViewBag.content", mailsModel.message);
                    string body = MsMail.BuldBodyTemplate("~/EmailTemplate/CustomEmail.cshtml", map);
                    MsMail mail = new MsMail();

                    await mail.Send(mailsModel.To, mailsModel.subject, body, null, null, listFichier);
                    db.SaveChanges();
                db.SaveChanges();

                String message = "candidature envoyé avec succès";
                return RedirectToAction("ErrorPage", "Home", new { sms = message });
            
                }
                catch (Exception e)
                {
 
                String message = "echec veuillez reessayer";
                return RedirectToAction("ErrorPage", "Home", new { sms = message });
            }

        }

        
        public ActionResult verifyEmailExist()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> verifyEmailExist(Inscrire inscrire)
        {
            if (ModelState.IsValidField("email"))
            {
                Inscrire currentInscrire = db.Inscrire.Where(p => p.archived == 1 && p.email.Equals(inscrire.email)).FirstOrDefault();

                if (currentInscrire== null)
                {
                    ViewBag.Message = "Cette adresse email n'est utilisé par aucun compte ou n'existe pas ! ";
                    return View(inscrire);
                }

                UtilisateursController utilisateurs = new UtilisateursController();

                currentInscrire.tokenPwdForget = utilisateurs.generatePassword();
                db.Entry(currentInscrire).State = EntityState.Modified;
                

                var url = MsMail.baseUrl + "Home/changePwdForget?token=" + currentInscrire.tokenPwdForget;

                var map = new Dictionary<String, String>();
                map.Add("@ViewBag.ConfirmationLink", url);
                map.Add("@ViewBag.login", "");
                string body = MsMail.BuldBodyTemplate("~/EmailTemplate/Text.cshtml", map);
                MsMail mail = new MsMail();
                await mail.Send(currentInscrire.email, " confirmation rénitialisation  mot de passe oubliez ", body);

                db.SaveChanges();


                String message =" Un mail de confirmation vous a été envoyé à cette adresse "+ currentInscrire.email+" veuillez valider ";
                return RedirectToAction("ErrorPage", "Home", new { sms = message });

            }

            ModelState.AddModelError("email", "Veuillez saisir une adresse email valide");

            return View(inscrire);
        }

        public ActionResult changePwdForget(string token)
        {
            Inscrire currentInscrire = db.Inscrire.Where(p =>p.tokenPwdForget.Equals(token)).FirstOrDefault();

            if (currentInscrire == null)
            {
                String message = "Impossible de  modifier ce compte. Veuillez reprendre le procédé";
                return RedirectToAction("ErrorPage", "Home", new { sms = message });
            }

            ViewBag.id = currentInscrire.id;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult changePwdForget(Inscrire inscrire)
        {
            if (ModelState.IsValidField("password") && ModelState.IsValidField("cpassword"))
            {
                Inscrire currentInscrire = db.Inscrire.Where(p => p.id==inscrire.id).FirstOrDefault();

                if (currentInscrire == null)
                {
                    @ViewBag.Message = "Un problème est survenu lors de l'opération. Veuillez reprendre le procédé";
                    return View(inscrire);
                }

                UtilisateursController utilisateurs = new UtilisateursController();

                currentInscrire.tokenPwdForget = utilisateurs.generatePassword();
                currentInscrire.password = inscrire.password;
                currentInscrire.cpassword = inscrire.password;
                db.Entry(currentInscrire).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.success = 1;

                return View(inscrire);


            }

            ModelState.AddModelError("password", "Minimum 6 caractères");
            ModelState.AddModelError("cpassword", "Mot de passe non identique");
            

            return View(inscrire);
        }
    }
}

   