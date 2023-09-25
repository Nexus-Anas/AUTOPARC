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
        public List<Societes> SocietesList { get; set; }
        public List<ImageVehicule> ImagesList { get; set; }


        [BindProperty]
        public Cheques Cheques { get; set; }

        [BindProperty]
        public Virements Virements { get; set; }

        [BindProperty]
        public Credits Credits { get; set; }


        public bool check_vehicule_exception, check_vehicule_matricule, check_operation_existance;
        public bool check_cheque_exception, check_numero_cheque_existance, check_cheque_date;
        public bool check_virement_exception;
        public bool check_credit_exception, check_credit_montantParMoi, check_credit_date;
        public bool check_cheque_existance, check_virement_existance, check_credit_existance;

        public bool operatons_exist;



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
            SocietesList = await _db.Societes.ToListAsync();
            ImagesList = await _db.ImageVehicule.Where(i => i.VehiculeNum == Vehicules.Num).ToListAsync();

            Cheques = await _db.Cheques.Where(chq => chq.Action == "Vehicule" && chq.ActionNum == Vehicules.Num).SingleOrDefaultAsync();
            Virements = await _db.Virements.Where(v => v.Action == "Vehicule" && v.ActionNum == Vehicules.Num).SingleOrDefaultAsync();
            Credits = await _db.Credits.Where(c => c.Action == "Vehicule" && c.ActionNum == Vehicules.Num).SingleOrDefaultAsync();

            var maintenance = await _db.Maintenances.Where(m => m.VehiculeId == Vehicules.Id).FirstOrDefaultAsync();
            var document = await _db.Docs.Where(d => d.VehiculeId == Vehicules.Id).FirstOrDefaultAsync();
            var carburant = await _db.RechargeCarburants.Where(rc => rc.VehiculeId == Vehicules.Id).FirstOrDefaultAsync();
            var cession = await _db.Cessions.Where(c => c.VehiculeId == Vehicules.Id).FirstOrDefaultAsync();
            if (maintenance != null || document != null || carburant != null || cession != null || Cheques != null || Virements != null || Credits != null)
            {
                operatons_exist = true;
            }
        }




        public async Task<IActionResult> OnPostUpdate()
        {
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


                if (isCheque && !await InsertChequeAsync())
                    return Page();

                if (isVirement && !await InsertVirementAsync())
                    return Page();

                if (isCredit && !await InsertCreditAsync())
                    return Page();

                if (vehicule.MontantPayeeEspece != Vehicules.MontantPayeeEspece)
                {
                    Vehicules.MontantPayeeTotal -= vehicule.MontantPayeeEspece;
                    vehicule.MontantPayeeEspece = Vehicules.MontantPayeeEspece;
                    Vehicules.MontantPayeeTotal += vehicule.MontantPayeeEspece;
                }
                vehicule.MontantPayeeCheque = Vehicules.MontantPayeeCheque;
                vehicule.MontantPayeeVirement = Vehicules.MontantPayeeVirement;
                vehicule.MontantPayeeCredit = Vehicules.MontantPayeeCredit;
                vehicule.MontantPayeeTotal = Vehicules.MontantPayeeTotal;

                if (!await CheckOperationExistance())
                    vehicule.SocieteId = Vehicules.SocieteId;

                if (!await InsertImagesAsync(Vehicules.Num))
                    return Page();

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
            if (await CheckPaiementExistance())
                return Page();

            if (await CheckOperationExistance())
                return Page();

            _db.Vehicules.Remove(Vehicules);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Vehicule/Index");
        }






        private async Task<bool> CheckPaiementExistance()
        {
            if (Vehicules.MontantPayeeCheque != 0)
            {
                check_cheque_existance = true;
                await OnGet(Vehicules.Id);
                return true;
            }
            if (Vehicules.MontantPayeeVirement != 0)
            {
                check_virement_existance = true;
                await OnGet(Vehicules.Id);
                return true;
            }
            if (Vehicules.MontantPayeeCredit != 0)
            {
                check_credit_existance = true;
                await OnGet(Vehicules.Id);
                return true;
            }
            return false;
        }

        private async Task<bool> CheckOperationExistance()
        {
            var maintenance = await _db.Maintenances.Where(m => m.VehiculeId == Vehicules.Id).FirstOrDefaultAsync();
            var document = await _db.Docs.Where(d => d.VehiculeId == Vehicules.Id).FirstOrDefaultAsync();
            var carburant = await _db.RechargeCarburants.Where(rc => rc.VehiculeId == Vehicules.Id).FirstOrDefaultAsync();
            var cession = await _db.Cessions.Where(c => c.VehiculeId == Vehicules.Id).FirstOrDefaultAsync();
            if (maintenance != null || document != null || carburant != null || cession != null)
            {
                check_operation_existance = true;
                await OnGet(Vehicules.Id);
                return true;
            }
            return false;
        }






        private async Task<bool> InsertChequeAsync()
        {
            if (Vehicules.MontantPayeeCheque != 0)
                return true;

            if (string.IsNullOrEmpty(Cheques.Numero) || string.IsNullOrEmpty(Cheques.AuNomDe) ||
                Cheques.DateReglement == null || Cheques.DateEcheance == null ||
                Cheques.Montant == 0 || Cheques.BanqueId == 0)
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
                await OnGet(Vehicules.Id);
                return false;
            }
        }






        private async Task<bool> InsertVirementAsync()
        {
            if (Vehicules.MontantPayeeVirement != 0)
                return true;

            if (string.IsNullOrEmpty(Virements.Rib) || string.IsNullOrEmpty(Virements.AuNomDe) ||
                Virements.Montant == 0 || Virements.BanqueId == 0 ||
                 Virements.DateVirement == null)
            {
                check_cheque_exception = true;
                await OnGet(Vehicules.Id);
                return false;
            }

            try
            {
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






        private async Task<bool> InsertCreditAsync()
        {
            if (Vehicules.MontantPayeeCredit != 0)
                return true;

            if (Credits.Montant == 0 || Credits.Mensualite == 0 || Credits.BanqueId == 0 ||
                Credits.DateDebut == null || Credits.DateFin == null)
            {
                check_credit_exception = true;
                await OnGet(Vehicules.Id);
                return false;
            }

            if (Credits.Montant < Credits.Mensualite)
            {
                check_credit_montantParMoi = true;
                await OnGet(Vehicules.Id);
                return false;
            }

            if (Credits.DateDebut > Credits.DateFin)
            {
                check_credit_date = true;
                await OnGet(Vehicules.Id);
                return false;
            }

            try
            {
                var num = await _db.Credits.OrderByDescending(n => n.Num).FirstOrDefaultAsync();
                Credits.Num = num != null ? num.Num + 1 : 1;
                Vehicules.MontantPayeeCredit = Credits.Montant;
                Credits.ActionNum = Vehicules.Num;
                await _db.Credits.AddAsync(Credits);
                return true;
            }
            catch
            {
                check_virement_exception = true;
                await OnGet(Vehicules.Id);
                return false;
            }
        }






        private async Task<bool> InsertImagesAsync(int vehicule_num)
        {
            try
            {
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        if (file != null && file.Length > 0)
                        {
                            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                            var filePath = Path.Combine(_env.WebRootPath, "images", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                                await file.CopyToAsync(stream);

                            var image = new ImageVehicule
                            {
                                Url = fileName,
                                VehiculeNum = vehicule_num
                            };
                            _db.ImageVehicule.Add(image);
                        }
                    }
                }
            }
            catch (Exception)
            {
                check_vehicule_exception = true;
                await OnGet(Vehicules.MarqueId);
                return false;
            }
            return true;
        }






        private async Task RemoveChequeModelStateEntriesAsync()
        {
            await Task.Run(() =>
            {
                foreach (var key in ModelState.Keys.ToList())
                    if (key.StartsWith("Cheques."))
                        ModelState.Remove(key);
            });
        }


        private async Task RemoveVirementModelStateEntriesAsync()
        {
            await Task.Run(() =>
            {
                foreach (var key in ModelState.Keys.ToList())
                    if (key.StartsWith("Virements."))
                        ModelState.Remove(key);
            });
        }


        private async Task RemoveCreditModelStateEntriesAsync()
        {
            await Task.Run(() =>
            {
                foreach (var key in ModelState.Keys.ToList())
                    if (key.StartsWith("Credits."))
                        ModelState.Remove(key);
            });
        }



    }
}
