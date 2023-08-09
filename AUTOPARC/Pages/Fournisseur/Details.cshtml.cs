using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Fournisseur
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public Fournisseurs Fournisseurs { get; set; }
        public List<TypeFournisseurs> TypeFournisseurs { get; set; }
        public List<Villes> Villes { get; set; }

        public bool check_exception;





        public async Task OnGet(int id)
        {
            Fournisseurs = await _db.Fournisseurs.FindAsync(id);
            TypeFournisseurs = await _db.TypeFournisseurs.ToListAsync();
            Villes = await _db.Villes.ToListAsync();
        }




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var frs = await _db.Fournisseurs.FindAsync(Fournisseurs.Id);
                frs.Nom = Fournisseurs.Nom;
                frs.Email = Fournisseurs.Email;
                frs.Adresse = Fournisseurs.Adresse;
                frs.VilleId = Fournisseurs.VilleId;
                frs.Telephone = Fournisseurs.Telephone;
                frs.Portable = Fournisseurs.Portable;
                frs.TypeFrsId = Fournisseurs.TypeFrsId;

                await _db.SaveChangesAsync();
                return RedirectToPage("/Fournisseur/Index");
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

            _db.Fournisseurs.Remove(Fournisseurs);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Fournisseur/Index");
        }
    }
}
