using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Fournisseur
{
    public class CreateModel : PageModel
    {
        private readonly DBC _db;
        public CreateModel(DBC db) => _db = db;


            

        [BindProperty]
        public Fournisseurs Fournisseurs { get; set; }
        public List<TypeFournisseurs> TypeFournisseurs { get; set; }
        public List<Villes> Villes { get; set; }

        public bool check_exception;




        public async Task OnGet()
        {
            TypeFournisseurs = await _db.TypeFournisseurs.ToListAsync();
            Villes = await _db.Villes.ToListAsync();
        }




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _db.Fournisseurs.AddAsync(Fournisseurs);
                await _db.SaveChangesAsync();
                return RedirectToPage("/Fournisseur/Index");
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException mySqlEx)
            {
                if (mySqlEx.Message.Contains("Telephone"))
                    ModelState.AddModelError("Fournisseurs.Telephone", "Ce numéro de Telephone existe déjà.");
                else if (mySqlEx.Message.Contains("Portable"))
                    ModelState.AddModelError("Fournisseurs.Portable", "Ce numéro de Portable existe déjà.");
                else if (mySqlEx.Message.Contains("Email"))
                    ModelState.AddModelError("Fournisseurs.Email", "Cet adresse mail existe déjà.");

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
