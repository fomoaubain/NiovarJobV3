
CREATE TABLE [dbo].[Abonnement](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[type] [varchar](254) NULL,
	[titre] [varchar](254) NULL,
	[description] [varchar](254) NULL,
	[montant] [float] NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
	[created] [datetime] NULL,
	[nbrePost] [int] NULL,
	[illimite] [int] NULL,
 CONSTRAINT [PK_ABONNEMENT] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[AnneeExp](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[libelle] [varchar](254) NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
	[created] [datetime] NULL,
 CONSTRAINT [PK_ANNEEEXP] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Autre](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Ins_id] [numeric](18, 0) NOT NULL,
	[etablissement] [varchar](254) NULL,
	[fonction] [varchar](254) NULL,
	[periode] [varchar](254) NULL,
	[description] [text] NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_AUTRE] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

CREATE TABLE [dbo].[CatAnneeExp](
	[Cat_id] [numeric](18, 0) NOT NULL,
	[Ann_id] [numeric](18, 0) NOT NULL,
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[prixHoraire] [int] NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
	[created] [datetime] NULL,
 CONSTRAINT [PK_CATANNEEEXP] PRIMARY KEY NONCLUSTERED 
(
	[Cat_id] ASC,
	[Ann_id] ASC,
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Categorie](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Typ_id] [numeric](18, 0) NOT NULL,
	[libelle] [varchar](254) NULL,
	[image] [varchar](254) NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_CATEGORIE] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Document](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Ins_id] [numeric](18, 0) NOT NULL,
	[Job_id] [numeric](18, 0) NOT NULL,
	[Pos_id] [numeric](18, 0) NOT NULL,
	[type] [varchar](254) NULL,
	[libelle] [varchar](254) NULL,
	[chemin] [varchar](254) NULL,
	[etat] [int] NULL,
	[status] [int] NULL,
	[archived] [int] NULL,
	[created] [datetime] NULL,
 CONSTRAINT [PK_DOCUMENT] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Education](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Ins_id] [numeric](18, 0) NOT NULL,
	[etablissement] [varchar](254) NULL,
	[diplome] [varchar](254) NULL,
	[periode] [varchar](254) NULL,
	[description] [text] NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_EDUCATION] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

CREATE TABLE [dbo].[Experience](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Ins_id] [numeric](18, 0) NOT NULL,
	[etablissement] [varchar](254) NULL,
	[fonction] [varchar](254) NULL,
	[periode] [varchar](254) NULL,
	[description] [text] NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_EXPERIENCE] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

