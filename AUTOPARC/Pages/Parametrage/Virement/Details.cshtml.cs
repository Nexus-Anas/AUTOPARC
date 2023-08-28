using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AUTOPARC.Pages.Parametrage.Virement
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public Virements Virements { get; set; }
        public Banques Banques { get; set; }



        public async Task OnGet(int id)
        {
            Virements = await _db.Virements.FindAsync(id);
            Banques = await _db.Banques.Where(b => b.Id == Virements.BanqueId).SingleOrDefaultAsync();
        }




        public async Task<IActionResult> OnPostPayee()
        {
            try
            {
                if (Virements.Action == "Maintenance")
                {
                    var maintenance = await _db.Maintenances.Where(m => m.Num == Virements.ActionNum).SingleOrDefaultAsync();
                    var virement = await _db.Virements.Where(v => v.Id == Virements.Id).SingleOrDefaultAsync();

                    virement.Etat = "payé";
                    maintenance.MontantPayeeTotal += virement.Montant;

                    await _db.SaveChangesAsync();
                    return RedirectToPage("/Parametrage/Virement/Index");
                }


                if (Virements.Action == "Vehicule")
                {
                    var vehicules = await _db.Vehicules.Where(v => v.Num == Virements.ActionNum).SingleOrDefaultAsync();
                    var virement = await _db.Virements.Where(v => v.Id == Virements.Id).SingleOrDefaultAsync();

                    virement.Etat = "payé";
                    vehicules.MontantPayeeTotal += virement.Montant;

                    await _db.SaveChangesAsync();
                    return RedirectToPage("/Parametrage/Virement/Index");
                }


                if (Virements.Action == "Document")
                {
                    var doc = await _db.Docs.Where(d => d.Num == Virements.ActionNum).SingleOrDefaultAsync();
                    var virement = await _db.Virements.Where(v => v.Id == Virements.Id).SingleOrDefaultAsync();

                    virement.Etat = "payé";
                    doc.MontantPayeeTotal += virement.Montant;

                    await _db.SaveChangesAsync();
                    return RedirectToPage("/Parametrage/Virement/Index");
                }


                if (Virements.Action == "Credit")
                {
                    var credit = await _db.Credits.Where(d => d.Num == Virements.ActionNum).SingleOrDefaultAsync();
                    var virement = await _db.Virements.Where(v => v.Id == Virements.Id).SingleOrDefaultAsync();

                    virement.Etat = "payé";
                    credit.MontantPayeeTotal += virement.Montant;

                    await _db.SaveChangesAsync();
                    return RedirectToPage("/Parametrage/Virement/Index");
                }


                await OnGet(Virements.Id);
                return Page();
            }
            catch (Exception)
            {
                await OnGet(Virements.Id);
                return Page();
            }
        }
    }
}