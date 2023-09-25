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

        [BindProperty]
        public Societes Societes { get; set; }

        public List<Societes> SocietesList { get; set; }

        public bool check_exception;
        public int NumDp, SocieteCount;


        public async Task OnGet()
        {
            SocietesList = await _db.Societes.ToListAsync();
            SocieteCount = await _db.Societes.CountAsync();
            var num = await _db.Suividepense.OrderByDescending(s => s.NumDepense).Select(x => x.NumDepense).FirstOrDefaultAsync();
            NumDp = num + 1;
        }




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var num = await _db.Suividepense.OrderByDescending(s => s.NumDepense).Select(x => x.NumDepense).FirstOrDefaultAsync();
                Suividepense.NumDepense = num + 1;
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
