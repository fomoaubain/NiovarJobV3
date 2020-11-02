using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Stripe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class InscriresController : MybaseController
    {
        //private NiovarJobEntities db = new NiovarJobEntities();

        // GET: Inscrires
        [CustomAuthorized]
        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN, Constante.roleGESTIONNAIRE })]
        [HttpGet]
        public ActionResult Index()
        {
       
        String pays = Request["pays"];
            String ville = Request["ville"];
            String type = Request["type"];
            String experience = Request["experience"];
            String nom = Request["nom"];
            

            var listInscrire = db.Inscrire.AsQueryable();
            listInscrire = listInscrire.Where(p => p.archived==1).OrderByDescending(p => p.created);
            if (!string.IsNullOrEmpty(pays)) listInscrire = listInscrire.Where(p => p.pays.Contains(pays));
            if (!string.IsNullOrEmpty(ville)) listInscrire = listInscrire.Where(p => p.ville.Contains(ville));
            if (!string.IsNullOrEmpty(type)) listInscrire = listInscrire.Where(p => p.type.Contains(type));
            if (!string.IsNullOrEmpty(nom)) listInscrire = listInscrire.Where(p => p.nom.Contains(nom));
            if (!string.IsNullOrEmpty(experience)) listInscrire = listInscrire.Where(p => p.anneeExperience.Contains(experience));

            List<AnneeExp> listAnneeExp = db.AnneeExp.ToList();
            ViewBag.listAnneeExp = listAnneeExp;

            return View(listInscrire.ToList());
        }


        [CustomAuthorized]
        // GET: Inscrires/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inscrire inscrire = db.Inscrire.Find(id);
            if (inscrire == null)
            {
                return HttpNotFound();
            }
            return View(inscrire);
        }

        // GET: Inscrires/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index","Home");
        }

        public ActionResult EditStatus(decimal id)
        {
            try
            {
                var currentIncris = db.Inscrire.FirstOrDefault(p => p.id == id);
                if (currentIncris == null)
                {
                    ViewBag.Message = "L'incris n'existe pas !";

                    return RedirectToAction("Index");
                }

                if (currentIncris.status == 1)
                {
                    currentIncris.status = 0;
                }
                else
                {
                    currentIncris.status = 1;
                }

                db.Entry(currentIncris).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.Message = "modification éffectuer avec succès  !";

                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                return RedirectToAction("Index");

            }
        }


        public async Task<ActionResult> Login()
        { 
            /*var app = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(AppDomain.CurrentDomain.BaseDirectory + "/Json/niovarjobsnotifications-firebase-adminsdk.json").CreateScoped("https://www.googleapis.com/auth/firebase.messaging")
            });
            FirebaseMessaging messaging = FirebaseMessaging.GetMessaging(app);
            Message message = new Message()
            {
                Token = null,
                Notification = new Notification()
                {
                    Body = "message",
                    Title = "titre"
                }
            }; 
            var result = await messaging.SendAsync(message);*/

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "email, password")] Inscrire inscrire, String returnUrl)
        {
            Inscrire currentUser = null;

            try
            {
                 currentUser = db.Inscrire.Single(x => x.email == inscrire.email && x.password==inscrire.password && x.archived == 1);

                if (currentUser != null)
                {
                    if (currentUser.etat == 0)
                    {
                        ViewBag.Message = " Ce compte n'a pas été confirmer  par mail !";
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(currentUser.id.ToString(),false);
                        Session.Clear();
                        Session.Add("currentUser", currentUser);
                        Session["email"] = currentUser.email;
                        Session["password"] = currentUser.password;
                        Session["login"] = currentUser.login;
                        Session["profil"] = currentUser.profil;
                        Session["nom"] = currentUser.nom;
                        Session["type"] = currentUser.type;
                        Session["id"] = currentUser.id;
                        Session["ville"] = currentUser.ville;
                        Session["pays"] = currentUser.pays;
                        Session["status"] = currentUser.status;
                        Session["nom_representant"] = currentUser.nomRepresentant;
                        Session["adresse"] = currentUser.adresse;
                        Session.Add("inscrire", currentUser);
                        Session["sessionConnect"] = "site";

                        if (!String.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);

                        ViewBag.Message = "Connexion éffectué avec succès !";
                        if(needAbonnement()) RedirectToAction("Abonnement");
                        return RedirectToAction("Index", "Home");
                    }

                }
              
            }catch(Exception)
            {
                ViewBag.Message = "Email ou mot de passe incorrect !";
                return View(inscrire);
            }

            return View(inscrire);
        }

        public ActionResult Confirm(decimal regId)
        {
            Inscrire inscrire = db.Inscrire.FirstOrDefault(p => p.id == regId);

            ViewBag.Message = "Merci de bien vouloir confirmer mon compte";
            ViewBag.Etat = 0;

            if (inscrire == null)
            {
                ViewBag.Message = "L'activation ne peut ce faire car se compte n'existe pas. Veuillez creer un compte";
                ViewBag.Etat = 2;
            }

            if (inscrire.etat == 1)
            {
                ViewBag.Message = "Votre compte à été activé avec succès !";
                ViewBag.Etat = 1;
            }


            return View(inscrire);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm([Bind(Include = "id")] Inscrire inscrire)
        {
           
            try
            {
                var currentInscrire = db.Inscrire.FirstOrDefault(p => p.id == inscrire.id);
                if (currentInscrire == null)
                {
                    ViewBag.Message = "Votre compte n'existe pas !";
                    return RedirectToAction("Confirm", new { regId = inscrire.id });
                }
                   
                currentInscrire.etat = 1;
                
                db.Entry(currentInscrire).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.Message = "Votre compte a été activer avec succès !";

                return RedirectToAction("Confirm", new {regId = currentInscrire.id });

            }
            catch (Exception)
            {
                return RedirectToAction("Confirm", new { regId = inscrire.id });

            }
        }

        // POST: Inscrires/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "login,email,password,cpassword,type")] Inscrire inscrire)
        {
            if (ModelState.IsValid)
            {
                Inscrire emailExist = db.Inscrire.Where(x => x.email == inscrire.email && x.archived==1).FirstOrDefault();

                if (emailExist != null)
                {
                    if (emailExist.etat==0)
                    {
                        ViewBag.Message = "Ce compte existe déja mais il n'a pas été confirmer par mail";
                    }
                    else
                    {
                        ViewBag.Message = "Cette adresse mail est dejà utilisé par un autre compte";
                    }
                   
                }
                else
                {
                    if (inscrire.type.Equals("client")) {
                        inscrire.nom = inscrire.login;
                    }


                    inscrire.status = 0;
                    inscrire.archived = 1;
                    inscrire.etat = 0;
                    inscrire.created = DateTime.Now;
                    db.Inscrire.Add(inscrire);
                    db.SaveChanges();

                    if (inscrire.type.Equals("client"))
                    {
                        Page page = new Page();
                        page.Ins_id = inscrire.id;
                        page.archived = 1;
                        page.created = DateTime.Now;
                        page.status = 1;
                        db.Page.Add(page);
                        db.SaveChanges();
                    }
                    

                    var url = MsMail.baseUrl + "Inscrires/Confirm?regId=" + inscrire.id; 

                    var map = new Dictionary<String, String>();
                    map.Add("@ViewBag.ConfirmationLink", url);
                    map.Add("@ViewBag.login", inscrire.login);
                    string body = MsMail.BuldBodyTemplate("~/EmailTemplate/Text.cshtml", map);
                    MsMail mail = new MsMail();
                   
                    await mail.Send(inscrire.email, "Email de confirmation du compte", body); 

                    ViewBag.Message = "Un courriel de confirmation vous a été envoyer à votre adresse mail ";

                    return View(inscrire);
                }
                         
            }

            return View(inscrire);
        }


        // GET: Inscrires/Create
        public ActionResult Abonnement()
        {
            if(_me == null || _me.type != "client") return RedirectToAction("Index", "Home");
            ViewBag.inscrire = _me;
            var datas = db.Abonnement.Where(a => a.archived == 1 && a.status == 1);

            if(hasIllimitedAbonnement(lastAsbonnement) && lastAsbonnement.status==1)
            {
                TempData["result_code"] = -1;
                TempData["message"] = "Vous ne pouvez souscrire a un autre abonnement quand vos posts illimités sont encore actifs.";
                TempData.Keep();
                return RedirectToAction("AbonnementList", "Profile");
            }
            if (!needAbonnement() || 
                (lastAsbonnement != null && hasFreeAbonnement(lastAsbonnement) && DateTime.Parse(lastAsbonnement.dateFin) >= DateTime.Now))
            {
                datas.Where(a => a.id != lastAsbonnement.Abo_id); // sa ne prend pas mais jai géré ca ds la vue
                ViewBag.canTakeFree = true;
            }
            ViewBag.lastAsbonnement = lastAsbonnement;
            return View(datas.ToList());
        }

        // GET: Inscrires/Create
        public async Task<ActionResult> Payments(decimal id)
        { 
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Abonnement abonnement = db.Abonnement.Find(id);
            if (abonnement == null) return HttpNotFound();


            if (hasIllimitedAbonnement(lastAsbonnement))
            {
                TempData["result_code"] = -1;
                TempData["message"] = "Vous ne pouvez souscrire a un autre abonnement quand vos posts illimités sont encore actifs.";
                TempData.Keep();
                return RedirectToAction("AbonnementList", "Profile");
            }
            if (!needAbonnement() && isFreeAbonnement(abonnement) || 
                lastAsbonnement != null && hasFreeAbonnement(lastAsbonnement) && DateTime.Parse(lastAsbonnement.dateFin) >= DateTime.Now)

            {
               // return RedirectToAction("Abonnement", "Inscrires");
            } 
            if (_me == null || _me.type != "client")
            {
                TempData["result_code"] = -1;
                TempData["message"] = "Temps fourni pour abonnement expiré veillez réessayer.";
                TempData.Keep();
                return RedirectToAction("Index", "Home");
            }

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

                ViewBag.inscrire = _me;
                ViewBag.insAbonne = insAbonne;
                ViewBag.hasPaid = true;
                return View(abonnement);
            }
            Session.Add("abonnement", abonnement); 
            ViewBag.inscrire = _me; 
            return View(abonnement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Payments(String stripeToken)
        { 
            Abonnement abonnement = (Abonnement) Session["abonnement"];
            if (abonnement == null || _me == null || _me.type != "client")
            {
                TempData["result_code"] = -1;
                TempData["message"] = "Temps fourni pour abonnement expiré veillez réessayer.";
                TempData.Keep();
                return RedirectToAction("Index", "Home");
            }
            if (String.IsNullOrEmpty(stripeToken)) return HttpNotFound();
            StripeConfiguration.SetApiKey(STRIPE_API_KEY);
             
            var stripeOptions  = new ChargeCreateOptions
            {

                Amount = (long) Math.Ceiling(abonnement.montant.Value) * 100, // 1 dollar is equal to 100 cent.

                Currency = "USD",
                Description = "Charge for "+ _me.email +" to abonnement "+ abonnement.titre,
                Source = stripeToken,
                //Customer = customer.Id
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
                if(this.lastAsbonnement != null)
                {
                    lastAsbonnement.status = 0;
                    db.Entry(lastAsbonnement).State = EntityState.Modified; 
                }
                db.SaveChanges(); 

                //TODO retraviller le mail avec du HTML 
                await mail.Send(_me.email, "Abonnement", body);  
            }catch(StripeException e)
            { 

            }
            ViewBag.inscrire = _me; 
            ViewBag.insAbonne = insAbonne;
            ViewBag.hasPaid = true;
            return View(abonnement);
        }
          

        // GET: Inscrires/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inscrire inscrire = db.Inscrire.Find(id);
            if (inscrire == null)
            {
                return HttpNotFound();
            }
            return View(inscrire);
        }

        // POST: Inscrires/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,type,nom,login,email,password,phone,website,description,facebook,linkedin,pays,ville,adresse,long,lat,titreEmploi,anneeExperience,salaireSouhaiter,salaireNegocier,profil,etat,status,archived,created,cpassword")] Inscrire inscrire)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inscrire).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inscrire);
        }

        // GET: Inscrires/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inscrire inscrire = db.Inscrire.Find(id);
            if (inscrire == null)
            {
                return HttpNotFound();
            }
            return View(inscrire);
        }

        // POST: Inscrires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Inscrire inscrire = db.Inscrire.Find(id);
            inscrire.archived = 0;
            db.Entry(inscrire).State = EntityState.Modified;
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
