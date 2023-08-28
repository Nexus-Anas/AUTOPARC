using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class Credits
    {
        public int Id { get; set; }
        public int Num { get; set; }
        public int? BanqueId { get; set; }
        public decimal Montant { get; set; }
        public decimal Mensualite { get; set; }
        public decimal MontantPayeeTotal { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public string Note { get; set; }
        public string Etat { get; set; }
        public string Action { get; set; }
        public int ActionNum { get; set; }

        public virtual Banques Banque { get; set; }
    }
}
