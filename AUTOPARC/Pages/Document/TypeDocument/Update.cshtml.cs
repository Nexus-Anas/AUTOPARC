using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Document.TypeDocument
{
    public class UpdateModel : PageModel
    {
        private readonly DBC _db;
        public UpdateModel(DBC db) => _db = db;




        [BindProperty]
        public TypeDocs TypeDocs { get; set; }




        public async Task OnGet(int id)
            => TypeDocs = await _db.TypeDocs.FindAsync(id);

        


        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var type = await _db.TypeDocs.FindAsync(TypeDocs.Id);
            type.Type = TypeDocs.Type;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Document/TypeDocument/Index");
        }
    }
}
