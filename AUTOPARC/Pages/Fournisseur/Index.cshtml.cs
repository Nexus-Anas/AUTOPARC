using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Fournisseur
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        public List<Fournisseurs> Fournisseurs { get; set; }
        public List<TypeFournisseurs> TypeFournisseurs { get; set; }
        public List<Villes> Villes { get; set; }




        public async Task OnGet()
        {
            Villes = await _db.Villes.ToListAsync();
            Fournisseurs = await _db.Fournisseurs.ToListAsync();
            TypeFournisseurs = await _db.TypeFournisseurs.ToListAsync();
        }
    }
}
