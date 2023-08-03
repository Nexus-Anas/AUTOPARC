using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Document.TypeDocument
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public TypeDocs TypeDocs { get; set; }

        public bool check_presence_typeDoc;




        public async Task OnGet(int id)
            => TypeDocs = await _db.TypeDocs.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (string.IsNullOrEmpty(TypeDocs.Type) || !ModelState.IsValid)
            {
                ModelState.AddModelError("TypeDocs.Type", "Le champ \"Type Document\" est requis.");
                return Page();
            }

            var typeDoc = await _db.TypeDocs.FindAsync(TypeDocs.Id);
            typeDoc.Type = TypeDocs.Type;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Document/TypeDocument/Index");
        }




        public async Task<IActionResult> OnPostDelete()
        {
            var typeDoc = await _db.TypeDocs.FindAsync(TypeDocs.Id);

            if (typeDoc is null)
                return NotFound();

            var doc = await _db.Docs.Where(x => x.TypeId == typeDoc.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (doc != 0)
            {
                check_presence_typeDoc = true;
                return Page();
            }

            _db.TypeDocs.Remove(typeDoc);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Document/TypeDocument/Index");
        }
    }
}
