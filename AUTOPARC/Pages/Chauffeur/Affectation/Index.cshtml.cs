using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Chauffeur.Affectation
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        public List<AffectationChauffeurVehicules> AffectationChauffeurVehicules { get; set; }
        public List<Vehicules> Vehicules { get; set; }
        public List<Chauffeurs> Chauffeurs { get; set; }




        public async Task OnGet()
        {
            AffectationChauffeurVehicules = await _db.AffectationChauffeurVehicules.ToListAsync();
            Vehicules = await _db.Vehicules.ToListAsync();
            Chauffeurs = await _db.Chauffeurs.ToListAsync();
        }
    }
}