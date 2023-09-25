using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
        public List<Banques> BanquesList { get; set; }

        public bool check_cheque_exception, check_cheque_date, check_numero_cheque_existance, check_cheque_payee;



        public async Task OnGet(int id)
        {
            Cheques = await _db.Cheques.FindAsync(id);
            BanquesList = await _db.Banques.ToListAsync();
        }






        public async Task<IActionResult> OnPostUpdate()
        {
            if (string.IsNullOrEmpty(Cheques.Numero) || string.IsNullOrEmpty(Cheques.AuNomDe) ||
                Cheques.DateReglement == null || Cheques.DateEcheance == null ||
                Cheques.Montant <= 0 || Cheques.BanqueId == 0)
            {
                check_cheque_exception = true;
                await OnGet(Cheques.Id);
                return Page();
            }

            if (Cheques.DateReglement > Cheques.DateEcheance)
            {
                check_cheque_date = true;
                await OnGet(Cheques.Id);
                return Page();
            }

            try
            {
                if (Cheques.Action == "Vehicule")
                {
                    var vehicule = await _db.Vehicules.Where(v => v.Num == Cheques.ActionNum).SingleOrDefaultAsync();
                    var cheque = await _db.Cheques.Where(chq => chq.Id == Cheques.Id).SingleOrDefaultAsync();

                    cheque.Etat = Cheques.Etat;
                    cheque.Numero = Cheques.Numero;
                    cheque.BanqueId = Cheques.BanqueId;
                    cheque.Montant = Cheques.Montant;
                    cheque.AuNomDe = Cheques.AuNomDe;
                    cheque.DateReglement = Cheques.DateReglement;
                    cheque.DateEcheance = Cheques.DateEcheance;
                    cheque.DateValeur = Cheques.DateValeur;
                    vehicule.MontantPayeeCheque = Cheques.Montant;


                    await _db.SaveChangesAsync();
                    await OnGet(Cheques.Id);
                    return Page();
                }




                if (Cheques.Action == "Maintenance")
                {
                    var maintenance = await _db.Maintenances.Where(m => m.Num == Cheques.ActionNum).SingleOrDefaultAsync();
                    var cheque = await _db.Cheques.Where(chq => chq.Id == Cheques.Id).SingleOrDefaultAsync();

                    cheque.BanqueId = Cheques.BanqueId;
                    cheque.Numero = Cheques.Numero;
                    cheque.AuNomDe = Cheques.AuNomDe;
                    cheque.Montant = Cheques.Montant;
                    cheque.DateReglement = Cheques.DateReglement;
                    cheque.DateEcheance = Cheques.DateEcheance;
                    cheque.DateValeur = Cheques.DateValeur;

                    maintenance.MontantPayeeCheque = Cheques.Montant;
                    await _db.SaveChangesAsync();

                    await OnGet(Cheques.Id);
                    return Page();
                }



                
                await OnGet(Cheques.Id);
                return Page();
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException mySqlEx)
            {
                if (mySqlEx.Message.Contains("Numero"))
                    check_numero_cheque_existance = true;

                await OnGet(Cheques.Id);
                return Page();
            }
            catch
            {
                check_cheque_exception = true;
                await OnGet(Cheques.Id);
                return Page();
            }
        }






        public async Task<IActionResult> OnPostDelete()
        {
            if (Cheques.Action == "Vehicule")
            {
                var cheque = await _db.Cheques.Where(chq => chq.Id == Cheques.Id).SingleOrDefaultAsync();

                if (cheque.Etat == "payé")
                {
                    check_cheque_payee = true;
                    await OnGet(Cheques.Id);
                    return Page();
                }

                var vehicule = await _db.Vehicules.Where(v => v.Num == cheque.ActionNum).SingleOrDefaultAsync();
                vehicule.MontantPayeeCheque = 0;
                _db.Cheques.Remove(cheque);
                await _db.SaveChangesAsync();
                return RedirectToPage("/Parametrage/Cheque/Index");
            }


            if (Cheques.Action == "Maintenance")
            {
                var cheque = await _db.Cheques.Where(chq => chq.Id == Cheques.Id).SingleOrDefaultAsync();

                if (cheque.Etat == "payé")
                {
                    check_cheque_payee = true;
                    await OnGet(Cheques.Id);
                    return Page();
                }

                var maintenance = await _db.Maintenances.Where(m => m.Num == cheque.ActionNum).SingleOrDefaultAsync();
                maintenance.MontantPayeeCheque = 0;
                _db.Cheques.Remove(cheque);
                await _db.SaveChangesAsync();
                return RedirectToPage("/Parametrage/Cheque/Index");
            }


            await OnGet(Cheques.Id);
            return Page();
        }











        public async Task<IActionResult> OnPostPayee()
        {
            if (string.IsNullOrEmpty(Cheques.Numero) || string.IsNullOrEmpty(Cheques.AuNomDe) ||
                Cheques.DateReglement == null || Cheques.DateEcheance == null ||
                Cheques.Montant <= 0 || Cheques.BanqueId == 0)
            {
                check_cheque_exception = true;
                await OnGet(Cheques.Id);
                return Page();
            }

            try
            {
                if (Cheques.Action == "Vehicule")
                {
                    var cheque = await _db.Cheques.Where(chq => chq.Id == Cheques.Id).SingleOrDefaultAsync();
                    var vehicules = await _db.Vehicules.Where(v => v.Num == cheque.ActionNum).SingleOrDefaultAsync();

                    cheque.Etat = "payé";
                    vehicules.MontantPayeeTotal += cheque.Montant;

                    await _db.SaveChangesAsync();
                    await OnGet(Cheques.Id);
                    return Page();
                }


                if (Cheques.Action == "Maintenance")
                {
                    var cheque = await _db.Cheques.Where(chq => chq.Id == Cheques.Id).SingleOrDefaultAsync();
                    var maintenance = await _db.Maintenances.Where(m => m.Num == cheque.ActionNum).SingleOrDefaultAsync();

                    cheque.Etat = "payé";
                    maintenance.MontantPayeeTotal += cheque.Montant;

                    await _db.SaveChangesAsync();
                    await OnGet(Cheques.Id);
                    return Page();
                }


                if (Cheques.Action == "Document")
                {
                    var cheque = await _db.Cheques.Where(chq => chq.Id == Cheques.Id).SingleOrDefaultAsync();
                    var doc = await _db.Docs.Where(d => d.Num == cheque.ActionNum).SingleOrDefaultAsync();

                    cheque.Etat = "payé";
                    doc.MontantPayeeTotal += cheque.Montant;

                    await _db.SaveChangesAsync();
                    await OnGet(Cheques.Id);
                    return Page();
                }


                if (Cheques.Action == "Credit")
                {
                    var cheque = await _db.Virements.Where(chq => chq.Id == Cheques.Id).SingleOrDefaultAsync();
                    var credit = await _db.Credits.Where(d => d.Num == cheque.ActionNum).SingleOrDefaultAsync();

                    cheque.Etat = "payé";
                    credit.MontantPayeeTotal += cheque.Montant;

                    await _db.SaveChangesAsync();
                    await OnGet(Cheques.Id);
                    return Page();
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
