using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule.Marque
{
    public class UpdateModel : PageModel
    {
        private readonly DBC _db;
        public UpdateModel(DBC db) => _db = db;




        [BindProperty]
        public Marques Marques { get; set; }




        public async Task OnGet(int id)
            => Marques = await _db.Marques.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var marque = await _db.Marques.FindAsync(Marques.Id);
            marque.Marque = Marques.Marque;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Marque/Index");
        }
    }
}
