CREATE DATABASE [NiovarJob]

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
	[description] [varchar](254) NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_AUTRE] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

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
	[description] [varchar](254) NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_EDUCATION] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Experience](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Ins_id] [numeric](18, 0) NOT NULL,
	[etablissement] [varchar](254) NULL,
	[fonction] [varchar](254) NULL,
	[periode] [varchar](254) NULL,
	[description] [varchar](254) NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_EXPERIENCE] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

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
	[lettre] [text] NULL,
	[competence] [varchar](254) NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_INFORMATION] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

CREATE TABLE [dbo].[Inscrire](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[type] [varchar](254) NULL,
	[nom] [varchar](250) NULL,
	[login] [varchar](250) NULL,
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
	[titreEmploi] [varchar](254) NULL,
	[anneeExperience] [varchar](254) NULL,
	[salaireSouhaiter] [float] NULL,
	[salaireNegocier] [float] NULL,
	[profil] [varchar](254) NULL,
	[etat] [int] NULL,
	[status] [int] NULL,
	[archived] [int] NULL,
	[created] [datetime] NULL,
	[cpassword] [varchar](255) NULL,
	[email_prof] [varchar](254) NULL,
 CONSTRAINT [PK_INSCRIRE] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

CREATE TABLE [dbo].[Job](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Typ_id] [numeric](18, 0) NOT NULL,
	[Cat_id] [numeric](18, 0) NOT NULL,
	[titre] [varchar](254) NULL,
	[description] [text] NULL,
	[dateEntre] [varchar](254) NULL,
	[ville] [varchar](254) NULL,
	[nbreEmploye] [int] NULL,
	[heureTravail] [varchar](254) NULL,
	[heureTravailJour] [varchar](254) NULL,
	[jourTravail] [varchar](254) NULL,
	[remuneration] [int] NULL,
	[totalHeureTravail] [int] NULL,
	[margeExperience] [varchar](254) NULL,
	[typeJob] [varchar](254) NULL,
	[etat] [varchar](254) NULL,
	[status] [int] NULL,
	[created] [datetime] NULL,
	[archived] [int] NULL,
	[responsabilite] [text] NULL,
	[exigence] [text] NULL,
	[autre] [varchar](254) NULL,
 CONSTRAINT [PK_JOB] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

CREATE TABLE [dbo].[Paiement](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Job_id] [numeric](18, 0) NOT NULL,
	[libelle] [varchar](254) NULL,
	[montant] [float] NULL,
	[avance] [float] NULL,
	[reste] [float] NULL,
	[type] [varchar](254) NULL,
	[etat] [int] NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
	[created] [datetime] NULL,
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
	[remuneration] [int] NULL,
	[etatAdmin] [varchar](254) NULL,
	[etat] [varchar](254) NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
	[created] [datetime] NULL,
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

CREATE NONCLUSTERED INDEX [ASSOCIATION4_FK] ON [dbo].[Autre]
(
	[Ins_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [ASSOCIATION2_FK] ON [dbo].[CatAnneeExp]
(
	[Cat_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [ASSOCIATION2_FK2] ON [dbo].[CatAnneeExp]
(
	[Ann_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [ASSOCIATION12_FK] ON [dbo].[Document]
(
	[Ins_id] ASC,
	[Job_id] ASC,
	[Pos_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [ASSOCIATION6_FK] ON [dbo].[Education]
(
	[Ins_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [ASSOCIATION5_FK] ON [dbo].[Experience]
(
	[Ins_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [ASSOCIATION8_FK] ON [dbo].[File]
(
	[Ins_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [ASSOCIATION7_FK] ON [dbo].[Information]
(
	[Ins_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [ASSOCIATION1_FK] ON [dbo].[Job]
(
	[Cat_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [ASSOCIATION9_FK] ON [dbo].[Job]
(
	[Typ_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [ASSOCIATION10_FK] ON [dbo].[Paiement]
(
	[Job_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [ASSOCIATION11_FK] ON [dbo].[Postuler]
(
	[Ins_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [ASSOCIATION11_FK2] ON [dbo].[Postuler]
(
	[Job_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [ASSOCIATION3_FK] ON [dbo].[Utilisateur]
(
	[Rol_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

ALTER TABLE [dbo].[Autre]  WITH CHECK ADD  CONSTRAINT [FK_AUTRE_ASSOCIATI_INSCRIRE] FOREIGN KEY([Ins_id])
REFERENCES [dbo].[Inscrire] ([id])

ALTER TABLE [dbo].[Autre] CHECK CONSTRAINT [FK_AUTRE_ASSOCIATI_INSCRIRE]

ALTER TABLE [dbo].[CatAnneeExp]  WITH CHECK ADD  CONSTRAINT [FK_CATANNEE_ASSOCIATI_ANNEEEXP] FOREIGN KEY([Ann_id])
REFERENCES [dbo].[AnneeExp] ([id])

ALTER TABLE [dbo].[CatAnneeExp] CHECK CONSTRAINT [FK_CATANNEE_ASSOCIATI_ANNEEEXP]

ALTER TABLE [dbo].[CatAnneeExp]  WITH CHECK ADD  CONSTRAINT [FK_CATANNEE_ASSOCIATI_CATEGORI] FOREIGN KEY([Cat_id])
REFERENCES [dbo].[Categorie] ([id])

ALTER TABLE [dbo].[CatAnneeExp] CHECK CONSTRAINT [FK_CATANNEE_ASSOCIATI_CATEGORI]

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

ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK_JOB_ASSOCIATI_CATEGORI] FOREIGN KEY([Cat_id])
REFERENCES [dbo].[Categorie] ([id])

ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK_JOB_ASSOCIATI_CATEGORI]

ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK_JOB_ASSOCIATI_TYPES] FOREIGN KEY([Typ_id])
REFERENCES [dbo].[Types] ([id])

ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK_JOB_ASSOCIATI_TYPES]

ALTER TABLE [dbo].[Paiement]  WITH CHECK ADD  CONSTRAINT [FK_PAIEMENT_ASSOCIATI_JOB] FOREIGN KEY([Job_id])
REFERENCES [dbo].[Job] ([id])

ALTER TABLE [dbo].[Paiement] CHECK CONSTRAINT [FK_PAIEMENT_ASSOCIATI_JOB]

ALTER TABLE [dbo].[Postuler]  WITH CHECK ADD  CONSTRAINT [FK_POSTULER_ASSOCIATI_INSCRIRE] FOREIGN KEY([Ins_id])
REFERENCES [dbo].[Inscrire] ([id])

ALTER TABLE [dbo].[Postuler] CHECK CONSTRAINT [FK_POSTULER_ASSOCIATI_INSCRIRE]

ALTER TABLE [dbo].[Postuler]  WITH CHECK ADD  CONSTRAINT [FK_POSTULER_ASSOCIATI_JOB] FOREIGN KEY([Job_id])
REFERENCES [dbo].[Job] ([id])

ALTER TABLE [dbo].[Postuler] CHECK CONSTRAINT [FK_POSTULER_ASSOCIATI_JOB]

ALTER TABLE [dbo].[Utilisateur]  WITH CHECK ADD  CONSTRAINT [FK_UTILISAT_ASSOCIATI_ROLE] FOREIGN KEY([Rol_id])
REFERENCES [dbo].[Role] ([id])

ALTER TABLE [dbo].[Utilisateur] CHECK CONSTRAINT [FK_UTILISAT_ASSOCIATI_ROLE]

