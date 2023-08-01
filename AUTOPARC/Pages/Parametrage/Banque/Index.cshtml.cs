using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Parametrage.Banque
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        [BindProperty]
        public Banques Banques { get; set; }
        public List<Banques> BanquesList { get; set; }

        public bool checkTypeID;




        public async Task OnGet()
            => BanquesList = await _db.Banques.ToListAsync();




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            await _db.Banques.AddAsync(Banques);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/Banque/Index");
        }




        public async Task<IActionResult> OnPostDelete(int id)
        {
            var banque = await _db.Banques.FindAsync(id);

            if (banque is null)
                return NotFound();

            var check = await _db.Cheques.Where(x => x.BanqueId == banque.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (check != 0)
            {
                checkTypeID = true;
                await OnGet();
                return Page();
            }

            _db.Banques.Remove(banque);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/Banque/Index");
        }
    }
}
