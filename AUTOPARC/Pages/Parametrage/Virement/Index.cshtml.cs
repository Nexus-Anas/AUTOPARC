using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Parametrage.Virement
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        public List<Virements> VirementsList { get; set; }
        public List<Banques> BanquesList { get; set; }




        public async Task OnGet()
        {
            VirementsList = await _db.Virements.ToListAsync();
            BanquesList = await _db.Banques.ToListAsync();
        }
    }
}
