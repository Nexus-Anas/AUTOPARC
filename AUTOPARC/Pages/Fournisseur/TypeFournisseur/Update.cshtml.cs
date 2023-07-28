using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Fournisseur.TypeFournisseur
{
    public class UpdateModel : PageModel
    {
        private readonly DBC _db;
        public UpdateModel(DBC db) => _db = db;




        [BindProperty]
        public TypeFournisseurs TypeFournisseurs { get; set; }




        public async Task OnGet(int id)
            => TypeFournisseurs = await _db.TypeFournisseurs.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var typeFrs = await _db.TypeFournisseurs.FindAsync(TypeFournisseurs.Id);
            typeFrs.Type = TypeFournisseurs.Type;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Fournisseur/TypeFournisseur/Index");
        }
    }
}