using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule.Categorie
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        [BindProperty]
        public Categories Categories { get; set; }
        public List<Categories> CategoriesList { get; set; }



        public async Task OnGet()
            => CategoriesList = await _db.Categories.ToListAsync();




        public async Task<IActionResult> OnPostCreate()
        {
            if (string.IsNullOrEmpty(Categories.Nom) || !ModelState.IsValid)
            {
                ModelState.AddModelError("Categories.Nom", "Le champ Categories est requis.");
                await OnGet();
                return Page();
            }

            await _db.Categories.AddAsync(Categories);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Categorie/Index");
        }
    }
}
