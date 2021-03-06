/*==============================================================*/
/* Table : Abonnement                                           */
/*==============================================================*/
create table Abonnement (
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
   constraint PK_ABONNEMENT primary key nonclustered (id)
)
go

/*==============================================================*/
/* Table : AnneeExp                                             */
/*==============================================================*/
create table AnneeExp (
   id                   numeric              identity,
   libelle              varchar(254)         null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_ANNEEEXP primary key nonclustered (id)
)
go

/*==============================================================*/
/* Table : AssocLangueNiveau                                    */
/*==============================================================*/
create table AssocLangueNiveau (
   Lan_id               numeric              not null,
   Niv_id               numeric              not null,
   id                   numeric              identity,
   libelle              varchar(254)         null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_ASSOCLANGUENIVEAU primary key nonclustered (Lan_id, Niv_id, id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION21_FK                                     */
/*==============================================================*/
create index ASSOCIATION21_FK on AssocLangueNiveau (
Lan_id ASC
)
go

/*==============================================================*/
/* Index : ASSOCIATION21_FK2                                    */
/*==============================================================*/
create index ASSOCIATION21_FK2 on AssocLangueNiveau (
Niv_id ASC
)
go

/*==============================================================*/
/* Table : Autre                                                */
/*==============================================================*/
create table Autre (
   id                   numeric              identity,
   Ins_id               numeric              not null,
   etablissement        varchar(254)         null,
   fonction             varchar(254)         null,
   periode              text        null,
   description          varchar(254)         null,
   archived             int                  null,
   status               int                  null,
   constraint PK_AUTRE primary key nonclustered (id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION4_FK                                      */
/*==============================================================*/
create index ASSOCIATION4_FK on Autre (
Ins_id ASC
)
go

/*==============================================================*/
/* Table : AvantageSociaux                                      */
/*==============================================================*/
create table AvantageSociaux (
   id                   numeric              identity,
   libelle              varchar(254)         null,
   description          text         null,
   image                varchar(254)         null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_AVANTAGESOCIAUX primary key nonclustered (id)
)
go

/*==============================================================*/
/* Table : AvantageSociauxJob                                   */
/*==============================================================*/
create table AvantageSociauxJob (
   Job_id               numeric              not null,
   Ava_id               numeric              not null,
   id                   numeric              identity,
   archived             int                  null,
   created              datetime             null,
   constraint PK_AVANTAGESOCIAUXJOB primary key nonclustered (Job_id, Ava_id, id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION26_FK                                     */
/*==============================================================*/
create index ASSOCIATION26_FK on AvantageSociauxJob (
Job_id ASC
)
go

/*==============================================================*/
/* Index : ASSOCIATION26_FK2                                    */
/*==============================================================*/
create index ASSOCIATION26_FK2 on AvantageSociauxJob (
Ava_id ASC
)
go

/*==============================================================*/
/* Table : Avis                                                 */
/*==============================================================*/
create table Avis (
   id                   numeric              identity,
   Pag_id               numeric              not null,
   libelle              text         null,
   iduser                int        null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_AVIS primary key nonclustered (id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION32_FK                                     */
/*==============================================================*/
create index ASSOCIATION32_FK on Avis (
Pag_id ASC
)
go

/*==============================================================*/
/* Table : CatAnneeExp                                          */
/*==============================================================*/
create table CatAnneeExp (
   Cat_id               numeric              not null,
   Ann_id               numeric              not null,
   id                   numeric              identity,
   prixHoraire          int                  null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_CATANNEEEXP primary key nonclustered (Cat_id, Ann_id, id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION2_FK                                      */
/*==============================================================*/
create index ASSOCIATION2_FK on CatAnneeExp (
Cat_id ASC
)
go

/*==============================================================*/
/* Index : ASSOCIATION2_FK2                                     */
/*==============================================================*/
create index ASSOCIATION2_FK2 on CatAnneeExp (
Ann_id ASC
)
go

/*==============================================================*/
/* Table : Categorie                                            */
/*==============================================================*/
create table Categorie (
   id                   numeric              identity,
   Typ_id               numeric              not null,
   libelle              varchar(254)         null,
   image                varchar(254)         null,
   archived             int                  null,
   status               int                  null,
   constraint PK_CATEGORIE primary key nonclustered (id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION15_FK                                     */
/*==============================================================*/
create index ASSOCIATION15_FK on Categorie (
Typ_id ASC
)
go

/*==============================================================*/
/* Table : Contrat                                              */
/*==============================================================*/
create table Contrat (
   id                   numeric              identity,
   libelle              varchar(254)         null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_CONTRAT primary key nonclustered (id)
)
go

/*==============================================================*/
/* Table : Diplome                                              */
/*==============================================================*/
create table Diplome (
   id                   numeric              identity,
   libelle              varchar(254)         null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_DIPLOME primary key nonclustered (id)
)
go

/*==============================================================*/
/* Table : DiplomeJob                                           */
/*==============================================================*/
create table DiplomeJob (
   Job_id               numeric              not null,
   Dip_id               numeric              not null,
   id                   numeric              identity,
   archived             int                  null,
   created              datetime             null,
   constraint PK_DIPLOMEJOB primary key nonclustered (Job_id, Dip_id, id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION18_FK                                     */
/*==============================================================*/
create index ASSOCIATION18_FK on DiplomeJob (
Job_id ASC
)
go

/*==============================================================*/
/* Index : ASSOCIATION18_FK2                                    */
/*==============================================================*/
create index ASSOCIATION18_FK2 on DiplomeJob (
Dip_id ASC
)
go

/*==============================================================*/
/* Table : Document                                             */
/*==============================================================*/
create table Document (
    [id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Ins_id] [numeric](18, 0) NOT NULL,
	[Job_id] [numeric](18, 0) NOT NULL,
	[Pos_id] [numeric](18, 0) NOT NULL,
	[type] [varchar](254) NULL,
	[libelle] text NULL,
	[chemin] [varchar](254) NULL,
	[etat] [int] NULL,
	[status] [int] NULL,
	[archived] [int] NULL,
	[created] [datetime] NULL,
   constraint PK_DOCUMENT primary key nonclustered (id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION12_FK                                     */
/*==============================================================*/
create index ASSOCIATION12_FK on Document (
Ins_id ASC,
Job_id ASC,
Pos_id ASC
)
go

/*==============================================================*/
/* Table : Education                                            */
/*==============================================================*/
create table Education (
   id                   numeric              identity,
   Ins_id               numeric              not null,
   etablissement        varchar(254)         null,
   diplome              varchar(254)         null,
   periode              varchar(254)         null,
   description          text        null,
   archived             int                  null,
   status               int                  null,
   constraint PK_EDUCATION primary key nonclustered (id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION6_FK                                      */
/*==============================================================*/
create index ASSOCIATION6_FK on Education (
Ins_id ASC
)
go

/*==============================================================*/
/* Table : Experience                                           */
/*==============================================================*/
create table Experience (
   id                   numeric              identity,
   Ins_id               numeric              not null,
   etablissement        varchar(254)         null,
   fonction             varchar(254)         null,
   periode              varchar(254)         null,
   description          text         null,
   archived             int                  null,
   status               int                  null,
   constraint PK_EXPERIENCE primary key nonclustered (id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION5_FK                                      */
/*==============================================================*/
create index ASSOCIATION5_FK on Experience (
Ins_id ASC
)
go

/*==============================================================*/
/* Table : "File"                                               */
/*==============================================================*/
create table "File" (
   id                   numeric              identity,
   Ins_id               numeric              not null,
   type                 varchar(254)         null,
   libelle              varchar(254)         null,
   chemin               varchar(254)         null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_FILE primary key nonclustered (id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION8_FK                                      */
/*==============================================================*/
create index ASSOCIATION8_FK on "File" (
Ins_id ASC
)
go

/*==============================================================*/
/* Table : FraisLocation                                        */
/*==============================================================*/
create table FraisLocation (
      [id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
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
   constraint PK_FRAISLOCATION primary key nonclustered (id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION17_FK                                     */
/*==============================================================*/
create index ASSOCIATION17_FK on FraisLocation (
Ins_id ASC,
Ins_id2 ASC,
Loc_id ASC
)
go

/*==============================================================*/
/* Table : Galerie                                              */
/*==============================================================*/
create table Galerie (
   id                   numeric              identity,
   Pag_id               numeric              not null,
   libelle              varchar(254)         null,
   image                varchar(254)         null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_GALERIE primary key nonclustered (id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION31_FK                                     */
/*==============================================================*/
create index ASSOCIATION31_FK on Galerie (
Pag_id ASC
)
go

/*==============================================================*/
/* Table : Information                                          */
/*==============================================================*/
create table Information (
   id                   numeric              identity,
   Ins_id               numeric              not null,
   lettre               varchar(254)         null,
   competence           text         null,
   archived             int                  null,
   status               int                  null,
   constraint PK_INFORMATION primary key nonclustered (id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION7_FK                                      */
/*==============================================================*/
create index ASSOCIATION7_FK on Information (
Ins_id ASC
)
go

/*==============================================================*/
/* Table : InsAbonne                                            */
/*==============================================================*/
create table InsAbonne (
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
   constraint PK_INSABONNE primary key nonclustered (Ins_id, Abo_id, id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION13_FK                                     */
/*==============================================================*/
create index ASSOCIATION13_FK on InsAbonne (
Ins_id ASC
)
go

/*==============================================================*/
/* Index : ASSOCIATION13_FK2                                    */
/*==============================================================*/
create index ASSOCIATION13_FK2 on InsAbonne (
Abo_id ASC
)
go

/*==============================================================*/
/* Table : Inscrire                                             */
/*==============================================================*/
create table Inscrire (
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
	[countryCode] [varchar](254) NULL,
	allowNotification                  varchar(254)         null,
        allowNewsletter                 varchar(254)         null,
   constraint PK_INSCRIRE primary key nonclustered (id)
)
go

/*==============================================================*/
/* Table : Job                                                  */
/*==============================================================*/
create table Job (
   id                   numeric              identity,
   Niv_id               numeric              not null,
   Typ_id               numeric              not null,
   Lan_id               numeric              not null,
   Con_id               numeric              not null,
   Qua_id               numeric              not null,
   Sta_id               numeric              not null,
   Cat_id               numeric              not null,
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
	[countryCode] [varchar](254) NULL,
 	avantageSociaux     varchar(254)    null,
	[equipeEmploi] [text] NULL,
	codePostal    varchar(254)    null,
	adressePostal     text    null,
	masquerEmplacement int null,
	quartTravail varchar(254)    null,
	DateEntreValeur varchar(254)    null,
	[dateDebut] [datetime] NULL,
	[dateFin] [datetime] NULL,
	niveauOrale int null,
	niveauEcrire int null,
		
   constraint PK_JOB primary key nonclustered (id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION1_FK                                      */
/*==============================================================*/
create index ASSOCIATION1_FK on Job (
Cat_id ASC
)
go

/*==============================================================*/
/* Index : ASSOCIATION19_FK                                     */
/*==============================================================*/
create index ASSOCIATION19_FK on Job (
Niv_id ASC
)
go

/*==============================================================*/
/* Index : ASSOCIATION20_FK                                     */
/*==============================================================*/
create index ASSOCIATION20_FK on Job (
Lan_id ASC
)
go

/*==============================================================*/
/* Index : ASSOCIATION22_FK                                     */
/*==============================================================*/
create index ASSOCIATION22_FK on Job (
Sta_id ASC
)
go

/*==============================================================*/
/* Index : ASSOCIATION23_FK                                     */
/*==============================================================*/
create index ASSOCIATION23_FK on Job (
Con_id ASC
)
go

/*==============================================================*/
/* Index : ASSOCIATION24_FK                                     */
/*==============================================================*/
create index ASSOCIATION24_FK on Job (
Qua_id ASC
)
go

/*==============================================================*/
/* Index : ASSOCIATION25_FK                                     */
/*==============================================================*/
create index ASSOCIATION25_FK on Job (
Typ_id ASC
)
go

/*==============================================================*/
/* Table : Langue                                               */
/*==============================================================*/
create table Langue (
   id                   numeric              identity,
   libelle              varchar(254)         null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_LANGUE primary key nonclustered (id)
)
go

/*==============================================================*/
/* Table : Location                                             */
/*==============================================================*/
create table Location (
   Ins_id               numeric              not null,
   Ins_id2              numeric              not null,
   id                   numeric              identity,
   periode              varchar(254)         null,
   heureTravail         varchar(254)         null,
   description          varchar(254)         null,
   montant              numeric              null,
   signClient           int                  null,
   signCandidat         int                  null,
   remuneration         numeric              null,
   dateSgnClt           datetime             null,
   dateSgnCdt           datetime             null,
   avisClient           int                  null,
   avisCandidat         int                  null,
   status               int                  null,
   etat                 int                  null,
   created              datetime             null,
   archived             int                  null,
   [dateDebut] [datetime] NULL,
   [dateFin] [datetime] NULL,
   constraint PK_LOCATION primary key nonclustered (Ins_id, Ins_id2, id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION16_FK                                     */
/*==============================================================*/
create index ASSOCIATION16_FK on Location (
Ins_id ASC
)
go

/*==============================================================*/
/* Index : ASSOCIATION17_FK                                     */
/*==============================================================*/
create index ASSOCIATION17_FK on Location (
Ins_id2 ASC
)
go

/*==============================================================*/
/* Table : NiveauLangue                                         */
/*==============================================================*/
create table NiveauLangue (
   id                   numeric              identity,
   libelle              varchar(254)         null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_NIVEAULANGUE primary key nonclustered (id)
)
go

/*==============================================================*/
/* Table : NiveauScolarite                                      */
/*==============================================================*/
create table NiveauScolarite (
   id                   numeric              identity,
   libelle              varchar(254)         null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_NIVEAUSCOLARITE primary key nonclustered (id)
)
go

/*==============================================================*/
/* Table : Page                                                 */
/*==============================================================*/
create table Page (
   id                   numeric              identity,
   Ins_id               numeric              not null,
   profilPage           varchar(254)         null,
   profil               varchar(254)         null,
   aPropos              text         null,
   pourquoiPostuler     text        null,
   description          text         null,
   nomPage              varchar(254)         null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_PAGE primary key nonclustered (id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION27_FK                                     */
/*==============================================================*/
create index ASSOCIATION27_FK on Page (
Ins_id ASC
)
go

/*==============================================================*/
/* Table : Paiement                                             */
/*==============================================================*/
create table Paiement (
  [id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Ins_id] [numeric](18, 0) NOT NULL,
	[Job_id] [numeric](18, 0) NOT NULL,
	[Pos_id] [numeric](18, 0) NOT NULL,
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
   constraint PK_PAIEMENT primary key nonclustered (id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION14_FK                                     */
/*==============================================================*/
create index ASSOCIATION14_FK on Paiement (
Ins_id ASC,
Job_id ASC,
Pos_id ASC
)
go

/*==============================================================*/
/* Table : Parametre                                            */
/*==============================================================*/
create table Parametre (
   id                   numeric              identity,
   libelle              varchar(254)         null,
   valeur               varchar(254)         null,
   description          varchar(254)         null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_PARAMETRE primary key nonclustered (id)
)
go


/*==============================================================*/
/* Table : Bibliotheque                                            */
/*==============================================================*/
create table Bibliotheque (
   id                   numeric              identity,
   libelle              varchar(254)         null,
   chemin               varchar(254)         null,
   description          varchar(254)         null,
   status               int                  null,
   created              datetime             null,
    archived             int                  null,
   constraint PK_BIBLIOTHEQUE primary key nonclustered (id)
)
go


/*==============================================================*/
/* Table : Postuler                                             */
/*==============================================================*/
create table Postuler (
      [Ins_id] [numeric](18, 0) NOT NULL,
	[Job_id] [numeric](18, 0) NOT NULL,
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[libelle] [varchar](254) NULL,
	[datePostule] [datetime] NULL,
	[heurePostule] [varchar](254) NULL,
	[remuneration] [float] NULL,
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
	[compagnyId] [int] NULL,
   constraint PK_POSTULER primary key nonclustered (Ins_id, Job_id, id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION11_FK                                     */
/*==============================================================*/
create index ASSOCIATION11_FK on Postuler (
Ins_id ASC
)
go

/*==============================================================*/
/* Index : ASSOCIATION11_FK2                                    */
/*==============================================================*/
create index ASSOCIATION11_FK2 on Postuler (
Job_id ASC
)
go

/*==============================================================*/
/* Table : QuartTravail                                         */
/*==============================================================*/
create table QuartTravail (
   id                   numeric              identity,
   libelle              varchar(254)         null,
   description          text         null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_QUARTTRAVAIL primary key nonclustered (id)
)
go

/*==============================================================*/
/* Table : Question                                             */
/*==============================================================*/
create table Question (
   id                   numeric              identity,
   Pag_id               numeric              not null,
   libelle             text         null,
   iduser               int          null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_QUESTION primary key nonclustered (id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION29_FK                                     */
/*==============================================================*/
create index ASSOCIATION29_FK on Question (
Pag_id ASC
)
go

/*==============================================================*/
/* Table : Reponse                                              */
/*==============================================================*/
create table Reponse (
   id                   numeric              identity,
   Que_id               numeric              not null,
   libelle              text         null,
   iduser                int          null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_REPONSE primary key nonclustered (id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION30_FK                                     */
/*==============================================================*/
create index ASSOCIATION30_FK on Reponse (
Que_id ASC
)
go

/*==============================================================*/
/* Table : Role                                                 */
/*==============================================================*/
create table Role (
   id                   numeric              identity,
   libelle              varchar(254)         null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_ROLE primary key nonclustered (id)
)
go

/*==============================================================*/
/* Table : StatutEmploi                                         */
/*==============================================================*/
create table StatutEmploi (
   id                   numeric              identity,
   libelle              varchar(254)         null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_STATUTEMPLOI primary key nonclustered (id)
)
go

/*==============================================================*/
/* Table : Suivre                                               */
/*==============================================================*/
create table Suivre (
   Ins_id               numeric              not null,
   Pag_id               numeric              not null,
   id                   numeric              identity,
   archived             int                  null,
   created              datetime             null,
   constraint PK_SUIVRE primary key nonclustered (Ins_id, Pag_id, id)
)
go

/*==============================================================*/
/* Index : PEUT_SUIVRE_FK                                       */
/*==============================================================*/
create index PEUT_SUIVRE_FK on Suivre (
Ins_id ASC
)
go

/*==============================================================*/
/* Index : EST_SUIVI_FK                                         */
/*==============================================================*/
create index EST_SUIVI_FK on Suivre (
Pag_id ASC
)
go

/*==============================================================*/
/* Table : TypeOffre                                            */
/*==============================================================*/
create table TypeOffre (
   id                   numeric              identity,
   libelle              varchar(254)         null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   constraint PK_TYPEOFFRE primary key nonclustered (id)
)
go

/*==============================================================*/
/* Table : Types                                                */
/*==============================================================*/
create table Types (
    [id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[libelle] [varchar](254) NULL,
	[archived] [int] NULL,
	[status] [int] NULL,
	[created] [datetime] NULL,
	[image] [varchar](254) NULL,
   constraint PK_TYPES primary key nonclustered (id)
)
go

/*==============================================================*/
/* Table : Utilisateur                                          */
/*==============================================================*/
create table Utilisateur (
   id                   numeric              identity,
   Rol_id               numeric              not null,
   nom                  varchar(254)         null,
   email                varchar(254)         null,
   password             varchar(254)         null,
   archived             int                  null,
   status               int                  null,
   created              datetime             null,
   allowNotification                  varchar(254)         null,
   allowNewsletter                 varchar(254)         null,
   constraint PK_UTILISATEUR primary key nonclustered (id)
)
go

/*==============================================================*/
/* Index : ASSOCIATION3_FK                                      */
/*==============================================================*/
create index ASSOCIATION3_FK on Utilisateur (
Rol_id ASC
)
go

alter table AssocLangueNiveau
   add constraint FK_ASSOCLAN_ASSOCIATI_LANGUE foreign key (Lan_id)
      references Langue (id)
go

alter table AssocLangueNiveau
   add constraint FK_ASSOCLAN_ASSOCIATI_NIVEAULA foreign key (Niv_id)
      references NiveauLangue (id)
go

alter table Autre
   add constraint FK_AUTRE_ASSOCIATI_INSCRIRE foreign key (Ins_id)
      references Inscrire (id)
go

alter table AvantageSociauxJob
   add constraint FK_AVANTAGE_ASSOCIATI_AVANTAGE foreign key (Ava_id)
      references AvantageSociaux (id)
go

alter table AvantageSociauxJob
   add constraint FK_AVANTAGE_ASSOCIATI_JOB foreign key (Job_id)
      references Job (id)
go

alter table Avis
   add constraint FK_AVIS_ASSOCIATI_PAGE foreign key (Pag_id)
      references Page (id)
go

alter table CatAnneeExp
   add constraint FK_CATANNEE_ASSOCIATI_ANNEEEXP foreign key (Ann_id)
      references AnneeExp (id)
go

alter table CatAnneeExp
   add constraint FK_CATANNEE_ASSOCIATI_CATEGORI foreign key (Cat_id)
      references Categorie (id)
go

alter table Categorie
   add constraint FK_CATEGORI_ASSOCIATI_TYPES foreign key (Typ_id)
      references Types (id)
go

alter table DiplomeJob
   add constraint FK_DIPLOMEJ_ASSOCIATI_DIPLOME foreign key (Dip_id)
      references Diplome (id)
go

alter table DiplomeJob
   add constraint FK_DIPLOMEJ_ASSOCIATI_JOB foreign key (Job_id)
      references Job (id)
go

alter table Document
   add constraint FK_DOCUMENT_ASSOCIATI_POSTULER foreign key (Ins_id, Job_id, Pos_id)
      references Postuler (Ins_id, Job_id, id)
go

alter table Education
   add constraint FK_EDUCATIO_ASSOCIATI_INSCRIRE foreign key (Ins_id)
      references Inscrire (id)
go

alter table Experience
   add constraint FK_EXPERIEN_ASSOCIATI_INSCRIRE foreign key (Ins_id)
      references Inscrire (id)
go

alter table "File"
   add constraint FK_FILE_ASSOCIATI_INSCRIRE foreign key (Ins_id)
      references Inscrire (id)
go

alter table FraisLocation
   add constraint FK_FRAISLOC_ASSOCIATI_LOCATION foreign key (Ins_id, Ins_id2, Loc_id)
      references Location (Ins_id, Ins_id2, id)
go

alter table Galerie
   add constraint FK_GALERIE_ASSOCIATI_PAGE foreign key (Pag_id)
      references Page (id)
go

alter table Information
   add constraint FK_INFORMAT_ASSOCIATI_INSCRIRE foreign key (Ins_id)
      references Inscrire (id)
go

alter table InsAbonne
   add constraint FK_INSABONN_ASSOCIATI_ABONNEME foreign key (Abo_id)
      references Abonnement (id)
go

alter table InsAbonne
   add constraint FK_INSABONN_ASSOCIATI_INSCRIRE foreign key (Ins_id)
      references Inscrire (id)
go

alter table Job
   add constraint FK_JOB_ASSOCIATI_CATEGORI foreign key (Cat_id)
      references Categorie (id)
go

alter table Job
   add constraint FK_JOB_ASSOCIATI_NIVEAUSC foreign key (Niv_id)
      references NiveauScolarite (id)
go

alter table Job
   add constraint FK_JOB_ASSOCIATI_LANGUE foreign key (Lan_id)
      references Langue (id)
go

alter table Job
   add constraint FK_JOB_ASSOCIATI_STATUTEM foreign key (Sta_id)
      references StatutEmploi (id)
go

alter table Job
   add constraint FK_JOB_ASSOCIATI_CONTRAT foreign key (Con_id)
      references Contrat (id)
go

alter table Job
   add constraint FK_JOB_ASSOCIATI_QUARTTRA foreign key (Qua_id)
      references QuartTravail (id)
go

alter table Job
   add constraint FK_JOB_ASSOCIATI_TYPEOFFR foreign key (Typ_id)
      references TypeOffre (id)
go

alter table Location
   add constraint FK_LOCATION_ASSOCIATI_INSCRIRE foreign key (Ins_id)
      references Inscrire (id)
go

alter table Location
   add constraint FK_LOCATION_ASSOCIATI_INSCRIRE foreign key (Ins_id2)
      references Inscrire (id)
go

alter table Page
   add constraint FK_PAGE_ASSOCIATI_INSCRIRE foreign key (Ins_id)
      references Inscrire (id)
go

alter table Paiement
   add constraint FK_PAIEMENT_ASSOCIATI_POSTULER foreign key (Ins_id, Job_id, Pos_id)
      references Postuler (Ins_id, Job_id, id)
go

alter table Postuler
   add constraint FK_POSTULER_ASSOCIATI_INSCRIRE foreign key (Ins_id)
      references Inscrire (id)
go

alter table Postuler
   add constraint FK_POSTULER_ASSOCIATI_JOB foreign key (Job_id)
      references Job (id)
go

alter table Question
   add constraint FK_QUESTION_ASSOCIATI_PAGE foreign key (Pag_id)
      references Page (id)
go

alter table Reponse
   add constraint FK_REPONSE_ASSOCIATI_QUESTION foreign key (Que_id)
      references Question (id)
go

alter table Suivre
   add constraint FK_SUIVRE_EST_SUIVI_PAGE foreign key (Pag_id)
      references Page (id)
go

alter table Suivre
   add constraint FK_SUIVRE_PEUT_SUIV_INSCRIRE foreign key (Ins_id)
      references Inscrire (id)
go

alter table Utilisateur
   add constraint FK_UTILISAT_ASSOCIATI_ROLE foreign key (Rol_id)
      references Role (id)
go
