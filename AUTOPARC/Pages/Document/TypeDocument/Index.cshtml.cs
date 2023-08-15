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
            if (string.IsNullOrEmpty(TypeDocs.Type) || !ModelState.IsValid)
            {
                ModelState.AddModelError("TypeDocs.Type", "Le champ \"Type Document\" est requis.");
                await OnGet();
                return Page();
            }

            await _db.TypeDocs.AddAsync(TypeDocs);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Document/TypeDocument/Index");
        }
    }
}
