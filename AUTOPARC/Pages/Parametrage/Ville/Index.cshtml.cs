using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Parametrage.Ville
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        [BindProperty]
        public Villes Villes { get; set; }
        public List<Villes> VillesList { get; set; }




        public async Task OnGet()
            => VillesList = await _db.Villes.ToListAsync();




        public async Task<IActionResult> OnPostCreate()
        {
            if (string.IsNullOrEmpty(Villes.Nom) || !ModelState.IsValid)
            {
                ModelState.AddModelError("Villes.Nom", "Le champ \"Nom Ville\" est requis.");
                await OnGet();
                return Page();
            }

            await _db.Villes.AddAsync(Villes);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/Ville/Index");
        }
    }
}
