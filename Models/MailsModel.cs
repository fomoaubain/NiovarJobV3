using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MailsModel
    {
        public int idJob { get; set; }
        public string message { get; set; }
        public string fichier { get; set; }
        public string subject { get; set; }
        public string To { get; set; }
    }
}