using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
        public List<ModePaiments> ModePaiments { get; set; }
        public List<Cheques> ChequesList { get; set; }
        public List<Virements> VirementsList { get; set; }
        public List<Credits> CreditsList { get; set; }

        private const string _action = "Document";




        public async Task OnGet()
        {
            Docs = await _db.Docs.ToListAsync();
            TypeDocs = await _db.TypeDocs.ToListAsync();
            Fournisseurs = await _db.Fournisseurs.ToListAsync();
            ModePaiments = await _db.ModePaiments.ToListAsync();
            ChequesList = await _db.Cheques.Where(chq => chq.Action == _action).ToListAsync();
            VirementsList = await _db.Virements.Where(v => v.Action == _action).ToListAsync();
            CreditsList = await _db.Credits.Where(v => v.Action == _action).ToListAsync();
        }
    }
}