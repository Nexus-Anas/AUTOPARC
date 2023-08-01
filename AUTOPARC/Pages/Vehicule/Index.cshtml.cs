using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        public List<Vehicules> Vehicules { get; set; }
        public List<Categories> Categories { get; set; }
        public List<Marques> Marques { get; set; }
        public List<Modeles> Modeles { get; set; }
        public List<TypeCarburants> TypeCarburants { get; set; }
        public List<EtatVehicules> Etatvehicules { get; set; }
        public List<Fournisseurs> Fournisseurs { get; set; }
        public List<ModePaiments> ModePaiments { get; set; }




        public async Task OnGet()
        {
            Vehicules = await _db.Vehicules.ToListAsync();
            Categories = await _db.Categories.ToListAsync();
            Marques = await _db.Marques.ToListAsync();
            Modeles = await _db.Modeles.ToListAsync();
            TypeCarburants = await _db.TypeCarburants.ToListAsync();
            Etatvehicules = await _db.EtatVehicules.ToListAsync();
            Fournisseurs = await _db.Fournisseurs.ToListAsync();
            ModePaiments = await _db.ModePaiments.ToListAsync();
        }
    }
}
