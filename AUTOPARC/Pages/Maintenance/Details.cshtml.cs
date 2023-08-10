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

namespace AUTOPARC.Pages.Maintenance
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
        public Maintenances Maintenances { get; set; }
        public List<Vehicules> VehiculesList { get; set; }
        public List<TypeMaintenances> TypeMaintenancesList { get; set; }
        public List<ModePaiments> ModePaimentsList { get; set; }


        [BindProperty]
        public Cheques Cheques { get; set; }
        public List<Banques> BanquesList { get; set; }


        public bool check_exception, check_cout_montantPayee;
        public bool check_cheque_exception, cheque_numero_cheque_existance;




        public async Task OnGet(int id)
        {
            Maintenances = await _db.Maintenances.FindAsync(id);
            var ids = await _db.AffectationChauffeurVehicules.Select(x => x.VehiculeId).ToListAsync();
            VehiculesList = await _db.Vehicules.Where(v => ids.Contains(v.Id)).ToListAsync();
            TypeMaintenancesList = await _db.TypeMaintenances.ToListAsync();
            ModePaimentsList = await _db.ModePaiments.ToListAsync();
            Cheques = await _db.Cheques.Where(chq => chq.ActionNum == Maintenances.Num).SingleOrDefaultAsync();
            BanquesList = await _db.Banques.ToListAsync();
        }




        public async Task<IActionResult> OnPostUpdate()
        {
            var modePaiement = await _db.ModePaiments.Where(m => m.Id == Maintenances.ModePaiementId).Select(m => m.Mode).SingleOrDefaultAsync();

            if (modePaiement == "Espece")
                await RemoveChequeModelStateEntriesAsync();


            if (!ModelState.IsValid)
            {
                await OnGet(Maintenances.Id);
                return Page();
            }


            if (Maintenances.MontantPayee > Maintenances.Cout)
            {
                check_cout_montantPayee = true;
                await OnGet(Maintenances.Id);
                return Page();
            }


            if (!await InsertDocumentAsync())
                return Page();


            if (modePaiement != "Espece")
            {
                if (!await ModifyChequeAsync())
                    return Page();
            }


            try
            {
                var maintenance = await _db.Maintenances.FindAsync(Maintenances.Id);
                maintenance.TypeId = Maintenances.TypeId;
                maintenance.VehiculeId = Maintenances.VehiculeId;
                maintenance.ChauffeurId = await _db.AffectationChauffeurVehicules.Where(a => a.VehiculeId == Maintenances.VehiculeId).Select(a => a.ChauffeurId).FirstOrDefaultAsync();
                maintenance.DateMaintenance = Maintenances.DateMaintenance;
                maintenance.Cout = Maintenances.Cout;
                maintenance.MontantPayee = Maintenances.MontantPayee;
                maintenance.ModePaiementId = Maintenances.ModePaiementId;
                maintenance.Description = Maintenances.Description;
                maintenance.UrlDoc = Maintenances.UrlDoc != null ? Maintenances.UrlDoc : maintenance.UrlDoc;
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




        public async Task<IActionResult> OnPostDelete()
        {
            if (!ModelState.IsValid)
            {
                await OnGet(Maintenances.Id);
                return Page();
            }

            _db.Maintenances.Remove(Maintenances);
            await _db.SaveChangesAsync();
            var cheque = await _db.Cheques.Where(chq => chq.ActionNum == Maintenances.Num).SingleOrDefaultAsync();
            if (cheque != null)
            {
                _db.Cheques.Remove(cheque);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage("/Maintenance/Index");
        }








        private async Task<bool> ModifyChequeAsync()
        {
            if (string.IsNullOrEmpty(Cheques.Numero) || string.IsNullOrEmpty(Cheques.AuNomDe) ||
                Cheques.DateReglement == null || Cheques.DateEcheance == null ||
                Cheques.Montant == 0 || Cheques.BanqueId == 0)
            {
                check_cheque_exception = true;
                await OnGet(Maintenances.VehiculeId);
                return false;
            }

            try
            {
                var cheque = await _db.Cheques.Where(chq => chq.ActionNum == Maintenances.Num).SingleOrDefaultAsync();

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
                    Cheques.ActionNum = Maintenances.Num;
                    await _db.Cheques.AddAsync(Cheques);
                    await _db.SaveChangesAsync();
                    return true;
                }
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException mySqlEx)
            {
                if (mySqlEx.Message.Contains("Numero"))
                {
                    cheque_numero_cheque_existance = true;
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
            if (Cheques.Id != 0)
            {
                _db.Cheques.Remove(Cheques);
                await _db.SaveChangesAsync();
            }
            else
            {
                foreach (var key in ModelState.Keys.ToList())
                    if (key.StartsWith("Cheques."))
                        ModelState.Remove(key);
            }
        }
    }
}
