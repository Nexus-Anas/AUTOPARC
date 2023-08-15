using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class Virements
    {
        public int Id { get; set; }
        public int? BanqueId { get; set; }
        public string Rib { get; set; }
        public decimal Montant { get; set; }
        public string AuNomDe { get; set; }
        public DateTime DateVirement { get; set; }
        public string Etat { get; set; }
        public string Action { get; set; }
        public int ActionNum { get; set; }

        public virtual Banques Banque { get; set; }
    }
}
