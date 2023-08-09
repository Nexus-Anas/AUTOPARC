using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Maintenance
{
    public class SelectionVehiculeModel : PageModel
    {
        private readonly DBC _db;
        public SelectionVehiculeModel(DBC db) => _db = db;



        [BindProperty]
        public Vehicules Vehicules { get; set; }
        public List<Vehicules> VehiculesList { get; set; }
        public List<Marques> MarquesList { get; set; }
        public List<Modeles> ModelesList { get; set; }




        public async Task OnGet()
        {
            VehiculesList = await _db.Vehicules.ToListAsync();
            MarquesList = await _db.Marques.ToListAsync();
            ModelesList = await _db.Modeles.ToListAsync();
        }




        public async Task<IActionResult> OnPostSelect()
        {
            if (Vehicules.Id == 0)
            {
                ModelState.AddModelError("Vehicules.Matricule", "Veuillez sélectionner une vehicule.");
                await OnGet();
                return Page();
            }
            return RedirectToPage("/Maintenance/Create", new { Vehicules.Id});
        }
    }
}
