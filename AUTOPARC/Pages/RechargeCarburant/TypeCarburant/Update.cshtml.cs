using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.RechargeCarburant.TypeCarburant
{
    public class UpdateModel : PageModel
    {
        private readonly DBC _db;
        public UpdateModel(DBC db) => _db = db;




        [BindProperty]
        public TypeCarburants TypeCarburants { get; set; }




        public async Task OnGet(int id)
            => TypeCarburants = await _db.TypeCarburants.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var type = await _db.TypeCarburants.FindAsync(TypeCarburants.Id);
            type.Type = TypeCarburants.Type;
            await _db.SaveChangesAsync();
            return RedirectToPage("/RechargeCarburant/TypeCarburant/Index");
        }
    }
}
