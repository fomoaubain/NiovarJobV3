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
    
    public partial class Reponse
    {
        public decimal id { get; set; }
        public decimal Que_id { get; set; }
        public string libelle { get; set; }
        public Nullable<int> iduser { get; set; }
        public Nullable<int> archived { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> created { get; set; }
    
        public virtual Question Question { get; set; }
    }
}
