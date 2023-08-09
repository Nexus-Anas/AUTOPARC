using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Parametrage.ModePaiment
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        [BindProperty]
        public ModePaiments ModePaiments { get; set; }
        public List<ModePaiments> ModePaimentsList { get; set; }

        public bool checkTypeID;



        public async Task OnGet()
            => ModePaimentsList = await _db.ModePaiments.ToListAsync();




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            await _db.ModePaiments.AddAsync(ModePaiments);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/ModePaiment/Index");
        }




        public async Task<IActionResult> OnPostDelete(int id)
        {
            var methode = await _db.ModePaiments.FindAsync(id);

            if (methode is null)
                return NotFound();

            var vehicule = await _db.Vehicules.Where(x => x.ModePayementId == methode.Id).Select(x => x.Id).FirstOrDefaultAsync();
            var recharge_carburon = await _db.RechargeCarburants.Where(x => x.ModePaimentId == methode.Id).Select(x => x.Id).FirstOrDefaultAsync();
            var vente = await _db.Cessions.Where(x => x.ModePaimentId == methode.Id).Select(x => x.Id).FirstOrDefaultAsync();
            var maintenance = await _db.Maintenances.Where(x => x.ModePaiementId == methode.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (vehicule != 0 && recharge_carburon != 0 && vehicule != 0 && maintenance != 0)
            {
                checkTypeID = true;
                await OnGet();
                return Page();
            }

            _db.ModePaiments.Remove(methode);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/ModePaiment/Index");
        }
    }
}
