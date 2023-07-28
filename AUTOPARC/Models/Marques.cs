using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class Marques
    {
        public Marques()
        {
            Modeles = new HashSet<Modeles>();
            Vehicules = new HashSet<Vehicules>();
        }

        public int Id { get; set; }
        public string Marque { get; set; }

        public virtual ICollection<Modeles> Modeles { get; set; }
        public virtual ICollection<Vehicules> Vehicules { get; set; }
    }
}
