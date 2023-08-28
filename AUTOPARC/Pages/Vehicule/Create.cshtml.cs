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
    public class CreateModel : PageModel
    {
        private readonly DBC _db;
        private readonly IWebHostEnvironment _env;
        public CreateModel(DBC db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }




        [BindProperty]
        public Vehicules Vehicules { get; set; }
        public Marques Marques { get; set; }
        public List<Categories> CategoriesList { get; set; }
        public List<Modeles> ModelesList { get; set; }
        public List<TypeCarburants> TypeCarburantsList { get; set; }
        public List<EtatVehicules> EtatVehiculesList { get; set; }
        public List<Fournisseurs> FournisseursList { get; set; }
        public List<ModePaiments> ModePaimentsList { get; set; }
        public List<Banques> BanquesList { get; set; }


        [BindProperty]
        public Cheques Cheques { get; set; }

        [BindProperty]
        public Virements Virements { get; set; }

        [BindProperty]
        public Credits Credits { get; set; }


        public bool check_vehicule_exception, check_vehicule_matricule, check_all_checkboxes;
        public bool check_cheque_exception, check_numero_cheque_existance, check_cheque_date;
        public bool check_virement_exception;
        public bool check_credit_exception, check_credit_montantParMoi, check_credit_date;




        public async Task OnGet(int id)
        {
            Marques = await _db.Marques.Where(m => m.Id == id).SingleOrDefaultAsync();
            CategoriesList = await _db.Categories.ToListAsync();
            ModelesList = await _db.Modeles.Where(x => x.MarqueId == id).ToListAsync();
            TypeCarburantsList = await _db.TypeCarburants.ToListAsync();
            EtatVehiculesList = await _db.EtatVehicules.ToListAsync();
            FournisseursList = await _db.Fournisseurs.ToListAsync();
            ModePaimentsList = await _db.ModePaiments.ToListAsync();
            BanquesList = await _db.Banques.ToListAsync();
        }




        public async Task<IActionResult> OnPostCreate()
        {
            var num = await _db.Vehicules.OrderByDescending(n => n.Num).FirstOrDefaultAsync();
            Vehicules.Num = num != null ? num.Num + 1 : 1;

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

            if (!isCheque)
                await RemoveChequeModelStateEntriesAsync();

            if (!isVirement)
                await RemoveVirementModelStateEntriesAsync();

            if (!isCredit)
                await RemoveCreditModelStateEntriesAsync();


            if (!ModelState.IsValid)
            {
                await OnGet(Vehicules.MarqueId);
                return Page();
            }


            try
            {
                if (!await InsertImagesAsync())
                    return Page();

                Vehicules.MontantPayeeTotal += Vehicules.MontantPayeeEspece;
                await _db.Vehicules.AddAsync(Vehicules);

                if (isCheque && !await InsertChequeAsync())
                    return Page();

                if (isVirement && !await InsertVirementAsync())
                    return Page();

                if (isCredit && !await InsertCreditAsync())
                    return Page();

                

                await _db.SaveChangesAsync();
                return RedirectToPage("/Vehicule/Index");
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException mySqlEx)
            {
                if (mySqlEx.Message.Contains("Matricule"))
                {
                    check_vehicule_matricule = true;
                    await OnGet(Vehicules.MarqueId);
                }
                return Page();
            }
            catch (System.Exception)
            {
                check_vehicule_exception = true;
                await OnGet(Vehicules.MarqueId);
                return Page();
            }
        }






        private async Task<bool> InsertImagesAsync()
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
                                VehiculeNum = Vehicules.Num
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