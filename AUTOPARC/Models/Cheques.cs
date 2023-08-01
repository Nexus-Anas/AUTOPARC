using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class Cheques
    {
        public int Id { get; set; }
        public int BanqueId { get; set; }
        public string Numero { get; set; }
        public decimal Montant { get; set; }
        public string AuNomDe { get; set; }
        public DateTime DateReglement { get; set; }
        public DateTime DateEcheance { get; set; }
        public DateTime? DateValeur { get; set; }

        public virtual Banques Banque { get; set; }
    }
}
