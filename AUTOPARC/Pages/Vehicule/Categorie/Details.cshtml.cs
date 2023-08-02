using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule.Categorie
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public Categories Categories { get; set; }

        public bool check_presence_categorie;




        public async Task OnGet(int id)
            => Categories = await _db.Categories.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var categorie = await _db.Categories.FindAsync(Categories.Id);
            categorie.Nom = Categories.Nom;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Categorie/Index");
        }




        public async Task<IActionResult> OnPostDelete()
        {
            var categorie = await _db.Categories.FindAsync(Categories.Id);

            if (categorie is null)
                return NotFound();

            var vehicule = await _db.Vehicules.Where(x => x.CategorieId == categorie.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (vehicule != 0)
            {
                check_presence_categorie = true;
                await OnGet(Categories.Id);
                return Page();
            }

            _db.Categories.Remove(categorie);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Categorie/Index");
        }
    }
}
