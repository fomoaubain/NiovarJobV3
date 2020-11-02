using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using WebApplication1.Models;
using X.PagedList;

namespace WebApplication1.Controllers
{
    public class RestJobController : MybaseController
    {

        [HttpGet]
        public JsonResult DeleteJob(decimal id)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            try
            {
                var job = db.Job.FirstOrDefault(p => p.id == id);
                if (job == null) return Json(new { result_code = -1, message = "Job not found" }, JsonRequestBehavior.AllowGet);

                job.archived = 0;
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { result_code = 1 }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { result_code = -2, message = "An error occured" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult DeletePostuler(decimal id)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            try
            {
                var item = db.Postuler.FirstOrDefault(p => p.id == id);
                if (item == null) return Json(new { result_code = -1, message = "Application not found" }, JsonRequestBehavior.AllowGet);
                item.archived = 0;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { result_code = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result_code = -2, message = "An error occured" }, JsonRequestBehavior.AllowGet);
            }
        }
 
        [HttpGet]
        public JsonResult postuler(decimal id, int companyId)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            if (_me.type.Equals("client") || _me.status == 0) return Json(new { result_code = -10, message = "Your are not Allowed" }, JsonRequestBehavior.AllowGet);
 
            Inscrire company = db.Inscrire.Find(companyId); 
            if(company == null) return Json(new { result_code = -1, message = "Company not found" }, JsonRequestBehavior.AllowGet);

            Job job = db.Job.FirstOrDefault(p => p.id == id);
            if (job == null) return Json(new { result_code = -1, message = "job not found" }, JsonRequestBehavior.AllowGet);

            Postuler result = db.Postuler.Where(p => p.Ins_id == _me.id && p.Job_id == id && p.Inscrire.type.Equals("candidat") && p.archived==1 && p.compagnyId == companyId).FirstOrDefault();
           
            if (result != null) return Json(new { result_code = -1, message = "You are already apply this job" }, JsonRequestBehavior.AllowGet);
  
            Postuler postuler = new Postuler();
            postuler.Job_id = id;
            postuler.Job = job;
            postuler.Ins_id = _me.id; 
            postuler.compagnyId = companyId;
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
            db.SaveChanges();
            return Json(new { result_code = 1, data = getJson(postuler) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult setReader(decimal id)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);

            Postuler postuler = db.Postuler.Where(p => p.id == id).FirstOrDefault();
            if (postuler == null) return Json(new { result_code = -1, message = "data not found" }, JsonRequestBehavior.AllowGet);

            postuler.etat = "1";
            ViewBag.immediat = postuler.Job.immediat;
            db.Entry(postuler).State = EntityState.Modified;
            db.SaveChanges(); 

            return Json(new { result_code = 1 }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ChangeStatusJob(decimal id)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            try
            {
                var job = db.Job.FirstOrDefault(p => p.id == id);
                if (job == null) return Json(new { result_code = -1, message = "Job not found" }, JsonRequestBehavior.AllowGet);

                job.status = job.status == null || job.status == 0 ? 1 : 0;
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { result_code = 1, status = job.status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result_code = -2, message = "An error occured" }, JsonRequestBehavior.AllowGet);
            }
        }

 
        //TODO pour un candidat
        [HttpGet]
        public JsonResult MesCandidatures()
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            String q = Request["q"];
            String location = Request["location"];
            int page = !string.IsNullOrEmpty(Request["page"]) ? Request["page"].AsInt() : 1;
            int limit = 10;

            var listPostuler = db.Postuler.AsQueryable();
            listPostuler = listPostuler.Where(p => p.Ins_id == _me.id && p.Job.archived == 1 && p.archived == 1 && p.Inscrire.type.Equals("candidat")).OrderByDescending(p => p.created);

