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




        public async Task OnGet()
            => ModePaimentsList = await _db.ModePaiments.ToListAsync();




        public async Task<IActionResult> OnPostCreate()
        {
            if (string.IsNullOrEmpty(ModePaiments.Mode) || !ModelState.IsValid)
            {
                ModelState.AddModelError("ModePaiments.Mode", "Le champ \"Mode Paiement\" est requis.");
                await OnGet();
                return Page();
            }

            await _db.ModePaiments.AddAsync(ModePaiments);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/ModePaiment/Index");
        }




        public async Task<IActionResult> OnPostDelete(int id)
        {
            var methode = await _db.ModePaiments.FindAsync(id);

            if (methode is null)
                return NotFound();

            _db.ModePaiments.Remove(methode);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/ModePaiment/Index");
        }
    }
}
