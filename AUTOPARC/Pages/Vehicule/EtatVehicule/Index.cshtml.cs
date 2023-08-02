using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule.EtatVehicule
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        [BindProperty]
        public EtatVehicules EtatVehicules { get; set; }
        public List<EtatVehicules> EtatVehiculesList { get; set; }

        public bool checkTypeID;



        public async Task OnGet()
            => EtatVehiculesList = await _db.EtatVehicules.ToListAsync();




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            await _db.EtatVehicules.AddAsync(EtatVehicules);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/EtatVehicule/Index");
        }
    }
}