            if (!string.IsNullOrEmpty(location)) listPostuler = listPostuler.Where(p => p.Job.pays.Contains(location) || p.Job.province.Contains(location) ||  p.Job.ville.Contains(location));
            if (!string.IsNullOrEmpty(q)) listPostuler = listPostuler.Where(p => p.libelle.Contains(q) || p.Job.titre.Contains(q) || p.outils.Contains(q) || p.Job.Categorie.libelle.Contains(q) || p.Job.Categorie.Types.libelle.Contains(q));
            var count = listPostuler.Count();
            
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in listPostuler.ToPagedList(page, limit)) datas.Add(getJson(item));
            return Json(new { result_code = 1, count = count, limit = limit, datas = datas }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListPostuler()
        {
            bool me = !string.IsNullOrEmpty(Request["me"]) &&  Request["me"].AsInt() == 1 ? true : false;
            if(me && _me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            String q = Request["q"];
            String location = Request["location"];
            Decimal cat = !string.IsNullOrEmpty(Request["cat"]) ? Convert.ToDecimal(Request["cat"]) : 0;
            //Decimal domaine = !string.IsNullOrEmpty(Request["domaine"]) ? Convert.ToDecimal(Request["domaine"]) : 0;
            String anneeExp = Request["anneeExp"];
            String heureTravail = Request["heureTravail"];
            String approbation = Request["approbation"];
            String datePostedRadio = Request["datePostedRadio"];
            String immediat = Request["immediat"];
            int page = !string.IsNullOrEmpty(Request["page"]) ? Request["page"].AsInt() : 1;
            int limit = 10;

            var listPostuler = db.Postuler.AsQueryable();
            listPostuler = listPostuler.Where(p => p.Job.archived == 1 && p.Inscrire.type == "client").OrderByDescending(p => p.Job.created);
            if (me) listPostuler = listPostuler.Where(p => p.Ins_id == _me.id);
            if (!string.IsNullOrEmpty(immediat)) listPostuler = listPostuler.Where(p => p.Job.immediat.Equals(immediat));
            if (!string.IsNullOrEmpty(location)) listPostuler = listPostuler.Where(p => p.Job.pays.Contains(location) || p.Job.province.Contains(location) ||  p.Job.ville.Contains(location));
            if (!string.IsNullOrEmpty(q)) listPostuler = listPostuler.Where(p => p.libelle.Contains(q) || p.Job.titre.Contains(q) || p.outils.Contains(q) || p.Job.Categorie.libelle.Contains(q) || p.Job.Categorie.Types.libelle.Contains(q));
            if (!string.IsNullOrEmpty(approbation)) listPostuler = listPostuler.Where(p => p.approbation.Equals(approbation));
            if (!string.IsNullOrEmpty(anneeExp)) listPostuler = listPostuler.Where(p => p.Job.margeExperience.Contains(anneeExp));
            if (!string.IsNullOrEmpty(heureTravail)) listPostuler = listPostuler.Where(p => p.Job.heureTravail.Equals(heureTravail));
            if (cat != 0) listPostuler = listPostuler.Where(p => p.Job.Cat_id == cat);
            var count = listPostuler.Count();

            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            Dictionary<string, Object> row = new Dictionary<string, Object>();
            foreach (var item in listPostuler.ToPagedList(page, limit))
            {
                item.nbreApply = db.Postuler.Where(p => p.Job_id == item.Job_id && p.archived == 1 && p.Inscrire.type.Equals("candidat")).ToList().Count;
                row = getJson(item);
                datas.Add(row);
            }

            return Json(new { result_code = 1, page = page, count = count, limit = limit, datas = datas }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ListPostulant(decimal id)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            
            int page = !string.IsNullOrEmpty(Request["page"]) ? Request["page"].AsInt() : 1;
            int limit = 10;
            Job job = db.Job.Where(p => p.id == id && p.archived == 1).FirstOrDefault();
            if (job == null) return Json(new { result_code = -1, message = "Job not found" }, JsonRequestBehavior.AllowGet);
   
            var listPostulant = db.Postuler.Where(p => p.Job_id == id && p.archived == 1 && p.Inscrire.type == "candidat");
            var count = listPostulant.Count();
            if (job.immediat.Equals("true")) listPostulant = listPostulant.Where(p => p.etatCandidat.Equals("2"));
            List <Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in listPostulant.OrderByDescending(p => p.created).ToPagedList(page, limit))  datas.Add(getJson(item)); 
            return Json(new { result_code = 1, page = page, count = count, limit = limit, datas = datas }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult TakeDecision(decimal idPostuler, String val)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);

            try
            {
                var currentPostuler = db.Postuler.FirstOrDefault(p => p.id == idPostuler);
                if (currentPostuler == null) return Json(new { result_code = -1, message = "item not found" }, JsonRequestBehavior.AllowGet);

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

                return Json(new { result_code = 1, data = getJson(currentPostuler) }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { result_code = -2, message = "An error occured" }, JsonRequestBehavior.AllowGet);

            }
        }
        

 
        public JsonResult confirmContratCandidat(decimal id)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);

            Postuler postuler = db.Postuler.FirstOrDefault(p => p.id == id);
            if (postuler == null) return Json(new { result_code = -1, message = "item not found" }, JsonRequestBehavior.AllowGet);

            postuler.signatures = "2";
            postuler.situationTravail = "1";
            postuler.dateSignatures = DateTime.Now;
            db.Entry(postuler).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { result_code = 1, data = getJson(postuler) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult addEntrevue(decimal idPostuler, String dateEntrevue, String heure, String duree, String details, String outils)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            try
            {
                var currentPostuler = db.Postuler.FirstOrDefault(p => p.id == idPostuler);
                if (currentPostuler == null) return Json(new { result_code = -1, message = "item not found" }, JsonRequestBehavior.AllowGet);

                currentPostuler.dateEntrevue = Convert.ToDateTime(dateEntrevue);
                currentPostuler.heure = heure;
                currentPostuler.duree = duree;
                currentPostuler.responsableEntrevue = details;
                currentPostuler.outils = outils;
                currentPostuler.etatCandidat = "4";

                db.Entry(currentPostuler).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { result_code = 1, data = getJson(currentPostuler) }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { result_code = -2, message = "An error occured" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult GetAllCatAnneeExp(decimal id)
        {
            List<CatAnneeExp> catAnneeExps = db.CatAnneeExp.Where(p => p.Cat_id == id && p.archived == 1 && p.status == 1).ToList();
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in catAnneeExps) datas.Add(getJson(item));
            return Json(new { result_code = 1, datas = datas }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAllAnneeExp()
        {
            //if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);

            List<AnneeExp> anneeExps = db.AnneeExp.Where(p => p.archived == 1 && p.status == 1).ToList();
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in anneeExps) datas.Add(getJson(item));
            return Json(new { result_code = 1, datas = datas }, JsonRequestBehavior.AllowGet);

        }
        

        //TODO on devrait voir comment recuperer les plus utilisépour l'accueil
        [HttpGet]
        public JsonResult GetCategories()
        {
            //if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);

            int page = !string.IsNullOrEmpty(Request["page"]) ? Request["page"].AsInt() : 1;
            int limit = 15;
            var categories = db.Categorie.Where(p => p.archived == 1 && p.status == 1).ToList();
            var count = categories.Count();
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in categories.ToPagedList(page, limit)) datas.Add(getJson(item));
            return Json(new { result_code = 1, page = page, count = count, limit = limit, datas = datas }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAllTitreEmploi(decimal id)
        {

            List<Categorie> categories = db.Categorie.Where(p => p.Typ_id == id && p.archived==1).ToList();
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in categories.ToList()) datas.Add(getJson(item)); 
            return Json(new { result_code = 1, datas = datas }, JsonRequestBehavior.AllowGet); 
        }

        [HttpGet]
        public JsonResult GetTypes()
        {
            //if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);

            int page = !string.IsNullOrEmpty(Request["page"]) ? Request["page"].AsInt() : 1;
            int limit = 15;
            var types = db.Types.Where(p => p.archived == 1 && p.status == 1).ToList();
            var count = types.Count();
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in types.ToPagedList(page, limit)) datas.Add(getJson(item, true));
            return Json(new { result_code = 1, page = page, count = count, limit = limit, datas = datas }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetAllTypes()
        {
            var types = db.Types.Where(p => p.archived == 1 && p.status == 1).ToList();
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in types) datas.Add(getJson(item));
            return Json(new { result_code = 1, datas = datas }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAllNiveauScolarite()
        {
            var list = db.NiveauScolarite.Where(p => p.archived == 1 && p.status == 1).ToList();
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in list) datas.Add(getJson(item));
            return Json(new { result_code = 1, datas = datas }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAllDiplome()
        {
            var list = db.Diplome.Where(p => p.archived == 1 && p.status == 1).ToList();
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in list) datas.Add(getJson(item));
            return Json(new { result_code = 1, datas = datas }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAllStatutEmploi()
        {
            var list = db.StatutEmploi.Where(p => p.archived == 1 && p.status == 1).ToList();
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in list) datas.Add(getJson(item));
            return Json(new { result_code = 1, datas = datas }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAllLangue()
        {
            var list = db.Langue.Where(p => p.archived == 1 && p.status == 1).ToList();
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in list) datas.Add(getJson(item));
            return Json(new { result_code = 1, datas = datas }, JsonRequestBehavior.AllowGet);

        }
        
        [HttpGet]
        public JsonResult GetAllNiveauLangue()
        {
            var list = db.NiveauLangue.Where(p => p.archived == 1 && p.status == 1).ToList();
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in list) datas.Add(getJson(item));
            return Json(new { result_code = 1, datas = datas }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAllTypeOffre()
        {
            var list = db.TypeOffre.Where(p => p.archived == 1 && p.status == 1).ToList();
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in list) datas.Add(getJson(item));
            return Json(new { result_code = 1, datas = datas }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetAllContrat()
        {
            var list = db.Contrat.Where(p => p.archived == 1 && p.status == 1).ToList();
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in list) datas.Add(getJson(item));
            return Json(new { result_code = 1, datas = datas }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetAllQuartTravail()
        {
            var list = db.QuartTravail.Where(p => p.archived == 1 && p.status == 1).ToList();
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in list) datas.Add(getJson(item));
            return Json(new { result_code = 1, datas = datas }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetAllAvantageSociaux()
        {
            var list = db.AvantageSociaux.Where(p => p.archived == 1 && p.status == 1).ToList();
            List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
            foreach (var item in list) datas.Add(getJson(item));
            return Json(new { result_code = 1, datas = datas }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult GetCountJobByCategory(decimal id)
        { 
            var count = db.Postuler.Where(p => p.Job.archived == 1 && p.Inscrire.type == "client" && p.Job.Categorie.Typ_id == id && p.status == 1 && (p.compagnyId == null || p.compagnyId<=0)).Count(); 
            return Json(new { result_code = 1, count = count }, JsonRequestBehavior.AllowGet);
        }
 
        [HttpPost]
        public JsonResult PostOffre(Job job)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);

            if (job.immediat.Equals("false") && needAbonnement()) return Json(new { result_code = -10, message = "You need abonnement" }, JsonRequestBehavior.AllowGet);
            try
            {
                string daysWork = "";
                for (int i = 0; i < job.workDay.Length; i++) daysWork += job.workDay[i].ToString() + ",";
                daysWork = daysWork.TrimEnd(',');
                job.jourTravail = daysWork;
                job.status = 1;
                job.archived = 1;
                job.etat = "1";
                job.created = DateTime.Now;
                db.Job.Add(job);
                db.SaveChanges();

                var dipArray = job.listDipStr.Split(',');
                for (int i = 0; i < dipArray.Length; i++)
                {
                    DiplomeJob diplomeJob = new DiplomeJob();
                    diplomeJob.archived = 1;
                    diplomeJob.created = DateTime.Now;
                    diplomeJob.Job_id = job.id;
                    var dipId = Decimal.Parse(dipArray[i]);
                    diplomeJob.Dip_id = dipId;
                    db.DiplomeJob.Add(diplomeJob);
                }

                var avanArray = job.listAvantageStr.Split(','); 
                for (int i = 0; i < avanArray.Length; i++)
                {
                    AvantageSociauxJob avantageSociauxJob = new AvantageSociauxJob();
                    avantageSociauxJob.archived = 1;
                    avantageSociauxJob.created = DateTime.Now;
                    avantageSociauxJob.Job_id = job.id;
                    avantageSociauxJob.Ava_id = Decimal.Parse(avanArray[i]);
                    db.AvantageSociauxJob.Add(avantageSociauxJob);
                } 
                db.SaveChanges();

                Postuler postuler = new Postuler();
                postuler.Job_id = job.id;
                postuler.Ins_id = _me.id;
                postuler.created = DateTime.Now;
                postuler.etatAdmin = "0";
                postuler.etatClient = "0";
                postuler.etatCandidat = "0";
                postuler.etat = "0";
                postuler.signatures = "0";
                postuler.signatureClient = 0;
                postuler.approbation = "0"; 
                postuler.situationTravail = "0";

                if (job.immediat.Equals("true")) postuler.approbation = "0";
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
                        return Json(new { result_code = -1, message = "Abonnement not found" }, JsonRequestBehavior.AllowGet);
                    } 
                } 
                postuler.situationTravail = "0";
                postuler.archived = 1; 
                db.Postuler.Add(postuler);
                db.SaveChanges();
                return Json(new { result_code = 1, data = getJson(postuler) }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                throw;
                return Json(new { result_code = -2, message = "An error occured" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EditOffre(Job job)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            if (job.id <= 0) return Json(new { result_code = -1, message = "Data not found" }, JsonRequestBehavior.AllowGet);
            
            try
            {
                //Job item = db.Job.Find(job.id);

                var currentJob = db.Job.FirstOrDefault(p => p.id == job.id);
                currentJob.status = job.status;
                currentJob.archived = job.archived;
                currentJob.etat = job.etat;

             

                string daysWork = "";
                for (int i = 0; i < job.workDay.Length; i++) daysWork += job.workDay[i].ToString() + ",";
                daysWork = daysWork.TrimEnd(',');
                currentJob.jourTravail = daysWork;

                db.Entry(currentJob).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { result_code = 1, data = getJson(currentJob) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result_code = -2, message = "An error occured" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult GetAllNotification()
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            List<Postuler> listResult = new List<Postuler>();
   
            if (_me.type.Equals("client"))
            {
                List<Postuler> listPostuler = db.Postuler.Where(p => p.Ins_id == _me.id && p.Job.archived == 1 && p.archived == 1 && p.Job.status == 1 && p.Inscrire.type.Equals("client")).OrderByDescending(p => p.Job.created).ToList();
                var count = listPostuler.Count;

                List<Dictionary<string, Object>> datas = new List<Dictionary<string, Object>>();
                Dictionary<string, Object> row = new Dictionary<string, Object>();
                foreach (var item in listPostuler)
                {
                    item.nbreApply = db.Postuler.Where(p => p.Job_id == item.Job_id && p.archived == 1 && p.etat.Equals("0") && p.Inscrire.type.Equals("candidat")).ToList().Count;
                    row = getJson(item);
                    datas.Add(row);
                }
                return Json(new { result_code = 1, count = count, datas = datas }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result_code = -1, message = "datas not found"}, JsonRequestBehavior.AllowGet);
        }

         
    }
}
