using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Document
{
    public class CreateModel : PageModel
    {
        private readonly DBC _db;
        public CreateModel(DBC db) => _db = db;




        [BindProperty]
        public Docs Docs { get; set; }
        public List<TypeDocs> TypeDocs { get; set; }
        public List<Vehicules> Vehicules { get; set; }
        public List<Fournisseurs> Fournisseurs { get; set; }




        public async Task OnGet()
        {
            TypeDocs = await _db.TypeDocs.ToListAsync();
            Vehicules = await _db.Vehicules.ToListAsync();
            Fournisseurs = await _db.Fournisseurs.ToListAsync();
        }




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            await _db.Docs.AddAsync(Docs);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Document/Index");
        }
    }
}
