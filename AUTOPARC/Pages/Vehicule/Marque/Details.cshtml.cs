using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule.Marque
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public Marques Marques { get; set; }

        public bool check_presence_marque;




        public async Task OnGet(int id)
            => Marques = await _db.Marques.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (string.IsNullOrEmpty(Marques.Nom) || !ModelState.IsValid)
            {
                ModelState.AddModelError("Marques.Nom", "Le champ Marque est requis.");
                return Page();
            }

            var marque = await _db.Marques.FindAsync(Marques.Id);
            marque.Nom = Marques.Nom;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Marque/Index");
        }




        public async Task<IActionResult> OnPostDelete()
        {
            var marque = await _db.Marques.FindAsync(Marques.Id);

            if (marque is null)
                return NotFound();

            var vehicule = await _db.Vehicules.Where(x => x.CategorieId == marque.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (vehicule != 0)
            {
                check_presence_marque = true;
                return Page();
            }

            _db.Marques.Remove(marque);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Marque/Index");
        }
    }
}
