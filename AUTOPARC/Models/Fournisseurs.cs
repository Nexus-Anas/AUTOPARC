using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class Fournisseurs
    {
        public Fournisseurs()
        {
            Docs = new HashSet<Docs>();
            Vehicules = new HashSet<Vehicules>();
        }

        public int Id { get; set; }
        public string Nom { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Portable { get; set; }
        public string Adresse { get; set; }
        public int TypeFrsId { get; set; }
        public int? VilleId { get; set; }

        public virtual TypeFournisseurs TypeFrs { get; set; }
        public virtual Villes Ville { get; set; }
        public virtual ICollection<Docs> Docs { get; set; }
        public virtual ICollection<Vehicules> Vehicules { get; set; }
    }
}
