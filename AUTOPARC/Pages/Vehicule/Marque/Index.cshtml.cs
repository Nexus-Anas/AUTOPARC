using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule.Marque
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        [BindProperty]
        public Marques Marques { get; set; }
        public List<Marques> MarquesList { get; set; }

        public bool checkTypeID;



        public async Task OnGet()
            => MarquesList = await _db.Marques.ToListAsync();




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            await _db.Marques.AddAsync(Marques);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Marque/Index");
        }




        public async Task<IActionResult> OnPostDelete(int id)
        {
            var marque = await _db.Marques.FindAsync(id);

            if (marque is null)
                return NotFound();

            var vehicule = await _db.Vehicules.Where(x => x.MarqueId == marque.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (vehicule != 0)
            {
                checkTypeID = true;
                await OnGet();
                return Page();
            }

            _db.Marques.Remove(marque);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Marque/Index");
        }
    }
}
