using AUTOPARC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        private readonly IWebHostEnvironment _env;
        public DetailsModel(DBC db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }




        [BindProperty]
        public Vehicules Vehicules { get; set; }
        public List<Categories> CategoriesList { get; set; }
        public List<Modeles> ModelesList { get; set; }
        public List<TypeCarburants> TypeCarburantsList { get; set; }
        public List<EtatVehicules> EtatVehiculesList { get; set; }
        public List<Fournisseurs> FournisseursList { get; set; }
        public List<ModePaiments> ModePaimentsList { get; set; }
        public List<Banques> BanquesList { get; set; }
        public List<ImageVehicule> ImagesList { get; set; }


        [BindProperty]
        public Cheques Cheques { get; set; }

        [BindProperty]
        public Virements Virements { get; set; }

        [BindProperty]
        public Credits Credits { get; set; }

        [BindProperty]
        public ImageVehicule ImageVehicule { get; set; }

        public bool check_vehicule_exception, check_vehicule_matricule, check_all_checkboxes;
        public bool check_cheque_exception, check_numero_cheque_existance, check_cheque_date, check_cheque_payee;
        public bool check_virement_exception, check_virement_payee;
        public bool check_credit_exception, check_credit_montantParMoi, check_credit_date, check_credit_payee;




        public async Task OnGet(int id)
        {
            Vehicules = await _db.Vehicules.FindAsync(id);
            CategoriesList = await _db.Categories.ToListAsync();
            ModelesList = await _db.Modeles.Where(x => x.MarqueId == Vehicules.MarqueId).ToListAsync();
            TypeCarburantsList = await _db.TypeCarburants.ToListAsync();
            EtatVehiculesList = await _db.EtatVehicules.ToListAsync();
            FournisseursList = await _db.Fournisseurs.ToListAsync();
            ModePaimentsList = await _db.ModePaiments.ToListAsync();
            BanquesList = await _db.Banques.ToListAsync();
            ImagesList = await _db.ImageVehicule.Where(i => i.VehiculeNum == Vehicules.Num).ToListAsync();

            Cheques = await _db.Cheques.Where(chq => chq.Action == "Vehicule" && chq.ActionNum == Vehicules.Num).SingleOrDefaultAsync();
            Virements = await _db.Virements.Where(v => v.Action == "Vehicule" && v.ActionNum == Vehicules.Num).SingleOrDefaultAsync();
            Credits = await _db.Credits.Where(c => c.Action == "Vehicule" && c.ActionNum == Vehicules.Num).SingleOrDefaultAsync();
        }




        public async Task<IActionResult> OnPostUpdate()
        {
            bool isEspece = Request.Form.TryGetValue("Espece", out var especeValue);
            bool isCheque = Request.Form.TryGetValue("Cheque", out var chequeValue);
            bool isVirement = Request.Form.TryGetValue("Virement", out var virementValue);
            bool isCredit = Request.Form.TryGetValue("Credit", out var creditValue);

            if (!isCheque)
                await RemoveChequeModelStateEntriesAsync();

            if (!isVirement)
                await RemoveVirementModelStateEntriesAsync();

            if (!isCredit)
                await RemoveCreditModelStateEntriesAsync();

            try
            {





                var vehicule = await _db.Vehicules.FindAsync(Vehicules.Id);
                vehicule.Matricule = Vehicules.Matricule;
                vehicule.CarburantId = Vehicules.CarburantId;
                vehicule.MarqueId = Vehicules.MarqueId;
                vehicule.ModeleId = Vehicules.ModeleId;
                vehicule.CarburantId = Vehicules.CarburantId;
                vehicule.EtatVehiculeId = Vehicules.EtatVehiculeId;
                vehicule.FrsId = Vehicules.FrsId;
                vehicule.PrixAchat = Vehicules.PrixAchat;
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


                if (isCheque && !await UpdateChequeAsync())
                    return Page();

                if (isVirement && !await UpdateVirementAsync())
                    return Page();

                if (isCredit && !await UpdateCreditAsync())
                    return Page();


                vehicule.MontantPayeeTotal -= vehicule.MontantPayeeEspece;
                vehicule.MontantPayeeEspece = Vehicules.MontantPayeeEspece;
                vehicule.MontantPayeeCheque = Vehicules.MontantPayeeCheque;
                vehicule.MontantPayeeVirement = Vehicules.MontantPayeeVirement;
                vehicule.MontantPayeeTotal += vehicule.MontantPayeeEspece;

                await _db.SaveChangesAsync();
                return RedirectToPage("/Vehicule/Index");
            }
            catch (Exception)
            {
                check_vehicule_exception = true;
                await OnGet(Vehicules.Id);
                return Page();
            }
        }




        public async Task<IActionResult> OnPostDelete()
        {
            //if (await Check_ChequePayee())
            //    return Page();

            if (!await TryDeleteCheck())
                return Page();

            if (!await TryDeleteVirement())
                return Page();

            if (!await TryDeleteCredit())
                return Page();

            _db.Vehicules.Remove(Vehicules);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Index");
        }





        private async Task<bool> UpdateChequeAsync()
        {
            if (string.IsNullOrEmpty(Cheques.Numero) || string.IsNullOrEmpty(Cheques.AuNomDe) ||
                Cheques.DateReglement == null || Cheques.DateEcheance == null ||
                Cheques.Montant <= 0 || Cheques.BanqueId == 0)
            {
                check_cheque_exception = true;
                await OnGet(Vehicules.Id);
                return false;
            }

            if (Cheques.DateReglement > Cheques.DateEcheance)
            {
                check_cheque_date = true;
                await OnGet(Vehicules.Id);
                return false;
            }

            try
            {
                var cheque = await _db.Cheques.Where(chq => chq.Action == "Vehicule" && chq.ActionNum == Vehicules.Num).FirstOrDefaultAsync();

                if (cheque != null)
                {
                    cheque.BanqueId = Cheques.BanqueId;
                    cheque.Numero = Cheques.Numero;
                    cheque.Montant = Cheques.Montant;
                    cheque.AuNomDe = Cheques.AuNomDe;
                    cheque.DateReglement = Cheques.DateReglement;
                    cheque.DateEcheance = Cheques.DateEcheance;
                    cheque.DateValeur = Cheques.DateValeur;
                    Vehicules.MontantPayeeCheque = Cheques.Montant;

                    return true;
                }

                Vehicules.MontantPayeeCheque = Cheques.Montant;
                Cheques.ActionNum = Vehicules.Num;
                await _db.Cheques.AddAsync(Cheques);
                return true;
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException mySqlEx)
            {
                if (mySqlEx.Message.Contains("Numero"))
                {
                    check_numero_cheque_existance = true;
                    await OnGet(Vehicules.Id);
                }
                return false;
            }
            catch
            {
                check_cheque_exception = true;
                await OnGet(Vehicules.Id);
                return false;
            }
        }






        private async Task<bool> UpdateVirementAsync()
        {
            if (string.IsNullOrEmpty(Virements.Rib) || string.IsNullOrEmpty(Virements.AuNomDe) ||
                Virements.DateVirement == null || Virements.Montant <= 0 || Virements.BanqueId == 0)
            {
                check_virement_exception = true;
                await OnGet(Vehicules.Id);
                return false;
            }

            try
            {
                var virement = await _db.Virements.Where(v => v.Action == "Vehicule" && v.ActionNum == Vehicules.Num).FirstOrDefaultAsync();

                if (virement != null)
                    return true;

                Vehicules.MontantPayeeVirement = Virements.Montant;
                Vehicules.MontantPayeeTotal += Virements.Montant;
                Virements.ActionNum = Vehicules.Num;
                await _db.Virements.AddAsync(Virements);
                return true;
            }
            catch
            {
                check_virement_exception = true;
                await OnGet(Vehicules.Id);
                return false;
            }
        }






        private async Task<bool> UpdateCreditAsync()
        {
            if (Credits.Montant <= 0 || Credits.Mensualite <= 0 || Credits.BanqueId == 0 ||
                Credits.DateDebut == null || Credits.DateFin == null)
            {
                check_credit_exception = true;
                await OnGet(Vehicules.Id);
                return false;
            }

            try
            {
                var credit = await _db.Credits.Where(c => c.Action == "Vehicule" && c.ActionNum == Vehicules.Num).FirstOrDefaultAsync();

                if (credit != null)
                {
                    credit.Mensualite = Credits.Mensualite;
                    credit.DateFin = Credits.DateFin;
                    return true;
                }

                Vehicules.MontantPayeeCredit = Credits.Montant;
                Credits.ActionNum = Vehicules.Num;
                Credits.Etat = "impayé";
                await _db.Credits.AddAsync(Credits);
                return true;
            }
            catch
            {
                check_credit_exception = true;
                await OnGet(Vehicules.Id);
                return false;
            }
        }





        private async Task<bool> TryDeleteCheck()
        {
            var cheque = await _db.Cheques.Where(chq => chq.Action == "Vehicule" && chq.ActionNum == Vehicules.Num).FirstOrDefaultAsync();
            if (cheque != null && cheque.Etat != "payé")
            {
                _db.Cheques.Remove(cheque);
                return true;
            }
            else if(cheque != null && cheque.Etat == "payé")
            {
                check_cheque_payee = true;
                await OnGet(Vehicules.Id);
                return false;
            }
            return true;
        }

        private async Task<bool> TryDeleteVirement()
        {
            var virement = await _db.Virements.Where(v => v.Action == "Vehicule" && v.ActionNum == Vehicules.Num).FirstOrDefaultAsync();
            if (virement != null && virement.Etat != "payé")
            {
                _db.Virements.Remove(virement);
                return true;
            }
            else if (virement != null && virement.Etat == "payé")
            {
                check_virement_payee = true;
                await OnGet(Vehicules.Id);
                return false;
            }
            return true;
        }

        private async Task<bool> TryDeleteCredit()
        {
            var credit = await _db.Credits.Where(c => c.Action == "Vehicule" && c.ActionNum == Vehicules.Num).FirstOrDefaultAsync();

            if (credit == null)
                return true;

            check_credit_payee = true;
            await OnGet(Vehicules.Id);
            return false;
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
        private async Task RemoveCreditModelStateEntriesAsync()
        {
            foreach (var key in ModelState.Keys.ToList())
                if (key.StartsWith("Credits."))
                    ModelState.Remove(key);
        }
    }
}
