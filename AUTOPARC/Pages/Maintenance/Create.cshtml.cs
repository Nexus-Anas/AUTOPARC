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
        public List<ModePaiments> ModePaimentsList { get; set; }
        public List<Banques> BanquesList { get; set; }


        [BindProperty]
        public Cheques Cheques { get; set; }


        [BindProperty]
        public Virements Virements { get; set; }


        public bool check_exception, check_cout_montantPayee, check_all_checkboxes;
        public bool check_cheque_exception, check_numero_cheque_existance, check_date_cheque;
        public bool check_virement_exception;




        public async Task OnGet(int id)
        {
            Vehicules = await _db.Vehicules.Where(v => v.Id == id).SingleOrDefaultAsync();
            Chauffeurs = await _db.AffectationChauffeurVehicules.Where(c => c.VehiculeId == id).Select(c => c.Chauffeur).FirstOrDefaultAsync();
            TypeMaintenancesList = await _db.TypeMaintenances.ToListAsync();
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

            if (!isEspece && !isCheque && !isVirement)
            {
                check_all_checkboxes = true;
                await OnGet(Maintenances.VehiculeId);
                return Page();
            }

            if (!isCheque)
                await RemoveChequeModelStateEntriesAsync();

            if (!isVirement)
                await RemoveVirementModelStateEntriesAsync();

            if (isCheque && !await InsertChequeAsync())
                return Page();

            if (isVirement && !await InsertVirementAsync())
                return Page();




            if (!ModelState.IsValid)
            {
                await OnGet(Maintenances.VehiculeId);
                return Page();
            }

            if (Maintenances.Cout < Maintenances.MontantPayeeEspece)
            {
                check_cout_montantPayee = true;
                await OnGet(Maintenances.VehiculeId);
                return Page();
            }

            try
            {
                Maintenances.MontantPayeeTotal += Maintenances.MontantPayeeEspece;
                await _db.Maintenances.AddAsync(Maintenances);

                if (!await InsertDocumentAsync())
                    return Page();

                await _db.SaveChangesAsync();
                return RedirectToPage("/Maintenance/Index");
            }
            catch (Exception)
            {
                check_exception = true;
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
                check_date_cheque = true;
                await OnGet(Maintenances.VehiculeId);
                return false;
            }

            try
            {
                Maintenances.MontantPayeeCheque = Cheques.Montant;
                Cheques.ActionNum = Maintenances.Num;
                await _db.Cheques.AddAsync(Cheques);
                await _db.SaveChangesAsync();
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
                Virements.ActionNum = Maintenances.Num;
                await _db.Virements.AddAsync(Virements);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                check_virement_exception = true;
                await OnGet(Maintenances.VehiculeId);
                return false;
            }
        }








        private async Task<bool> InsertDocumentAsync()
        {
            try
            {
                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    var file = HttpContext.Request.Form.Files[0];
                    if (file != null && file.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                        var filePath = Path.Combine(_env.WebRootPath, "Documents", "Maintenances", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                            await file.CopyToAsync(stream);

                        Maintenances.UrlDoc = fileName;
                    }
                }
            }
            catch (Exception)
            {
                check_exception = true;
                await OnGet(Maintenances.VehiculeId);
                return false;
            }
            return true;
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