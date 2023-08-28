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
        public bool check_cheque_exception, check_numero_cheque_existance, check_cheque_date;
        public bool check_virement_exception;
        public bool check_credit_exception, check_credit_montantParMoi, check_credit_date;




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

            if (!isEspece && !isCheque && !isVirement && !isCredit)
            {
                check_all_checkboxes = true;
                await OnGet(Vehicules.MarqueId);
                return Page();
            }


            //if (await Check_ChequePayee())
            //    return Page();


            //if (Vehicules.PrixAchat < Vehicules.MontantPayeeEspece)
            //{
            //    check_Prix_MontantPayee = true;
            //    await OnGet(Vehicules.Id);
            //    return Page();
            //}


            //var modePaiement = await _db.ModePaiments.Where(m => m.Id == Vehicules.ModePayementId).Select(m => m.Mode).SingleOrDefaultAsync();
            //if (modePaiement == "Espece")
            //    await RemoveChequeModelStateEntriesAsync();
            //else if (!await TryUpdateCheque())
            //    return Page();


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
                vehicule.MontantPayeeEspece = Vehicules.MontantPayeeEspece;
                //vehicule.ModePayementId = Vehicules.ModePayementId;
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
            catch (Exception)
            {
                //check_exception = true;
                await OnGet(Vehicules.Id);
                return Page();
            }
        }




        public async Task<IActionResult> OnPostDelete()
        {
            //if (await Check_ChequePayee())
            //    return Page();

            await TryDeleteCheck();

            _db.Vehicules.Remove(Vehicules);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Index");
        }






        private async Task<bool> UpdateCheque()
        {
            if (string.IsNullOrEmpty(Cheques.Numero) || string.IsNullOrEmpty(Cheques.AuNomDe) ||
                Cheques.DateReglement == null || Cheques.DateEcheance == null ||
                Cheques.Montant == 0 || Cheques.BanqueId == 0)
            {
                check_cheque_exception = true;
                await OnGet(Vehicules.Id);
                return false;
            }

            if (Vehicules.PrixAchat != (Vehicules.MontantPayeeEspece + Cheques.Montant))
            {
                //check_montant = true;
                await OnGet(Vehicules.Id);
                return false;
            }

            if (Cheques.DateReglement > Cheques.DateEcheance)
            {
                //check_date = true;
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

                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    Cheques.ActionNum = Vehicules.Num;
                    Cheques.Etat = "impayé";
                    await _db.Cheques.AddAsync(Cheques);
                    await _db.SaveChangesAsync();
                    return true;
                }
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











        private async Task TryDeleteCheck()
        {
            var cheque = await _db.Cheques.Where(chq => chq.Action == "Vehicule" && chq.ActionNum == Vehicules.Num).FirstOrDefaultAsync();
            if (cheque != null)
            {
                _db.Cheques.Remove(cheque);
                await _db.SaveChangesAsync();
            }
        }






















        private async Task<bool> InsertChequeAsync()
        {
            if (string.IsNullOrEmpty(Cheques.Numero) || string.IsNullOrEmpty(Cheques.AuNomDe) ||
                Cheques.DateReglement == null || Cheques.DateEcheance == null ||
                Cheques.Montant == 0 || Cheques.BanqueId == 0)
            {
                check_cheque_exception = true;
                await OnGet(Vehicules.MarqueId);
                return false;
            }

            if (Cheques.DateReglement > Cheques.DateEcheance)
            {
                check_cheque_date = true;
                await OnGet(Vehicules.MarqueId);
                return false;
            }

            try
            {
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
                    await OnGet(Vehicules.MarqueId);
                }
                return false;
            }
            catch
            {
                check_cheque_exception = true;
                await OnGet(Vehicules.MarqueId);
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
                await OnGet(Vehicules.MarqueId);
                return false;
            }

            try
            {
                Vehicules.MontantPayeeVirement = Virements.Montant;
                Virements.ActionNum = Vehicules.Num;
                await _db.Virements.AddAsync(Virements);
                return true;
            }
            catch
            {
                check_virement_exception = true;
                await OnGet(Vehicules.MarqueId);
                return false;
            }
        }




        private async Task<bool> InsertCreditAsync()
        {
            if (Credits.Montant == 0 || Credits.Mensualite == 0 || Credits.BanqueId == 0 ||
                Credits.DateDebut == null || Credits.DateFin == null)
            {
                check_credit_exception = true;
                await OnGet(Vehicules.MarqueId);
                return false;
            }

            if (Credits.Montant < Credits.Mensualite)
            {
                check_credit_montantParMoi = true;
                await OnGet(Vehicules.MarqueId);
                return false;
            }

            if (Credits.DateDebut > Credits.DateFin)
            {
                check_credit_date = true;
                await OnGet(Vehicules.MarqueId);
                return false;
            }

            try
            {
                Vehicules.MontantPayeeCredit = Credits.Montant;
                Credits.ActionNum = Vehicules.Num;
                await _db.Credits.AddAsync(Credits);
                return true;
            }
            catch
            {
                check_virement_exception = true;
                await OnGet(Vehicules.MarqueId);
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
        private async Task RemoveCreditModelStateEntriesAsync()
        {
            foreach (var key in ModelState.Keys.ToList())
                if (key.StartsWith("Credits."))
                    ModelState.Remove(key);
        }
    }
}
