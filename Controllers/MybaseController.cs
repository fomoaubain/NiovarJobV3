using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class MybaseController: Controller
    {

        protected NiovarJobEntities db = new NiovarJobEntities();

        protected InsAbonne lastAsbonnement;
        protected Inscrire _me;


        protected double pourcentage = 35;

        protected String STRIPE_API_KEY = "sk_test_51Gx6jgIK0UhIWHGbs9dcTW7tyGLkl39s6waxls9Z5D8E0arsL4bjy9N0g563Tlzo5JNvbeFOkAl5fMEY85eerPIx00mYJFiqLY";



        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {


            Parametre globalParametre = db.Parametre.FirstOrDefault();
            if (globalParametre != null) { 
                ViewBag.pourcentage = Convert.ToDouble( globalParametre.valeur);
                pourcentage = Convert.ToDouble(globalParametre.valeur);
            } 
            else { 
                ViewBag.pourcentage = 35; 
            }

            ViewBag.nbrCandidat = db.Inscrire.Where(p => p.archived == 1 && p.status == 1 && p.type.Equals("candidat")).ToList().Count;

            ViewBag.nbrClient = db.Inscrire.Where(p => p.archived == 1 && p.status == 1 && p.type.Equals("client")).ToList().Count;

            ViewBag.nbrOffre = db.Postuler.Where(p => p.archived == 1 && p.Inscrire.type.Equals("client")).ToList().Count;

            ViewBag.nbrPostulant = db.Postuler.Where(p => p.archived == 1 && p.status == 1 && p.Inscrire.type.Equals("candidat")).ToList().Count;

            ViewBag.nbreOffreUrgent = db.Postuler.Where(p => p.Job.archived == 1 && p.Inscrire.type.Equals("client") && p.Job.immediat.Equals("true") && p.approbation.Equals("0")).OrderByDescending(p => p.Job.created).ToList().Count;

            ViewBag.nbreEmployesNew = db.Postuler.Where(p => p.archived == 1 && p.Inscrire.archived == 1 && p.signatures.Equals("2") && p.situationTravail.Equals("1") && p.Inscrire.type.Equals("candidat") && p.status == 1).ToList().Count;

            _me = (Inscrire)Session["inscrire"];
            ViewData["me"] = _me;
            if (_me != null && _me.type == "client")
            {
                try
                {
                    lastAsbonnement = db.InsAbonne.Where(p => p.Ins_id == _me.id && p.archived == 1).OrderByDescending(p => p.id).First();
                    //db.InsAbonne.LastOrDefault(p => p.Ins_id == _me.id && p.archived == 1);
                    if (lastAsbonnement != null && lastAsbonnement.status == 1 && DateTime.Now > DateTime.Parse(lastAsbonnement.dateFin))
                    {
                        lastAsbonnement.status = 0;
                        db.Entry(lastAsbonnement).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }
                catch (Exception e)
                {
                    lastAsbonnement = null;
                }
            }
            ViewBag.lastAsbonnement = lastAsbonnement;

            List<Types> listTypes = db.Types.Where(p => p.archived == 1).ToList();
            List<Categorie> listCategorie = db.Categorie.Where(p => p.archived == 1).ToList();
            List<AnneeExp> listAnneeExp = db.AnneeExp.Where(p => p.archived == 1).ToList();

            ViewBag.listTypes = listTypes;
            ViewBag.listCategorie = listCategorie;
            ViewBag.listAnneeExp = listAnneeExp;

            List<NiveauScolarite> listNiveauScolarite = db.NiveauScolarite.Where(p => p.archived == 1).ToList();
            List<Diplome> listDiplome = db.Diplome.Where(p => p.archived == 1).ToList();
            List<StatutEmploi> listStatutEmploi = db.StatutEmploi.Where(p => p.archived == 1).ToList();
            List<Langue> listLangue = db.Langue.Where(p => p.archived == 1).ToList();
            List<TypeOffre> listTypeOffre = db.TypeOffre.Where(p => p.archived == 1).ToList();
            List<Contrat> listContrat = db.Contrat.Where(p => p.archived == 1).ToList();
            List<QuartTravail> listQuartTravail = db.QuartTravail.Where(p => p.archived == 1).ToList();
            List<AvantageSociaux> listAvantageSociaux = db.AvantageSociaux.Where(p => p.archived == 1).ToList();
            List<NiveauLangue> listNiveauLangue = db.NiveauLangue.Where(p => p.archived == 1).ToList();

            ViewBag.listNiveauScolarite = listNiveauScolarite;
            ViewBag.listDiplome = listDiplome;
            ViewBag.listStatutEmploi = listStatutEmploi;
            ViewBag.listLangue = listLangue;
            ViewBag.listTypeOffre = listTypeOffre;
            ViewBag.listContrat = listContrat;
            ViewBag.listQuartTravail = listQuartTravail;
            ViewBag.listAvantageSociaux = listAvantageSociaux;
            ViewBag.listNiveauLangue = listNiveauLangue;

            //retourne l'objet des informations  sur  la  compagnie niovar
            ViewBag.compagnie = db.Inscrire.Where(p => p.type.Equals("compagnie") && p.archived == 1).FirstOrDefault();

            List<Bibliotheque> listBibliotheque = db.Bibliotheque.Where(p => p.archived == 1).OrderByDescending(p => p.created).ToList();
            ViewBag.listBibliotheque = listBibliotheque;

            base.OnActionExecuting(filterContext);
        }

        protected bool needAbonnement()
        {
            if (hasIllimitedAbonnement(lastAsbonnement)) return false;
            return (lastAsbonnement == null || lastAsbonnement.status == 0 || lastAsbonnement.etat == 0);
        }

        protected bool hasIllimitedAbonnement(InsAbonne val)
        {
            return (val != null && val.Abonnement.illimite.Value == 1 && DateTime.Parse(val.dateFin)>=DateTime.Now);
        }
        protected bool isIllimitedAbonnement(Abonnement val)
        {
            return (val != null && val.illimite.Value == 1);
        }
        protected bool hasFreeAbonnement(InsAbonne val)
        {
            return (isFreeAbonnement(val.Abonnement) && DateTime.Parse(val.dateFin)>=DateTime.Now);
        }
        protected bool isFreeAbonnement(Abonnement val)
        {
            return (val != null && val.montant.Value == 0 && !isIllimitedAbonnement(val));
        }
         
        protected Dictionary<string, Object> getJson(Inscrire item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("type", item.type);
            json.Add("nom", item.nom);
            json.Add("countryCode", item.countryCode);
            json.Add("login", item.login);
            json.Add("email", item.email);
            json.Add("password", item.password);
            json.Add("phone", item.phone);
            json.Add("website", item.website);
            json.Add("description", item.description);
            json.Add("facebook", item.facebook);
            json.Add("linkedin", item.linkedin);
            json.Add("pays", item.pays);
            json.Add("province", item.province);
            json.Add("ville", item.ville);
            json.Add("adresse", item.adresse);
            json.Add("longitude", item.longitude);
            json.Add("lat", item.lat);
            json.Add("titreEmploi", item.titreEmploi);
            json.Add("anneeExperience", item.anneeExperience);
            json.Add("salaireSouhaiter", item.salaireSouhaiter);
            json.Add("salaireNegocier", item.salaireNegocier);
            json.Add("profil", MsMail.baseUrl +(!String.IsNullOrEmpty(item.profil) ? "Fichier/" + item.profil : "Assets/images/team/s1.jpg"));
            json.Add("etat", item.etat);
            json.Add("status", item.status);
            json.Add("created", item.created.HasValue ? item.created.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("created_format", item.created.HasValue ? Utils.dateFullMonth(item.created.Value) : null);
            json.Add("email_prof", item.emailProf);
            json.Add("categorie", item.categorie);
            json.Add("domaine", item.domaine);
            json.Add("code_postal", item.codePostal);
            json.Add("nom_representant", item.nomRepresentant);
            json.Add("prenom_representant", item.prenomRepresentant);
            json.Add("numero_poste", item.numeroPoste);
            json.Add("catAnneeExpAmount",  Utils.AnneeExpAmount(db, item.categorie));
            json.Add("allowNotification",  item.allowNotification);
            json.Add("allowNewsletter",  item.allowNewsletter);
            json.Add("fcmToken",  item.fcmToken);
            return json;
        }


        protected Dictionary<string, Object> getJson(Abonnement item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("type", item.type);
            json.Add("titre", item.titre);
            json.Add("description", item.description);
            json.Add("montant", item.montant);
            json.Add("amount", item.amount);
            json.Add("status", item.status);
            json.Add("nbrePost", item.nbrePost);
            json.Add("illimite", item.illimite);
            return json;
        }
        protected Dictionary<string, Object> getJson(Postuler item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("typeJob", item.typeJob);
            json.Add("Ins_id", item.Ins_id);
            json.Add("Job_id", item.Job_id);
            json.Add("libelle", item.libelle);
            json.Add("datePostule", item.datePostule.HasValue ? item.datePostule.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("heurePostule", item.heurePostule);
            json.Add("remuneration", item.remuneration);
            json.Add("remuneration2", item.Job.immediat.Equals("true") ? (Convert.ToDouble(item.Job.remunerationN) / 2) : (item.Job.remuneration / 2));
            json.Add("etatAdmin", item.etatAdmin);
            json.Add("etat", item.etat);
            json.Add("status", item.status);
            json.Add("nbreApply", item.nbreApply);
            json.Add("created", item.created.HasValue ? item.created.Value.ToString("dd/MM/yyyy HH:mm") : null); //("dddd, dd MMMM yyyy")
            json.Add("created_format", item.created.HasValue ? Utils.dateFullMonth(item.created.Value) : null);
            json.Add("etatClient", item.etatClient);
            json.Add("etatCandidat", item.etatCandidat);
            json.Add("approbation", item.approbation);
            json.Add("signatures", item.signatures);
            json.Add("situation_travail", item.situationTravail);
            json.Add("date_embauche", item.dateEmbauche.HasValue ? item.dateEmbauche.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("date_embauche_format", item.dateEmbauche.HasValue ? Utils.dateFullMonth(item.dateEmbauche.Value) : null);
            json.Add("date_signatures", item.dateSignatures.HasValue ? item.dateSignatures.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("date_signatures_format", item.dateSignatures.HasValue ? Utils.dateFullMonth(item.dateSignatures.Value) : null);
            json.Add("dateEntrevue", item.dateEntrevue.HasValue ? item.dateEntrevue.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("dateEntrevue_format", item.dateEntrevue.HasValue ? Utils.dateFullMonth(item.dateEntrevue.Value) : null);
            json.Add("heure", item.heure);
            json.Add("responsableEntrevue", item.responsableEntrevue);
            json.Add("duree", item.duree);
            json.Add("outils", item.outils);
            json.Add("signatureClient", item.signatureClient);
            json.Add("dateSignClient", item.dateSignClient.HasValue ? item.dateSignClient.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("inscrire", getJson(item.Inscrire));
            json.Add("job", getJson(item.Job));
            json.Add("image", MsMail.baseUrl + (item.Job.immediat.Equals("true") ? "Assets/images/logo2.png" : "Fichier/" + item.Inscrire.profil));
            json.Add("companyId", item.compagnyId.HasValue ? item.compagnyId.Value : 0);
            json.Add("company", getJson(Utils.getCompany(db, item.compagnyId)));
            return json;
        }
        protected Dictionary<string, Object> getJson(File item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("Ins_id", item.Ins_id);
            json.Add("type", item.type);
            json.Add("libelle", item.libelle);
            json.Add("name", item.libelle);
            json.Add("chemin", MsMail.baseUrl + "Fichier/" + item.chemin);
            json.Add("status", item.status);
            json.Add("created", item.created.HasValue ? item.created.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("created_format", item.created.HasValue ? Utils.dateFullMonth(item.created.Value) : null);
            return json;
        }
        protected Dictionary<string, Object> getJson(Job item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("Typ_id", item.Categorie.Typ_id);
            json.Add("Cat_id", item.Cat_id);
            json.Add("titre", item.titre);
            json.Add("description", item.description);
            json.Add("dateEntre", item.dateEntre);
            json.Add("dateEntre_format", !String.IsNullOrEmpty(item.dateEntre) ? Utils.dateFullMonth(item.dateEntre) : null);
            json.Add("ville", item.ville);
            json.Add("nbreEmploye", item.nbreEmploye);
            json.Add("heureTravail", item.heureTravail);
            json.Add("heureTravailJour", item.heureTravailJour);
            json.Add("jourTravail", item.jourTravail);
            json.Add("jourTravailH", item.jourTravail);
            json.Add("remuneration", item.remuneration);
            json.Add("totalHeureTravail", item.totalHeureTravail);
            json.Add("margeExperience", item.margeExperience);
            json.Add("typeJob", item.typeJob);
            json.Add("etat", item.etat);
            json.Add("status", item.status);
            json.Add("created", item.created.HasValue ? item.created.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("created_format", item.created.HasValue ? Utils.dateFullMonth(item.created.Value) : null);
            json.Add("responsabilite", item.responsabilite);
            json.Add("exigence", item.exigence);
            json.Add("autre", item.autre);
            json.Add("pays", item.pays);
            json.Add("immediat", item.immediat.Equals("true") ? 1 : 0);
            json.Add("province", item.province);
            json.Add("remuneration_n", item.remuneration);
            json.Add("country_code", item.countryCode);
            json.Add("niveauEtude", item.niveauEtude);
            json.Add("category", getJson(item.Categorie));
            json.Add("contrat", getJson(item.Contrat));
            json.Add("langue", getJson(item.Langue));
            json.Add("niveauScolarite", getJson(item.NiveauScolarite));
            json.Add("quartTravail1", getJson(item.QuartTravail1));
            json.Add("statutEmploi", getJson(item.StatutEmploi));
            json.Add("TypeOffre", getJson(item.TypeOffre));
            json.Add("codePostal", item.codePostal);
            json.Add("adressePostal", item.adressePostal);
            json.Add("masquerEmplacement", item.masquerEmplacement);
            json.Add("quartTravail", item.quartTravail);
            json.Add("DateEntreValeur", item.DateEntreValeur);
            json.Add("dateDebut", item.dateDebut.HasValue ? item.dateDebut.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("dateDebut_format", item.dateDebut.HasValue ? Utils.dateFullMonth(item.dateDebut.Value) : null);
            json.Add("dateFin", item.dateFin.HasValue ? item.dateFin.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("dateFin_format", item.dateFin.HasValue ? Utils.dateFullMonth(item.dateFin.Value) : null);
            json.Add("niveauOrale", item.niveauOrale);
            json.Add("niveauEcrire", item.niveauEcrire);
            json.Add("DiplomeJob", getJson(item.DiplomeJob));
            json.Add("AvantageSociauxJob", getJson(item.AvantageSociauxJob));
            return json;
        }

        protected Dictionary<string, Object> getJson(Location item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("Ins_id", item.Ins_id);
            json.Add("Ins_id2", item.Ins_id2);
            json.Add("periode", item.periode);
            json.Add("dateDebut", item.dateDebut.HasValue ? item.dateDebut.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("dateDebut_format", item.dateDebut.HasValue ? Utils.dateFullMonth(item.dateDebut.Value) : null);
            json.Add("dateFin", item.dateFin.HasValue ? item.dateFin.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("dateFin_format", item.dateFin.HasValue ? Utils.dateFullMonth(item.dateFin.Value) : null);
            json.Add("periode", item.periode);
            json.Add("heureTravail", item.heureTravail);
            json.Add("description", item.description);
            json.Add("montant", item.montant.HasValue ? item.montant.Value : 0);
            json.Add("signClient", item.signClient.HasValue ? item.signClient.Value : 0);
            json.Add("signCandidat", item.signCandidat.HasValue ? item.signCandidat.Value : 0);
            json.Add("remuneration", item.remuneration.HasValue ? item.remuneration.Value : 0);
            json.Add("dateSgnClt", item.dateSgnClt.HasValue ? item.dateSgnClt.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("dateSgnClt_format", item.dateSgnClt.HasValue ? Utils.dateFullMonth(item.dateSgnClt.Value) : null);
            json.Add("dateSgnCdt", item.dateSgnCdt.HasValue ? item.dateSgnCdt.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("dateSgnCdt_format", item.dateSgnCdt.HasValue ? Utils.dateFullMonth(item.dateSgnCdt.Value) : null);
            json.Add("avisClient", item.avisClient.HasValue ? item.avisClient.Value : 0);
            json.Add("avisCandidat", item.avisCandidat.HasValue ? item.avisCandidat.Value : 0);
            json.Add("status", item.status.HasValue ? item.status.Value : 0);
            json.Add("etat", item.etat.HasValue ? item.etat.Value : 0);
            json.Add("created", item.created.HasValue ? item.created.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("created_format", item.created.HasValue ? Utils.dateFullMonth(item.created.Value) : null);
            json.Add("candidat", getJson(item.Inscrire));
            json.Add("company", getJson(Utils.getCompany(db, Convert.ToInt32(item.Ins_id2))));
            return json;
        }
        protected Dictionary<string, Object> getJson(Categorie item, bool relation = true)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("Typ_id", item.Typ_id);
            json.Add("libelle", item.libelle);
            json.Add("name", item.libelle);
            json.Add("image", MsMail.baseUrl + "Fichier/" + item.image);
            json.Add("status", item.status);
            if(relation) json.Add("type", getJson(item.Types));
            return json;
        }
        protected Dictionary<string, Object> getJson(Types item, bool count = false)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return json;
            json.Add("id", item.id);
            json.Add("libelle", item.libelle);
            json.Add("name", item.libelle);
            json.Add("image", MsMail.baseUrl + "Fichier/" + item.image);
            json.Add("created", item.created.HasValue ? item.created.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("created_format", item.created.HasValue ? Utils.dateFullMonth(item.created.Value) : null);
            json.Add("status", item.status);
            if(count) json.Add("count", db.Postuler.Where(p => p.Job.archived == 1 && p.Inscrire.type == "client" && p.Job.Categorie.Typ_id == item.id).Count());
            return json;
        }
        protected Dictionary<string, Object> getJson(CatAnneeExp item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("Cat_id", item.Cat_id);
            json.Add("Ann_id", item.Ann_id);
            json.Add("status", item.status);
            json.Add("prixHoraire", item.prixHoraire);
            json.Add("anneeExp", getJson(item.AnneeExp));
            json.Add("category", getJson(item.Categorie));
            json.Add("created", item.created.HasValue ? item.created.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("created_format", item.created.HasValue ? Utils.dateFullMonth(item.created.Value) : null);
            return json;
        }
        protected Dictionary<string, Object> getJson(AnneeExp item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("libelle", item.libelle);
            json.Add("name", item.libelle);
            json.Add("status", item.status);
            json.Add("created", item.created.HasValue ? item.created.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("created_format", item.created.HasValue ? Utils.dateFullMonth(item.created.Value) : null);
            return json;
        }
        protected Dictionary<string, Object> getJson(InsAbonne item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("Ins_id", item.Ins_id);
            json.Add("Abo_id", item.Abo_id);
            json.Add("libelle", item.libelle);
            json.Add("name", item.libelle);
            json.Add("dateDebut", item.dateDebut);
            json.Add("dateDebut_format", Utils.dateFullMonth(item.dateDebut));
            json.Add("dateFin", item.dateFin);
            json.Add("dateFin_format", Utils.dateFullMonth(item.dateFin));
            json.Add("etat", item.etat);
            json.Add("status", item.status);
            json.Add("created", item.created.HasValue ? item.created.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("created_format", item.created.HasValue ? Utils.dateFullMonth(item.created.Value) : null);
            json.Add("nbrePost", item.nbrePost);
            json.Add("type", getJson(item.Abonnement));
            return json;
        }
        protected Dictionary<string, Object> getJson(Paiement item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("Job_id", item.Job_id);
            json.Add("user_id", item.userId);
            json.Add("libelle", item.libelle);
            json.Add("name", item.libelle);
            json.Add("montant", item.montant);
            json.Add("avance", item.avance);
            json.Add("reste", item.reste);
            json.Add("etat", item.etat);
            json.Add("status", item.status);
            json.Add("created", item.created.HasValue ? item.created.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("created_format", item.created.HasValue ? Utils.dateFullMonth(item.created.Value) : null);
            json.Add("type", item.type);
            json.Add("postuler", getJson(item.Postuler));
            return json;
        }
        protected Dictionary<string, Object> getJson(Autre item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("Ins_id", item.Ins_id);
            json.Add("etablissement", item.etablissement);
            json.Add("fonction", item.fonction);
            json.Add("periode", item.periode);
            json.Add("description", item.description);
            json.Add("status", item.status);
            return json;
        }
        protected Dictionary<string, Object> getJson(Document item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("Ins_id", item.Ins_id);
            json.Add("Job_id", item.Job_id);
            json.Add("Pos_id", item.Pos_id);
            json.Add("type", item.type);
            json.Add("libelle", item.libelle);
            json.Add("name", item.libelle);
            json.Add("chemin", item.chemin);
            json.Add("etat", item.etat);
            json.Add("status", item.status);
            json.Add("created", item.created.HasValue ? item.created.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("created_format", item.created.HasValue ? Utils.dateFullMonth(item.created.Value) : null);
            return json;
        }
        protected Dictionary<string, Object> getJson(FraisLocation item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("Ins_id", item.Ins_id);
            json.Add("Ins_id2", item.Ins_id2);
            json.Add("libelle", item.libelle);
            json.Add("name", item.libelle);
            json.Add("montant", item.montant);
            json.Add("avance", item.avance);
            json.Add("reste", item.reste);
            json.Add("type", item.type);
            json.Add("etat", item.etat);
            json.Add("status", item.status);
            json.Add("userId", item.userId);
            json.Add("created", item.created.HasValue ? item.created.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("created_format", item.created.HasValue ? Utils.dateFullMonth(item.created.Value) : null);
            json.Add("location", getJson(item.Location));
            return json;
        }
        protected Dictionary<string, Object> getJson(Education item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("Ins_id", item.Ins_id);
            json.Add("etablissement", item.etablissement);
            json.Add("diplome", item.diplome);
            json.Add("periode", item.periode);
            json.Add("description", item.description);
            //json.Add("status", item.status);
            return json;
        }
        protected Dictionary<string, Object> getJson(Experience item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("Ins_id", item.Ins_id);
            json.Add("etablissement", item.etablissement);
            json.Add("fonction", item.fonction);
            json.Add("periode", item.periode);
            json.Add("description", item.description);
            //json.Add("status", item.status);
            return json;
        }
        protected Dictionary<string, Object> getJson(Information item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("Ins_id", item.Ins_id);
            json.Add("lettre", item.lettre);
            json.Add("competence", item.competence);
            //json.Add("status", item.status);
            return json;
        }
        protected Dictionary<string, Object> getJson(NiveauScolarite item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("libelle", item.libelle);
            json.Add("name", item.libelle);
            //json.Add("status", item.status);
            return json;
        }
        protected Dictionary<string, Object> getJson(Diplome item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("libelle", item.libelle);
            json.Add("name", item.libelle);
            //json.Add("status", item.status);
            return json;
        }
        protected Dictionary<string, Object> getJson(StatutEmploi item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("libelle", item.libelle);
            json.Add("name", item.libelle);
            //json.Add("status", item.status);
            return json;
        }
        protected Dictionary<string, Object> getJson(Langue item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("libelle", item.libelle);
            json.Add("name", item.libelle);
            //json.Add("status", item.status);
            return json;
        }
        protected Dictionary<string, Object> getJson(NiveauLangue item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("libelle", item.libelle);
            json.Add("name", item.libelle);
            //json.Add("status", item.status);
            return json;
        }
        protected Dictionary<string, Object> getJson(TypeOffre item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("libelle", item.libelle);
            json.Add("name", item.libelle);
            //json.Add("status", item.status);
            return json;
        }
        protected Dictionary<string, Object> getJson(Contrat item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("libelle", item.libelle);
            json.Add("name", item.libelle);
            //json.Add("status", item.status);
            return json;
        }
        protected Dictionary<string, Object> getJson(ICollection<DiplomeJob> list)
        { 
            var json = new Dictionary<string, Object>(); 
            if (list == null || list.Count == 0) return null;
            foreach(var item in list)
            { 
                json.Add(item.Dip_id.ToString(), item.Diplome.libelle);
            } 
            return json;
        }
        protected Dictionary<string, Object> getJson(ICollection<AvantageSociauxJob> list)
        {
            var json = new Dictionary<string, Object>();
            if (list == null || list.Count == 0) return null;
            foreach (var item in list)
            {
                json.Add(item.Ava_id.ToString(), item.AvantageSociaux.libelle);
            }
            return json;
        } 
        protected Dictionary<string, Object> getJson(QuartTravail item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("libelle", item.libelle);
            json.Add("name", item.libelle);
            json.Add("description", item.description);
            //json.Add("status", item.status);
            return json;
        }
        protected Dictionary<string, Object> getJson(AvantageSociaux item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("libelle", item.libelle);
            json.Add("name", item.libelle);
            json.Add("description", item.description);
            json.Add("image", MsMail.baseUrl + "Fichier/" + item.image);
            //json.Add("status", item.status);
            return json;
        }

        protected Dictionary<string, Object> getJson(Page item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("aPropos", item.aPropos);
            json.Add("couleurFond", item.couleurFond);
            json.Add("description", item.description);
            json.Add("nomPage", item.nomPage);
            json.Add("pourquoiPostuler", item.pourquoiPostuler);
            json.Add("profil", MsMail.baseUrl + "Fichier/" + item.profil);
            json.Add("profilPage", MsMail.baseUrl + "Fichier/" + item.profilPage);  
            json.Add("company", getJson(item.Inscrire)); 
            return json;
        }
        protected Dictionary<string, Object> getJson(Galerie item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("libelle", item.libelle);
            json.Add("Pag_id", item.Pag_id);
            json.Add("image", MsMail.baseUrl + "Fichier/" + item.image); 
            return json;
        }
        protected Dictionary<string, Object> getJson(Avis item)
        {
            var json = new Dictionary<string, Object>();
            if (item == null) return null;
            json.Add("id", item.id);
            json.Add("libelle", item.libelle);
            json.Add("iduser", item.iduser);
            json.Add("Pag_id", item.Pag_id);
            json.Add("nbreEtoile", item.nbreEtoile);
            json.Add("Profil", MsMail.baseUrl + "Fichier/" + item.Profil);
            json.Add("Pseudo", item.Pseudo);
            json.Add("created", item.created.HasValue ? item.created.Value.ToString("dd/MM/yyyy HH:mm") : null);
            json.Add("created_format", item.created.HasValue ? Utils.dateFullMonth(item.created.Value) : null);
            return json;
        }


        protected Message CreateNotification(string title, string notificationBody, string token)
        {
            return new Message()
            {
                Token = token,
                Notification = new Notification()
                {
                    Body = notificationBody,
                    Title = title
                }
            };
        }

        public async Task<String> SendNotification(string token, string title, string body)
        {
            FirebaseMessaging messaging;
            var app = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(AppDomain.CurrentDomain.BaseDirectory + "/Json/niovarjobsnotifications-firebase-adminsdk.json").CreateScoped("https://www.googleapis.com/auth/firebase.messaging")
            });
            messaging = FirebaseMessaging.GetMessaging(app);
            return await messaging.SendAsync(CreateNotification(title, body, token));
            //do something with result
        }
    }
}
