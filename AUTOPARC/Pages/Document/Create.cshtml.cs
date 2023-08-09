using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Document
{
    public class CreateModel : PageModel
    {
        private readonly DBC _db;
        public CreateModel(DBC db) => _db = db;




        [BindProperty]
        public Docs Docs { get; set; }
        public List<TypeDocs> TypeDocs { get; set; }
        public Vehicules Vehicules { get; set; }
        public List<Fournisseurs> Fournisseurs { get; set; }

        public bool check_exception, check_date;

        public int vehiculeID;

        public string vehiculeMatricule;




        public async Task OnGet(int id)
        {
            vehiculeID = id;
            vehiculeMatricule = await _db.Vehicules.Where(x => x.Id == id).Select(x => x.Matricule).FirstOrDefaultAsync();
            TypeDocs = await _db.TypeDocs.ToListAsync();
            Fournisseurs = await _db.Fournisseurs.ToListAsync();
            Vehicules = await _db.Vehicules.Where(x => x.Id == id).FirstOrDefaultAsync();
        }




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
            {
                await OnGet(Docs.VehiculeId);
                return Page();
            }

            if (Docs.DateDebut >= Docs.DateFin)
            {
                check_date = true;
                await OnGet(Docs.VehiculeId);
                return Page();
            }

            try
            {
                await _db.Docs.AddAsync(Docs);
                await _db.SaveChangesAsync();
                return RedirectToPage("/Document/Index");
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException mySqlEx)
            {
                if (mySqlEx.Message.Contains("Numero"))
                    ModelState.AddModelError("Docs.Numero", "Ce numéro de document existe déjà.");

                await OnGet(Docs.VehiculeId);
                return Page();
            }
            catch (Exception)
            {
                check_exception = true;
                return Page();
            }
        }
    }
}
