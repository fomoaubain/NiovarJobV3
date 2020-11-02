using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{

    public class Country
    {
        public decimal id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string iso3 { get; set; }
        public string iso2 { get; set; }
        public string phone_code { get; set; }
        public string capital { get; set; }
        public string currency { get; set; }
        public decimal country_id { get; set; }

        public virtual List<State> states { get; set; }
    }
    public class State
    {
        public decimal id { get; set; }
        public string name { get; set; }
        public string state_code { get; set; }
        public string country_id { get; set; }
        public virtual List<City> cities { get; set; }
    }
    public class City
    {
        public decimal id { get; set; }
        public string name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }
}