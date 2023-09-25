using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.SuiviDepense
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        public List<Suividepense> SuividepensesList { get; set; }
        public List<Societes> SocietesList { get; set; }




        public async Task OnGet()
        {
            SuividepensesList = await _db.Suividepense.ToListAsync();
            SocietesList = await _db.Societes.ToListAsync();
        }
    }
}
