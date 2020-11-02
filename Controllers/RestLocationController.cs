using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class RestLocationController : MybaseController
    {

        [HttpGet]
        public JsonResult GetAllCountries()
        {
            string json = System.IO.File.ReadAllText(Server.MapPath("~/Json/countries+states.json"));
            List<Country> list = JsonConvert.DeserializeObject<List<Country>>(json);
            return Json(new { result_code = 1, datas = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllStates()
        {
            string state_code = Request["state_code"];
            string json = System.IO.File.ReadAllText(Server.MapPath("~/Json/countries+states.json"));
            List<Country> list = JsonConvert.DeserializeObject<List<Country>>(json);
            if(String.IsNullOrEmpty(state_code)) return Json(new { result_code = 1, datas = list }, JsonRequestBehavior.AllowGet);
            foreach (var item in list)
            {
                if(item.iso2 == state_code) return Json(new { result_code = 1, datas = item.states }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result_code = -1, message = "An error occured" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllCities()
        {
            decimal id = !string.IsNullOrEmpty(Request["id"]) ? Convert.ToDecimal(Request["id"]) : -1;
             

            string json = System.IO.File.ReadAllText(Server.MapPath("~/Json/states+cities.json"));
            List<State> list = JsonConvert.DeserializeObject<List<State>>(json);
            if (id == -1)
            {
                var jsonResult = Json(new { result_code = 1, datas = list }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            foreach (var item in list)
            {
                if (item.id == id)
                {
                    var jsonResult = Json(new { result_code = 1, datas = item.cities }, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
            }
            return Json(new { id = id, count = list.Count, result_code = -1, message = "An error occured" }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public JsonResult ListAllPays()
        {
            string json = System.IO.File.ReadAllText(Server.MapPath("~/Json/countries.json"));
            List<Country> list = JsonConvert.DeserializeObject<List<Country>>(json);
            return Json(new { result_code = 1, datas = list.ToList() }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ListAllProvince()
        {
            string country_name = Request["name"];
            string json = System.IO.File.ReadAllText(Server.MapPath("~/Json/countries+states.json"));
            List<Country> list = JsonConvert.DeserializeObject<List<Country>>(json);
            foreach (var item in list)
            {
                if (item.name == country_name) return Json(new { result_code = 1, datas = item.states.ToList() }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result_code = -1, message = "An error occured" }, JsonRequestBehavior.AllowGet);
        }

    }
}
