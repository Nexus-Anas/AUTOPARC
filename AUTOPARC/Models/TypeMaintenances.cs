using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class TypeMaintenances
    {
        public TypeMaintenances()
        {
            Maintenances = new HashSet<Maintenances>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Maintenances> Maintenances { get; set; }
    }
}
