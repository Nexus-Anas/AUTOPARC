using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class Suividepense
    {
        public int Id { get; set; }
        public int NumDepense { get; set; }
        public DateTime DateDepense { get; set; }
        public string Objet { get; set; }
        public decimal Montant { get; set; }
        public int ModePayementId { get; set; }

        public virtual ModePaiments ModePayement { get; set; }
    }
}
