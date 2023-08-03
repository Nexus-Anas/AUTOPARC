using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule
{
    public class CreateModel : PageModel
    {
        private readonly DBC _db;
        public CreateModel(DBC db) => _db = db;




        [BindProperty]
        public Vehicules Vehicules { get; set; }
        public List<Categories> Categories { get; set; }
        public List<Modeles> Modeles { get; set; }
        public List<TypeCarburants> TypeCarburants { get; set; }
        public List<EtatVehicules> Etatvehicules { get; set; }
        public List<Fournisseurs> Fournisseurs { get; set; }
        public List<ModePaiments> ModePaiments { get; set; }

        public bool check_Prix_MontantPayee;
        public int marqueID;




        public async Task OnGet(int id)
        {
            marqueID = id;
            Categories = await _db.Categories.ToListAsync();
            Modeles = await _db.Modeles.Where(x => x.MarqueId == id).ToListAsync();
            TypeCarburants = await _db.TypeCarburants.ToListAsync();
            Etatvehicules = await _db.EtatVehicules.ToListAsync();
            Fournisseurs = await _db.Fournisseurs.ToListAsync();
            ModePaiments = await _db.ModePaiments.ToListAsync();
        }




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
            {
                await OnGet(marqueID);
                return Page();
            }

            var condition = Vehicules.PrixAchat < Vehicules.MontantPayee;
            if (condition)
            {
                check_Prix_MontantPayee = true;
                await OnGet(marqueID);
                return Page();
            }

            await _db.Vehicules.AddAsync(Vehicules);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Index");
        }
    }
}