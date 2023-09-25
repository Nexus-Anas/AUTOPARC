using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class Maintenances
    {
        public int Id { get; set; }
        public int Num { get; set; }
        public int TypeId { get; set; }
        public int OperationId { get; set; }
        public int VehiculeId { get; set; }
        public int ChauffeurId { get; set; }
        public int FrsId { get; set; }
        public DateTime DateMaintenance { get; set; }
        public decimal Cout { get; set; }
        public decimal MontantPayeeEspece { get; set; }
        public decimal MontantPayeeCheque { get; set; }
        public decimal MontantPayeeVirement { get; set; }
        public decimal MontantPayeeCredit { get; set; }
        public decimal MontantPayeeTotal { get; set; }
        public string Description { get; set; }

        public virtual Chauffeurs Chauffeur { get; set; }
        public virtual Fournisseurs Frs { get; set; }
        public virtual OperationMaintenances Operation { get; set; }
        public virtual TypeMaintenances Type { get; set; }
        public virtual Vehicules Vehicule { get; set; }
    }
}
