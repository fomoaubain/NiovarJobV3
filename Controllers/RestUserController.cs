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

namespace WebApplication1.Controllers
{
    public class RestUserController : MybaseController
    {

        [HttpPost]
        public JsonResult Login([Bind(Include = "email, password")] Inscrire inscrire)
        {
            Inscrire user = null;

            try
            {
                user = db.Inscrire.Single(x => x.email == inscrire.email && x.password == inscrire.password && x.archived == 1);
                if (user == null) return Json(new { result_code = -1, message = "Account not found. Create an account !" }, JsonRequestBehavior.AllowGet);
                if (user.etat == 0)  return Json(new { result_code = -3, message = "Account not activated. See your mail to activate !" }, JsonRequestBehavior.AllowGet);  
                    
                FormsAuthentication.SetAuthCookie(user.id.ToString(), true);
                Session.Clear();
                Session.Add("inscrire", user);

                return Json(new { result_code = 1, user = getJson(user) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result_code = -2, message = "Accound not found. Create an account !" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public async Task<JsonResult> Create([Bind(Include = "login,email,password,cpassword,type")] Inscrire inscrire)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var emailExist = db.Inscrire.Where(x => x.email == inscrire.email).FirstOrDefault();

                    if (emailExist != null) return Json(new { result_code = -1, message = "This email already exist" }, JsonRequestBehavior.AllowGet);
                 
                    inscrire.status = 0;
                    inscrire.archived = 1;
                    inscrire.etat = 0;
                    inscrire.created = DateTime.Now;
                    db.Inscrire.Add(inscrire);

                    var url = MsMail.baseUrl + "Inscrires/Confirm?regId=" + inscrire.id;

                    var map = new Dictionary<String, String>();
                    map.Add("@ViewBag.ConfirmationLink", url);
                    map.Add("@ViewBag.login", inscrire.login);
                    string body = MsMail.BuldBodyTemplate("~/EmailTemplate/Text.cshtml", map);
                    MsMail mail = new MsMail();

                    db.SaveChanges();

                    await mail.Send(inscrire.email, "Email de vérification du compte", body);

                    return Json(new { result_code = 1 }, JsonRequestBehavior.AllowGet);


                }
                catch (Exception)
                {
                    return Json(new { result_code = -2, message = "An error occured" }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { result_code = -2, message = "Datas not found" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Edit()
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            try
            {
                Inscrire inscrire = db.Inscrire.Find(_me.id); 
                if (!String.IsNullOrEmpty(Request.Form["nom"])) inscrire.nom = Request.Form["nom"];
                if (!String.IsNullOrEmpty(Request.Form["phone"])) inscrire.phone = Request.Form["phone"];
                if (!String.IsNullOrEmpty(Request.Form["login"])) inscrire.login = Request.Form["login"];
                if (!String.IsNullOrEmpty(Request.Form["email_prof"])) inscrire.emailProf = Request.Form["email_prof"];
                if (!String.IsNullOrEmpty(Request.Form["code_postal"])) inscrire.codePostal = Request.Form["code_postal"];
                if (!String.IsNullOrEmpty(Request.Form["domaine"])) inscrire.domaine = Request.Form["domaine"];
                if (!String.IsNullOrEmpty(Request.Form["categorie"])) inscrire.categorie = Request.Form["categorie"];
                if (!String.IsNullOrEmpty(Request.Form["description"])) inscrire.description = Request.Form["description"];
                if (!String.IsNullOrEmpty(Request.Form["nom_representant"])) inscrire.nomRepresentant = Request.Form["nom_representant"];
                if (!String.IsNullOrEmpty(Request.Form["prenom_representant"])) inscrire.prenomRepresentant = Request.Form["prenom_representant"];
                if (!String.IsNullOrEmpty(Request.Form["pays"])) inscrire.pays = Request.Form["pays"];
                if (!String.IsNullOrEmpty(Request.Form["province"])) inscrire.province = Request.Form["province"];
                if (!String.IsNullOrEmpty(Request.Form["ville"])) inscrire.ville = Request.Form["ville"];
                if (!String.IsNullOrEmpty(Request.Form["numero_poste"])) inscrire.numeroPoste = Request.Form["numero_poste"];
                if (!String.IsNullOrEmpty(Request.Form["adresse"])) inscrire.adresse = Request.Form["adresse"];
                if (!String.IsNullOrEmpty(Request.Form["website"])) inscrire.website = Request.Form["website"];
                if (!String.IsNullOrEmpty(Request.Form["facebook"])) inscrire.facebook = Request.Form["facebook"];
                if (!String.IsNullOrEmpty(Request.Form["linkedin"])) inscrire.linkedin = Request.Form["linkedin"];

                if (!String.IsNullOrEmpty(Request.Form["lat"])) inscrire.lat = Request.Form["lat"];
                if (!String.IsNullOrEmpty(Request.Form["longitude"])) inscrire.longitude = Request.Form["longitude"];
                if (!String.IsNullOrEmpty(Request.Form["titreEmploi"])) inscrire.titreEmploi = Request.Form["titreEmploi"];
                if (!String.IsNullOrEmpty(Request.Form["anneeExperience"])) inscrire.anneeExperience = Request.Form["anneeExperience"];
                if (!String.IsNullOrEmpty(Request.Form["salaireSouhaiter"])) inscrire.salaireSouhaiter = Decimal.Parse(Request.Form["salaireSouhaiter"]);
                if (!String.IsNullOrEmpty(Request.Form["profil"])) inscrire.profil =Request.Form["profil"];
                if (!String.IsNullOrEmpty(Request.Form["fcmToken"])) inscrire.fcmToken = Request.Form["fcmToken"];
                if (!String.IsNullOrEmpty(Request.Form["allowNotification"])) inscrire.allowNotification = Request.Form["allowNotification"]; //TODO mettre en int ou bool
                if (!String.IsNullOrEmpty(Request.Form["allowNewsletter"])) inscrire.allowNewsletter = Request.Form["allowNewsletter"]; //TODO mettre en int ou bool
                db.Entry(inscrire).State = EntityState.Modified;
                db.SaveChanges();
                Session.Add("inscrire", inscrire);
                return Json(new { result_code = 1, user = getJson(inscrire) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result_code = -2, message = "An error occured" }, JsonRequestBehavior.AllowGet);
            } 
        }


        [HttpPost]
        public JsonResult EditPassword(ChangePasswordViewModel model)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            if (ModelState.IsValid)
            {
                if (_me.password != model.oldPassword) return Json(new { result_code = -1, message = "Old password not valid" }, JsonRequestBehavior.AllowGet);
                 
                try
                {
                    Inscrire inscrire = db.Inscrire.Find(_me.id);

                    inscrire.password = model.password;
                    inscrire.cpassword = model.password;
                    db.Entry(inscrire).State = EntityState.Modified;
                    db.SaveChanges();
                    Session.Add("inscrire", inscrire);
                    return Json(new { result_code = 1, user = getJson(inscrire) }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { result_code = -2, message = "An error occured" }, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(new { result_code = 0, message = "Datas not valid" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ChangePhoto(HttpPostedFileBase file)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            try
            {
                Inscrire inscrire = db.Inscrire.Find(_me.id);
                if (file != null && file.ContentLength > 0) { 
                    
                    var filename = Path.GetFileName(file.FileName);

                    //verification de l'extention du fichiers
                    var supportedTypes = new[] { ".png", ".jpg" };
                    var extention = Path.GetExtension(filename);
                    if (!supportedTypes.Contains(extention)) return Json(new { result_code = -1, message = "Bad extensions" }, JsonRequestBehavior.AllowGet);

                    var formattedFilename = string.Format("{0}-{1}{2}"
                    , Path.GetFileNameWithoutExtension(filename)
                    , Guid.NewGuid().ToString("N")
                    , Path.GetExtension(filename));
                    string path = Path.Combine(Server.MapPath("~/Fichier"), Path.GetFileName(formattedFilename));
                    file.SaveAs(path);
                    inscrire.profil = formattedFilename; 
                    db.Entry(inscrire).State = EntityState.Modified;
                    db.SaveChanges();
                    Session.Add("inscrire", inscrire);
                    return Json(new { result_code = 1, user = getJson(inscrire) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { result_code = -2, message = "An error occured" }, JsonRequestBehavior.AllowGet);
            }
             
            return Json(-1);
        }

        [HttpPost]
        public async Task<JsonResult> ContactUsAsync()
        {
            //if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            try
            {
                String email = Request.Form["email"];
                String subject = Request.Form["subject"];
                String message = Request.Form["message"];
                if (String.IsNullOrEmpty(subject) && String.IsNullOrEmpty(message)) return Json(new { result_code = -1, message = "Datas not valid" }, JsonRequestBehavior.AllowGet);
                //TODO faire parvenir le message du client a l'admin
                MsMail mail = new MsMail();
                await mail.Send(mail.getEmail(), email+" "+subject, message);
                return Json(new { result_code = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result_code = -2, message = "An error occured" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ForgotPassword()
        {
            String email = Request.Form["email"]; 
            if(String.IsNullOrEmpty(email)) return Json(new { result_code = 0, message = "Datas not valid" }, JsonRequestBehavior.AllowGet);
            Inscrire user = null;
            try
            {
                user = db.Inscrire.Single(x => x.email == email && x.archived == 1 && x.etat == 1);
                //Inscrire user = db.Inscrire.Where(x => x.email == email && x.archived == 1 && x.etat == 1).FirstOrDefault();
                if (user == null) return Json(new { result_code = -1, message = "Account not found. Create an account !" }, JsonRequestBehavior.AllowGet);
                MsMail mail = new MsMail();
                await mail.Send(user.email, "Forgot password ?", "Mr "+ user.login +" Your password is "+ user.password);
                return Json(new { result_code = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result_code = -2, message = "Account not found. Create an account !" }, JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return Json(new { result_code = 1 }, JsonRequestBehavior.AllowGet);
        }
         
    }
}
