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
    public class RestCVController : MybaseController
    {

        [HttpGet]
        public JsonResult CvResume()
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);

            decimal id = !String.IsNullOrEmpty(Request["id"]) ? Request["id"].AsInt() : _me.id;
            Inscrire inscrire = db.Inscrire.Find(id);

            Information information = db.Information.FirstOrDefault(p => p.Ins_id == id);
            List<Education> education = db.Education.Where(p => p.Ins_id == id).ToList();
            List<Experience> experiences = db.Experience.Where(p => p.Ins_id == id).ToList();
            List<Autre> autres = db.Autre.Where(p => p.Ins_id == id).ToList();

            List<Dictionary<string, Object>> datasEducation = new List<Dictionary<string, Object>>();
            List<Dictionary<string, Object>> datasExperiences = new List<Dictionary<string, Object>>();
            List<Dictionary<string, Object>> datasAutre = new List<Dictionary<string, Object>>();
            foreach (var item in education) datasEducation.Add(getJson(item));
            foreach (var item in experiences) datasExperiences.Add(getJson(item));
            foreach (var item in autres) datasAutre.Add(getJson(item));

            return Json(new { result_code = 1, inscrire = getJson(inscrire), information = getJson(information), education = datasEducation, experiences = datasExperiences, autres = datasAutre }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult SeeCvResumePostulant(decimal id)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
  
            Information information = db.Information.FirstOrDefault(p => p.Ins_id == id);
            Inscrire inscrire = db.Inscrire.Find(id); 
            if (information == null) information = new Information();
            List<Education> education = db.Education.Where(p => p.Ins_id == id).ToList();
            List<Experience> experiences = db.Experience.Where(p => p.Ins_id == id).ToList();
            List<Autre> autres = db.Autre.Where(p => p.Ins_id == id).ToList();

            List<Dictionary<string, Object>> datasEducation = new List<Dictionary<string, Object>>();
            List<Dictionary<string, Object>> datasExperiences = new List<Dictionary<string, Object>>();
            List<Dictionary<string, Object>> datasAutre = new List<Dictionary<string, Object>>();
            foreach (var item in education) datasEducation.Add(getJson(item));
            foreach (var item in experiences) datasExperiences.Add(getJson(item));
            foreach (var item in autres) datasAutre.Add(getJson(item));

            return Json(new { result_code = 1, inscrire = getJson(inscrire), information = getJson(information), education = datasEducation, experiences = datasExperiences, autres = datasAutre }, JsonRequestBehavior.AllowGet);
        }
 
        [HttpGet]
        public JsonResult DocumentPostulant(decimal id)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);

            List<Models.File> files = db.File.Where(p => p.Ins_id == id).ToList();
            var listDiplome = db.File.Where(p => p.type.Equals("diplome") && p.Ins_id == id).ToList();
            var listCv = db.File.Where(p => p.type.Equals("cv") && p.Ins_id == id).ToList();
            var listAutre = db.File.Where(p => p.type.Equals("autre") && p.Ins_id == id).ToList();

            List<Dictionary<string, Object>> dataDiplome = new List<Dictionary<string, Object>>();
            List<Dictionary<string, Object>> dataCv = new List<Dictionary<string, Object>>();
            List<Dictionary<string, Object>> dataAutre = new List<Dictionary<string, Object>>();
            foreach (var item in listDiplome) dataDiplome.Add(getJson(item));
            foreach (var item in listCv) dataCv.Add(getJson(item));
            foreach (var item in listAutre) dataAutre.Add(getJson(item));
            return Json(new { result_code = 1, diplome = dataDiplome, cv = dataCv, autre = dataAutre }, JsonRequestBehavior.AllowGet);

        } 

        [HttpPost]
        public JsonResult SaveInformation(Information information)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
             
            try
            {
                Information currentexist = db.Information.FirstOrDefault(p => p.Ins_id == _me.id);

                if (currentexist == null)
                {
                    information.archived = 1;
                    information.status = 1;
                    information.Ins_id = _me.id;
                    db.Information.Add(information);
                    db.SaveChanges();
                    return Json(new { result_code = 1, data = getJson(information) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    currentexist.competence = information.competence;
                    currentexist.lettre = information.lettre;

                    db.Entry(currentexist).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { result_code = 1, data = getJson(currentexist) }, JsonRequestBehavior.AllowGet);
                }



            }
            catch (Exception ex)
            {
                return Json(new { result_code = -2, message = "An error occured" }, JsonRequestBehavior.AllowGet);
            }
             
        }

        [HttpPost] 
        public JsonResult SaveEducation(Education education)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);

            try
            { 
                education.archived = 1;
                education.status = 1;
                education.Ins_id = _me.id;
                db.Education.Add(education);
                db.SaveChanges();
                return Json(new { result_code = 1, data = getJson(education) }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { result_code = -2, message = "An error occured" }, JsonRequestBehavior.AllowGet);
            }
             
        }
 
        [HttpPost]
        public JsonResult SaveExperience(Experience experience)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);

            try
            { 
                experience.archived = 1;
                experience.status = 1;
                experience.Ins_id = _me.id;
                db.Experience.Add(experience);
                db.SaveChanges();
                return Json(new { result_code = 1, data = getJson(experience) }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { result_code = -2, message = "An error occured" }, JsonRequestBehavior.AllowGet);
            } 
        }
 
        [HttpPost]
        public JsonResult SaveAutre(Autre autre)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);

            try
            {
                autre.archived = 1;
                autre.status = 1;
                autre.Ins_id = _me.id;
                db.Autre.Add(autre);
                db.SaveChanges();
                return Json(new { result_code = 1, data = getJson(autre) }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { result_code = -2, message = "An error occured" }, JsonRequestBehavior.AllowGet);
            }
        }
 
        [HttpGet]
        public JsonResult deleteEducation(decimal id)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
 
            Education education = db.Education.Find(id);
            if (education == null) return Json(new { result_code = -1, message = "item not valid" }, JsonRequestBehavior.AllowGet);
            db.Education.Remove(education);
            db.SaveChanges();
            return Json(new { result_code = 1 }, JsonRequestBehavior.AllowGet);
        }
 
        [HttpGet]
        public JsonResult deleteExperience(decimal id)
        {

            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
             
            Experience experience = db.Experience.Find(id);
            if (experience == null) return Json(new { result_code = -1, message = "item not valid" }, JsonRequestBehavior.AllowGet);

            db.Experience.Remove(experience);
            db.SaveChanges();
            return Json(new { result_code = 1 }, JsonRequestBehavior.AllowGet);
        }

 
        [HttpGet]
        public JsonResult deleteAutre(decimal id)
        {

            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            Autre autre = db.Autre.Find(id);
            if (autre == null) return Json(new { result_code = -1, message = "item not valid" }, JsonRequestBehavior.AllowGet);

            db.Autre.Remove(autre);
            db.SaveChanges();
            return Json(new { result_code = 1 }, JsonRequestBehavior.AllowGet);
        }


 
        [HttpPost]
        public JsonResult CvDocument(Models.File fileObject)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet); 

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        var filename = Path.GetFileName(file.FileName); 
                        var supportedTypes = new[] { ".png", ".jpg", ".jpeg", ".gif", ".txt", ".doc", ".docx", ".pdf", ".xls", ".xlsx" };
                        var extention = Path.GetExtension(filename);
                        if (!supportedTypes.Contains(extention)) return Json(new { result_code = -1, message = "Extension file not valid" }, JsonRequestBehavior.AllowGet);
                           
                        var formattedFilename = string.Format("{0}-{1}{2}"
                            , Path.GetFileNameWithoutExtension(filename)
                            , Guid.NewGuid().ToString("N")
                            , Path.GetExtension(filename));
                        string path = Path.Combine(Server.MapPath("~/Fichier"),
                                                   Path.GetFileName(formattedFilename));
                        file.SaveAs(path);
                        fileObject.chemin = formattedFilename;
                    }
                    catch (Exception ex)
                    {
                        return Json(new { result_code = -2, message = "An error occured" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            try
            {

                fileObject.archived = 1;
                fileObject.status = 1;
                fileObject.created = DateTime.Now;
                fileObject.Ins_id = _me.id;
                db.File.Add(fileObject);
                db.SaveChanges();
                return Json(new { result_code = 1, data = getJson(fileObject) }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { result_code = -2, message = "An error occured" }, JsonRequestBehavior.AllowGet);
            } 
        }
 
        [HttpGet]
        public JsonResult deleteCvDocument(decimal id)
        {
            if (_me == null) return Json(new { result_code = -1000, message = "Session expired" }, JsonRequestBehavior.AllowGet);

            Models.File file = db.File.Find(id);
            if (file == null) return Json(new { result_code = -1, message = "item not valid" }, JsonRequestBehavior.AllowGet);
             
            db.File.Remove(file);
            db.SaveChanges();
            return Json(new { result_code = 1 }, JsonRequestBehavior.AllowGet);
        }

    }
}
