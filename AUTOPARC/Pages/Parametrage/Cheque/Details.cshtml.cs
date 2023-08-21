using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Parametrage.Cheque
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public Cheques Cheques { get; set; }
        public Banques Banques { get; set; }



        public async Task OnGet(int id)
        {
            Cheques = await _db.Cheques.FindAsync(id);
            Banques = await _db.Banques.Where(b => b.Id == Cheques.BanqueId).SingleOrDefaultAsync();
        }




        public async Task<IActionResult> OnPostPayee()
        {
            try
            {
                if (Cheques.Action == "Maintenance")
                {
                    var maintenance = await _db.Maintenances.Where(m => m.Num == Cheques.ActionNum).SingleOrDefaultAsync();
                    var cheque = await _db.Cheques.Where(chq => chq.Id == Cheques.Id).SingleOrDefaultAsync();

                    cheque.Etat = "payé";
                    maintenance.MontantPayeeTotal += cheque.Montant;

                    await _db.SaveChangesAsync();
                    return RedirectToPage("/Parametrage/Cheque/Index");
                }


                if (Cheques.Action == "Vehicule")
                {
                    var vehicules = await _db.Vehicules.Where(v => v.Num == Cheques.ActionNum).SingleOrDefaultAsync();
                    var cheque = await _db.Cheques.Where(chq => chq.Id == Cheques.Id).SingleOrDefaultAsync();

                    cheque.Etat = "payé";
                    vehicules.MontantPayeeTotal += cheque.Montant;

                    await _db.SaveChangesAsync();
                    return RedirectToPage("/Parametrage/Cheque/Index");
                }


                await OnGet(Cheques.Id);
                return Page();
            }
            catch (Exception)
            {
                await OnGet(Cheques.Id);
                return Page();
            }
        }
    }
}
