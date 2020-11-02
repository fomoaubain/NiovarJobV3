using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CvListObject
    {
        public Information information { get; set; }
        public Education education { get; set; }

        public Experience experience { get; set; }

        public Autre autre { get; set; }

        public Inscrire inscrire { get; set; }

    }
}