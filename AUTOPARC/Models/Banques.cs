using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class Banques
    {
        public Banques()
        {
            Cheques = new HashSet<Cheques>();
            Virements = new HashSet<Virements>();
        }

        public int Id { get; set; }
        public string Nom { get; set; }

        public virtual ICollection<Cheques> Cheques { get; set; }
        public virtual ICollection<Virements> Virements { get; set; }
    }
}
