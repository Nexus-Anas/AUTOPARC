using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Parametrage.Ville
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public Villes Villes { get; set; }

        public bool check_presence_ville;




        public async Task OnGet(int id)
            => Villes = await _db.Villes.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (string.IsNullOrEmpty(Villes.Nom) || !ModelState.IsValid)
            {
                ModelState.AddModelError("Villes.Nom", "Le champ \"Nom Ville\" est requis.");
                return Page();
            }

            var ville = await _db.Villes.FindAsync(Villes.Id);
            ville.Nom = Villes.Nom;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/Ville/Index");
        }




        public async Task<IActionResult> OnPostDelete()
        {
            var ville = await _db.Villes.FindAsync(Villes.Id);

            if (ville is null)
                return NotFound();

            var frs = await _db.Fournisseurs.Where(f => f.VilleId == ville.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (frs != 0)
            {
                check_presence_ville = true;
                return Page();
            }

            _db.Villes.Remove(ville);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/Ville/Index");
        }
    }
}
