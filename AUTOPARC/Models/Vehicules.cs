using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class Vehicules
    {
        public Vehicules()
        {
            Docs = new HashSet<Docs>();
            HistoriqueChauffeurVehicule = new HashSet<HistoriqueChauffeurVehicule>();
            Maintenances = new HashSet<Maintenances>();
            RechargeCarburants = new HashSet<RechargeCarburants>();
            Ventes = new HashSet<Ventes>();
        }

        public int Id { get; set; }
        public string Matricule { get; set; }
        public int CategorieId { get; set; }
        public int MarqueId { get; set; }
        public int ModeleId { get; set; }
        public int CarburantId { get; set; }
        public int EtatVehiculeId { get; set; }
        public int FrsId { get; set; }
        public decimal PrixAchat { get; set; }
        public decimal MontantPayee { get; set; }
        public int MethodePayementId { get; set; }
        public DateTime DateAchat { get; set; }
        public DateTime? DateMisEnCirculation { get; set; }
        public string Note { get; set; }
        public string Moteur { get; set; }
        public string Architecture { get; set; }
        public string PuissanceFiscale { get; set; }
        public string PuissanceMax { get; set; }
        public string BoiteAVitesse { get; set; }
        public string ConsomationMixte { get; set; }
        public string VitesseMax { get; set; }
        public string NbrPlace { get; set; }
        public string Poids { get; set; }
        public string Longueur { get; set; }
        public string Largeur { get; set; }
        public string Hauteur { get; set; }
        public string VolumeCoffre { get; set; }

        public virtual TypeCarburants Carburant { get; set; }
        public virtual Categories Categorie { get; set; }
        public virtual EtatVehicules EtatVehicule { get; set; }
        public virtual Fournisseurs Frs { get; set; }
        public virtual Marques Marque { get; set; }
        public virtual MethodePayements MethodePayement { get; set; }
        public virtual Modeles Modele { get; set; }
        public virtual ICollection<Docs> Docs { get; set; }
        public virtual ICollection<HistoriqueChauffeurVehicule> HistoriqueChauffeurVehicule { get; set; }
        public virtual ICollection<Maintenances> Maintenances { get; set; }
        public virtual ICollection<RechargeCarburants> RechargeCarburants { get; set; }
        public virtual ICollection<Ventes> Ventes { get; set; }
    }
}
