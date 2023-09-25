using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AUTOPARC.Pages.Societe
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
        public Societes Societes { get; set; }
        public List<Villes> VillesList { get; set; }

        public bool check_exception, only_one_company, societe_existe;
        public int company_count;




        public async Task OnGet(int id)
        {
            Societes = await _db.Societes.FindAsync(id);
            VillesList = await _db.Villes.ToListAsync();
            company_count = await _db.Societes.CountAsync();
        }




        public async Task<IActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var societe = await _db.Societes.FindAsync(Societes.Id);
                societe.RaisonSocial = Societes.RaisonSocial;
                societe.VilleId = Societes.VilleId;
                societe.Adresse = Societes.Adresse;
                societe.Bas1 = Societes.Bas1;
                societe.Bas2 = Societes.Bas2;

                if (await ChangeLogoAsync())
                    societe.LogoUrl = Societes.LogoUrl;

                if (!societe.SocieteParDefault)
                {
                    Societes.SocieteParDefault = Request.Form.TryGetValue("SocieteParDefault", out var def);
                    if (Societes.SocieteParDefault)
                        await _db.Societes.ForEachAsync(s => s.SocieteParDefault = false);
                    societe.SocieteParDefault = Societes.SocieteParDefault;
                }

                await _db.SaveChangesAsync();
                return RedirectToPage("/Societe/Index");
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
                return Page();

            if (await _db.Societes.CountAsync() ==  1)
            {
                only_one_company = true;
                await OnGet(Societes.Id);
                return Page();
            }

            if (Societes.SocieteParDefault)
            {
                var societe = await _db.Societes.Where(s => s.Id != Societes.Id).FirstOrDefaultAsync();
                societe.SocieteParDefault = true;
            }

            var vehichule = await _db.Vehicules.Where(v => v.SocieteId == Societes.Id).FirstOrDefaultAsync();
            var depense = await _db.Suividepense.Where(d => d.SocieteId == Societes.Id).FirstOrDefaultAsync();
            if (vehichule != null || depense != null)
            {
                societe_existe = true;
                await OnGet(Societes.Id);
                return Page();
            }

            _db.Societes.Remove(Societes);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Societe/Index");
        }






        private async Task<bool> ChangeLogoAsync()
        {
            try
            {
                var file = HttpContext.Request.Form.Files[0];

                if (file != null)
                {
                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var filePath = Path.Combine(_env.WebRootPath, "images", "logo", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await file.CopyToAsync(stream);

                    Societes.LogoUrl = fileName;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
