using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        public List<Vehicules> VehiculesList { get; set; }
        public List<Categories> CategoriesList { get; set; }
        public List<Marques> MarquesList { get; set; }
        public List<Modeles> ModelesList { get; set; }
        public List<TypeCarburants> TypeCarburantsList { get; set; }
        public List<EtatVehicules> EtatvehiculesList { get; set; }
        public List<Fournisseurs> FournisseursList { get; set; }
        public List<Cheques> ChequesList { get; set; }
        public List<Virements> VirementsList { get; set; }
        public List<Credits> CreditsList { get; set; }




        public async Task OnGet()
        {
            VehiculesList = await _db.Vehicules.ToListAsync();
            CategoriesList = await _db.Categories.ToListAsync();
            MarquesList = await _db.Marques.ToListAsync();
            ModelesList = await _db.Modeles.ToListAsync();
            TypeCarburantsList = await _db.TypeCarburants.ToListAsync();
            EtatvehiculesList = await _db.EtatVehicules.ToListAsync();
            FournisseursList = await _db.Fournisseurs.ToListAsync();
            ChequesList = await _db.Cheques.Where(chq => chq.Action == "Vehicule").ToListAsync();
            VirementsList = await _db.Virements.Where(v => v.Action == "Vehicule").ToListAsync();
            CreditsList = await _db.Credits.Where(c => c.Action == "Vehicule").ToListAsync();
        }
    }
}
