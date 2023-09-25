using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.SuiviDepense
{
    public class SelectionSocieteModel : PageModel
    {
        private readonly DBC _db;
        public SelectionSocieteModel(DBC db) => _db = db;



        [BindProperty]
        public Societes Societes { get; set; }
        public List<Societes> SocietesList { get; set; }

        public bool company_not_found;




        public async Task OnGet()
        {
            SocietesList = await _db.Societes.ToListAsync();
            var societe_count = await _db.Societes.CountAsync();
        }




        public async Task<IActionResult> OnPostSelect()
        {
            int societe_id = Societes.Id;

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

            return RedirectToPage("/Vehicule/Create", new { societe_id });
        }
    }
}
