using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule.Modele
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        [BindProperty]
        public Modeles Modeles { get; set; }
        public List<Modeles> ModelesList { get; set; }
        public List<Marques> MarquesList { get; set; }

        public bool checkTypeID;



        public async Task OnGet()
        {
            ModelesList = await _db.Modeles.ToListAsync();
            MarquesList = await _db.Marques.ToListAsync();
        }




        public async Task<IActionResult> OnPostCreate()
        {
            if (string.IsNullOrEmpty(Modeles.Nom) || !ModelState.IsValid)
            {
                ModelState.AddModelError("Modeles.Nom", "Le champ Modele est requis.");
                await OnGet();
                return Page();
            }

            await _db.Modeles.AddAsync(Modeles);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Modele/Index");
        }
    }
}
