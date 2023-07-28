using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vente
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public Ventes Ventes { get; set; }
        public List<Vehicules> Vehicules { get; set; }
        public List<MethodePayements> MethodePayements { get; set; }





        public async Task OnGet(int id)
        {
            Ventes = await _db.Ventes.FindAsync(id);
            Vehicules = await _db.Vehicules.ToListAsync();
            MethodePayements = await _db.MethodePayements.ToListAsync();
        }




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var vente = await _db.Ventes.FindAsync(Ventes.Id);
            vente.VehiculeId = Ventes.VehiculeId;
            vente.DateVente = Ventes.DateVente;
            vente.PrixVente = Ventes.PrixVente;
            vente.MontantRecu = Ventes.MontantRecu;
            vente.MethodePayementId = Ventes.MethodePayementId;
            vente.NomAcheteur = Ventes.NomAcheteur;
            vente.ContactAcheteur = Ventes.ContactAcheteur;

            await _db.SaveChangesAsync();
            return RedirectToPage("/Vente/Index");
        }




        public async Task<IActionResult> OnPostDelete()
        {
            if (!ModelState.IsValid)
                return Page();

            _db.Ventes.Remove(Ventes);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vente/Index");
        }
    }
}
