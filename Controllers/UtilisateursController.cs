using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.WebPages;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UtilisateursController : MybaseController
    {
        //private NiovarJobEntities db = new NiovarJobEntities();

        // GET: Utilisateurs
        [CustomAuthorized]
        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN})]
        public ActionResult Index()
        {
            

            var utilisateur = db.Utilisateur.Include(u => u.Role);
            return View(utilisateur.Where(p => p.archived == 1).ToList());
        }



       
        [HttpPost]
        public ActionResult SendMail()
        {
            ViewBag.emails = Request.Form["emails"];
            return View();
        }

       
        public async Task<JsonResult> SendMailJsonAsync()
        {
            int totalFiles = Request.Form["totalFiles"].AsInt();
            String subject = Request.Form["subject"];
            String message = Request.Form["message"];
            String[] emails = Request.Form["to"].Split(',');
            if (emails.Length == 0) return Json(new { result_code = -1, message = "Recipients not found." }, JsonRequestBehavior.AllowGet);

            List<String> To = new List<String>();
            for (int i = 0; i < emails.Length; i++)
            {
                To.Add(emails[i]);
            }

            List<String> Attachement = new List<String>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                var filename = Path.GetFileName(file.FileName);
                var formattedFileName = string.Format("{0}-{1}{2}"
                    , Path.GetFileNameWithoutExtension(filename)
                    , Guid.NewGuid().ToString("N")
                    , Path.GetExtension(filename));
                var path = Path.Combine(Server.MapPath("~/Assets/mails/"), formattedFileName);
                file.SaveAs(path);
                Attachement.Add(path);
            }
            MsMail mail = new MsMail();
            await mail.Send(To, subject, message, null, null, Attachement);
            return Json(new { result_code = 1, msg = "Email envoyé avec succès"/*, totalFiles = totalFiles, subject = subject, message = message, emails = emails*/ }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("LoginAdmin");
        }

        public ActionResult LoginAdmin()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginAdmin([Bind(Include = "email,password")] Utilisateur utilisateur, String returnUrl)
        {
            Utilisateur currentUser = null;

            try
            {
                currentUser = db.Utilisateur.Single(x => x.email == utilisateur.email && x.password == utilisateur.password && x.status == 1 && x.archived == 1);


                if (currentUser != null)
                {
                    FormsAuthentication.SetAuthCookie(currentUser.id.ToString(), false);
                    Session.Clear();
                    Session.Add("currentUserAdmin", currentUser);
                    Session["emailAdmin"] = currentUser.email;
                    Session["passwordAdmin"] = currentUser.password;
                    Session["nomAdmin"] = currentUser.nom;
                    Session["role"] = currentUser.Role.libelle;
                    Session["sessionConnect"] = "panel";

                    if (!String.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    ViewBag.Message = "Connexion éffectué avec succès !";
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.Message = " Login ou mot de passe incorrect !";
                    return View();
                }

            }
            catch (Exception)
            {
                ViewBag.Message = " Login ou mot de passe incorrect !";
                return View();
            }
                 

           return View();
        }

        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN })]
        // GET: Utilisateurs/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateur.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN })]
        // GET: Utilisateurs/Create
        [CustomAuthorized]
        public ActionResult Create()
        {
           

            ViewBag.Rol_id = new SelectList(db.Role, "id", "libelle");
            return View();
        }

        // POST: Utilisateurs/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Rol_id,nom,email")] Utilisateur utilisateur)
        {

            if (ModelState.IsValid)
            {
                Utilisateur currentUser = db.Utilisateur.Where(x => x.email == utilisateur.email && x.archived == 1).FirstOrDefault();
                if (currentUser !=null)
                {
                    String message = "cette adresse email est deja utilisé !";
                    return RedirectToAction("ErrorPage", "Home", new { sms = message });
                }

                utilisateur.created = DateTime.Now;
                utilisateur.archived = 1;
                utilisateur.status = 0;
                utilisateur.password = generatePassword();
                db.Utilisateur.Add(utilisateur);

                try
                {
                    var map = new Dictionary<String, String>();
                    map.Add("@ViewBag.titre", "Création compte administrateur NiovarJobs ");
                    map.Add("@ViewBag.login", utilisateur.nom);
                    map.Add("@ViewBag.content", "Votre compte administrateur a été créer avec succès utilisé ces identifiant pour vous connecter email :"+ utilisateur.email+ "et mot de passe :"+utilisateur.password );
                    string body = MsMail.BuldBodyTemplate("~/EmailTemplate/CustomEmail.cshtml", map);
                    MsMail mail = new MsMail();

                    await mail.Send(utilisateur.email, "Identifiant de votre compte", body);
                    db.SaveChanges();
                    ViewBag.Message = "Compte administrateur crée avec succès";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ViewBag.Rol_id = new SelectList(db.Role, "id", "libelle", utilisateur.Rol_id);
                    return View(utilisateur);
                }
                 
              
            }

            ViewBag.Rol_id = new SelectList(db.Role, "id", "libelle", utilisateur.Rol_id);
            return View(utilisateur);
        }

        public String generatePassword()
        {
            String allowedChars = "";
            allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";  
            allowedChars += "1,2,3,4,5,6,7,8,9,0,#,@";
            char[] sep = {','};
            string[] arr = allowedChars.Split(sep);
            string passwordString = ""; 
            string temp = ""; 
            Random rand = new Random(); 
            for (int i = 0; i < Convert.ToInt32(8); i++) 
            { 
                temp = arr[rand.Next(0, arr.Length)]; 
                passwordString += temp;
            }
            String generatePassword = passwordString;
            return generatePassword;
        }

        [CustomAuthorized]
        public ActionResult EditStatus(decimal id)
        {
            
            try
            {
                var currentUser = db.Utilisateur.FirstOrDefault(p => p.id == id);
                if (currentUser == null)
                {
                    ViewBag.Message = "Votre compte n'existe pas !";

                    return RedirectToAction("Index");
                }

                if (currentUser.status==1)
                {
                    currentUser.status = 0;
                }
                else {
                    currentUser.status = 1;
                }

                db.Entry(currentUser).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.Message = "modification éffectuer avec succès avec succès !";

                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                return RedirectToAction("Index");

            }
        }

        // GET: Utilisateurs/Edit/5
        [CustomAuthorized]
        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN })]
        public ActionResult Edit(decimal id)
        {
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Utilisateur utilisateur = db.Utilisateur.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            ViewBag.Rol_id = new SelectList(db.Role, "id", "libelle", utilisateur.Rol_id);
            return View(utilisateur);
        }

        // POST: Utilisateurs/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Rol_id,nom,email")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                utilisateur.archived = 1;
                db.Entry(utilisateur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Rol_id = new SelectList(db.Role, "id", "libelle", utilisateur.Rol_id);
            return View(utilisateur);
        }

        // GET: Utilisateurs/Delete/5
        [CustomAuthorized]
        [AuthorizedAccessAction(roleAccess = new string[] { Constante.roleADMIN })]
        public ActionResult Delete(decimal id)
        {
           

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateur.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // POST: Utilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Utilisateur utilisateur = db.Utilisateur.Find(id);
            utilisateur.archived = 0;
            db.Entry(utilisateur).State = EntityState.Modified;
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

        /*
        [HttpGet]
        public async Task<ActionResult> SendMailAsync()
        {
            String subject = Request["subject"];
            String message = Request["message"];
            String[] emails = Request["to"].Split(',');
            if (emails.Length == 0) RedirectToAction("SendMail");

            List<String> To = new List<String>();
            for (int i = 0; i < emails.Length; i++)
            {
                To.Add(emails[i]);
            }

            List<String> Attachement = new List<String>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                var filename = Path.GetFileName(file.FileName);
                var formattedFileName = string.Format("{0}-{1}{2}"
                    , Path.GetFileNameWithoutExtension(filename)
                    , Guid.NewGuid().ToString("N")
                    , Path.GetExtension(filename));
                var path = Path.Combine(Server.MapPath("~/Assets/mails/"), formattedFileName);
                file.SaveAs(path);
                Attachement.Add(path);
            }
            MsMail mail = new MsMail();
            await mail.Send(To, subject, message, null, null, Attachement);
            return RedirectToAction("SendMail");
        }*/
    }
}
