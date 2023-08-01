using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Parametrage.ModePaiment
{
    public class UpdateModel : PageModel
    {
        private readonly DBC _db;
        public UpdateModel(DBC db) => _db = db;




        [BindProperty]
        public ModePaiments ModePaiments { get; set; }




        public async Task OnGet(int id)
            => ModePaiments = await _db.ModePaiments.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var methode = await _db.ModePaiments.FindAsync(ModePaiments.Id);
            methode.Mode = ModePaiments.Mode;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/ModePaiment/Index");
        }
    }
}
