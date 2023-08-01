using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class Chauffeurs
    {
        public Chauffeurs()
        {
            Maintenances = new HashSet<Maintenances>();
            RechargeCarburants = new HashSet<RechargeCarburants>();
        }

        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Cin { get; set; }
        public DateTime? DateNaissance { get; set; }
        public string Portable { get; set; }
        public string Email { get; set; }
        public string Adresse { get; set; }
        public string NumeroPermisConduire { get; set; }
        public DateTime? DateExpirationPermis { get; set; }
        public string Disponibilite { get; set; }
        public DateTime DateEmbauche { get; set; }
        public string Remarques { get; set; }

        public virtual AffectationChauffeurVehicules AffectationChauffeurVehicules { get; set; }
        public virtual ICollection<Maintenances> Maintenances { get; set; }
        public virtual ICollection<RechargeCarburants> RechargeCarburants { get; set; }
    }
}
