using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Maintenance.TypeMaintenance
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        [BindProperty]
        public TypeMaintenances TypeMaintenances { get; set; }
        public List<TypeMaintenances> TypeMaintenancesList { get; set; }

        public bool checkTypeID;




        public async Task OnGet()
            => TypeMaintenancesList = await _db.TypeMaintenances.ToListAsync();




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            await _db.TypeMaintenances.AddAsync(TypeMaintenances);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Maintenance/TypeMaintenance/Index");
        }




        public async Task<IActionResult> OnPostDelete(int id)
        {
            var type = await _db.TypeMaintenances.FindAsync(id);

            if (type is null)
                return NotFound();

            var maintenance = await _db.Maintenances.Where(x => x.TypeId == type.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (maintenance != 0)
            {
                checkTypeID = true;
                await OnGet();
                return Page();
            }

            _db.TypeMaintenances.Remove(type);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Maintenance/TypeMaintenance/Index");
        }
    }
}
