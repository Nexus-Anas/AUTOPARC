using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Maintenance.TypeMaintenance
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public TypeMaintenances TypeMaintenances { get; set; }

        public bool check_presence_typeMaintenance;




        public async Task OnGet(int id)
            => TypeMaintenances = await _db.TypeMaintenances.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (string.IsNullOrEmpty(TypeMaintenances.Type) || !ModelState.IsValid)
            {
                ModelState.AddModelError("TypeMaintenances.Type", "Le champ \"Type Maintenance\" est requis.");
                return Page();
            }

            var typeMaintenance = await _db.TypeMaintenances.FindAsync(TypeMaintenances.Id);
            typeMaintenance.Type = TypeMaintenances.Type;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Maintenance/TypeMaintenance/Index");
        }




        public async Task<IActionResult> OnPostDelete()
        {
            var typeMaintenance = await _db.TypeMaintenances.FindAsync(TypeMaintenances.Id);

            if (typeMaintenance is null)
                return NotFound();

            var maintenance = await _db.Maintenances.Where(x => x.TypeId == typeMaintenance.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (maintenance != 0)
            {
                check_presence_typeMaintenance = true;
                return Page();
            }

            _db.TypeMaintenances.Remove(typeMaintenance);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Maintenance/TypeMaintenance/Index");
        }
    }
}
