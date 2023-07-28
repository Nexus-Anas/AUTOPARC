using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class RechargeCarburants
    {
        public int Id { get; set; }
        public int VehiculeId { get; set; }
        public decimal Quantite { get; set; }
        public decimal Pu { get; set; }
        public int MethodePayementId { get; set; }
        public decimal? Km { get; set; }
        public DateTime DateRecharge { get; set; }
        public string Note { get; set; }

        public virtual MethodePayements MethodePayement { get; set; }
        public virtual Vehicules Vehicule { get; set; }
    }
}
