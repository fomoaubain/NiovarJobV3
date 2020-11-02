using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Utils
    {
        public static double getDayBetween2String(String begin, String end)
        {
            return getDayBetween2Date(DateTime.Parse(end), DateTime.Parse(begin));
        }
        public static double getDayBetween2Date(DateTime begin, DateTime end)
        {
            return (end - begin).TotalDays;
        }
        public static String dateFullMonth(DateTime d)
        {
            return d.Day + " " + d.ToString("MMMM") + " " + d.Year + " " + (d.Hour < 10 ? "0" : "") + d.Hour + ":" + (d.Minute < 10 ? "0" : "") + d.Minute;
        }
        public static String dateFullMonth(String dStr)
        {
            DateTime d = DateTime.Parse(dStr);
            return d.Day + " " + d.ToString("MMMM") + " " + d.Year + " " + (d.Hour < 10 ? "0" : "") + d.Hour + ":" + (d.Minute < 10 ? "0" : "") + d.Minute;
        }

        public static int AnneeExpAmount(NiovarJobEntities db, String categorie)
        {
            if (String.IsNullOrEmpty(categorie)) return 0;
            try
            {
                decimal id = Convert.ToDecimal(categorie);
                var data = db.CatAnneeExp.FirstOrDefault(p => p.Cat_id == id);
                if (data != null) return data.prixHoraire.HasValue ? data.prixHoraire.Value : 0;
            }
            catch(Exception e)
            {
               
                return 0;
            }
            return 0;
        }
        public static Inscrire getCompany(NiovarJobEntities db, int? companyId)
        {
            if (!companyId.HasValue) return null;
            try
            {
                var id = Convert.ToDecimal(companyId.Value);
                var data = db.Inscrire.FirstOrDefault(p => p.id == id);
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static Inscrire getCompany(NiovarJobEntities db, int companyId)
        {
            if (companyId<=0) return null;
            try
            {
                var id = Convert.ToDecimal(companyId);
                var data = db.Inscrire.FirstOrDefault(p => p.id == id);
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}