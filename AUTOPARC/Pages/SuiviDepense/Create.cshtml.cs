using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace AUTOPARC.Pages.SuiviDepense
{
    public class CreateModel : PageModel
    {
        private readonly DBC _db;
        public CreateModel(DBC db) => _db = db;




        [BindProperty]
        public Suividepense Suividepense { get; set; }
        public List<ModePaiments> ModePaiments { get; set; }

        public bool check_exception;
        public int NumDp;



        public async Task OnGet()
        {
            var num = await _db.Suividepense.OrderByDescending(s => s.NumDepense).FirstOrDefaultAsync();
            NumDp = num != null ? num.NumDepense + 1 : 1;
            ModePaiments = await _db.ModePaiments.ToListAsync();
        }




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var num = await _db.Suividepense.OrderByDescending(s => s.NumDepense).FirstOrDefaultAsync();
                Suividepense.NumDepense = num != null ? num.NumDepense + 1 : 1;
                await _db.Suividepense.AddAsync(Suividepense);
                await _db.SaveChangesAsync();
                return RedirectToPage("/SuiviDepense/Index");
            }
            catch (Exception)
            {
                check_exception = true;
                return Page();
            }
        }
    }
}
