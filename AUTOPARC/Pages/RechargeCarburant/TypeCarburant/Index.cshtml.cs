using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.RechargeCarburant.TypeCarburant
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        [BindProperty]
        public TypeCarburants TypeCarburants { get; set; }
        public List<TypeCarburants> TypeCarburantsList { get; set; }

        public bool checkTypeID;




        public async Task OnGet()
            => TypeCarburantsList = await _db.TypeCarburants.ToListAsync();




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
                return Page();

            await _db.TypeCarburants.AddAsync(TypeCarburants);
            await _db.SaveChangesAsync();
            return RedirectToPage("/RechargeCarburant/TypeCarburant/Index");
        }




        public async Task<IActionResult> OnPostDelete(int id)
        {
            var type = await _db.TypeCarburants.FindAsync(id);

            if (type is null)
                return NotFound();

            var carburant = await _db.Vehicules.Where(x => x.CarburantId == type.Id).Select(x => x.Id).FirstOrDefaultAsync();
            if (carburant != 0)
            {
                checkTypeID = true;
                await OnGet();
                return Page();
            }

            _db.TypeCarburants.Remove(type);
            await _db.SaveChangesAsync();
            return RedirectToPage("/RechargeCarburant/TypeCarburant/Index");
        }
    }
}
