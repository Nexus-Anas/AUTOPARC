using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vente
{
    public class CreateModel : PageModel
    {
        private readonly DBC _db;
        public CreateModel(DBC db) => _db = db;




        [BindProperty]
        public Ventes Ventes { get; set; }
        public List<Vehicules> Vehicules { get; set; }
        public List<MethodePayements> MethodePayements { get; set; }




        public async Task OnGet()
        {
            Vehicules = await _db.Vehicules.ToListAsync();
            MethodePayements = await _db.MethodePayements.ToListAsync();
        }




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            var matricule = await _db.Vehicules.Where(x => x.Id == Ventes.VehiculeId).Select(x => x.Matricule).FirstOrDefaultAsync();
            var vehicule = _db.Vehicules.Where(x => x.Matricule == matricule).FirstOrDefault();
            var etatVehicule = await _db.EtatVehicules.Where(x => x.Etat == "Vendu").Select(x => x.Id).FirstOrDefaultAsync();
            vehicule.EtatVehiculeId = etatVehicule;
            await _db.Ventes.AddAsync(Ventes);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vente/Index");
        }
    }
}
