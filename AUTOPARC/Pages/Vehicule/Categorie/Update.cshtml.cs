using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule.Categorie
{
    public class UpdateModel : PageModel
    {
        private readonly DBC _db;
        public UpdateModel(DBC db) => _db = db;




        [BindProperty]
        public Categories Categories { get; set; }




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
    }
}
