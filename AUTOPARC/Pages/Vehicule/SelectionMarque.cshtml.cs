using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule
{
    public class SelectionMarqueModel : PageModel
    {
        private readonly DBC _db;
        public SelectionMarqueModel(DBC db) => _db = db;



        [BindProperty]
        public Marques Marques { get; set; }
        public List<Marques> MarquesList { get; set; }




        public async Task OnGet()
            => MarquesList = await _db.Marques.ToListAsync();




        public async Task<IActionResult> OnPostSelect()
        {
            if (Marques.Id == 0)
            {
                ModelState.AddModelError("Marques.Nom", "Veuillez sélectionner une marque.");
                await OnGet();
                return Page();
            }

            return RedirectToPage("/Vehicule/Create", new {Marques.Id});
        }
    }
}
