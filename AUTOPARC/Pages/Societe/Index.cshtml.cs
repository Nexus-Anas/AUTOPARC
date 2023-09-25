using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Societe
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        public List<Societes> SocietesList { get; set; }
        public List<Villes> VillesList { get; set; }




        public async Task OnGet()
        {
            SocietesList = await _db.Societes.ToListAsync();
            VillesList = await _db.Villes.ToListAsync();
        }
    }
}
