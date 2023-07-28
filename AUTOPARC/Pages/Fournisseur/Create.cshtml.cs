using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Fournisseur
{
    public class CreateModel : PageModel
    {
        private readonly DBC _db;
        public CreateModel(DBC db) => _db = db;


            

        [BindProperty]
        public Fournisseurs Fournisseurs { get; set; }
        public List<TypeFournisseurs> TypeFournisseurs { get; set; }
        public List<Villes> Villes { get; set; }




        public async Task OnGet()
        {
            TypeFournisseurs = await _db.TypeFournisseurs.ToListAsync();
            Villes = await _db.Villes.ToListAsync();
        }




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
            {
                await OnGet();
                return Page();
            }

            await _db.Fournisseurs.AddAsync(Fournisseurs);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Fournisseur/Index");
        }
    }
}
