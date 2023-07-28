using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vente
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        public List<Ventes> Ventes { get; set; }
        public List<Vehicules> Vehicules { get; set; }
        public List<MethodePayements> MethodePayements { get; set; }




        public async Task OnGet()
        {
            Ventes = await _db.Ventes.ToListAsync();
            Vehicules = await _db.Vehicules.ToListAsync();
            MethodePayements = await _db.MethodePayements.ToListAsync();
        }
    }
}
