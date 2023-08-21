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
        public List<Banques> BanquesList { get; set; }


        [BindProperty]
        public Cheques Cheques { get; set; }


        [BindProperty]
        public Virements Virements { get; set; }


        public bool check_exception, check_cout_montantPayee, check_all_checkboxes, check_etat_paye;
        public bool check_cheque_exception, check_numero_cheque_existance, check_montant_cheque, check_date_cheque;
        public bool check_virement_exception, check_montant_virement;



        public async Task OnGet(int id)
        {
            Maintenances = await _db.Maintenances.FindAsync(id);
            var ids = await _db.AffectationChauffeurVehicules.Select(x => x.VehiculeId).ToListAsync();
            VehiculesList = await _db.Vehicules.Where(v => ids.Contains(v.Id)).ToListAsync();
            TypeMaintenancesList = await _db.TypeMaintenances.ToListAsync();
            ModePaimentsList = await _db.ModePaiments.ToListAsync();
            BanquesList = await _db.Banques.ToListAsync();
            Cheques = await _db.Cheques.Where(chq => chq.Action == "Maintenance" && chq.ActionNum == Maintenances.Num).SingleOrDefaultAsync();
            Virements = await _db.Virements.Where(v => v.Action == "Maintenance" && v.ActionNum == Maintenances.Num).SingleOrDefaultAsync();
        }




        public async Task<IActionResult> OnPostUpdate()
        {
            bool isEspece = Request.Form.TryGetValue("Espece", out var especeValue);
            bool isCheque = Request.Form.TryGetValue("Cheque", out var chequeValue);
            bool isVirement = Request.Form.TryGetValue("Virement", out var virementValue);

            if (!isEspece && !isCheque && !isVirement)
            {
                check_all_checkboxes = true;
                await OnGet(Maintenances.Id);
                return Page();
            }


            if (await Check_ChequePayee())
                return Page();

            if (await Check_VirementPayee())
                return Page();

            if (!isCheque)
                await RemoveChequeModelStateEntriesAsync();

            if (!isVirement)
                await RemoveVirementModelStateEntriesAsync();

            if (isCheque && !await TryUpdateCheque())
                return Page();

            if (isVirement && !await TryUpdateVirement())
                return Page();


            if (!ModelState.IsValid)
            {
                await OnGet(Maintenances.Id);
                return Page();
            }



            if (Maintenances.Cout < Maintenances.MontantPayeeEspece)
            {
                check_cout_montantPayee = true;
                await OnGet(Maintenances.Id);
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
                maintenance.MontantPayeeEspece = Maintenances.MontantPayeeEspece;
                maintenance.MontantPayeeCheque = Maintenances.MontantPayeeCheque;
                maintenance.MontantPayeeVirement = Maintenances.MontantPayeeVirement;
                maintenance.Description = Maintenances.Description;
                maintenance.UrlDoc = Maintenances.UrlDoc ?? maintenance.UrlDoc;

                if (!await InsertDocumentAsync())
                    return Page();

                await _db.SaveChangesAsync();
                return RedirectToPage("/Maintenance/Index");
            }
            catch (Exception)
            {
                check_exception = true;
                await OnGet(Maintenances.Id);
                return Page();
            }
        }






        public async Task<IActionResult> OnPostDelete()
        {
            if (await Check_ChequePayee())
                return Page();

            await TryDeleteCheck();
            await TryDeleteVirement();

            _db.Maintenances.Remove(Maintenances);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Maintenance/Index");
        }






        private async Task<bool> TryUpdateCheque()
        {
            if (string.IsNullOrEmpty(Cheques.Numero) || string.IsNullOrEmpty(Cheques.AuNomDe) ||
                Cheques.DateReglement == null || Cheques.DateEcheance == null ||
                Cheques.Montant == 0 || Cheques.BanqueId == 0)
            {
                check_cheque_exception = true;
                await OnGet(Maintenances.Id);
                return false;
            }

            if (Maintenances.Cout != (Maintenances.MontantPayeeEspece + Cheques.Montant + Virements.Montant))
            {
                check_montant_cheque = true;
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
                var cheque = await _db.Cheques.Where(chq => chq.Action == "Maintenance" && chq.ActionNum == Maintenances.Num).SingleOrDefaultAsync();
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
                    Maintenances.MontantPayeeCheque = Cheques.Montant;
                    Cheques.ActionNum = Maintenances.Num;
                    Cheques.Etat = "impay�";
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
                    await OnGet(Maintenances.Id);
                }
                return false;
            }
            catch
            {
                check_cheque_exception = true;
                await OnGet(Maintenances.Id);
                return false;
            }
        }




        private async Task<bool> TryUpdateVirement()
        {
            if (string.IsNullOrEmpty(Virements.Rib) || string.IsNullOrEmpty(Virements.AuNomDe) ||
                Virements.Montant == 0 || Virements.BanqueId == 0 ||
                 Virements.DateVirement == null)
            {
                check_cheque_exception = true;
                await OnGet(Maintenances.VehiculeId);
                return false;
            }

            if (Maintenances.Cout != (Maintenances.MontantPayeeEspece + Cheques.Montant + Virements.Montant))
            {
                check_montant_virement = true;
                await OnGet(Maintenances.VehiculeId);
                return false;
            }

            try
            {
                var virement = await _db.Virements.Where(v => v.Action == "Maintenance" && v.ActionNum == Maintenances.Num).SingleOrDefaultAsync();
                if (virement != null)
                {
                    virement.BanqueId = Virements.BanqueId;
                    virement.Rib = Virements.Rib;
                    virement.Montant = Virements.Montant;
                    virement.AuNomDe = Virements.AuNomDe;
                    virement.DateVirement = Virements.DateVirement;

                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    Maintenances.MontantPayeeVirement = Virements.Montant;
                    Virements.ActionNum = Maintenances.Num;
                    Virements.Etat = "impay�";
                    await _db.Virements.AddAsync(Virements);
                    await _db.SaveChangesAsync();
                    return true;
                }
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
                await OnGet(Maintenances.Id);
                return false;
            }
            return true;
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

        private async Task<bool> RemoveVirementModelStateEntriesAsync()
        {
            if (Virements.Id != 0)
            {
                await TryDeleteVirement();
                return true;
            }
            else
            {
                foreach (var key in ModelState.Keys.ToList())
                    if (key.StartsWith("Virements."))
                        ModelState.Remove(key);
                return true;
            }
        }






        private async Task<bool> Check_ChequePayee()
        {
            var cheque = await _db.Cheques.Where(chq => chq.Action == "Maintenance" && chq.ActionNum == Maintenances.Num).SingleOrDefaultAsync();
            if (cheque != null && cheque.Etat == "pay�")
            {
                check_etat_paye = true;
                await OnGet(Maintenances.Id);
                return true;
            }
            return false;
        }




        private async Task<bool> Check_VirementPayee()
        {
            var virement = await _db.Virements.Where(v => v.Action == "Maintenance" && v.ActionNum == Maintenances.Num).SingleOrDefaultAsync();
            if (virement != null && virement.Etat == "pay�")
            {
                check_etat_paye = true;
                await OnGet(Maintenances.Id);
                return true;
            }
            return false;
        }






        private async Task TryDeleteCheck()
        {
            var cheque = await _db.Cheques.Where(chq => chq.Action == "Maintenance" && chq.ActionNum == Maintenances.Num).SingleOrDefaultAsync();
            if (cheque != null)
            {
                _db.Cheques.Remove(cheque);
                await _db.SaveChangesAsync();
            }
        }




        private async Task TryDeleteVirement()
        {
            var virement = await _db.Virements.Where(v => v.Action == "Maintenance" && v.ActionNum == Maintenances.Num).SingleOrDefaultAsync();
            if (virement != null)
            {
                _db.Virements.Remove(virement);
                await _db.SaveChangesAsync();
            }
        }






        public async Task<IActionResult> OnPostDownloadFile()
        {
            var url = await _db.Maintenances.Where(m => m.Id == Maintenances.Id).Select(m => m.UrlDoc).SingleOrDefaultAsync();
            if (!string.IsNullOrEmpty(url))
            {
                var fileName = url[(url.IndexOf("_") + 1)..]; // same as url.Substring(url.IndexOf("_") + 1);
                var filePath = Path.Combine(_env.WebRootPath, "Documents", "Maintenances", url);

                if (System.IO.File.Exists(filePath))
                {
                    var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    var contentType = "application/octet-stream"; // You may need to set the appropriate content type

                    return File(fileStream, contentType, fileName);
                }
            }

            return NotFound(); // Document not found
        }

    }
}
