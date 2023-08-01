using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public Vehicules Vehicules { get; set; }
        public List<Categories> Categories { get; set; }
        public List<Marques> Marques { get; set; }
        public List<Modeles> Modeles { get; set; }
        public List<TypeCarburants> TypeCarburants { get; set; }
        public List<EtatVehicules> Etatvehicules { get; set; }
        public List<Fournisseurs> Fournisseurs { get; set; }
        public List<ModePaiments> ModePaiments { get; set; }




        public async Task OnGet(int id)
        {
            Vehicules = await _db.Vehicules.FindAsync(id);
            Categories = await _db.Categories.ToListAsync();
            Marques = await _db.Marques.ToListAsync();
            Modeles = await _db.Modeles.ToListAsync();
            TypeCarburants = await _db.TypeCarburants.ToListAsync();
            Etatvehicules = await _db.EtatVehicules.ToListAsync();
            Fournisseurs = await _db.Fournisseurs.ToListAsync();
            ModePaiments = await _db.ModePaiments.ToListAsync();
        }




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var vehicule = await _db.Vehicules.FindAsync(Vehicules.Id);
            vehicule.Matricule = Vehicules.Matricule;
            vehicule.CarburantId = Vehicules.CarburantId;
            vehicule.MarqueId = Vehicules.MarqueId;
            vehicule.ModeleId = Vehicules.ModeleId;
            vehicule.CarburantId = Vehicules.CarburantId;
            vehicule.EtatVehiculeId = Vehicules.EtatVehiculeId;
            vehicule.FrsId = Vehicules.FrsId;
            vehicule.PrixAchat = Vehicules.PrixAchat;
            vehicule.MontantPayee = Vehicules.MontantPayee;
            vehicule.ModePayementId = Vehicules.ModePayementId;
            vehicule.DateAchat = Vehicules.DateAchat;
            vehicule.DateMisEnCirculation = Vehicules.DateMisEnCirculation;
            vehicule.Kilometrage = Vehicules.Kilometrage;
            vehicule.Note = Vehicules.Note;
            vehicule.Moteur = Vehicules.Moteur;
            vehicule.Architecture = Vehicules.Architecture;
            vehicule.PuissanceFiscale = Vehicules.PuissanceFiscale;
            vehicule.PuissanceMax = Vehicules.PuissanceMax;
            vehicule.BoiteAVitesse = Vehicules.BoiteAVitesse;
            vehicule.ConsomationMixte = Vehicules.ConsomationMixte;
            vehicule.VitesseMax = Vehicules.VitesseMax;
            vehicule.NbrPlace = Vehicules.NbrPlace;
            vehicule.Poids = Vehicules.Poids;
            vehicule.Longueur = Vehicules.Longueur;
            vehicule.Largeur = Vehicules.Largeur;
            vehicule.Hauteur = Vehicules.Hauteur;
            vehicule.VolumeCoffre = Vehicules.VolumeCoffre;

            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Index");
        }




        public async Task<IActionResult> OnPostDelete()
        {
            if (!ModelState.IsValid)
                return Page();

            _db.Vehicules.Remove(Vehicules);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Index");
        }
    }
}
