using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Chauffeur
{
    public class CreateModel : PageModel
    {
        private readonly DBC _db;
        public CreateModel(DBC db) => _db = db;




        [BindProperty]
        public Chauffeurs Chauffeurs { get; set; }

        public bool check_exception;




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _db.Chauffeurs.AddAsync(Chauffeurs);
                await _db.SaveChangesAsync();
                return RedirectToPage("/Chauffeur/Index");
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException mySqlEx)
            {
                if (mySqlEx.Message.Contains("CIN"))
                    ModelState.AddModelError("Chauffeurs.Cin", "Ce CIN existe déjà.");
                else if (mySqlEx.Message.Contains("Portable"))
                    ModelState.AddModelError("Chauffeurs.Portable", "Ce numéro de Portable existe déjà.");
                else if (mySqlEx.Message.Contains("Email"))
                    ModelState.AddModelError("Chauffeurs.Email", "Cet adresse mail existe déjà.");

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
