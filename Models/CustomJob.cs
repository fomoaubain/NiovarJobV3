using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CustomJob
    {
        public decimal id { get; set; }
        public decimal Typ_id { get; set; }
        public decimal Cat_id { get; set; }
        public string titre { get; set; }
        public string description { get; set; }
        public string dateEntre { get; set; }
        public string ville { get; set; }
        public Nullable<int> nbreEmploye { get; set; }
        public string heureTravail { get; set; }
        public string heureTravailJour { get; set; }
        public string jourTravail { get; set; }
        public string profil { get; set; }
        public string nom { get; set; }
        public Nullable<double> remuneration { get; set; }
        public Nullable<int> totalHeureTravail { get; set; }
        public string margeExperience { get; set; }
        public string typeJob { get; set; }

        public string immediat { get; set; }
        public string remuneration_n { get; set; }
        public string negociable { get; set; }
        public string salaireAnnuel { get; set; }
    }

    public class ChangePasswordViewModel
    {

        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Ancien mot de passe", Prompt = "Ancien mot de passe")]
        public string oldPassword { get; set; }

        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "nouveau mot de passe", Prompt = "Ancien mot de passe")]
        public string password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe", Prompt = "Confirmer le mot de passe")]
        [Compare("password", ErrorMessage = "Le mot de passe et le mot de passe de confirmation ne correspondent pas.")]
        public string confirmPassword { get; set; }

    }
    public class StateObject
    {
        public decimal id { get; set; }
        public string name { get; set; }
        public string state_code { get; set; }
        public string country_id { get; set; }
    }
    public class CityObject
    {
        public decimal id { get; set; }
        public string name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }
}