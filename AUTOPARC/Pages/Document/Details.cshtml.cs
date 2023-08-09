using AUTOPARC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Document
{
    public class DetailsModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly DBC _db;
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

            try
            {
                if (Request.Form.Files.Count > 0) // Check if any files were uploaded
                {
                    var file = Request.Form.Files[0]; // Get the first uploaded file

                    if (file.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_env.WebRootPath, "Document", "URL_Document");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        Docs.UrlDoc = filePath; // Store the file path in your model if needed
                    }
                }

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
    }
}
