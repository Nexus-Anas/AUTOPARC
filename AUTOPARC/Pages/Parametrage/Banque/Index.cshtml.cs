using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Parametrage.Banque
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        [BindProperty]
        public Banques Banques { get; set; }
        public List<Banques> BanquesList { get; set; }




        public async Task OnGet()
            => BanquesList = await _db.Banques.ToListAsync();




        public async Task<IActionResult> OnPostCreate()
        {
            if (string.IsNullOrEmpty(Banques.Nom) || !ModelState.IsValid)
            {
                ModelState.AddModelError("Banques.Nom", "Le champ \"Nom Banque\" est requis.");
                await OnGet();
                return Page();
            }

            await _db.Banques.AddAsync(Banques);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/Banque/Index");
        }
    }
}
