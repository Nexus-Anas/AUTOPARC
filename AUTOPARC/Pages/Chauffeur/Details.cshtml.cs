using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Chauffeur
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public Chauffeurs Chauffeurs { get; set; }

        public bool check_exception;




        public async Task OnGet(int id)
            => Chauffeurs = await _db.Chauffeurs.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var chauffeur = await _db.Chauffeurs.FindAsync(Chauffeurs.Id);
                chauffeur.Nom = Chauffeurs.Nom;
                chauffeur.Prenom = Chauffeurs.Prenom;
                chauffeur.Cin = Chauffeurs.Cin;
                chauffeur.DateNaissance = Chauffeurs.DateNaissance;
                chauffeur.Portable = Chauffeurs.Portable;
                chauffeur.Email = Chauffeurs.Email;
                chauffeur.Adresse = Chauffeurs.Adresse;
                chauffeur.NumeroPermisConduire = Chauffeurs.NumeroPermisConduire;
                chauffeur.DateExpirationPermis = Chauffeurs.DateExpirationPermis;
                chauffeur.Disponibilite = Chauffeurs.Disponibilite;
                chauffeur.DateEmbauche = Chauffeurs.DateEmbauche;
                chauffeur.Remarques = Chauffeurs.Remarques;

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




        public async Task<IActionResult> OnPostDelete()
        {
            if (!ModelState.IsValid)
                return Page();

            _db.Chauffeurs.Remove(Chauffeurs);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Chauffeur/Index");
        }
    }
}
