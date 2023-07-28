using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Chauffeur.Historique
{
    public class CreateModel : PageModel
    {
        private readonly DBC _db;
        public CreateModel(DBC db) => _db = db;




        [BindProperty]
        public HistoriqueChauffeurVehicule HistoriqueChauffeurVehicule { get; set; }
        public List<Vehicules> Vehicules { get; set; }
        public List<Chauffeurs> Chauffeurs { get; set; }




        public async Task OnGet()
        {
            Vehicules = await _db.Vehicules.ToListAsync();
            Chauffeurs = await _db.Chauffeurs.ToListAsync();
        }




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
            {
                await OnGet();
                return Page();
            }

            await _db.HistoriqueChauffeurVehicule.AddAsync(HistoriqueChauffeurVehicule);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Chauffeur/Historique/Index");
        }
    }
}
