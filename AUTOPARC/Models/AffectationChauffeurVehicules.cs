using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class AffectationChauffeurVehicules
    {
        public int Id { get; set; }
        public int ChauffeurId { get; set; }
        public int VehiculeId { get; set; }
        public DateTime DateDebutAffectation { get; set; }
        public DateTime DateFinAffectation { get; set; }
        public string RaisonFinAffectation { get; set; }

        public virtual Chauffeurs Chauffeur { get; set; }
        public virtual Vehicules Vehicule { get; set; }
    }
}
