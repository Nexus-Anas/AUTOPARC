using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace AUTOPARC.Pages.SuiviDepense
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public Suividepense Suividepense { get; set; }

        [BindProperty]
        public Societes Societes { get; set; }

        public List<Societes> SocietesList { get; set; }

        public bool check_exception;
        public int SocieteCount;




        public async Task OnGet(int id)
        {
            Suividepense = await _db.Suividepense.FindAsync(id);
            SocietesList = await _db.Societes.ToListAsync();
            SocieteCount = await _db.Societes.CountAsync();
        }




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var depense = await _db.Suividepense.FindAsync(Suividepense.Id);
                depense.DateDepense = Suividepense.DateDepense;
                depense.Objet = Suividepense.Objet;
                depense.Montant = Suividepense.Montant;
                depense.SocieteId = Suividepense.SocieteId;

                await _db.SaveChangesAsync();
                return RedirectToPage("/SuiviDepense/Index");
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

            _db.Suividepense.Remove(Suividepense);
            await _db.SaveChangesAsync();
            return RedirectToPage("/SuiviDepense/Index");
        }
    }
}
