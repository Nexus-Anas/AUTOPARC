using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Operation.MethodePayement
{
    public class UpdateModel : PageModel
    {
        private readonly DBC _db;
        public UpdateModel(DBC db) => _db = db;




        [BindProperty]
        public MethodePayements MethodePayements { get; set; }




        public async Task OnGet(int id)
            => MethodePayements = await _db.MethodePayements.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var methode = await _db.MethodePayements.FindAsync(MethodePayements.Id);
            methode.Methode = MethodePayements.Methode;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Operation/MethodePayement/Index");
        }
    }
}
