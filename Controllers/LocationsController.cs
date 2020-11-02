using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Stripe;
using WebApplication1.Models;
using X.PagedList;

namespace WebApplication1.Controllers
{
    public class LocationsController : MybaseController
    {
        private NiovarJobEntities db = new NiovarJobEntities();

   
        [Authorize]
        [AuthorizedClient]
        public ActionResult louerEmployer()
        {
            if (Session["type"] != null && Session["type"].Equals("client"))
            {
                decimal status = Convert.ToDecimal(Session["status"]);
                if (status == 0)
                {
                    String message = "Impossible de publier une offre. Vérifier que vos informations sont bien renseignez dans votre profil  et attendez l'activation de votre profil par les administrateurs NiovarJobs. Sinon contactez nous ";

                    return RedirectToAction("ErrorPage", "Home", new { sms = message });
                }
            }

            List<Inscrire> listFreelancer = db.Inscrire.Where(p => p.archived == 1 && p.type.Equals("candidat") && p.status == 1 && p.etat == 1 ).OrderByDescending(p => p.created).ToList();
            
            int limit = 4;
            ViewBag.limit = limit;
            ViewBag.count = listFreelancer.ToList().Count;
          
            if (listFreelancer.Count() >0 )
            {
                ViewBag.listFreelancer = listFreelancer.ToList().ToPagedList(1, limit);
                
            }
            else
            {
                ViewBag.listFreelancer = null;
            }
           
            List<Types> listTypes = db.Types.ToList();
            List<Categorie> listCategorie = db.Categorie.ToList();
            List<AnneeExp> listAnneeExp = db.AnneeExp.ToList();

            ViewBag.listTypes = listTypes;
            ViewBag.listCategorie = listCategorie;
            ViewBag.listAnneeExp = listAnneeExp;

            return View();
        }

        [HttpGet]
        public ActionResult filtreLouerEmployer()
        {
            //db.Configuration.ProxyCreationEnabled = false;
           
            Decimal categoriesSearch = 0, domaine = 0;
            if (!string.IsNullOrEmpty(Request["categoriesSearch"]))
            {
                categoriesSearch = Convert.ToDecimal(Request["categoriesSearch"]);
            }
            if (!string.IsNullOrEmpty(Request["domaine"]))
            {
                domaine = Convert.ToDecimal(Request["domaine"]);
            }

            String anneeExp = Request["anneeExp"];
            String pays = Request["pays"];
            String province = Request["province"];
            String ville = Request["ville"];
            int page = Request["page"].AsInt();
            int limit = 4;
           // Console.WriteLine("libelleSearch=" + libelleSearch + ", locationSearch=" + locationSearch + "+categoriesSearch=" + categoriesSearch);

            var listInscrire = db.Inscrire.AsQueryable();
            listInscrire = listInscrire.Where(p => p.archived == 1 && p.type.Equals("candidat") && p.status == 1 && p.etat == 1).OrderByDescending(p => p.created);
            if (!string.IsNullOrEmpty(anneeExp)) listInscrire = listInscrire.Where(p => p.anneeExperience.Contains(anneeExp));
            if (!string.IsNullOrEmpty(pays)) listInscrire = listInscrire.Where(p => p.pays.Contains(pays));
            if (!string.IsNullOrEmpty(province)) listInscrire = listInscrire.Where(p => p.province.Contains(province));
            if (!string.IsNullOrEmpty(ville)) listInscrire = listInscrire.Where(p => p.ville.Contains(ville));
            if (categoriesSearch != 0) listInscrire = listInscrire.Where(p => p.categorie.Contains(""+categoriesSearch+"") );
            if (domaine != 0) listInscrire = listInscrire.Where(p =>p.domaine.Contains(""+domaine+""));
            var count = listInscrire.Count();
  

            List<Inscrire> listResult = new List<Inscrire>();

            foreach (Inscrire item in listInscrire.ToList().ToPagedList(page, limit))
            {
                Inscrire inscrire = new Inscrire();
                inscrire.id = item.id;
                inscrire.login = item.login;
                inscrire.ville = item.ville;
                inscrire.province = item.province;
                inscrire.pays = item.pays;
                inscrire.anneeExperience = item.anneeExperience;
                inscrire.profil = item.profil;
                inscrire.titreEmploi = item.titreEmploi;
              
                listResult.Add(inscrire);
            }

            return Json(new { count = count, limit = limit, datas = listResult }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AuthorizedClient]
        public ActionResult mesDemandeLocation()
        {

            decimal id = (decimal) Session["id"];

            List<Location> listLocation = db.Location.Where(p => p.archived == 1 && p.Ins_id2==id  ).OrderByDescending(p => p.created).ToList();
            ViewBag.nbreLocation = db.Location.Where(p => p.archived == 1 && p.Ins_id2 == id).ToList().Count;
            ViewBag.nbreLocationActif = db.Location.Where(p => p.archived == 1 && p.Ins_id2 == id && p.status==1).ToList().Count;


            List<CustomLocation> newlistLocation = new List<CustomLocation>();
            foreach (Location item in listLocation)
            {
                CustomLocation customLocation = new CustomLocation();
                Inscrire inscrire = db.Inscrire.Where(p => p.id == item.Ins_id).FirstOrDefault();

                customLocation.inscrire = inscrire;
                customLocation.location = item;

                newlistLocation.Add(customLocation);

            }

            ViewBag.newlistLocation = newlistLocation;
            return View();
        }

        [CustomAuthorized]
        // GET: Locations/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Location.Where(p => p.id == id).FirstOrDefault();
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            ViewBag.Ins_id = new SelectList(db.Inscrire, "id", "type");
            return View();
        }

