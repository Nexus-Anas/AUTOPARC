using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.RechargeCarburant
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        public List<RechargeCarburants> RechargeCarburants { get; set; }
        public List<Vehicules> Vehicules { get; set; }
        public List<TypeCarburants> TypeCarburants { get; set; }
        public List<ModePaiments> ModePaiments { get; set; }




        public async Task OnGet()
        {
            RechargeCarburants = await _db.RechargeCarburants.ToListAsync();
            Vehicules = await _db.Vehicules.ToListAsync();
            TypeCarburants = await _db.TypeCarburants.ToListAsync();
            ModePaiments = await _db.ModePaiments.ToListAsync();
        }
    }
}
