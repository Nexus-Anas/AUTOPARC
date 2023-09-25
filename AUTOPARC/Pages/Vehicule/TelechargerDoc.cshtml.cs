using AUTOPARC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Vehicule
{
    public class TelechargerDocModel : PageModel
    {
        private readonly DBC _db;
        private readonly IWebHostEnvironment _env;
        public TelechargerDocModel(DBC db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }




        public List<ImageVehicule> ImageVehiculesList { get; set; }



        public async Task OnGet(int id)
            => ImageVehiculesList = await _db.ImageVehicule.Where(i => i.VehiculeNum == id).ToListAsync();

        





        public async Task<IActionResult> OnPostDownload(int id)
        {
            var url = await _db.ImageVehicule.Where(i => i.Id == id).Select(i => i.Url).SingleOrDefaultAsync();
            if (!string.IsNullOrEmpty(url))
            {
                var filePath = Path.Combine(_env.WebRootPath, "images", url);

                if (System.IO.File.Exists(filePath))
                {
                    var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    var contentType = "application/octet-stream"; // You may need to set the appropriate content type

                    var fileName = url.Substring(url.LastIndexOf("_") + 1);
                    return File(fileStream, contentType, fileName);
                }
            }

            return RedirectToPage("/Vehicule/Index");
        }
    }
}
