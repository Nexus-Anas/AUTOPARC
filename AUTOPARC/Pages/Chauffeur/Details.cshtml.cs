using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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




        public async Task OnGet(int id)
            => Chauffeurs = await _db.Chauffeurs.FindAsync(id);




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

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
