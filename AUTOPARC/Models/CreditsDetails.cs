using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class CreditsDetails
    {
        public int Id { get; set; }
        public int CreditId { get; set; }
        public DateTime DateReglement { get; set; }
        public decimal MensualitePayeeEspece { get; set; }
        public decimal MensualitePayeeCheque { get; set; }
        public decimal MensualitePayeeVirement { get; set; }
    }
}
