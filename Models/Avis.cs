//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Avis
    {

        [NotMapped]
        public string Pseudo { get; set; }

        [NotMapped]
        public string Profil { get; set; }
        public decimal id { get; set; }
        public decimal Pag_id { get; set; }
        public string libelle { get; set; }
        public Nullable<int> iduser { get; set; }
        public Nullable<int> archived { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> created { get; set; }
        public Nullable<int> nbreEtoile { get; set; }

        public virtual Page Page { get; set; }
    }
}