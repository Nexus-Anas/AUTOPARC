using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule.EtatVehicule
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public EtatVehicules EtatVehicules { get; set; }

        public bool check_presence_etat;




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


        public async Task<IActionResult> OnPostDelete()
        {
            var etat = await _db.EtatVehicules.FindAsync(EtatVehicules.Id);

            if (etat is null)
                return NotFound();

            var vehicule = await _db.Vehicules.Where(x => x.EtatVehiculeId == etat.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (vehicule != 0)
            {
                check_presence_etat = true;
                await OnGet(EtatVehicules.Id);
                return Page();
            }

            _db.EtatVehicules.Remove(etat);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/EtatVehicule/Index");
        }
    }
}
