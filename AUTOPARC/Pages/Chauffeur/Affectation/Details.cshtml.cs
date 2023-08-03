using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Linq;

namespace AUTOPARC.Pages.Chauffeur.Affectation
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public AffectationChauffeurVehicules AffectationChauffeurVehicules { get; set; }
        public List<Vehicules> Vehicules { get; set; }
        public List<Chauffeurs> Chauffeurs { get; set; }

        public bool check_exception, check_date;




        public async Task OnGet(int id)
        {
            AffectationChauffeurVehicules = await _db.AffectationChauffeurVehicules.FindAsync(id);
            Vehicules = await _db.Vehicules.ToListAsync();
            Chauffeurs = await _db.Chauffeurs.ToListAsync();
        }




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            if (AffectationChauffeurVehicules.DateDebutAffectation >= AffectationChauffeurVehicules.DateFinAffectation)
            {
                check_date = true;
                await OnGet(AffectationChauffeurVehicules.Id);
                return Page();
            }

            try
            {
                var affectation = await _db.AffectationChauffeurVehicules.FindAsync(AffectationChauffeurVehicules.Id);
                affectation.VehiculeId = AffectationChauffeurVehicules.VehiculeId;
                affectation.ChauffeurId = AffectationChauffeurVehicules.ChauffeurId;
                affectation.DateDebutAffectation = AffectationChauffeurVehicules.DateDebutAffectation;
                affectation.DateFinAffectation = AffectationChauffeurVehicules.DateFinAffectation;
                affectation.RaisonFinAffectation = AffectationChauffeurVehicules.RaisonFinAffectation;

                await _db.SaveChangesAsync();
                return RedirectToPage("/Chauffeur/Affectation/Index");
            }
            catch (Exception)
            {
                check_exception = true;
                return Page();
            }
        }




        public async Task<IActionResult> OnPostDelete()
        {
            if (!ModelState.IsValid)
                return Page();

            _db.AffectationChauffeurVehicules.Remove(AffectationChauffeurVehicules);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Chauffeur/Affectation/Index");
        }
    }
}
