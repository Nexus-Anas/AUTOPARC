using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class Societes
    {
        public Societes()
        {
            Suividepense = new HashSet<Suividepense>();
            Vehicules = new HashSet<Vehicules>();
        }

        public int Id { get; set; }
        public string RaisonSocial { get; set; }
        public int? VilleId { get; set; }
        public string Adresse { get; set; }
        public string Bas1 { get; set; }
        public string Bas2 { get; set; }
        public string LogoUrl { get; set; }
        public bool SocieteParDefault { get; set; }

        public virtual Villes Ville { get; set; }
        public virtual ICollection<Suividepense> Suividepense { get; set; }
        public virtual ICollection<Vehicules> Vehicules { get; set; }
    }
}
