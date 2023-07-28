using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Document
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        public List<Docs> Docs { get; set; }
        public List<TypeDocs> TypeDocs { get; set; }
        public List<Fournisseurs> Fournisseurs { get; set; }




        public async Task OnGet()
        {
            Docs = await _db.Docs.ToListAsync();
            TypeDocs = await _db.TypeDocs.ToListAsync();
            Fournisseurs = await _db.Fournisseurs.ToListAsync();
        }
    }
}