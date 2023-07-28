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

        public bool checkTypeID;



        public async Task OnGet()
            => CategoriesList = await _db.Categories.ToListAsync();




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            await _db.Categories.AddAsync(Categories);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Categorie/Index");
        }




        public async Task<IActionResult> OnPostDelete(int id)
        {
            var categorie = await _db.Categories.FindAsync(id);

            if (categorie is null)
                return NotFound();

            var vehicule = await _db.Vehicules.Where(x => x.CategorieId == categorie.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (vehicule != 0)
            {
                checkTypeID = true;
                await OnGet();
                return Page();
            }

            _db.Categories.Remove(categorie);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Categorie/Index");
        }
    }
}
