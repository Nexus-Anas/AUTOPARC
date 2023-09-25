using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule
{
    public class SelectionMarqueModel : PageModel
    {
        private readonly DBC _db;
        public SelectionMarqueModel(DBC db) => _db = db;



        [BindProperty]
        public Marques Marques { get; set; }
        [BindProperty]
        public Societes Societes { get; set; }
        public List<Marques> MarquesList { get; set; }
        public List<Societes> SocietesList { get; set; }

        public bool company_not_found;




        public async Task OnGet()
        {
            MarquesList = await _db.Marques.ToListAsync();
            SocietesList = await _db.Societes.ToListAsync();
        }




        public async Task<IActionResult> OnPostSelect()
        {
            int marque_id = Marques.Id;
            int societe_id = Societes.Id;

            if (marque_id == 0)
            {
                ModelState.AddModelError("Marques.Nom", "Veuillez sélectionner une marque.");
                await OnGet();
                return Page();
            }

            var societeCount = await _db.Societes.CountAsync();
            if (societeCount == 0)
            {
                company_not_found = true;
                await OnGet();
                return Page();
            }
            if (societeCount == 1)
            {
                var societe = await _db.Societes.FirstOrDefaultAsync();
                societe_id = societe.Id;
            }

            return RedirectToPage("/Vehicule/Create", new { marque_id, societe_id });
        }
    }
}