        // POST: Locations/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( Location location)
        {
            decimal sessionId= (decimal)Session["id"];

            Location existLocation = db.Location.Where(p => p.Ins_id == location.Ins_id && p.Ins_id2==sessionId && p.status==1 && p.archived==1).FirstOrDefault();

            if(existLocation != null)
            {
                String message = "Vous ne pouvez pas Louer un même  employé deux fois lorsque sa demande est encore en cour de traitement . Vérifier que vous avez supprimez ou désactivez cette demande et réessayez à nouveau.";
                return RedirectToAction("ErrorPage", "Home", new { sms = message });
            }

            Inscrire candidat = db.Inscrire.Where(p => p.id == location.Ins_id).First();
            if (candidat == null) { return HttpNotFound(); }
            Inscrire client = db.Inscrire.Where(p => p.id == sessionId).First();
            if (client == null) { return HttpNotFound(); }

            if (ModelState.IsValid)
            {
                location.Ins_id2 = sessionId;
                location.signClient = 0;
                location.signCandidat = 0;
                location.avisClient = 0;
                location.avisCandidat = 0;
                location.status = 1;
                location.etat = 0;
                location.archived = 1;
                location.created = DateTime.Now;

                db.Location.Add(location);
                
               
            }

           

            try
            {
           

                var map = new Dictionary<String, String>();
                map.Add("@ViewBag.titre", "Votre demande location a été effectué avec succès pour un salaire de : " +location.remuneration);
                map.Add("@ViewBag.login", client.login);
                map.Add("@ViewBag.content", "Votre demande est bien pris en compte et suivra la procédure adéquat. Merci bénéficiez du meilleur de nos service.");
                string body = MsMail.BuldBodyTemplate("~/EmailTemplate/CustomEmail.cshtml", map);

                

                var map1 = new Dictionary<String, String>();
                map1.Add("@ViewBag.titre","Mr/Mme :"+candidat.login+ "Vous êtes sollicité pour un travail pour une période de "+location.periode+ "pour un salaire de "
                    + Convert.ToDouble(location.remuneration)* (100-pourcentage)/100);
                map1.Add("@ViewBag.login", candidat.login);
                map1.Add("@ViewBag.content", "Vous êtes conviez à bien a suivre votre dossier via votre espace compte sur https://niovar.solutions/Inscrires/Login. Merci bénéficiez du meilleur de nos service.");
                string body2 = MsMail.BuldBodyTemplate("~/EmailTemplate/CustomEmail.cshtml", map1);

                MsMail mail = new MsMail();
                MsMail mail2 = new MsMail();

                db.SaveChanges();

                //TODO retraviller le mail avec du HTML 
                await mail.Send(client.email, "NiovarJobs, demande location ", body);
                await mail2.Send(candidat.email, "NiovarJobs, Nouveau job disponible pour vous  ", body2);
                //TODO rediriger a la liste des candidatures
                return RedirectToAction("mesDemandeLocation");
            }
            catch (StripeException e)
            {
                return RedirectToAction("cvCandidatLocation", new { id = location.Ins_id, page=0 });
                
            }

        }

