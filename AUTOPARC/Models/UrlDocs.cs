using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class UrlDocs
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Action { get; set; }
        public int ActionNum { get; set; }
    }
}
