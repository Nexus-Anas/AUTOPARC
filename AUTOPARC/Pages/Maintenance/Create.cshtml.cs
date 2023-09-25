using AUTOPARC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Maintenance
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
        public Maintenances Maintenances { get; set; }

        public Vehicules Vehicules { get; set; }
        public Chauffeurs Chauffeurs { get; set; }
        public List<TypeMaintenances> TypeMaintenancesList { get; set; }
        public List<OperationMaintenances> OperationMaintenancesList { get; set; }
        public List<Fournisseurs> FournisseursList { get; set; }
        public List<ModePaiments> ModePaimentsList { get; set; }
        public List<Banques> BanquesList { get; set; }


        [BindProperty]
        public Cheques Cheques { get; set; }

        [BindProperty]
        public Virements Virements { get; set; }

        [BindProperty]
        public Credits Credits { get; set; }


        public bool check_maintenance_exception;
        public bool check_cheque_exception, check_numero_cheque_existance, check_cheque_date;
        public bool check_virement_exception;
        public bool check_credit_exception, check_credit_montantParMoi, check_credit_date;
        private const string _action = "Maintenance";




        public async Task OnGet(int id)
        {
            Vehicules = await _db.Vehicules.FindAsync(id);
            Chauffeurs = await _db.AffectationChauffeurVehicules.Where(c => c.VehiculeId == id).Select(c => c.Chauffeur).FirstOrDefaultAsync();
            TypeMaintenancesList = await _db.TypeMaintenances.ToListAsync();
            OperationMaintenancesList = await _db.OperationMaintenances.ToListAsync();
            FournisseursList = await _db.Fournisseurs.ToListAsync();
            ModePaimentsList = await _db.ModePaiments.ToListAsync();
            BanquesList = await _db.Banques.ToListAsync();
        }




        public async Task<IActionResult> OnPostCreate()
        {
            var num = await _db.Maintenances.OrderByDescending(m => m.Num).Select(m => m.Num).FirstOrDefaultAsync();
            Maintenances.Num = num != 0 ? num + 1 : 1;

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


            if (!ModelState.IsValid)
            {
                await OnGet(Maintenances.VehiculeId);
                return Page();
            }

            try
            {
                Maintenances.MontantPayeeTotal += Maintenances.MontantPayeeEspece;
                await _db.Maintenances.AddAsync(Maintenances);

                if (isCheque && !await InsertChequeAsync())
                    return Page();

                if (isVirement && !await InsertVirementAsync())
                    return Page();

                if (isCredit && !await InsertCreditAsync())
                    return Page();

                if (!await InsertDocumentsAsync(Maintenances.Num))
                    return Page();



                await _db.SaveChangesAsync();
                return RedirectToPage("/Maintenance/Index");
            }
            catch (Exception)
            {
                check_maintenance_exception = true;
                await OnGet(Maintenances.VehiculeId);
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
                await OnGet(Maintenances.VehiculeId);
                return false;
            }

            if (Cheques.DateReglement > Cheques.DateEcheance)
            {
                check_cheque_date = true;
                await OnGet(Maintenances.VehiculeId);
                return false;
            }

            try
            {
                Maintenances.MontantPayeeCheque = Cheques.Montant;
                Cheques.Action = _action;
                Cheques.ActionNum = Maintenances.Num;
                await _db.Cheques.AddAsync(Cheques);
                return true;
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException mySqlEx)
            {
                if (mySqlEx.Message.Contains("Numero"))
                {
                    check_numero_cheque_existance = true;
                    await OnGet(Maintenances.VehiculeId);
                }
                return false;
            }
            catch
            {
                check_cheque_exception = true;
                await OnGet(Maintenances.VehiculeId);
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
                await OnGet(Maintenances.VehiculeId);
                return false;
            }

            try
            {
                Maintenances.MontantPayeeVirement = Virements.Montant;
                Maintenances.MontantPayeeTotal += Virements.Montant;
                Virements.Action = _action;
                Virements.ActionNum = Maintenances.Num;
                await _db.Virements.AddAsync(Virements);
                return true;
            }
            catch
            {
                check_virement_exception = true;
                await OnGet(Maintenances.VehiculeId);
                return false;
            }
        }




        private async Task<bool> InsertCreditAsync()
        {
            if (Credits.Montant == 0 || Credits.Mensualite == 0 || Credits.BanqueId == 0 ||
                Credits.DateDebut == null || Credits.DateFin == null)
            {
                check_credit_exception = true;
                await OnGet(Maintenances.VehiculeId);
                return false;
            }

            if (Credits.Montant < Credits.Mensualite)
            {
                check_credit_montantParMoi = true;
                await OnGet(Maintenances.VehiculeId);
                return false;
            }

            if (Credits.DateDebut > Credits.DateFin)
            {
                check_credit_date = true;
                await OnGet(Maintenances.VehiculeId);
                return false;
            }

            try
            {
                var num = await _db.Credits.OrderByDescending(n => n.Num).FirstOrDefaultAsync();
                Credits.Num = num != null ? num.Num + 1 : 1;
                Maintenances.MontantPayeeCredit = Credits.Montant;
                Credits.Action = _action;
                Credits.ActionNum = Maintenances.Num;
                await _db.Credits.AddAsync(Credits);
                return true;
            }
            catch
            {
                check_virement_exception = true;
                await OnGet(Maintenances.VehiculeId);
                return false;
            }
        }






        private async Task<bool> InsertDocumentsAsync(int maintenance_num)
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
                            var filePath = Path.Combine(_env.WebRootPath, "Docs", "Maintenances", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                                await file.CopyToAsync(stream);

                            var doc = new UrlDocs
                            {
                                Url = fileName,
                                Action = _action,
                                ActionNum = maintenance_num
                            };
                            _db.UrlDocs.Add(doc);
                        }
                    }
                }
            }
            catch (Exception)
            {
                check_maintenance_exception = true;
                await OnGet(Maintenances.VehiculeId);
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