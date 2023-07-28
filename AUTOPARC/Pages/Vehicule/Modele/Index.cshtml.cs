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
            if (!ModelState.IsValid)
                return Page();

            await _db.Modeles.AddAsync(Modeles);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Modele/Index");
        }




        public async Task<IActionResult> OnPostDelete(int id)
        {
            var modele = await _db.Modeles.FindAsync(id);

            if (modele is null)
                return NotFound();

            var vehicule = await _db.Vehicules.Where(x => x.ModeleId == modele.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (vehicule != 0)
            {
                checkTypeID = true;
                await OnGet();
                return Page();
            }

            _db.Modeles.Remove(modele);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Modele/Index");
        }
    }
}
