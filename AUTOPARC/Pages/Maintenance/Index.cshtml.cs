using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Maintenance
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        public List<Maintenances> Maintenances { get; set; }
        public List<TypeMaintenances> TypeMaintenances { get; set; }
        public List<Vehicules> Vehicules { get; set; }
        public List<Chauffeurs> Chauffeurs { get; set; }
        public List<ModePaiments> ModePaiments { get; set; }




        public async Task OnGet()
        {
            Maintenances = await _db.Maintenances.ToListAsync();
            TypeMaintenances = await _db.TypeMaintenances.ToListAsync();
            Vehicules = await _db.Vehicules.ToListAsync();
            ModePaiments = await _db.ModePaiments.ToListAsync();
            Chauffeurs = await _db.Chauffeurs.ToListAsync();
        }
    }
}
