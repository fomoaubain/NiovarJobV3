﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class NiovarJobEntities : DbContext
    {
        public NiovarJobEntities()
            : base("name=NiovarJobEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Abonnement> Abonnement { get; set; }
        public virtual DbSet<AnneeExp> AnneeExp { get; set; }
        public virtual DbSet<AssocLangueNiveau> AssocLangueNiveau { get; set; }
        public virtual DbSet<Autre> Autre { get; set; }
        public virtual DbSet<AvantageSociaux> AvantageSociaux { get; set; }
        public virtual DbSet<AvantageSociauxJob> AvantageSociauxJob { get; set; }
        public virtual DbSet<Avis> Avis { get; set; }
        public virtual DbSet<CatAnneeExp> CatAnneeExp { get; set; }
        public virtual DbSet<Categorie> Categorie { get; set; }
        public virtual DbSet<Contrat> Contrat { get; set; }
        public virtual DbSet<Diplome> Diplome { get; set; }
        public virtual DbSet<DiplomeJob> DiplomeJob { get; set; }
        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<Education> Education { get; set; }
        public virtual DbSet<Experience> Experience { get; set; }
        public virtual DbSet<File> File { get; set; }
        public virtual DbSet<FraisLocation> FraisLocation { get; set; }
        public virtual DbSet<Galerie> Galerie { get; set; }
        public virtual DbSet<Information> Information { get; set; }
        public virtual DbSet<InsAbonne> InsAbonne { get; set; }
        public virtual DbSet<Inscrire> Inscrire { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<Langue> Langue { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<NiveauLangue> NiveauLangue { get; set; }
        public virtual DbSet<NiveauScolarite> NiveauScolarite { get; set; }
        public virtual DbSet<Page> Page { get; set; }
        public virtual DbSet<Paiement> Paiement { get; set; }
        public virtual DbSet<Parametre> Parametre { get; set; }
        public virtual DbSet<Postuler> Postuler { get; set; }
        public virtual DbSet<QuartTravail> QuartTravail { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<Reponse> Reponse { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<StatutEmploi> StatutEmploi { get; set; }
        public virtual DbSet<Suivre> Suivre { get; set; }
        public virtual DbSet<TypeOffre> TypeOffre { get; set; }
        public virtual DbSet<Types> Types { get; set; }
        public virtual DbSet<Utilisateur> Utilisateur { get; set; }
        public virtual DbSet<Bibliotheque> Bibliotheque { get; set; }
    }
}