        // GET: Locations/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Location.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            ViewBag.Ins_id = new SelectList(db.Inscrire, "id", "type", location.Ins_id);
            return View(location);
        }

        // POST: Locations/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Ins_id,Ins_id2,id,periode,heureTravail,description,montant,signClient,signCandidat,remuneration,dateSgnClt,dateSgnCdt,avisClient,avisCandidat,status,etat,created,archived")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Ins_id = new SelectList(db.Inscrire, "id", "type", location.Ins_id);
            return View(location);
        }

        // GET: Locations/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Location.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Location location = db.Location.Find(id);
            db.Location.Remove(location);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        [AuthorizedClient]
        public ActionResult cvCandidatLocation(decimal id, decimal page)
        {
            ViewBag.page = page;

            if (id == null)
            {
                String message = "Une erreur c'est produit durant l'opération veuillez reessayer !";
                return RedirectToAction("ErrorPage", "Home", new { sms = message });
            }

            Information information = db.Information.FirstOrDefault(p => p.Ins_id == id);

            Inscrire inscrire = db.Inscrire.FirstOrDefault(p => p.id == id);
            int montant = 0;
            if (inscrire != null)
            {
                if (inscrire.categorie!=null)
                {
                    decimal idCategorie = Convert.ToDecimal(inscrire.categorie);
                    CatAnneeExp catAnneeExp = db.CatAnneeExp.FirstOrDefault(p => p.Cat_id == idCategorie);
                    if (catAnneeExp != null)
                    {
                        montant = (int) catAnneeExp.prixHoraire;
                    }
                }

                Location location = UserLocationExist(inscrire.id);
                if (location != null)
                {
                    ViewBag.locationExist = location;
                }
                else { ViewBag.locationExist = null; }

              
            }
            else { return HttpNotFound(); }

            ViewBag.montant = montant;

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
        [AuthorizedClient]
        public ActionResult EditStatus(decimal id)
        {

            try
            {
                var currentLocation = db.Location.FirstOrDefault(p => p.id == id);
                if (currentLocation == null)
                {
                    ViewBag.Message = "Votre compte n'existe pas !";

                    return RedirectToAction("mesDemandeLocation");
                }

                if (currentLocation.status == 1)
                {
                    currentLocation.status = 0;
                }
                else
                {
                    currentLocation.status = 1;
                }

                db.Entry(currentLocation).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.Message = "modification éffectuer avec succès avec succès !";

                return RedirectToAction("mesDemandeLocation");

            }
            catch (Exception)
            {
                return RedirectToAction("mesDemandeLocation");

            }
        }

        [Authorize]
        [AuthorizedClient]
        public ActionResult deleteLocation(decimal id)
        {

            try
            {
                var currentLocation = db.Location.FirstOrDefault(p => p.id == id);
                if (currentLocation == null)
                {
                    ViewBag.Message = "Votre compte n'existe pas !";

                    return RedirectToAction("mesDemandeLocation");
                }

                currentLocation.archived = 0;

                db.Entry(currentLocation).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.Message = "modification éffectuer avec succès avec succès !";

                return RedirectToAction("mesDemandeLocation");

            }
            catch (Exception)
            {
                return RedirectToAction("mesDemandeLocation");

            }
        }

        [CustomAuthorized]
        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN, Constante.roleGESTIONNAIRE })]
        public ActionResult Index(decimal type)
        {

            List<Location> listLocation = db.Location.Where(p => p.archived == 1 && p.status==1 && p.type== type).OrderByDescending(p => p.created).ToList();
            ViewBag.nbreLocation = db.Location.Where(p => p.archived == 1 && p.status == 1 && p.type==type).ToList().Count;
           
            List<CustomLocation> newlistLocation = new List<CustomLocation>();

            foreach (Location item in listLocation)
            {
                CustomLocation customLocation = new CustomLocation();
                Inscrire inscrire = db.Inscrire.Where(p => p.id == item.Ins_id2).FirstOrDefault();

                customLocation.inscrire = inscrire;
                customLocation.location = item;

                newlistLocation.Add(customLocation);

            }

            ViewBag.newlistLocation = newlistLocation;

            ViewBag.type = type;
            return View();
        }

        [Authorize]
        [AuthorizedCandidat]
        public ActionResult mesLocationsCandidat()
        {
            decimal id = (decimal) Session["id"];

            List<Location> listLocation = db.Location.Where(p => p.archived == 1 && p.Ins_id == id && p.status==1 ).OrderByDescending(p => p.created).ToList();
            ViewBag.nbreLocation = listLocation.Count;

            ViewBag.listLocation = listLocation;

            return View();
        }

        [Authorize]
        [AuthorizedCandidat]
        public ActionResult contratLocationCandidat(decimal id)
        {
            Location location = db.Location.Where(p => p.archived == 1 && p.id == id && p.status == 1).First();

            if(location != null)
            {
                Inscrire candidat = db.Inscrire.Where(p => p.id ==location.Ins_id).First();
                Inscrire client = db.Inscrire.Where(p => p.id == location.Ins_id2).First();


                ViewBag.candidat = candidat;

                ViewBag.client = client;

            }
            else { return HttpNotFound(); }


            return View(location);
        }

        [Authorize]
        [AuthorizedClient]
        public ActionResult contratLocationClient(decimal id)
        {
            Location location = db.Location.Where(p => p.archived == 1 && p.id == id && p.status == 1).First();

            if (location != null)
            {
                Inscrire candidat = db.Inscrire.Where(p => p.id == location.Ins_id).First();
                Inscrire client = db.Inscrire.Where(p => p.id == location.Ins_id2).First();


                ViewBag.candidat = candidat;

                ViewBag.client = client;

            }
            else { return HttpNotFound(); }

            return View(location);
        }

        [Authorize]
        public ActionResult confirmContratLocation(decimal id, int val, string type)
        {

            Location location = db.Location.FirstOrDefault(p => p.id == id);
            if (location == null)
            {
                return HttpNotFound();
            }

            if (type.Equals("candidat"))
            {
                location.dateSgnCdt = DateTime.Now;
                location.avisCandidat = val;
                location.signCandidat = val;

            }
            else if (type.Equals("client")) {
                location.dateSgnClt = DateTime.Now;
                location.avisClient = val;
                location.signClient = val;
            }

            db.Entry(location).State = EntityState.Modified;
            db.SaveChanges();

            if (type.Equals("candidat"))
            {
                return RedirectToAction("mesLocationsCandidat");
            }
            else if(type.Equals("client"))
            {
                return RedirectToAction("mesDemandeLocation");
            }
            else
            {
                return HttpNotFound();
            }

            
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> contratLocationClient(String stripeToken, decimal idLocation)
        {
            Location currentLocation = db.Location.FirstOrDefault(p => p.id == idLocation);
            if (currentLocation == null )
            {
                TempData["result_code"] = -1;
                TempData["message"] = "Temps fourni pour le payment est expiré veuillez réessayer.";
                TempData.Keep();
                //TODO rediriger a la liste des candidatures
                return RedirectToAction("mesDemandeLocation");
            }

            Inscrire candidat = db.Inscrire.Where(p => p.id == currentLocation.Ins_id).First();
            if (candidat == null) { return HttpNotFound(); }
            Inscrire client = db.Inscrire.Where(p => p.id == currentLocation.Ins_id2).First();
            if (client == null) { return HttpNotFound(); }


            if (String.IsNullOrEmpty(stripeToken)) return HttpNotFound();
            StripeConfiguration.SetApiKey("sk_test_51Gx6jgIK0UhIWHGbs9dcTW7tyGLkl39s6waxls9Z5D8E0arsL4bjy9N0g563Tlzo5JNvbeFOkAl5fMEY85eerPIx00mYJFiqLY");

            var stripeOptions = new ChargeCreateOptions
            {

                Amount = (long)Math.Ceiling(currentLocation.remuneration.Value) * 35 / 100, // 1 dollar is equal to 100 cent. 
                Currency = "USD",
                Description = "Charge for payment of 35% of Location" ,
                Source = stripeToken,
                //Customer = customer.Id
            };
            var service = new ChargeService();

            if (Session["id"] == null)
            {
                return HttpNotFound();
            }
            int idUserSession = Convert.ToInt32(Session["id"]);

            currentLocation.dateSgnClt = DateTime.Now;
            currentLocation.avisClient = 2;
            currentLocation.signClient = 2;
            db.Entry(currentLocation).State = EntityState.Modified;


            double amount = (long)Math.Ceiling(currentLocation.remuneration.Value) * 35 / 100;
            FraisLocation fraisLocation = new FraisLocation();
            fraisLocation.archived = 1;
            fraisLocation.avance = amount;
            fraisLocation.created = DateTime.Now;
            fraisLocation.etat = 0; //TODO mettre le bon etat
            fraisLocation.Ins_id = candidat.id;
            fraisLocation.Ins_id2 = client.id;
            fraisLocation.Loc_id = currentLocation.id;
            fraisLocation.libelle = "Paiement de 35% pour la location";
            fraisLocation.montant = Convert.ToDouble(currentLocation.remuneration);
            fraisLocation.userId = idUserSession;
            fraisLocation.reste = Convert.ToDouble(currentLocation.remuneration) - amount;
            fraisLocation.status = 0;
         
            //TODO mettre le bon status je suppose o veut dire non achevé   
            db.FraisLocation.Add(fraisLocation);


            try
            {
                Charge charge = service.Create(stripeOptions);
                var map = new Dictionary<String, String>();
                map.Add("@ViewBag.titre", "Paiement de 35% du salaire pour la location  de l'employé " + candidat.login);
                map.Add("@ViewBag.login", client.login);
                map.Add("@ViewBag.content", "Votre paiement a bien été pris en compte. Le montant à été prélever de votre compte avec succès. Bénéficiez du meilleur de nos service.");
                string body = MsMail.BuldBodyTemplate("~/EmailTemplate/CustomEmail.cshtml", map);

                var map1 = new Dictionary<String, String>();
                map1.Add("@ViewBag.titre", "Vous êtes sollicité pour un travail  ");
                map1.Add("@ViewBag.login", candidat.login);
                map1.Add("@ViewBag.content", "Votre demande de location a été confimer. Vous êtes conviez a prendre service le plutôt possible.");
                string body2 = MsMail.BuldBodyTemplate("~/EmailTemplate/CustomEmail.cshtml", map1);

                MsMail mail = new MsMail();
                MsMail mail2 = new MsMail();

                db.SaveChanges();

                //TODO retraviller le mail avec du HTML 
                await mail.Send(client.email, "Paiement location", body);
                await mail2.Send(candidat.email, "NiovarJobs, Demande location accepté ", body2);
                //TODO rediriger a la liste des candidatures
                return RedirectToAction("mesDemandeLocation");
            }
            catch (StripeException e)
            {
               // throw;
                return RedirectToAction("contratLocationClient", new { id = currentLocation.id });
            }

            return RedirectToAction("contratLocationClient",new { id = currentLocation.id});
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // vérifie si un candidat est deja louer (location en cour) ou pas et retourne un booléen
        private Location UserLocationExist(decimal id) { 
            Location location = db.Location.Where(p => p.archived == 1 && p.Ins_id==id && p.status == 1 && p.avisCandidat==2 && p.avisClient==2).FirstOrDefault();
            return location;
        }
    }
}
