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
    
    public partial class AvantageSociauxJob
    {
        public decimal Job_id { get; set; }
        public decimal Ava_id { get; set; }
        public decimal id { get; set; }
        public Nullable<int> archived { get; set; }
        public Nullable<System.DateTime> created { get; set; }
    
        public virtual AvantageSociaux AvantageSociaux { get; set; }
        public virtual Job Job { get; set; }
    }
}
