using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using static System.Collections.Specialized.BitVector32;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AUTOPARC.Pages.Societe
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
        public Societes Societes { get; set; }

        public List<Villes> VillesList { get; set; }

        public bool check_exception, societe_exist;



        public async Task OnGet()
        {
            VillesList = await _db.Villes.ToListAsync();
            societe_exist = await _db.Societes.AnyAsync();
        }




        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
            {
                await OnGet();
                return Page();
            }

            try
            {
                if (!await _db.Societes.AnyAsync())
                    Societes.SocieteParDefault = true;
                else
                {
                    Societes.SocieteParDefault = Request.Form.TryGetValue("SocieteParDefault", out var def);
                    if (Societes.SocieteParDefault)
                        await _db.Societes.ForEachAsync(s => s.SocieteParDefault = false);
                }

                if (!await InsertLogoAsync())
                    return Page();

                await _db.Societes.AddAsync(Societes);
                await _db.SaveChangesAsync();
                return RedirectToPage("/Societe/Index");
            }
            catch (Exception)
            {
                check_exception = true;
                await OnGet();
                return Page();
            }
        }



        private async Task<bool> InsertLogoAsync()
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
                return true;
            }
            return true;
        }
    }
}
