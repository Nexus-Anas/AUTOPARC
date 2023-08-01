using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.RechargeCarburant
{
    public class CreateModel : PageModel
    {
        private readonly DBC _db;
        public CreateModel(DBC db) => _db = db;




        [BindProperty]
        public RechargeCarburants RechargeCarburants { get; set; }
        public List<Vehicules> Vehicules { get; set; }
        public List<ModePaiments> ModePaiments { get; set; }




        public async Task OnGet()
        {
            Vehicules = await _db.Vehicules.ToListAsync();
            ModePaiments = await _db.ModePaiments.ToListAsync();
        }




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            await _db.RechargeCarburants.AddAsync(RechargeCarburants);
            await _db.SaveChangesAsync();
            return RedirectToPage("/RechargeCarburant/Index");
        }
    }
}
