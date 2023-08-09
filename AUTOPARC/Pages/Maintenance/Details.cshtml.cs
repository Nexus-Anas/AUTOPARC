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
        public List<TypeMaintenances> TypeMaintenancesList { get; set; }
        public List<Vehicules> VehiculesList { get; set; }
        public List<Chauffeurs> ChauffeursList { get; set; }
        public List<ModePaiments> ModePaimentsList { get; set; }

        public bool check_exception, check_cout_montantPayee;




        public async Task OnGet(int id)
        {
            Maintenances = await _db.Maintenances.FindAsync(id);
            TypeMaintenancesList = await _db.TypeMaintenances.ToListAsync();
            VehiculesList = await _db.Vehicules.ToListAsync();
            ChauffeursList = await _db.Chauffeurs.ToListAsync();
            ModePaimentsList = await _db.ModePaiments.ToListAsync();
        }




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                await OnGet(Maintenances.Id);
                return Page();
            }

            if (Maintenances.MontantPayee > Maintenances.Cout)
            {
                check_cout_montantPayee = true;
                await OnGet(Maintenances.Id);
                return Page();
            }

            var maintenance = await _db.Maintenances.FindAsync(Maintenances.Id);
            maintenance.TypeId = Maintenances.TypeId;
            maintenance.VehiculeId = Maintenances.VehiculeId;
            maintenance.DateMaintenance = Maintenances.DateMaintenance;
            maintenance.Cout = Maintenances.Cout;
            maintenance.MontantPayee = Maintenances.MontantPayee;
            maintenance.ModePaiementId = Maintenances.ModePaiementId;
            maintenance.Description = Maintenances.Description;
            maintenance.UrlDoc = Maintenances.UrlDoc;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Maintenance/Index");
        }




        public async Task<IActionResult> OnPostDelete()
        {
            if (!ModelState.IsValid)
            {
                await OnGet(Maintenances.Id);
                return Page();
            }

            _db.Maintenances.Remove(Maintenances);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Maintenance/Index");
        }
    }
}
