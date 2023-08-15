using AUTOPARC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Document
{
    public class DetailsModel : PageModel
    {
        private readonly DBC _db;
        private readonly IWebHostEnvironment _env;
        public DetailsModel(DBC db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }




        [BindProperty]
        public Docs Docs { get; set; }
        public List<TypeDocs> TypeDocs { get; set; }
        public List<Vehicules> Vehicules { get; set; }
        public List<Fournisseurs> Fournisseurs { get; set; }

        public bool check_exception, check_date;




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
            {
                await OnGet(Docs.Id);
                return Page();
            }


            if (Docs.DateDebut >= Docs.DateFin)
            {
                check_date = true;
                await OnGet(Docs.Id);
                return Page();
            }


            if (!await InsertDocumentAsync())
                return Page();


            try
            {
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
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException mySqlEx)
            {
                if (mySqlEx.Message.Contains("Numero"))
                    ModelState.AddModelError("Docs.Numero", "Ce numéro de document existe déjà.");

                await OnGet(Docs.VehiculeId);
                return Page();
            }
            catch (Exception)
            {
                check_exception = true;
                return Page();
            }
        }




        public async Task<IActionResult> OnPostDelete()
        {
            if (!ModelState.IsValid)
            {
                await OnGet(Docs.Id);
                return Page();
            }

            _db.Docs.Remove(Docs);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Document/Index");
        }






        private async Task<bool> InsertDocumentAsync()
        {
            try
            {
                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    var file = HttpContext.Request.Form.Files[0];
                    if (file != null && file.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                        var filePath = Path.Combine(_env.WebRootPath, "Documents", "Docs", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                            await file.CopyToAsync(stream);

                        Docs.UrlDoc = fileName;
                    }
                }
            }
            catch (Exception)
            {
                check_exception = true;
                await OnGet(Docs.VehiculeId);
                return false;
            }
            return true;
        }






        public async Task<IActionResult> OnPostDownloadFile()
        {
            var url = await _db.Docs.Where(m => m.Id == Docs.Id).Select(m => m.UrlDoc).SingleOrDefaultAsync();
            if (!string.IsNullOrEmpty(url))
            {
                var fileName = url[(url.IndexOf("_") + 1)..]; // same as url.Substring(url.IndexOf("_") + 1);
                var filePath = Path.Combine(_env.WebRootPath, "Documents", "Docs", url);

                if (System.IO.File.Exists(filePath))
                {
                    var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    var contentType = "application/octet-stream"; // You may need to set the appropriate content type

                    return File(fileStream, contentType, fileName);
                }
            }

            return NotFound(); // Document not found
        }
    }
}
