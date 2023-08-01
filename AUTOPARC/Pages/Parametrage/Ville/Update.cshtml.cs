using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Parametrage.Ville
{
    public class UpdateModel : PageModel
    {
        private readonly DBC _db;
        public UpdateModel(DBC db) => _db = db;




        [BindProperty]
        public Villes Villes { get; set; }




        public async Task OnGet(int id)
            => Villes = await _db.Villes.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var ville = await _db.Villes.FindAsync(Villes.Id);
            ville.Nom = Villes.Nom;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/Ville/Index");
        }
    }
}
