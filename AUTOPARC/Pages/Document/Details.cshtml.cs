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

namespace AUTOPARC.Pages.Document
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
        public Docs Docs { get; set; }
        public List<TypeDocs> TypeDocsList { get; set; }
        public List<Vehicules> VehiculesList { get; set; }
        public List<Fournisseurs> FournisseursList { get; set; }
        public List<ModePaiments> ModePaimentsList { get; set; }
        public List<Banques> BanquesList { get; set; }


        [BindProperty]
        public Cheques Cheques { get; set; }

        [BindProperty]
        public Virements Virements { get; set; }

        [BindProperty]
        public Credits Credits { get; set; }


        public bool check_doc_exception, check_doc_numero, check_doc_date;
        public bool check_cheque_exception, check_numero_cheque_existance, check_cheque_date;
        public bool check_virement_exception;
        public bool check_credit_exception, check_credit_montantParMoi, check_credit_date;
        public bool check_cheque_existance, check_virement_existance, check_credit_existance;
        private const string _action = "Document";




        public async Task OnGet(int id)
        {
            Docs = await _db.Docs.FindAsync(id);
            TypeDocsList = await _db.TypeDocs.ToListAsync();
            VehiculesList = await _db.Vehicules.ToListAsync();
            FournisseursList = await _db.Fournisseurs.ToListAsync();
            ModePaimentsList = await _db.ModePaiments.ToListAsync();
            BanquesList = await _db.Banques.ToListAsync();
            Cheques = await _db.Cheques.Where(chq => chq.Action == _action && chq.ActionNum == Docs.Num).SingleOrDefaultAsync();
            Virements = await _db.Virements.Where(v => v.Action == _action && v.ActionNum == Docs.Num).SingleOrDefaultAsync();
            Credits = await _db.Credits.Where(c => c.Action == _action && c.ActionNum == Docs.Num).SingleOrDefaultAsync();
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

            if (Docs.DateDebut >= Docs.DateFin)
            {
                check_doc_date = true;
                await OnGet(Docs.Id);
                return Page();
            }


            try
            {
                var doc = await _db.Docs.FindAsync(Docs.Id);
                doc.Numero = Docs.Numero;
                doc.TypeId = Docs.TypeId;
                doc.VehiculeId = Docs.VehiculeId;
                doc.FrsId = Docs.FrsId;
                doc.DateDebut = Docs.DateDebut;
                doc.DateFin = Docs.DateFin;
                doc.PrixAchat = Docs.PrixAchat;


                if (isCheque && !await InsertChequeAsync())
                    return Page();

                if (isVirement && !await InsertVirementAsync())
                    return Page();

                if (isCredit && !await InsertCreditAsync())
                    return Page();

                if (doc.MontantPayeeEspece != Docs.MontantPayeeEspece)
                {
                    Docs.MontantPayeeTotal -= doc.MontantPayeeEspece;
                    doc.MontantPayeeEspece = Docs.MontantPayeeEspece;
                    Docs.MontantPayeeTotal += doc.MontantPayeeEspece;
                }
                doc.MontantPayeeCheque = Docs.MontantPayeeCheque;
                doc.MontantPayeeVirement = Docs.MontantPayeeVirement;
                doc.MontantPayeeCredit = Docs.MontantPayeeCredit;
                doc.MontantPayeeTotal = Docs.MontantPayeeTotal;

                if (!await InsertDocumentsAsync(Docs.Num))
                    return Page();

                await _db.SaveChangesAsync();
                return RedirectToPage("/Document/Index");
            }
            catch (Exception)
            {
                check_doc_exception = true;
                await OnGet(Docs.Id);
                return Page();
            }
            
        }




        public async Task<IActionResult> OnPostDelete()
        {
            if (await CheckPaiementExistance())
                return Page();

            _db.Docs.Remove(Docs);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Document/Index");
        }






        private async Task<bool> CheckPaiementExistance()
        {
            if (Docs.MontantPayeeCheque != 0)
            {
                check_cheque_existance = true;
                await OnGet(Docs.Id);
                return true;
            }
            if (Docs.MontantPayeeVirement != 0)
            {
                check_virement_existance = true;
                await OnGet(Docs.Id);
                return true;
            }
            if (Docs.MontantPayeeCredit != 0)
            {
                check_credit_existance = true;
                await OnGet(Docs.Id);
                return true;
            }
            return false;
        }






        private async Task<bool> InsertChequeAsync()
        {
            if (Docs.MontantPayeeCheque != 0)
                return true;

            if (string.IsNullOrEmpty(Cheques.Numero) || string.IsNullOrEmpty(Cheques.AuNomDe) ||
                Cheques.DateReglement == null || Cheques.DateEcheance == null ||
                Cheques.Montant == 0 || Cheques.BanqueId == 0)
            {
                check_cheque_exception = true;
                await OnGet(Docs.Id);
                return false;
            }

            if (Cheques.DateReglement > Cheques.DateEcheance)
            {
                check_cheque_date = true;
                await OnGet(Docs.Id);
                return false;
            }

            try
            {
                Docs.MontantPayeeCheque = Cheques.Montant;
                Cheques.Action = _action;
                Cheques.ActionNum = Docs.Num;
                await _db.Cheques.AddAsync(Cheques);
                return true;
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException mySqlEx)
            {
                if (mySqlEx.Message.Contains("Numero"))
                {
                    check_numero_cheque_existance = true;
                    await OnGet(Docs.Id);
                }
                return false;
            }
            catch
            {
                check_cheque_exception = true;
                await OnGet(Docs.Id);
                return false;
            }
        }






        private async Task<bool> InsertVirementAsync()
        {
            if (Docs.MontantPayeeVirement != 0)
                return true;

            if (string.IsNullOrEmpty(Virements.Rib) || string.IsNullOrEmpty(Virements.AuNomDe) ||
                Virements.Montant == 0 || Virements.BanqueId == 0 ||
                 Virements.DateVirement == null)
            {
                check_cheque_exception = true;
                await OnGet(Docs.Id);
                return false;
            }

            try
            {
                Docs.MontantPayeeVirement = Virements.Montant;
                Docs.MontantPayeeTotal += Virements.Montant;
                Virements.Action = _action;
                Virements.ActionNum = Docs.Num;
                await _db.Virements.AddAsync(Virements);
                return true;
            }
            catch
            {
                check_virement_exception = true;
                await OnGet(Docs.Id);
                return false;
            }
        }






        private async Task<bool> InsertCreditAsync()
        {
            if (Docs.MontantPayeeCredit != 0)
                return true;

            if (Credits.Montant == 0 || Credits.Mensualite == 0 || Credits.BanqueId == 0 ||
                Credits.DateDebut == null || Credits.DateFin == null)
            {
                check_credit_exception = true;
                await OnGet(Docs.Id);
                return false;
            }

            if (Credits.Montant < Credits.Mensualite)
            {
                check_credit_montantParMoi = true;
                await OnGet(Docs.Id);
                return false;
            }

            if (Credits.DateDebut > Credits.DateFin)
            {
                check_credit_date = true;
                await OnGet(Docs.Id);
                return false;
            }

            try
            {
                var num = await _db.Credits.OrderByDescending(n => n.Num).FirstOrDefaultAsync();
                Credits.Num = num != null ? num.Num + 1 : 1;
                Docs.MontantPayeeCredit = Credits.Montant;
                Credits.Action = _action;
                Credits.ActionNum = Docs.Num;
                await _db.Credits.AddAsync(Credits);
                return true;
            }
            catch
            {
                check_virement_exception = true;
                await OnGet(Docs.Id);
                return false;
            }
        }






        private async Task<bool> InsertDocumentsAsync(int doc_num)
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
                            var filePath = Path.Combine(_env.WebRootPath, "Docs", "Documents", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                                await file.CopyToAsync(stream);

                            var doc = new UrlDocs
                            {
                                Url = fileName,
                                Action = _action,
                                ActionNum = doc_num
                            };
                            _db.UrlDocs.Add(doc);
                        }
                    }
                }
            }
            catch (Exception)
            {
                check_doc_exception = true;
                await OnGet(Docs.Id);
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
