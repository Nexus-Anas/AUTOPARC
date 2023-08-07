using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Chauffeur
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;

        public List<Chauffeurs> Chauffeurs { get; set; }
        public Chauffeurs Chauffeur { get; set; }

        public async Task OnGet()
            => Chauffeurs = await _db.Chauffeurs.ToListAsync();
    }

}
