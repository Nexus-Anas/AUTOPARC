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
    public class CreateModel : PageModel
    {
        private readonly DBC _db;
        private readonly IWebHostEnvironment _env;
        public CreateModel(DBC db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }




        [BindProperty]
        public Docs Docs { get; set; }
        public Vehicules Vehicules { get; set; }
        public List<TypeDocs> TypeDocsList { get; set; }
        public List<Fournisseurs> FournisseursList { get; set; }

        public bool check_exception, check_date;




        public async Task OnGet(int id)
        {
            Vehicules = await _db.Vehicules.Where(v => v.Id == id).SingleOrDefaultAsync();
            TypeDocsList = await _db.TypeDocs.ToListAsync();
            FournisseursList = await _db.Fournisseurs.ToListAsync();
        }




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
            {
                await OnGet(Docs.VehiculeId);
                return Page();
            }


            if (Docs.DateDebut >= Docs.DateFin)
            {
                check_date = true;
                await OnGet(Docs.VehiculeId);
                return Page();
            }


            if (!await InsertDocumentAsync())
                return Page();


            try
            {
                await _db.Docs.AddAsync(Docs);
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
                await OnGet(Docs.VehiculeId);
                return Page();
            }
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
    }
}
