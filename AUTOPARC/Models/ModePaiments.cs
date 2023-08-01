using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class ModePaiments
    {
        public ModePaiments()
        {
            Cessions = new HashSet<Cessions>();
            RechargeCarburants = new HashSet<RechargeCarburants>();
            Vehicules = new HashSet<Vehicules>();
        }

        public int Id { get; set; }
        public string Mode { get; set; }

        public virtual ICollection<Cessions> Cessions { get; set; }
        public virtual ICollection<RechargeCarburants> RechargeCarburants { get; set; }
        public virtual ICollection<Vehicules> Vehicules { get; set; }
    }
}
