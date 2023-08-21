using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Parametrage.ModePaiment
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public ModePaiments ModePaiments { get; set; }

        public bool check_presence_mode;




        public async Task OnGet(int id)
            => ModePaiments = await _db.ModePaiments.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (string.IsNullOrEmpty(ModePaiments.Mode) || !ModelState.IsValid)
            {
                ModelState.AddModelError("ModePaiments.Mode", "Le champ \"Mode Paiement\" est requis.");
                return Page();
            }

            var mode = await _db.ModePaiments.FindAsync(ModePaiments.Id);
            mode.Mode = ModePaiments.Mode;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/ModePaiment/Index");
        }




        public async Task<IActionResult> OnPostDelete()
        {
            var mode = await _db.ModePaiments.FindAsync(ModePaiments.Id);

            if (mode is null)
                return NotFound();

            var vehicule = await _db.Vehicules.Where(x => x.Id == mode.Id).Select(x => x.Id).FirstOrDefaultAsync();
            var recharge_carburon = await _db.RechargeCarburants.Where(x => x.ModePaimentId == mode.Id).Select(x => x.Id).FirstOrDefaultAsync();
            var vente = await _db.Cessions.Where(x => x.ModePaimentId == mode.Id).Select(x => x.Id).FirstOrDefaultAsync();
            //var maintenance = await _db.Maintenances.Where(x => x.ModePaiementId == mode.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (vehicule != 0 && recharge_carburon != 0 && vehicule != 0)
            {
                check_presence_mode = true;
                return Page();
            }

            _db.ModePaiments.Remove(mode);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/ModePaiment/Index");
        }
    }
}
