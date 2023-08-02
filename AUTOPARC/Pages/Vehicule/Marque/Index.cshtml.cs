using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule.Marque
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        [BindProperty]
        public Marques Marques { get; set; }
        public List<Marques> MarquesList { get; set; }



        public async Task OnGet()
            => MarquesList = await _db.Marques.ToListAsync();




        public async Task<IActionResult> OnPostCreate()
        {
            if (string.IsNullOrEmpty(Marques.Nom) || !ModelState.IsValid)
            {
                ModelState.AddModelError("Marques.Nom", "Le champ Marque est requis.");
                await OnGet();
                return Page();
            }

            await _db.Marques.AddAsync(Marques);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Marque/Index");
        }
    }
}
