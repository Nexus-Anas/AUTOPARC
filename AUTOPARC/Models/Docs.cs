using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class Docs
    {
        public int Id { get; set; }
        public int Num { get; set; }
        public string Numero { get; set; }
        public int TypeId { get; set; }
        public int VehiculeId { get; set; }
        public int FrsId { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public decimal PrixAchat { get; set; }
        public decimal MontantPayeeEspece { get; set; }
        public decimal MontantPayeeCheque { get; set; }
        public decimal MontantPayeeVirement { get; set; }
        public decimal MontantPayeeCredit { get; set; }
        public decimal MontantPayeeTotal { get; set; }

        public virtual Fournisseurs Frs { get; set; }
        public virtual TypeDocs Type { get; set; }
        public virtual Vehicules Vehicule { get; set; }
    }
}
