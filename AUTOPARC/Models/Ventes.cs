using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class Ventes
    {
        public int Id { get; set; }
        public int VehiculeId { get; set; }
        public DateTime? DateVente { get; set; }
        public decimal PrixVente { get; set; }
        public decimal MontantRecu { get; set; }
        public int MethodePayementId { get; set; }
        public string NomAcheteur { get; set; }
        public string ContactAcheteur { get; set; }

        public virtual MethodePayements MethodePayement { get; set; }
        public virtual Vehicules Vehicule { get; set; }
    }
}
