using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class Cessions
    {
        public int Id { get; set; }
        public int VehiculeId { get; set; }
        public DateTime DateCession { get; set; }
        public decimal PrixCession { get; set; }
        public decimal MontantRecu { get; set; }
        public int ModePaimentId { get; set; }
        public string NomAcheteur { get; set; }
        public string ContactAcheteur { get; set; }

        public virtual ModePaiments ModePaiment { get; set; }
        public virtual Vehicules Vehicule { get; set; }
    }
}
