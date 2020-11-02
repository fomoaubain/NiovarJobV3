using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class StripeViewModel
    {

        [Required]
        [StringLength(254)]
        [Display(Name = "Nom", Prompt = "Name")]
        [RegularExpression("", ErrorMessage = "Veillez entrez un nom")] 
        public string Name { get; set; }

        [Required]
        [Display(Name = "Email", Prompt = "Email")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                            ErrorMessage = "Veillez entrez un mail vailde")] 
        public string Email { get; set; } 

        [Required]
        [StringLength(254)]
        [Display(Name = "Numéro de compte", Prompt = "Card number")]
        [RegularExpression("", ErrorMessage = "Veillez entrez un Numéro de compte")]
        public string CardNber { get; set; }


        [Required]
        [StringLength(254)]
        [Display(Name = "Cvc", Prompt = "Cvc")]
        [RegularExpression("", ErrorMessage = "Veillez entrez un Cvc")]
        public string Cvc { get; set; }

        [Required]
        [StringLength(254)]
        [Display(Name = "Exp", Prompt = "Exp")]
        [RegularExpression("", ErrorMessage = "Veillez entrez la date Exp")]
        public string Exp { get; set; }
    }
}