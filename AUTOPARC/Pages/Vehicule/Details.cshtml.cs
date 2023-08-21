using AUTOPARC.Models;
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
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public Vehicules Vehicules { get; set; }
        public List<Categories> Categories { get; set; }
        public List<Modeles> ModelesList { get; set; }
        public List<TypeCarburants> TypeCarburantsList { get; set; }
        public List<EtatVehicules> EtatVehiculesList { get; set; }
        public List<Fournisseurs> FournisseursList { get; set; }
        public List<ModePaiments> ModePaimentsList { get; set; }


        [BindProperty]
        public Cheques Cheques { get; set; }
        public List<Banques> BanquesList { get; set; }

        public bool check_exception, check_Prix_MontantPayee;
        public bool check_cheque_exception, check_numero_cheque_existance, check_montant, check_date, check_etat_paye;




        public async Task OnGet(int id)
        {
            Vehicules = await _db.Vehicules.FindAsync(id);
            Categories = await _db.Categories.ToListAsync();
            ModelesList = await _db.Modeles.Where(x => x.MarqueId == Vehicules.MarqueId).ToListAsync();
            TypeCarburantsList = await _db.TypeCarburants.ToListAsync();
            EtatVehiculesList = await _db.EtatVehicules.ToListAsync();
            FournisseursList = await _db.Fournisseurs.ToListAsync();
            ModePaimentsList = await _db.ModePaiments.ToListAsync();
            Cheques = await _db.Cheques.Where(chq => chq.Action == "Vehicule" && chq.ActionNum == Vehicules.Num).SingleOrDefaultAsync();
            BanquesList = await _db.Banques.ToListAsync();
        }




        public async Task<IActionResult> OnPostUpdate()
        {
            if (await Check_ChequePayee())
                return Page();


            if (Vehicules.PrixAchat < Vehicules.MontantPayeeEspece)
            {
                check_Prix_MontantPayee = true;
                await OnGet(Vehicules.Id);
                return Page();
            }


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
                check_exception = true;
                await OnGet(Vehicules.Id);
                return Page();
            }
        }




        public async Task<IActionResult> OnPostDelete()
        {
            if (await Check_ChequePayee())
                return Page();

            await TryDeleteCheck();

            _db.Vehicules.Remove(Vehicules);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Index");
        }






        private async Task<bool> TryUpdateCheque()
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
                check_montant = true;
                await OnGet(Vehicules.Id);
                return false;
            }

            if (Cheques.DateReglement > Cheques.DateEcheance)
            {
                check_date = true;
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






        private async Task<bool> RemoveChequeModelStateEntriesAsync()
        {
            if (Cheques.Id != 0)
            {
                await TryDeleteCheck();
                return true;
            }
            else
            {
                foreach (var key in ModelState.Keys.ToList())
                    if (key.StartsWith("Cheques."))
                        ModelState.Remove(key);
                return true;
            }
        }






        private async Task<bool> Check_ChequePayee()
        {
            var cheque = await _db.Cheques.Where(chq => chq.Action == "Vehicule" && chq.ActionNum == Vehicules.Num).FirstOrDefaultAsync();
            if (cheque != null && cheque.Etat == "payé")
            {
                check_etat_paye = true;
                await OnGet(Vehicules.Id);
                return true;
            }
            return false;
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
    }
}
