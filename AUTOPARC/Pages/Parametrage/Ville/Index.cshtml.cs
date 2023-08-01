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

        public bool checkTypeID;



        public async Task OnGet()
            => VillesList = await _db.Villes.ToListAsync();




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            await _db.Villes.AddAsync(Villes);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/Ville/Index");
        }




        public async Task<IActionResult> OnPostDelete(int id)
        {
            var ville = await _db.Villes.FindAsync(id);

            if (ville is null)
                return NotFound();

            var frs = await _db.Fournisseurs.Where(x => x.VilleId == ville.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (frs != 0)
            {
                checkTypeID = true;
                await OnGet();
                return Page();
            }

            _db.Villes.Remove(ville);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/Ville/Index");
        }
    }
}
