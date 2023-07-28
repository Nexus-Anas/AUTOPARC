using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Operation.MethodePayement
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        [BindProperty]
        public MethodePayements MethodePayements { get; set; }
        public List<MethodePayements> MethodePayementsList { get; set; }

        public bool checkTypeID;



        public async Task OnGet()
            => MethodePayementsList = await _db.MethodePayements.ToListAsync();




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            await _db.MethodePayements.AddAsync(MethodePayements);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Operation/MethodePayement/Index");
        }




        public async Task<IActionResult> OnPostDelete(int id)
        {
            var methode = await _db.MethodePayements.FindAsync(id);

            if (methode is null)
                return NotFound();

            var vehicule = await _db.Vehicules.Where(x => x.MethodePayementId == methode.Id).Select(x => x.Id).FirstOrDefaultAsync();
            var recharge_carburon = await _db.RechargeCarburants.Where(x => x.MethodePayementId == methode.Id).Select(x => x.Id).FirstOrDefaultAsync();
            var vente = await _db.Ventes.Where(x => x.MethodePayementId == methode.Id).Select(x => x.Id).FirstOrDefaultAsync();
            var maintenance = await _db.Maintenances.Where(x => x.MethodePayementId == methode.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (vehicule != 0 && recharge_carburon != 0 && vehicule != 0 && maintenance != 0)
            {
                checkTypeID = true;
                await OnGet();
                return Page();
            }

            _db.MethodePayements.Remove(methode);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Operation/MethodePayement/Index");
        }
    }
}
