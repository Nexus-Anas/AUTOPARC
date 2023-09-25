using AUTOPARC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Document
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




        public List<UrlDocs> UrlDocsList { get; set; }



        public async Task OnGet(int id)
            => UrlDocsList = await _db.UrlDocs.Where(u => u.Action == "Document" && u.ActionNum == id).ToListAsync();







        public async Task<IActionResult> OnPostDownload(int id)
        {
            var url = await _db.UrlDocs.Where(u => u.Id == id).Select(u => u.Url).SingleOrDefaultAsync();
            if (!string.IsNullOrEmpty(url))
            {
                var filePath = Path.Combine(_env.WebRootPath, "Docs", "Documents", url);

                if (System.IO.File.Exists(filePath))
                {
                    var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    var contentType = "application/octet-stream"; // You may need to set the appropriate content type

                    var fileName = url.Substring(url.LastIndexOf("_") + 1);
                    return File(fileStream, contentType, fileName);
                }
            }

            return NotFound(); // Document not found
        }
    }
}
