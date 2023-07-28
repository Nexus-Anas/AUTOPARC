using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Document.TypeDocument
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        [BindProperty]
        public TypeDocs TypeDocs { get; set; }
        public List<TypeDocs> TypeDocsList { get; set; }

        public bool checkTypeID;




        public async Task OnGet()
            => TypeDocsList = await _db.TypeDocs.ToListAsync();




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            await _db.TypeDocs.AddAsync(TypeDocs);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Document/TypeDocument/Index");
        }




        public async Task<IActionResult> OnPostDelete(int id)
        {
            var type = await _db.TypeDocs.FindAsync(id);

            if (type is null)
                return NotFound();

            var doc = await _db.Docs.Where(x => x.TypeId == type.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (doc != 0)
            {
                checkTypeID = true;
                await OnGet();
                return Page();
            }

            _db.TypeDocs.Remove(type);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Document/TypeDocument/Index");
        }
    }
}