CREATE TABLE [dbo].[File](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Ins_id] [numeric](18, 0) NOT NULL,
	[type] [varchar](254) NULL,
	[libelle] [varchar](254) NULL,
	[chemin] [varchar](254) NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
	[created] [datetime] NULL,
 CONSTRAINT [PK_FILE] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Information](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Ins_id] [numeric](18, 0) NOT NULL,
	[lettre] [varchar](254) NULL,
	[competence] [varchar](254) NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_INFORMATION] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[InsAbonne](
	[Ins_id] [numeric](18, 0) NOT NULL,
	[Abo_id] [numeric](18, 0) NOT NULL,
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[libelle] [varchar](254) NULL,
	[dateDebut] [varchar](254) NULL,
	[dateFin] [varchar](254) NULL,
	[etat] [int] NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
	[created] [datetime] NULL,
	[nbrePost] [int] NULL,
 CONSTRAINT [PK_INSABONNE] PRIMARY KEY NONCLUSTERED 
(
	[Ins_id] ASC,
	[Abo_id] ASC,
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Inscrire](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[type] [varchar](254) NULL,
	[nom] [varchar](254) NULL,
	[login] [varchar](254) NULL,
	[email] [varchar](254) NULL,
	[password] [varchar](254) NULL,
	[phone] [varchar](254) NULL,
	[website] [varchar](254) NULL,
	[description] [text] NULL,
	[facebook] [varchar](254) NULL,
	[linkedin] [varchar](254) NULL,
	[pays] [varchar](254) NULL,
	[ville] [varchar](254) NULL,
	[adresse] [varchar](254) NULL,
	[longitude] [varchar](254) NULL,
	[lat] [varchar](254) NULL,
	[titreEmploi] [text] NULL,
	[anneeExperience] [varchar](254) NULL,
	[salaireSouhaiter] [numeric](18, 0) NULL,
	[salaireNegocier] [numeric](18, 0) NULL,
	[profil] [varchar](254) NULL,
	[etat] [int] NULL,
	[status] [int] NULL,
	[archived] [int] NULL,
	[created] [datetime] NULL,
	[cpassword] [varchar](254) NULL,
	[emailProf] [varchar](254) NULL,
	[categorie] [varchar](254) NULL,
	[domaine] [varchar](254) NULL,
	[codePostal] [varchar](254) NULL,
	[nomRepresentant] [varchar](254) NULL,
	[prenomRepresentant] [varchar](254) NULL,
	[numeroPoste] [varchar](254) NULL,
	[province] [varchar](254) NULL,
 CONSTRAINT [PK_INSCRIRE] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

CREATE TABLE [dbo].[Job](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Cat_id] [numeric](18, 0) NOT NULL,
	[titre] [varchar](254) NULL,
	[description] [text] NULL,
	[dateEntre] [varchar](254) NULL,
	[ville] [varchar](254) NULL,
	[nbreEmploye] [int] NULL,
	[heureTravail] [varchar](254) NULL,
	[heureTravailJour] [varchar](254) NULL,
	[jourTravail] [varchar](254) NULL,
	[remuneration] [float] NULL,
	[totalHeureTravail] [int] NULL,
	[margeExperience] [varchar](254) NULL,
	[typeJob] [varchar](254) NULL,
	[etat] [varchar](254) NULL,
	[status] [int] NULL,
	[created] [datetime] NULL,
	[archived] [int] NULL,
	[niveauEtude] [varchar](254) NULL,
	[responsabilite] [text] NULL,
	[exigence] [text] NULL,
	[autre] [varchar](254) NULL,
	[pays] [varchar](254) NULL,
	[immediat] [varchar](254) NULL,
	[province] [varchar](254) NULL,
	[remunerationN] [float] NULL,
 CONSTRAINT [PK_JOB] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

CREATE TABLE [dbo].[Location](
	[Ins_id] [numeric](18, 0) NOT NULL,
	[Ins_id2] [numeric](18, 0) NOT NULL,
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[periode] [varchar](254) NULL,
	[heureTravail] [varchar](254) NULL,
	[description] [varchar](254) NULL,
	[montant] [numeric](18, 0) NULL,
	[signClient] [int] NULL,
	[signCandidat] [int] NULL,
	[remuneration] [numeric](18, 0) NULL,
	[dateSgnClt] [datetime] NULL,
	[dateSgnCdt] [datetime] NULL,
	[avisClient] [int] NULL,
	[avisCandidat] [int] NULL,
	[status] [int] NULL,
	[etat] [int] NULL,
	[created] [datetime] NULL,
	[archived] [int] NULL,
 CONSTRAINT [PK_LOCATION] PRIMARY KEY NONCLUSTERED 
(
	[Ins_id] ASC,
	[Ins_id2] ASC,
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Paiement](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Pos_Ins_id] [numeric](18, 0) NOT NULL,
	[Job_id] [numeric](18, 0) NOT NULL,
	[Pos_id] [numeric](18, 0) NOT NULL,
	[Ins_id] [numeric](18, 0) NOT NULL,
	[Ins_id2] [numeric](18, 0) NOT NULL,
	[Loc_id] [numeric](18, 0) NOT NULL,
	[libelle] [varchar](254) NULL,
	[montant] [float] NULL,
	[avance] [float] NULL,
	[reste] [float] NULL,
	[type] [varchar](254) NULL,
	[etat] [int] NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
	[created] [datetime] NULL,
	[userId] [int] NULL,
 CONSTRAINT [PK_PAIEMENT] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Postuler](
	[Ins_id] [numeric](18, 0) NOT NULL,
	[Job_id] [numeric](18, 0) NOT NULL,
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[libelle] [varchar](254) NULL,
	[datePostule] [datetime] NULL,
	[heurePostule] [varchar](254) NULL,
	[remuneration] [numeric](18, 0) NULL,
	[etatAdmin] [varchar](254) NULL,
	[etat] [varchar](254) NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
	[created] [datetime] NULL,
	[etatClient] [varchar](254) NULL,
	[etatCandidat] [varchar](254) NULL,
	[approbation] [varchar](254) NULL,
	[signatures] [varchar](254) NULL,
	[situationTravail] [varchar](254) NULL,
	[dateEmbauche] [datetime] NULL,
	[dateSignatures] [datetime] NULL,
	[dateEntrevue] [datetime] NULL,
	[heure] [varchar](254) NULL,
	[responsableEntrevue] [varchar](254) NULL,
	[duree] [varchar](254) NULL,
	[outils] [varchar](254) NULL,
	[signatureClient] [int] NULL,
	[dateSignClient] [datetime] NULL,
 CONSTRAINT [PK_POSTULER] PRIMARY KEY NONCLUSTERED 
(
	[Ins_id] ASC,
	[Job_id] ASC,
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Role](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[libelle] [varchar](254) NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
	[created] [datetime] NULL,
 CONSTRAINT [PK_ROLE] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Types](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[libelle] [varchar](254) NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
	[created] [datetime] NULL,
 CONSTRAINT [PK_TYPES] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Utilisateur](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Rol_id] [numeric](18, 0) NOT NULL,
	[nom] [varchar](254) NULL,
	[email] [varchar](254) NULL,
	[password] [varchar](254) NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
	[created] [datetime] NULL,
 CONSTRAINT [PK_UTILISATEUR] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[Autre]  WITH CHECK ADD  CONSTRAINT [FK_AUTRE_ASSOCIATI_INSCRIRE] FOREIGN KEY([Ins_id])
REFERENCES [dbo].[Inscrire] ([id])

ALTER TABLE [dbo].[Autre] CHECK CONSTRAINT [FK_AUTRE_ASSOCIATI_INSCRIRE]

ALTER TABLE [dbo].[CatAnneeExp]  WITH CHECK ADD  CONSTRAINT [FK_CATANNEE_ASSOCIATI_ANNEEEXP] FOREIGN KEY([Ann_id])
REFERENCES [dbo].[AnneeExp] ([id])

ALTER TABLE [dbo].[CatAnneeExp] CHECK CONSTRAINT [FK_CATANNEE_ASSOCIATI_ANNEEEXP]

ALTER TABLE [dbo].[CatAnneeExp]  WITH CHECK ADD  CONSTRAINT [FK_CATANNEE_ASSOCIATI_CATEGORI] FOREIGN KEY([Cat_id])
REFERENCES [dbo].[Categorie] ([id])

ALTER TABLE [dbo].[CatAnneeExp] CHECK CONSTRAINT [FK_CATANNEE_ASSOCIATI_CATEGORI]

ALTER TABLE [dbo].[Categorie]  WITH CHECK ADD  CONSTRAINT [FK_CATEGORI_ASSOCIATI_TYPES] FOREIGN KEY([Typ_id])
REFERENCES [dbo].[Types] ([id])

ALTER TABLE [dbo].[Categorie] CHECK CONSTRAINT [FK_CATEGORI_ASSOCIATI_TYPES]

ALTER TABLE [dbo].[Document]  WITH CHECK ADD  CONSTRAINT [FK_DOCUMENT_ASSOCIATI_POSTULER] FOREIGN KEY([Ins_id], [Job_id], [Pos_id])
REFERENCES [dbo].[Postuler] ([Ins_id], [Job_id], [id])

ALTER TABLE [dbo].[Document] CHECK CONSTRAINT [FK_DOCUMENT_ASSOCIATI_POSTULER]

ALTER TABLE [dbo].[Education]  WITH CHECK ADD  CONSTRAINT [FK_EDUCATIO_ASSOCIATI_INSCRIRE] FOREIGN KEY([Ins_id])
REFERENCES [dbo].[Inscrire] ([id])

ALTER TABLE [dbo].[Education] CHECK CONSTRAINT [FK_EDUCATIO_ASSOCIATI_INSCRIRE]

ALTER TABLE [dbo].[Experience]  WITH CHECK ADD  CONSTRAINT [FK_EXPERIEN_ASSOCIATI_INSCRIRE] FOREIGN KEY([Ins_id])
REFERENCES [dbo].[Inscrire] ([id])

ALTER TABLE [dbo].[Experience] CHECK CONSTRAINT [FK_EXPERIEN_ASSOCIATI_INSCRIRE]

ALTER TABLE [dbo].[File]  WITH CHECK ADD  CONSTRAINT [FK_FILE_ASSOCIATI_INSCRIRE] FOREIGN KEY([Ins_id])
REFERENCES [dbo].[Inscrire] ([id])

ALTER TABLE [dbo].[File] CHECK CONSTRAINT [FK_FILE_ASSOCIATI_INSCRIRE]

ALTER TABLE [dbo].[Information]  WITH CHECK ADD  CONSTRAINT [FK_INFORMAT_ASSOCIATI_INSCRIRE] FOREIGN KEY([Ins_id])
REFERENCES [dbo].[Inscrire] ([id])

ALTER TABLE [dbo].[Information] CHECK CONSTRAINT [FK_INFORMAT_ASSOCIATI_INSCRIRE]

ALTER TABLE [dbo].[InsAbonne]  WITH CHECK ADD  CONSTRAINT [FK_INSABONN_ASSOCIATI_ABONNEME] FOREIGN KEY([Abo_id])
REFERENCES [dbo].[Abonnement] ([id])

ALTER TABLE [dbo].[InsAbonne] CHECK CONSTRAINT [FK_INSABONN_ASSOCIATI_ABONNEME]

ALTER TABLE [dbo].[InsAbonne]  WITH CHECK ADD  CONSTRAINT [FK_INSABONN_ASSOCIATI_INSCRIRE] FOREIGN KEY([Ins_id])
REFERENCES [dbo].[Inscrire] ([id])

ALTER TABLE [dbo].[InsAbonne] CHECK CONSTRAINT [FK_INSABONN_ASSOCIATI_INSCRIRE]

ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK_JOB_ASSOCIATI_CATEGORI] FOREIGN KEY([Cat_id])
REFERENCES [dbo].[Categorie] ([id])

ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK_JOB_ASSOCIATI_CATEGORI]

ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_LOCATION_ASSOCIATI_INSCRIRE] FOREIGN KEY([Ins_id])
REFERENCES [dbo].[Inscrire] ([id])

ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_LOCATION_ASSOCIATI_INSCRIRE]

