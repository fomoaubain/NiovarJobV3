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
    
    public partial class CatAnneeExp
    {
        public decimal Cat_id { get; set; }
        public decimal Ann_id { get; set; }
        public decimal id { get; set; }
        public Nullable<int> prixHoraire { get; set; }
        public Nullable<int> archived { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> created { get; set; }
    
        public virtual AnneeExp AnneeExp { get; set; }
        public virtual Categorie Categorie { get; set; }
    }
}
