using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class Modeles
    {
        public Modeles()
        {
            Vehicules = new HashSet<Vehicules>();
        }

        public int Id { get; set; }
        public string Nom { get; set; }
        public int MarqueId { get; set; }

        public virtual Marques Marque { get; set; }
        public virtual ICollection<Vehicules> Vehicules { get; set; }
    }
}