ALTER TABLE [dbo].[Paiement]  WITH CHECK ADD  CONSTRAINT [FK_PAIEMENT_ASSOCIATI_LOCATION] FOREIGN KEY([Ins_id], [Ins_id2], [Loc_id])
REFERENCES [dbo].[Location] ([Ins_id], [Ins_id2], [id])

ALTER TABLE [dbo].[Paiement] CHECK CONSTRAINT [FK_PAIEMENT_ASSOCIATI_LOCATION]

ALTER TABLE [dbo].[Paiement]  WITH CHECK ADD  CONSTRAINT [FK_PAIEMENT_ASSOCIATI_POSTULER] FOREIGN KEY([Pos_Ins_id], [Job_id], [Pos_id])
REFERENCES [dbo].[Postuler] ([Ins_id], [Job_id], [id])

ALTER TABLE [dbo].[Paiement] CHECK CONSTRAINT [FK_PAIEMENT_ASSOCIATI_POSTULER]

ALTER TABLE [dbo].[Postuler]  WITH CHECK ADD  CONSTRAINT [FK_POSTULER_ASSOCIATI_INSCRIRE] FOREIGN KEY([Ins_id])
REFERENCES [dbo].[Inscrire] ([id])

ALTER TABLE [dbo].[Postuler] CHECK CONSTRAINT [FK_POSTULER_ASSOCIATI_INSCRIRE]

ALTER TABLE [dbo].[Postuler]  WITH CHECK ADD  CONSTRAINT [FK_POSTULER_ASSOCIATI_JOB] FOREIGN KEY([Job_id])
REFERENCES [dbo].[Job] ([id])

ALTER TABLE [dbo].[Postuler] CHECK CONSTRAINT [FK_POSTULER_ASSOCIATI_JOB]

ALTER TABLE [dbo].[Utilisateur]  WITH CHECK ADD  CONSTRAINT [FK_UTILISAT_ASSOCIATI_ROLE] FOREIGN KEY([Rol_id])
REFERENCES [dbo].[Role] ([id])

ALTER TABLE [dbo].[Utilisateur] CHECK CONSTRAINT [FK_UTILISAT_ASSOCIATI_ROLE]


