using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Parametrage.Banque
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public Banques Banques { get; set; }

        public bool check_presence_banque;




        public async Task OnGet(int id)
            => Banques = await _db.Banques.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (string.IsNullOrEmpty(Banques.Nom) || !ModelState.IsValid)
            {
                ModelState.AddModelError("Banques.Nom", "Le champ \"Nom Banque\" est requis.");
                return Page();
            }

            var banque = await _db.Banques.FindAsync(Banques.Id);
            banque.Nom = Banques.Nom;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/Banque/Index");
        }




        public async Task<IActionResult> OnPostDelete()
        {
            var banque = await _db.Banques.FindAsync(Banques.Id);

            if (banque is null)
                return NotFound();

            var cheque = await _db.Cheques.Where(chq => chq.BanqueId == banque.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (cheque != 0)
            {
                check_presence_banque = true;
                return Page();
            }

            _db.Banques.Remove(banque);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/Banque/Index");
        }
    }
}
