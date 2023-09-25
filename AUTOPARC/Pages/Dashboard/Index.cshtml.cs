using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;

        public IndexModel(DBC db)
        {
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
