using Stripe;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.WebPages;
using WebApplication1.Models;
using X.PagedList;

namespace WebApplication1.Controllers
{
    public class RestPaymentController : MybaseController
    {

        [HttpGet]
        public JsonResult List()
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            String q = Request["q"];
            int page = !string.IsNullOrEmpty(Request["page"]) ? Request["page"].AsInt() : 1;
            int limit = 10;
            var listPaiement = db.Paiement.AsQueryable();
            listPaiement = listPaiement.Where(p => p.userId == _me.id);
            if (!string.IsNullOrEmpty(q)) listPaiement = listPaiement.Where(p => p.libelle.Contains(q) || 
            p.Postuler.Job.titre.Contains(q) || 
            p.Postuler.Inscrire.nom.Contains(q) ||
            p.Postuler.Inscrire.email.Contains(q) || 
            p.Postuler.Inscrire.login.Contains(q) || 
            p.Postuler.Inscrire.domaine.Contains(q));
           
            var count = listPaiement.Count();

            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in listPaiement.OrderByDescending(p => p.created).ToPagedList(page, limit)) datas.Add(getJson(item));
            return Json(new { result_code = 1, page = page, count = count, limit = limit, datas = datas }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult ListPaiementUser()
        {

            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            if (_me.Equals("candidat")) return Json(new { result_code = -1, message = "Your are not allowed" }, JsonRequestBehavior.AllowGet);

            int page = !string.IsNullOrEmpty(Request["page"]) ? Request["page"].AsInt() : 1;
            int limit = 10;

            var listFraisLocation = db.FraisLocation.AsQueryable();
            listFraisLocation = listFraisLocation.Where(p => p.userId == _me.id).OrderByDescending(p => p.created);
            var count = listFraisLocation.Count();
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (FraisLocation item in listFraisLocation.ToPagedList(page, limit)) datas.Add(getJson(item));
            return Json(new { result_code = 1, count = count, limit = limit, datas = datas }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAbonnement()
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            if(_me.type != "client") return Json(new { result_code = -10, message = "Not autorized for this action" }, JsonRequestBehavior.AllowGet);
            return Json(new { result_code = 1, item = getJson(this.lastAsbonnement) }, JsonRequestBehavior.AllowGet); 
        }


        [HttpPost]
        public async Task<JsonResult> PayContratClient(String stripeToken, Decimal idPostuler)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            if (_me.type != "client") return Json(new { result_code = -10, message = "Not autorized for this action" }, JsonRequestBehavior.AllowGet);
            Postuler postuler = db.Postuler.FirstOrDefault(x => x.id == idPostuler);
            if (postuler == null || String.IsNullOrEmpty(stripeToken)) return Json(new { result_code = -1, message = "Data not found" }, JsonRequestBehavior.AllowGet);
            StripeConfiguration.SetApiKey(STRIPE_API_KEY);

            double amount = postuler.Job.remuneration.Value * 25 / 100;
            if (amount<=0) return Json(new { result_code = -1, message = "Amount not valid" }, JsonRequestBehavior.AllowGet);
            var stripeOptions = new ChargeCreateOptions
            {
                Amount = (long) amount, // 1 dollar is equal to 100 cent. 
                Currency = "USD",
                Description = "Charge for payment of 25% of" + postuler.libelle,
                Source = stripeToken,
            };
            var service = new ChargeService();
            postuler.etatClient = "2";
            postuler.signatureClient = 1;
            postuler.dateSignClient = DateTime.Now;
            db.Entry(postuler).State = EntityState.Modified;

            Paiement paiement = new Paiement();
            paiement.archived = 1;
            paiement.avance = amount;
            paiement.created = DateTime.Now;
            paiement.etat = 0; //TODO mettre le bon etat
            paiement.status = 0; //TODO mettre le bon status je suppose o veut dire non achevé 
            paiement.Ins_id = postuler.Ins_id;
            paiement.Job_id = postuler.Job_id;
            paiement.libelle = "Paiement de 25% de l'offre";
            paiement.montant = postuler.Job.remuneration.Value;
            paiement.Pos_id = postuler.id;
            paiement.userId = Convert.ToInt32(_me.id);
            paiement.reste = postuler.Job.remuneration - amount; 

            db.Paiement.Add(paiement); 
            try
            {
                Charge charge = service.Create(stripeOptions);
                var map = new Dictionary<String, String>();
                map.Add("@ViewBag.titre", "Paiement de 25% du salaire via skype" + postuler.libelle);
                map.Add("@ViewBag.login", _me.login);
                map.Add("@ViewBag.content", "Votre paiement a bien été pris en compte. Le montant à été prélever de votre compte avec succès. Reste à payer : 5 $. Bénéficiez du meilleur de nos service.");
                string body = MsMail.BuldBodyTemplate("~/EmailTemplate/CustomEmail.cshtml", map);
                MsMail mail = new MsMail();

                db.SaveChanges();

                //TODO retraviller le mail avec du HTML 
                await mail.Send(_me.email, "Paiement", body);
                //TODO rediriger a la liste des candidatures
                return Json(new { result_code = 1, postuler = getJson(postuler), payment = getJson(paiement) }, JsonRequestBehavior.AllowGet);
            }
            catch (StripeException e)
            {
                return Json(new { result_code = -1, message = "An exception occured" }, JsonRequestBehavior.AllowGet);
            } 
        }


        [HttpGet]
        public JsonResult ListTypeAbonnement()
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            if (_me.type != "client") return Json(new { result_code = -10, message = "Not autorized for this action" }, JsonRequestBehavior.AllowGet);
            ViewBag.inscrire = _me;
            var list = db.Abonnement.Where(a => a.archived == 1 && a.status == 1);

            int illimitedAbonnement = (hasIllimitedAbonnement(lastAsbonnement) && lastAsbonnement.status == 1) ? 1 : 0;

            int canTakeFree = 0;
            if (!needAbonnement() ||
                (lastAsbonnement != null && 
                hasFreeAbonnement(lastAsbonnement) &&
                DateTime.Parse(lastAsbonnement.dateFin) >= DateTime.Now)
                )
            {
                list.Where(a => !a.id.Equals(lastAsbonnement.Abo_id)); // sa ne prend pas mais jai géré ca ds la vue
                canTakeFree = 1;
            }
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in list.ToList()) datas.Add(getJson(item));
            return Json(new { result_code = 1, datas = datas, canTakeFree = canTakeFree, illimitedAbonnement = illimitedAbonnement }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<JsonResult> PreparePayment(decimal id)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            if (_me.type != "client") return Json(new { result_code = -10, message = "Not autorized for this action" }, JsonRequestBehavior.AllowGet);
            Abonnement abonnement = db.Abonnement.Find(id);
            if (abonnement == null) return Json(new { result_code = -1, message = "Data not found" }, JsonRequestBehavior.AllowGet);

            if (hasIllimitedAbonnement(lastAsbonnement) && lastAsbonnement.status == 1)
            {
                return Json(new { result_code = -1, message = "You don't need abonnement." }, JsonRequestBehavior.AllowGet);
            }

            if (hasFreeAbonnement(lastAsbonnement)) Json(new { result_code = -1, message = "Your free abonnement is not complete." }, JsonRequestBehavior.AllowGet);

            if (isFreeAbonnement(abonnement))

            {
                InsAbonne insAbonne = new InsAbonne();
                insAbonne.Abo_id = abonnement.id;
                insAbonne.Ins_id = _me.id;
                insAbonne.libelle = "Abonnement " + abonnement.titre;
                insAbonne.status = 1;
                insAbonne.archived = 1;
                insAbonne.etat = abonnement.nbrePost;
                insAbonne.created = DateTime.Now;
                insAbonne.dateDebut = DateTime.Now.ToString();
                insAbonne.dateFin = DateTime.Now.AddMonths(1).ToString();
                db.InsAbonne.Add(insAbonne);
                if (this.lastAsbonnement != null)
                {
                    lastAsbonnement.status = 0;
                    db.Entry(lastAsbonnement).State = EntityState.Modified;
                }
                db.SaveChanges();

                //TODO retraviller le mail avec du HTML
                MsMail mail = new MsMail();
                await mail.Send(_me.email, "Abonnement " + abonnement.titre, "Votre abonnement a bien été pris en compte. Bénéficiez du meilleur de nos service.", null, null, null);

                return Json(new { result_code = 1, data = getJson(insAbonne)}, JsonRequestBehavior.AllowGet);
            } else
            {
                return Json(new { result_code = 2, abonnement = getJson(abonnement) }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> PayAbonnement(String stripeToken, Decimal id)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            if (_me.type != "client") return Json(new { result_code = -10, message = "Not autorized for this action" }, JsonRequestBehavior.AllowGet);
            Abonnement abonnement = db.Abonnement.Find(id);
            if (abonnement == null || String.IsNullOrEmpty(stripeToken)) return Json(new { result_code = -1, message = "Data not found" }, JsonRequestBehavior.AllowGet);
            StripeConfiguration.SetApiKey(STRIPE_API_KEY);


            double amount = abonnement.montant.Value * 100;
            if (amount <= 0) return Json(new { result_code = -1, message = "Amount not valid" }, JsonRequestBehavior.AllowGet);
            var stripeOptions = new ChargeCreateOptions
            { 
                Amount = (long)Math.Ceiling(amount), // 1 dollar is equal to 100 cent 
                Currency = "USD",
                Description = "Charge for " + _me.email + " to abonnement " + abonnement.titre,
                Source = stripeToken,
            };
            var service = new ChargeService();

            InsAbonne insAbonne = new InsAbonne();
            insAbonne.Abo_id = abonnement.id;
            insAbonne.Ins_id = _me.id;
            insAbonne.libelle = "Abonnement " + abonnement.titre;
            insAbonne.status = 1;
            insAbonne.archived = 1;
            insAbonne.etat = abonnement.nbrePost;
            insAbonne.created = DateTime.Now;
            insAbonne.dateDebut = DateTime.Now.ToString();

            if (!needAbonnement() && DateTime.Parse(lastAsbonnement.dateFin) > DateTime.Now)
            {
                insAbonne.dateFin = DateTime.Parse(lastAsbonnement.dateFin).AddMonths(1).ToString();
                insAbonne.etat += lastAsbonnement.etat;
                lastAsbonnement.status = 0;
                db.Entry(lastAsbonnement).State = EntityState.Modified;
            }
            else
            {
                insAbonne.dateFin = DateTime.Now.AddMonths(1).ToString();
            }
            db.InsAbonne.Add(insAbonne);
            try
            {
                Charge charge = service.Create(stripeOptions);
                var map = new Dictionary<String, String>();
                map.Add("@ViewBag.titre", " Payment de l'abonnement " + abonnement.titre);
                map.Add("@ViewBag.login", _me.login);
                map.Add("@ViewBag.content", "Votre abonnement a bien été pris en compte. Bénéficiez du meilleur de nos service.");
                string body = MsMail.BuldBodyTemplate("~/EmailTemplate/CustomEmail.cshtml", map);
                MsMail mail = new MsMail();
                if (this.lastAsbonnement != null)
                {
                    lastAsbonnement.status = 0;
                    db.Entry(lastAsbonnement).State = EntityState.Modified;
                }
                db.SaveChanges();

                //TODO retraviller le mail avec du HTML 
                await mail.Send(_me.email, "Abonnement", body);
                return Json(new { result_code = 1, data = getJson(insAbonne) }, JsonRequestBehavior.AllowGet);
            }
            catch (StripeException e)
            {
                return Json(new { result_code = -1, message = "An exception occured" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
