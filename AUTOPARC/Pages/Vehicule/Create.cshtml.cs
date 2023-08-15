using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule
{
    public class CreateModel : PageModel
    {
        private readonly DBC _db;
        public CreateModel(DBC db) => _db = db;




        [BindProperty]
        public Vehicules Vehicules { get; set; }
        public List<Categories> CategoriesList { get; set; }
        public List<Modeles> ModelesList { get; set; }
        public List<TypeCarburants> TypeCarburantsList { get; set; }
        public List<EtatVehicules> EtatVehiculesList { get; set; }
        public List<Fournisseurs> FournisseursList { get; set; }
        public List<ModePaiments> ModePaimentsList { get; set; }


        [BindProperty]
        public Cheques Cheques { get; set; }
        public List<Banques> BanquesList { get; set; }

        public bool check_exception, check_Prix_MontantPayee;
        public bool check_cheque_exception, check_numero_cheque_existance, check_montant, check_date;
        public int marqueID;




        public async Task OnGet(int id)
        {
            marqueID = id;
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


            var modePaiement = await _db.ModePaiments.Where(m => m.Id == Vehicules.ModePayementId).Select(m => m.Mode).SingleOrDefaultAsync();
            if (modePaiement == "Espece")
                await RemoveChequeModelStateEntriesAsync();
            else if (!await InsertChequeAsync())
                return Page();


            if (!ModelState.IsValid)
            {
                await OnGet(marqueID);
                return Page();
            }

            if (Vehicules.MontantPayee > Vehicules.PrixAchat)
            {
                check_Prix_MontantPayee = true;
                await OnGet(marqueID);
                return Page();
            }

            try
            {
                await _db.Vehicules.AddAsync(Vehicules);
                await _db.SaveChangesAsync();
                return RedirectToPage("/Vehicule/Index");
            }
            catch (System.Exception)
            {
                check_exception = true;
                await OnGet(marqueID);
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
                await OnGet(marqueID);
                return false;
            }

            if (Vehicules.PrixAchat != (Vehicules.MontantPayee + Cheques.Montant))
            {
                check_montant = true;
                await OnGet(marqueID);
                return false;
            }

            if (Cheques.DateReglement > Cheques.DateEcheance)
            {
                check_date = true;
                await OnGet(marqueID);
                return false;
            }

            try
            {
                Cheques.ActionNum = Vehicules.Num;
                await _db.Cheques.AddAsync(Cheques);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException mySqlEx)
            {
                if (mySqlEx.Message.Contains("Numero"))
                {
                    check_numero_cheque_existance = true;
                    await OnGet(marqueID);
                }
                return false;
            }
            catch
            {
                check_cheque_exception = true;
                await OnGet(marqueID);
                return false;
            }
        }






        private async Task RemoveChequeModelStateEntriesAsync()
        {
            foreach (var key in ModelState.Keys.ToList())
                if (key.StartsWith("Cheques."))
                    ModelState.Remove(key);
        }
    }
}