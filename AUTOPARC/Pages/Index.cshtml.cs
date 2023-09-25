using AUTOPARC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DBC _db;

        public IndexModel(ILogger<IndexModel> logger, DBC db)
        {
            _logger = logger;
            _db = db;
        }

        public int VehiculesCount { get; set; }
        public int UsersCount { get; set; }
        public int VehiculesVendus { get; set; }
        public decimal AchatTotaux { get; set; }




        public async Task OnGet()
        {
            VehiculesCount = await _db.Vehicules.CountAsync();
            UsersCount = await _db.Users.CountAsync();
            VehiculesVendus = await _db.Vehicules.Where(v => v.EtatVehiculeId == 9).CountAsync();
            AchatTotaux = await _db.Vehicules.SumAsync(v => v.PrixAchat);
        }
    }
}
