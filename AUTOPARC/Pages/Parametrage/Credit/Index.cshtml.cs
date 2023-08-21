using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Parametrage.Credit
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        public List<Credits> CreditsList { get; set; }
        public List<Banques> BanquesList { get; set; }




        public async Task OnGet()
        {
            CreditsList = await _db.Credits.ToListAsync();
            BanquesList = await _db.Banques.ToListAsync();
        }
    }
}
