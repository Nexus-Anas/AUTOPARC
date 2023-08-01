using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Parametrage.Banque
{
    public class UpdateModel : PageModel
    {
        private readonly DBC _db;
        public UpdateModel(DBC db) => _db = db;




        [BindProperty]
        public Banques Banques { get; set; }




        public async Task OnGet(int id)
            => Banques = await _db.Banques.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var banque = await _db.Banques.FindAsync(Banques.Id);
            banque.Nom = Banques.Nom;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/Banque/Index");
        }
    }
}
