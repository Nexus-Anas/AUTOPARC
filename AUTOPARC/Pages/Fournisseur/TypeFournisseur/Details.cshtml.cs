using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Fournisseur.TypeFournisseur
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public TypeFournisseurs TypeFournisseurs { get; set; }

        public bool check_presence_typeFrs;




        public async Task OnGet(int id)
            => TypeFournisseurs = await _db.TypeFournisseurs.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (string.IsNullOrEmpty(TypeFournisseurs.Type) || !ModelState.IsValid)
            {
                ModelState.AddModelError("TypeFournisseurs.Type", "Le champ \"Type Fournisseurs\" est requis.");
                return Page();
            }

            var typeFrs = await _db.TypeFournisseurs.FindAsync(TypeFournisseurs.Id);
            typeFrs.Type = TypeFournisseurs.Type;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Fournisseur/TypeFournisseur/Index");
        }




        public async Task<IActionResult> OnPostDelete()
        {
            var typeFrs = await _db.TypeFournisseurs.FindAsync(TypeFournisseurs.Id);

            if (typeFrs is null)
                return NotFound();

            var frs = await _db.Fournisseurs.Where(x => x.TypeFrsId == typeFrs.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (frs != 0)
            {
                check_presence_typeFrs = true;
                return Page();
            }

            _db.TypeFournisseurs.Remove(typeFrs);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Fournisseur/TypeFournisseur/Index");
        }
    }
}