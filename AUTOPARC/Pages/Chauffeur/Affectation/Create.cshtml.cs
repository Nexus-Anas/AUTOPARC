using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Chauffeur.Affectation
{
    public class CreateModel : PageModel
    {
        private readonly DBC _db;
        public CreateModel(DBC db) => _db = db;




        [BindProperty]
        public AffectationChauffeurVehicules AffectationChauffeurVehicules { get; set; }
        public List<Vehicules> Vehicules { get; set; }
        public List<Chauffeurs> Chauffeurs { get; set; }

        public bool check_exception, check_date;




        public async Task OnGet()
        {
            Vehicules = await _db.Vehicules.ToListAsync();
            Chauffeurs = await _db.Chauffeurs.ToListAsync();
        }




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            if (AffectationChauffeurVehicules.DateDebutAffectation >= AffectationChauffeurVehicules.DateFinAffectation)
            {
                check_date = true;
                await OnGet();
                return Page();
            }

            try
            {
                await _db.AffectationChauffeurVehicules.AddAsync(AffectationChauffeurVehicules);
                await _db.SaveChangesAsync();
                return RedirectToPage("/Chauffeur/Affectation/Index");
            }
            catch (Exception)
            {
                check_exception = true;
                return Page();
            }
        }
    }
}
