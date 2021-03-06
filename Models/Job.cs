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
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class Job
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Job()
        {
            this.AvantageSociauxJob = new HashSet<AvantageSociauxJob>();
            this.DiplomeJob = new HashSet<DiplomeJob>();
            this.Postuler = new HashSet<Postuler>();
            this.AssocLangueNiveau = new HashSet<AssocLangueNiveau>();
        }

    

        [NotMapped]
        public string nomEntreprise { get; set; }
        [NotMapped]
        public string adresse { get; set; }
        [NotMapped]
        public string telephone { get; set; }
        [NotMapped]
        public string courriel { get; set; }

        public string[] workDay { get; set; }
        public string workDayStr { get; set; }
        public string[] listDip { get; set; }
        public string listDipStr { get; set; }

        public string[] listAvantage { get; set; }
        public string listAvantageStr { get; set; }

        public decimal id { get; set; }
        public decimal Niv_id { get; set; }
        public decimal Typ_id { get; set; }
        public decimal Lan_id { get; set; }
        public decimal Con_id { get; set; }
        public decimal Qua_id { get; set; }
        public decimal Sta_id { get; set; }
        public decimal Cat_id { get; set; }
        public string titre { get; set; }

        [AllowHtml]
        public string description { get; set; }
        public string dateEntre { get; set; }
        public string ville { get; set; }
        public Nullable<int> nbreEmploye { get; set; }
        public string heureTravail { get; set; }
        public string heureTravailJour { get; set; }
        public string jourTravail { get; set; }
        public Nullable<double> remuneration { get; set; }
        public Nullable<int> totalHeureTravail { get; set; }
        public string margeExperience { get; set; }
        public string typeJob { get; set; }
        public string etat { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> created { get; set; }
        public Nullable<int> archived { get; set; }
        public string niveauEtude { get; set; }

        [AllowHtml]
        public string responsabilite { get; set; }

        [AllowHtml]
        public string exigence { get; set; }
        public string autre { get; set; }
        public string pays { get; set; }
        public string immediat { get; set; }
        public string province { get; set; }
        public Nullable<double> remunerationN { get; set; }
        public string countryCode { get; set; }
        public string avantageSociaux { get; set; }
        public string equipeEmploi { get; set; }
        public string codePostal { get; set; }
        public string adressePostal { get; set; }
        public Nullable<int> masquerEmplacement { get; set; }
        public string quartTravail { get; set; }
        public string DateEntreValeur { get; set; }
        public Nullable<System.DateTime> dateDebut { get; set; }
        public Nullable<System.DateTime> dateFin { get; set; }

        public Nullable<int> niveauOrale { get; set; }
        public Nullable<int> niveauEcrire { get; set; }
        public Nullable<int> negociable { get; set; }
        public Nullable<double> salaireAnnuel { get; set; }
        public Nullable<System.DateTime> dateFinOffre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvantageSociauxJob> AvantageSociauxJob { get; set; }
        public virtual Categorie Categorie { get; set; }
        public virtual Contrat Contrat { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiplomeJob> DiplomeJob { get; set; }
        public virtual Langue Langue { get; set; }
        public virtual NiveauScolarite NiveauScolarite { get; set; }
        public virtual QuartTravail QuartTravail1 { get; set; }
        public virtual StatutEmploi StatutEmploi { get; set; }
        public virtual TypeOffre TypeOffre { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Postuler> Postuler { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssocLangueNiveau> AssocLangueNiveau { get; set; }
    }
}
