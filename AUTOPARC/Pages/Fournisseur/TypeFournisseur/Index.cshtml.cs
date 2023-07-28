using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Fournisseur.TypeFournisseur
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        [BindProperty]
        public TypeFournisseurs TypeFournisseurs { get; set; }
        public List<TypeFournisseurs> TypeFournisseursList { get; set; }

        public bool checkTypeID; // to check if the supplier type exists in the "Fournisseur" table



        public async Task OnGet()
            => TypeFournisseursList = await _db.TypeFournisseurs.ToListAsync();




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            await _db.TypeFournisseurs.AddAsync(TypeFournisseurs);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Fournisseur/TypeFournisseur/Index");
        }




        public async Task<IActionResult> OnPostDelete(int id)
        {
            var type = await _db.TypeFournisseurs.FindAsync(id);

            if (type is null)
                return NotFound();

            var frs = await _db.Fournisseurs.Where(x => x.TypeFrsId == type.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (frs != 0)
            {
                checkTypeID = true;
                await OnGet();
                return Page();
            }

            _db.TypeFournisseurs.Remove(type);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Fournisseur/TypeFournisseur/Index");
        }
    }
}
