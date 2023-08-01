using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Maintenance
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public Maintenances Maintenances { get; set; }
        public List<TypeMaintenances> TypeMaintenances { get; set; }
        public List<Vehicules> Vehicules { get; set; }
        public List<ModePaiments> ModePaiments { get; set; }




        public async Task OnGet(int id)
        {
            Maintenances = await _db.Maintenances.FindAsync(id);
            TypeMaintenances = await _db.TypeMaintenances.ToListAsync();
            Vehicules = await _db.Vehicules.ToListAsync();
            ModePaiments = await _db.ModePaiments.ToListAsync();
        }




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var maintenance = await _db.Maintenances.FindAsync(Maintenances.Id);
            maintenance.TypeId = Maintenances.TypeId;
            maintenance.VehiculeId = Maintenances.VehiculeId;
            maintenance.DateMaintenance = Maintenances.DateMaintenance;
            maintenance.Cout = Maintenances.Cout;
            maintenance.MontantPayee = Maintenances.MontantPayee;
            maintenance.MethodePayementId = Maintenances.MethodePayementId;
            maintenance.Description = Maintenances.Description;
            maintenance.UrlDoc = Maintenances.UrlDoc;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Maintenance/Index");
        }




        public async Task<IActionResult> OnPostDelete()
        {
            if (!ModelState.IsValid)
                return Page();

            _db.Maintenances.Remove(Maintenances);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Maintenance/Index");
        }
    }
}
