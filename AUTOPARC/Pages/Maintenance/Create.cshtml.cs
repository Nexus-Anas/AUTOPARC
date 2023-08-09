using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Maintenance
{
    public class CreateModel : PageModel
    {
        private readonly DBC _db;
        public CreateModel(DBC db) => _db = db;




        [BindProperty]
        public Maintenances Maintenances { get; set; }
        public List<TypeMaintenances> TypeMaintenancesList { get; set; }
        public List<Chauffeurs> ChauffeursList { get; set; }
        public List<ModePaiments> ModePaimentsList { get; set; }

        public bool check_exception, check_cout_montantPayee;

        public int vehiculeID;

        public string vehiculeMatricule;




        public async Task OnGet(int id)
        {
            vehiculeID = id;
            vehiculeMatricule = await _db.Vehicules.Where(x => x.Id == id).Select(x => x.Matricule).FirstOrDefaultAsync();
            TypeMaintenancesList = await _db.TypeMaintenances.ToListAsync();
            ChauffeursList = await _db.Chauffeurs.ToListAsync();
            ModePaimentsList = await _db.ModePaiments.ToListAsync();
        }




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
            {
                await OnGet(vehiculeID);
                return Page();
            }

            if (Maintenances.MontantPayee > Maintenances.Cout)
            {
                check_cout_montantPayee = true;
                await OnGet(vehiculeID);
                return Page();
            }

            try
            {
                await _db.Maintenances.AddAsync(Maintenances);
                await _db.SaveChangesAsync();
                return RedirectToPage("/Maintenance/Index");
            }
            catch (Exception)
            {
                check_exception = true;
                return Page();
            }
        }
    }
}
