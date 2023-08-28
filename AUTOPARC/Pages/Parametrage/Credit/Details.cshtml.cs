using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace AUTOPARC.Pages.Parametrage.Credit
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;



        [BindProperty]
        public Credits Credits { get; set; }
        public Banques Banques { get; set; }
        public Vehicules Vehicules { get; set; }
        public List<ModePaiments> ModePaimentsList { get; set; }
        public List<Banques> BanquesList { get; set; }


        [BindProperty]
        public CreditsDetails CreditsDetails { get; set; }
        public List<CreditsDetails> CreditsDetailsList { get; set; }


        [BindProperty]
        public Cheques Cheques { get; set; }

        [BindProperty]
        public Virements Virements { get; set; }

        public bool check_details_credit_exception, check_all_checkboxes;
        public bool check_cheque_exception, check_numero_cheque_existance, check_cheque_date;
        public bool check_virement_exception;



        public async Task OnGet(int id)
        {
            Credits = await _db.Credits.FindAsync(id);
            Banques = await _db.Banques.Where(b => b.Id == Credits.BanqueId).SingleOrDefaultAsync();
            Vehicules = await _db.Vehicules.Where(v => v.Num == Credits.ActionNum).SingleOrDefaultAsync();
            CreditsDetailsList = await _db.CreditsDetails.Where(c => c.CreditId == Credits.Id).ToListAsync();
            ModePaimentsList = await _db.ModePaiments.ToListAsync();
            BanquesList = await _db.Banques.ToListAsync();
        }






        public async Task<IActionResult> OnPostUpdate()
        {
            if (ModelState.IsValid)
            {
                await OnGet(Credits.Id);
                return Page();
            }

            var credit = await _db.Credits.Where(c => c.Id == Credits.Id).SingleOrDefaultAsync();
            credit.Note = Credits.Note;
            credit.Etat = Credits.Etat;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Parametrage/Credit/Index");
        }





        public async Task<IActionResult> OnPostPayee()
        {
            bool isEspece = Request.Form.TryGetValue("Espece", out var especeValue);
            bool isCheque = Request.Form.TryGetValue("Cheque", out var chequeValue);
            bool isVirement = Request.Form.TryGetValue("Virement", out var virementValue);

            if (!isEspece && !isCheque && !isVirement)
            {
                check_all_checkboxes = true;
                await OnGet(CreditsDetails.CreditId);
                return Page();
            }

            if (!isEspece)
                CreditsDetails.MensualitePayeeEspece = 0;

            if (!isCheque)
                await RemoveChequeModelStateEntriesAsync();

            if (!isVirement)
                await RemoveVirementModelStateEntriesAsync();


            if (!ModelState.IsValid)
            {
                await OnGet(CreditsDetails.CreditId);
                return Page();
            }


            try
            {
                if (isCheque && !await InsertChequeAsync())
                    return Page();

                if (isVirement && !await InsertVirementAsync())
                    return Page();

                var credit = await _db.Credits.Where(c => c.Id == CreditsDetails.CreditId).SingleOrDefaultAsync();
                credit.MontantPayeeTotal += CreditsDetails.MensualitePayeeEspece;

                if (credit.Action == "Vehicule")
                {
                    var vehicule = await _db.Vehicules.Where(v => v.Num == credit.ActionNum).SingleOrDefaultAsync();
                    vehicule.MontantPayeeTotal += CreditsDetails.MensualitePayeeEspece;
                }


                if (credit.Action == "Document")
                {
                    var doc = await _db.Docs.Where(d => d.Num == credit.ActionNum).SingleOrDefaultAsync();
                    doc.MontantPayeeTotal += CreditsDetails.MensualitePayeeEspece;
                }


                if (credit.Montant == credit.MontantPayeeTotal)
                    credit.Etat = "payé";


                await _db.CreditsDetails.AddAsync(CreditsDetails);
                await _db.SaveChangesAsync();
                return RedirectToPage("/Parametrage/Credit/Index");
            }
            catch (System.Exception)
            {
                check_details_credit_exception = true;
                await OnGet(CreditsDetails.CreditId);
                return Page();
            }
        }




        private async Task<bool> InsertChequeAsync()
        {
            if (string.IsNullOrEmpty(Cheques.Numero) || string.IsNullOrEmpty(Cheques.AuNomDe) ||
                Cheques.DateReglement == null || Cheques.DateEcheance == null ||
                Cheques.Montant == 0 || Cheques.BanqueId == 0)
            {
                check_cheque_exception = true;
                await OnGet(CreditsDetails.CreditId);
                return false;
            }

            if (Cheques.DateReglement > Cheques.DateEcheance)
            {
                check_cheque_date = true;
                await OnGet(CreditsDetails.CreditId);
                return false;
            }

            try
            {
                CreditsDetails.MensualitePayeeCheque = Cheques.Montant;

                var credit = await _db.Credits.Where(c => c.Id == CreditsDetails.CreditId).SingleOrDefaultAsync();
                credit.MontantPayeeTotal += CreditsDetails.MensualitePayeeCheque;

                if (credit.Action == "Vehicule")
                {
                    var vehicule = await _db.Vehicules.Where(v => v.Num == credit.ActionNum).SingleOrDefaultAsync();
                    vehicule.MontantPayeeTotal += CreditsDetails.MensualitePayeeCheque;
                }


                if (credit.Action == "Document")
                {
                    var doc = await _db.Docs.Where(d => d.Num == credit.ActionNum).SingleOrDefaultAsync();
                    doc.MontantPayeeTotal += CreditsDetails.MensualitePayeeCheque;
                }

                Cheques.ActionNum = credit.Num;
                await _db.Cheques.AddAsync(Cheques);
                return true;
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException mySqlEx)
            {
                if (mySqlEx.Message.Contains("Numero"))
                {
                    check_numero_cheque_existance = true;
                    await OnGet(CreditsDetails.CreditId);
                }
                return false;
            }
            catch
            {
                check_cheque_exception = true;
                await OnGet(CreditsDetails.CreditId);
                return false;
            }
        }




        private async Task<bool> InsertVirementAsync()
        {
            if (string.IsNullOrEmpty(Virements.Rib) || string.IsNullOrEmpty(Virements.AuNomDe) ||
                Virements.Montant == 0 || Virements.BanqueId == 0 ||
                 Virements.DateVirement == null)
            {
                check_cheque_exception = true;
                await OnGet(CreditsDetails.CreditId);
                return false;
            }

            try
            {
                CreditsDetails.MensualitePayeeVirement = Virements.Montant;

                var credit = await _db.Credits.Where(c => c.Id == CreditsDetails.CreditId).SingleOrDefaultAsync();
                credit.MontantPayeeTotal += CreditsDetails.MensualitePayeeVirement;

                if (credit.Action == "Vehicule")
                {
                    var vehicule = await _db.Vehicules.Where(v => v.Num == credit.ActionNum).SingleOrDefaultAsync();
                    vehicule.MontantPayeeTotal += CreditsDetails.MensualitePayeeVirement;
                }


                if (credit.Action == "Document")
                {
                    var doc = await _db.Docs.Where(d => d.Num == credit.ActionNum).SingleOrDefaultAsync();
                    doc.MontantPayeeTotal += CreditsDetails.MensualitePayeeVirement;
                }

                Virements.ActionNum = credit.Num;
                await _db.Virements.AddAsync(Virements);
                return true;
            }
            catch
            {
                check_virement_exception = true;
                await OnGet(CreditsDetails.CreditId);
                return false;
            }
        }




        private async Task RemoveChequeModelStateEntriesAsync()
        {
            foreach (var key in ModelState.Keys.ToList())
                if (key.StartsWith("Cheques."))
                    ModelState.Remove(key);
        }
        private async Task RemoveVirementModelStateEntriesAsync()
        {
            foreach (var key in ModelState.Keys.ToList())
                if (key.StartsWith("Virements."))
                    ModelState.Remove(key);
        }
    }
}