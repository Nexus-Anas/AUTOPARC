using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Maintenance.TypeMaintenance
{
    public class UpdateModel : PageModel
    {
        private readonly DBC _db;
        public UpdateModel(DBC db) => _db = db;




        [BindProperty]
        public TypeMaintenances Typemaintenances { get; set; }




        public async Task OnGet(int id)
            => Typemaintenances = await _db.TypeMaintenances.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var type = await _db.TypeMaintenances.FindAsync(Typemaintenances.Id);
            type.Type = Typemaintenances.Type;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Maintenance/TypeMaintenance/Index");
        }
    }
}
