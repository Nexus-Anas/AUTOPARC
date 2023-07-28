using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule.Modele
{
    public class UpdateModel : PageModel
    {
        private readonly DBC _db;
        public UpdateModel(DBC db) => _db = db;




        [BindProperty]
        public Modeles Modeles { get; set; }




        public async Task OnGet(int id)
            => Modeles = await _db.Modeles.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var modele = await _db.Modeles.FindAsync(Modeles.Id);
            modele.Modele = Modeles.Modele;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Modele/Index");
        }
    }
}
