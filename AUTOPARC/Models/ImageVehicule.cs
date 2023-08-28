﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class ImageVehicule
    {
        public int Id { get; set; }
        public int VehiculeNum { get; set; }
        public string Url { get; set; }
    }
}
