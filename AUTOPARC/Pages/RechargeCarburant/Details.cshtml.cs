using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.RechargeCarburant
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public RechargeCarburants RechargeCarburants { get; set; }
        public List<Vehicules> Vehicules { get; set; }
        public List<ModePaiments> ModePaiments { get; set; }





        public async Task OnGet(int id)
        {
            RechargeCarburants = await _db.RechargeCarburants.FindAsync(id);
            Vehicules = await _db.Vehicules.ToListAsync();
            ModePaiments = await _db.ModePaiments.ToListAsync();
        }




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var recharge_carburant = await _db.RechargeCarburants.FindAsync(RechargeCarburants.Id);
            recharge_carburant.VehiculeId = RechargeCarburants.VehiculeId;
            recharge_carburant.Quantite = RechargeCarburants.Quantite;
            recharge_carburant.Pu = RechargeCarburants.Pu;
            recharge_carburant.ModePaimentId = RechargeCarburants.ModePaimentId;
            recharge_carburant.Km = RechargeCarburants.Km;
            recharge_carburant.DateRecharge = RechargeCarburants.DateRecharge;
            recharge_carburant.Note = RechargeCarburants.Note;

            await _db.SaveChangesAsync();
            return RedirectToPage("/rechargeCarburant/Index");
        }




        public async Task<IActionResult> OnPostDelete()
        {
            if (!ModelState.IsValid)
                return Page();

            _db.RechargeCarburants.Remove(RechargeCarburants);
            await _db.SaveChangesAsync();
            return RedirectToPage("/rechargeCarburant/Index");
        }
    }
}
