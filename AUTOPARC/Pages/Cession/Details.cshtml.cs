using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Cession
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public Cessions Cessions { get; set; }
        public List<Vehicules> Vehicules { get; set; }
        public List<ModePaiments> ModePaiments { get; set; }





        public async Task OnGet(int id)
        {
            Cessions = await _db.Cessions.FindAsync(id);
            Vehicules = await _db.Vehicules.ToListAsync();
            ModePaiments = await _db.ModePaiments.ToListAsync();
        }




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var vente = await _db.Cessions.FindAsync(Cessions.Id);
            vente.VehiculeId = Cessions.VehiculeId;
            vente.DateCession = Cessions.DateCession;
            vente.PrixCession = Cessions.PrixCession;
            vente.MontantRecu = Cessions.MontantRecu;
            vente.ModePaimentId = Cessions.ModePaimentId;
            vente.NomAcheteur = Cessions.NomAcheteur;
            vente.ContactAcheteur = Cessions.ContactAcheteur;

            await _db.SaveChangesAsync();
            return RedirectToPage("/Cession/Index");
        }




        public async Task<IActionResult> OnPostDelete()
        {
            if (!ModelState.IsValid)
                return Page();

            _db.Cessions.Remove(Cessions);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Cession/Index");
        }
    }
}
