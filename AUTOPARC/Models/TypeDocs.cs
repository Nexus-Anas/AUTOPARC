using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AUTOPARC.Models
{
    public partial class TypeDocs
    {
        public TypeDocs()
        {
            Docs = new HashSet<Docs>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Docs> Docs { get; set; }
    }
}
