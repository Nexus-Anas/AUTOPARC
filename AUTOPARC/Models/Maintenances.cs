﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class Maintenances
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int VehiculeId { get; set; }
        public virtual Vehicules Vehicule { get; set; }
        public DateTime DateMaintenance { get; set; }
        public decimal Cout { get; set; }
        public decimal MontantPayee { get; set; }
        public int MethodePayementId { get; set; }
        public string Description { get; set; }
        public string UrlDoc { get; set; }

        public virtual MethodePayements MethodePayement { get; set; }
        public virtual TypeMaintenances Type { get; set; }
       
    }
}
