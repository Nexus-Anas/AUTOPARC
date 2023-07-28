using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule.EtatVehicule
{
    public class UpdateModel : PageModel
    {
        private readonly DBC _db;
        public UpdateModel(DBC db) => _db = db;




        [BindProperty]
        public EtatVehicules EtatVehicules { get; set; }




        public async Task OnGet(int id)
            => EtatVehicules = await _db.EtatVehicules.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var etat = await _db.EtatVehicules.FindAsync(EtatVehicules.Id);
            etat.Etat = EtatVehicules.Etat;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/EtatVehicule/Index");
        }
    }
}
