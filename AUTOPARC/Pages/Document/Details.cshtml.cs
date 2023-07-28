using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Document
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        public DetailsModel(DBC db) => _db = db;




        [BindProperty]
        public Docs Docs { get; set; }
        public List<TypeDocs> TypeDocs { get; set; }
        public List<Vehicules> Vehicules { get; set; }
        public List<Fournisseurs> Fournisseurs { get; set; }




        public async Task OnGet(int id)
        {
            Docs = await _db.Docs.FindAsync(id);
            TypeDocs = await _db.TypeDocs.ToListAsync();
            Vehicules = await _db.Vehicules.ToListAsync();
            Fournisseurs = await _db.Fournisseurs.ToListAsync();
        }




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var doc = await _db.Docs.FindAsync(Docs.Id);
            doc.Numero = Docs.Numero;
            doc.TypeId = Docs.TypeId;
            doc.VehiculeId = Docs.VehiculeId;
            doc.FrsId = Docs.FrsId;
            doc.DateDebut = Docs.DateDebut;
            doc.DateFin = Docs.DateFin;
            doc.UrlDoc = Docs.UrlDoc;
            await _db.SaveChangesAsync();
            return RedirectToPage("/Document/Index");
        }




        public async Task<IActionResult> OnPostDelete()
        {
            if (!ModelState.IsValid)
                return Page();

            _db.Docs.Remove(Docs);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Document/Index");
        }
    }
}
