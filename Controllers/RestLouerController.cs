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
    public class RestLouerController : MybaseController
    { 

        [HttpGet]
        public JsonResult filtreLouerEmployer()
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            if (_me.Equals("candidat")) return Json(new { result_code = -1, message = "Your are not allowed" }, JsonRequestBehavior.AllowGet);
            Decimal cat = !string.IsNullOrEmpty(Request["cat"]) ? Convert.ToDecimal(Request["cat"]) : 0;
            Decimal domaine = !string.IsNullOrEmpty(Request["domaine"]) ? Convert.ToDecimal(Request["domaine"]) : 0;
            String anneeExp = Request["anneeExp"];
            String location = Request["location"];
            int page = !string.IsNullOrEmpty(Request["page"]) ? Request["page"].AsInt() : 1; 
            int limit = 10; 

            var listInscrire = db.Inscrire.AsQueryable();
            listInscrire = listInscrire.Where(p => p.archived == 1 && p.type.Equals("candidat") && p.status == 1 && p.etat == 1).OrderByDescending(p => p.created);
            if (!string.IsNullOrEmpty(anneeExp)) listInscrire = listInscrire.Where(p => p.anneeExperience.Contains(anneeExp));
            if (!string.IsNullOrEmpty(location)) listInscrire = listInscrire.Where(p => p.pays.Contains(location) || p.province.Contains(location) || p.ville.Contains(location));
            if (cat != 0) listInscrire = listInscrire.Where(p => p.categorie.Contains(""+cat+"") );
            if (domaine != 0) listInscrire = listInscrire.Where(p =>p.domaine.Contains(""+domaine+""));
            var count = listInscrire.Count(); 
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (Inscrire item in listInscrire.ToPagedList(page, limit)) datas.Add(getJson(item)); 
            return Json(new { result_code = 1, count = count, limit = limit, datas = datas }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult mesDemandeLocation()
        {

            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            if (_me.Equals("candidat")) return Json(new { result_code = -1, message = "Your are not allowed" }, JsonRequestBehavior.AllowGet);

            int page = !string.IsNullOrEmpty(Request["page"]) ? Request["page"].AsInt() : 1;
            int limit = 10;

            var listLocation = db.Location.AsQueryable();
            listLocation = listLocation.Where(p => p.archived == 1 && p.Ins_id2 == _me.id).OrderByDescending(p => p.created);
            var count = listLocation.Count();

            //var nbreLocationActif = listLocation.Where(p => p.status==1).Count(); 

            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (Location item in listLocation.ToPagedList(page, limit)) datas.Add(getJson(item));
            return Json(new { result_code = 1, count = count, limit = limit, datas = datas }, JsonRequestBehavior.AllowGet);
        }
         
        [HttpGet] 
        public JsonResult existLocation(Decimal id)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            Location existLocation = db.Location.Where(p => p.Ins_id == id && p.Ins_id2 == _me.id && p.status==1 && p.archived==1).FirstOrDefault();

            return Json(new { result_code = 1, data = getJson(existLocation) }, JsonRequestBehavior.AllowGet); 

        }
        [HttpPost]
        public async Task<JsonResult> Create([Bind(Include = "Ins_id,periode,heureTravail,montant,remuneration,dateDebut,dateFin")] Location location)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            if (_me.Equals("candidat")) return Json(new { result_code = -1, message = "Your are not allowed" }, JsonRequestBehavior.AllowGet);


            Inscrire candidat = db.Inscrire.Where(p => p.id == location.Ins_id).First();
            if (candidat == null) return Json(new { result_code = -1, message = "Candidat not found" }, JsonRequestBehavior.AllowGet); 
            location.Ins_id2 = _me.id;
            location.signClient = 0;
            location.signCandidat = 0;
            location.avisClient = 0;
            location.avisCandidat = 0;
            location.status = 1;
            location.etat = 0;
            location.archived = 1;
            location.created = DateTime.Now; 
            db.Location.Add(location);    
            try
            {
           

                var map = new Dictionary<String, String>();
                map.Add("@ViewBag.titre", "Votre demande location a été effectué avec succès pour un salaire de : " +location.remuneration);
                map.Add("@ViewBag.login", _me.nom);
                map.Add("@ViewBag.content", "Votre demande est bien pris en compte et suivra la procédure adéquat. Merci bénéficiez du meilleur de nos service.");
                string body = MsMail.BuldBodyTemplate("~/EmailTemplate/CustomEmail.cshtml", map);

                var map1 = new Dictionary<String, String>();
                map1.Add("@ViewBag.titre","Mr/Mme : "+candidat.login+ "Vous êtes sollicité pour un travail pour une période de "+location.periode+ "pour un salaire de "
                    + location.remuneration*75/100);
                map1.Add("@ViewBag.login", candidat.login);
                map1.Add("@ViewBag.content", "Vous êtes conviez à bien a suivre votre dossier via votre espace compte sur https://niovar.solutions/Inscrires/Login. Merci bénéficiez du meilleur de nos service.");
                string body2 = MsMail.BuldBodyTemplate("~/EmailTemplate/CustomEmail.cshtml", map1); 
                MsMail mail = new MsMail();
                MsMail mail2 = new MsMail();

                db.SaveChanges();

                //TODO retraviller le mail avec du HTML 
                await mail.Send(_me.email, "NiovarJobs, demande location ", body);
                await mail2.Send(candidat.email, "NiovarJobs, Nouveau job disponible pour vous  ", body2);
                return Json(new { result_code = 1, data = getJson(location) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
               return Json(new { result_code = -2, message = "An execption occured" }, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        public JsonResult Edit([Bind(Include = "Ins_id,id,periode,heureTravail,montant,remuneration,dateDebut,dateFin")] Location location)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            if (_me.Equals("candidat")) return Json(new { result_code = -1, message = "Your are not allowed" }, JsonRequestBehavior.AllowGet);
  
            try
            {
                Location item = db.Location.FirstOrDefault(p => p.id == location.id);
                if(item == null) return Json(new { result_code = -1, message = "data not found" }, JsonRequestBehavior.AllowGet);
                item.periode = location.periode;
                item.heureTravail = location.heureTravail;
                item.montant = location.montant;
                item.remuneration = location.remuneration;
                item.dateDebut = location.dateDebut;
                item.dateFin = location.dateFin;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { result_code = 1, data = getJson(item) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result_code = -2, message = "An execption occured" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Delete(decimal id)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            if (_me.Equals("candidat")) return Json(new { result_code = -1, message = "Your are not allowed" }, JsonRequestBehavior.AllowGet);

            try
            {
                var currentLocation = db.Location.FirstOrDefault(p => p.id == id);
                if (currentLocation == null) return Json(new { result_code = -1, message = "data not found" }, JsonRequestBehavior.AllowGet);
                currentLocation.archived = 0; 
                db.Entry(currentLocation).State = EntityState.Modified; 
                db.SaveChanges();
                return Json(new { result_code = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result_code = -2, message = "An execption occured" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult EditStatus(decimal id)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            if (_me.Equals("candidat")) return Json(new { result_code = -1, message = "Your are not allowed" }, JsonRequestBehavior.AllowGet);

            try
            {
                var currentLocation = db.Location.FirstOrDefault(p => p.id == id);
                if (currentLocation == null) return Json(new { result_code = -1, message = "item not found" }, JsonRequestBehavior.AllowGet);
                 
                currentLocation.status = currentLocation.status == 1 ? 0 : 1;  
                db.Entry(currentLocation).State = EntityState.Modified;
                db.SaveChanges(); 
                return Json(new { result_code = 1, data = getJson(currentLocation) }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { result_code = -2, message = "An execption occured" }, JsonRequestBehavior.AllowGet); 
            }
        }

        [HttpGet]
        public JsonResult mesLocationsCandidat()
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);

            int page = !string.IsNullOrEmpty(Request["page"]) ? Request["page"].AsInt() : 1;
            int limit = 10;

            var listLocation = db.Location.AsQueryable();
            listLocation = listLocation.Where(p => p.archived == 1 && p.Ins_id == _me.id && p.status==1 ).OrderByDescending(p => p.created);
            var count = listLocation.Count();
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (Location item in listLocation.ToPagedList(page, limit)) datas.Add(getJson(item));
            return Json(new { result_code = 1, count = count, limit = limit, datas = datas }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult confirmContratLocation(decimal id, int val)
        {

            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);

            try
            {
                Location location = db.Location.FirstOrDefault(p => p.id == id);
                if (location == null) return Json(new { result_code = -1, message = "item not found" }, JsonRequestBehavior.AllowGet);
           
                if (_me.type.Equals("candidat"))
                {
                    location.dateSgnCdt = DateTime.Now;
                    location.avisCandidat = val;
                    location.signCandidat = val; 
                }
                else if (_me.type.Equals("client")) {
                    location.dateSgnClt = DateTime.Now;
                    location.avisClient = val;
                    location.signClient = val;
                }

                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { result_code = 1, data = getJson(location) }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { result_code = -2, message = "An execption occured" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost] 
        public async Task<JsonResult> contratLocationClient(String stripeToken, decimal idLocation)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            if (_me.Equals("candidat")) return Json(new { result_code = -1, message = "Your are not allowed" }, JsonRequestBehavior.AllowGet);

            Location currentLocation = db.Location.FirstOrDefault(p => p.id == idLocation);
            if (currentLocation == null) return Json(new { result_code = -1, message = "item not found" }, JsonRequestBehavior.AllowGet);
     
            StripeConfiguration.SetApiKey(STRIPE_API_KEY);
            var amount = (long)Math.Ceiling(currentLocation.remuneration.Value) * 35 / 100;
            var stripeOptions = new ChargeCreateOptions
            {

                Amount = (long)amount, // 1 dollar is equal to 100 cent. 
                Currency = "USD",
                Description = "Charge for payment of 35% of Location" ,
                Source = stripeToken, 
            };
            var service = new ChargeService();  
            currentLocation.dateSgnClt = DateTime.Now;
            currentLocation.avisClient = 2;
            currentLocation.signClient = 2;
            db.Entry(currentLocation).State = EntityState.Modified;

             
            FraisLocation fraisLocation = new FraisLocation();
            fraisLocation.archived = 1;
            fraisLocation.avance = amount;
            fraisLocation.created = DateTime.Now;
            fraisLocation.etat = 0; //TODO mettre le bon etat
            fraisLocation.Ins_id = currentLocation.Ins_id;
            fraisLocation.Ins_id2 = currentLocation.Ins_id2;
            fraisLocation.Loc_id = currentLocation.id;
            fraisLocation.libelle = "Paiement de 35% pour la location";
            fraisLocation.montant = Convert.ToDouble(currentLocation.remuneration);
            fraisLocation.userId = (int)_me.id;
            fraisLocation.reste = Convert.ToDouble(currentLocation.remuneration) - amount;
            fraisLocation.status = 0;
         
            //TODO mettre le bon status je suppose o veut dire non achevé   
            db.FraisLocation.Add(fraisLocation);


            try
            {
                Charge charge = service.Create(stripeOptions);
                var map = new Dictionary<String, String>();
                map.Add("@ViewBag.titre", "Paiement de 35% du salaire pour la location  de l'employé " + currentLocation.Inscrire.nom);
                map.Add("@ViewBag.login", _me.nom);
                map.Add("@ViewBag.content", "Votre paiement a bien été pris en compte. Le montant à été prélever de votre compte avec succès. Bénéficiez du meilleur de nos service.");
                string body = MsMail.BuldBodyTemplate("~/EmailTemplate/CustomEmail.cshtml", map);

                var map1 = new Dictionary<String, String>();
                map1.Add("@ViewBag.titre", "Vous êtes sollicité pour un travail  ");
                map1.Add("@ViewBag.login", currentLocation.Inscrire.nom);
                map1.Add("@ViewBag.content", "Votre demande de location a été confimer. Vous êtes conviez a prendre service le plutôt possible.");
                string body2 = MsMail.BuldBodyTemplate("~/EmailTemplate/CustomEmail.cshtml", map1);

                MsMail mail = new MsMail();
                MsMail mail2 = new MsMail();

                db.SaveChanges();

                //TODO retraviller le mail avec du HTML 
                await mail.Send(_me.email, "Paiement location", body);
                await mail2.Send(currentLocation.Inscrire.email, "NiovarJobs, Demande location accepté ", body2);
                //TODO rediriger a la liste des candidatures
                return Json(new { result_code = 1, data = getJson(currentLocation) }, JsonRequestBehavior.AllowGet);
            }
            catch (StripeException e)
            {
                return Json(new { result_code = -2, message = "An execption occured" }, JsonRequestBehavior.AllowGet);
            }

        } 
    }
}
