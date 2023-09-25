using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Document
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

        public bool error;




        public async Task OnGet()
        {
            VehiculesList = await _db.Vehicules.ToListAsync();
            MarquesList = await _db.Marques.ToListAsync();
            ModelesList = await _db.Modeles.ToListAsync();
        }




        public async Task<IActionResult> OnPostSelect()
        {
            var affectation = await _db.AffectationChauffeurVehicules.Where(a => a.VehiculeId == Vehicules.Id).FirstOrDefaultAsync();
            if (affectation == null)
            {
                error = true;
                await OnGet();
                return Page();
            }
            return RedirectToPage("/Document/Create", new { Vehicules.Id});
        }
    }
}
