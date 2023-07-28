using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class MethodePayements
    {
        public MethodePayements()
        {
            Maintenances = new HashSet<Maintenances>();
            RechargeCarburants = new HashSet<RechargeCarburants>();
            Vehicules = new HashSet<Vehicules>();
            Ventes = new HashSet<Ventes>();
        }

        public int Id { get; set; }
        public string Methode { get; set; }

        public virtual ICollection<Maintenances> Maintenances { get; set; }
        public virtual ICollection<RechargeCarburants> RechargeCarburants { get; set; }
        public virtual ICollection<Vehicules> Vehicules { get; set; }
        public virtual ICollection<Ventes> Ventes { get; set; }
    }
}
