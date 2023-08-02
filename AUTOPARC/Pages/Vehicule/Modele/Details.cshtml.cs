using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule.Modele
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public Modeles Modeles { get; set; }

        public bool check_presence_modele;




        public async Task OnGet(int id)
            => Modeles = await _db.Modeles.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (string.IsNullOrEmpty(Modeles.Nom) || !ModelState.IsValid)
            {
                ModelState.AddModelError("Modeles.Nom", "Le champ Modele est requis.");
                return Page();
            }

            var modele = await _db.Modeles.FindAsync(Modeles.Id);
            modele.Nom = Modeles.Nom;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Modele/Index");
        }




        public async Task<IActionResult> OnPostDelete()
        {
            var modele = await _db.Modeles.FindAsync(Modeles.Id);

            if (modele is null)
                return NotFound();

            var vehicule = await _db.Vehicules.Where(x => x.ModeleId == modele.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (vehicule != 0)
            {
                check_presence_modele = true;
                return Page();
            }

            _db.Modeles.Remove(modele);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Modele/Index");
        }
    }
}