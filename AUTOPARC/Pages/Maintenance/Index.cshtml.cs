using AUTOPARC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Pages.Maintenance
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        public IndexModel(DBC db) => _db = db;




        public List<Maintenances> MaintenancesList { get; set; }
        public List<TypeMaintenances> TypeMaintenancesList { get; set; }
        public List<OperationMaintenances> OperationMaintenancesList { get; set; }
        public List<Fournisseurs> FournisseursList { get; set; }
        public List<Vehicules> VehiculesList { get; set; }
        public List<Chauffeurs> ChauffeursList { get; set; }
        public List<Cheques> ChequesList { get; set; }
        public List<Virements> VirementsList { get; set; }
        public List<Credits> CreditsList { get; set; }

        private const string _action = "Maintenance";




        public async Task OnGet()
        {
            MaintenancesList = await _db.Maintenances.ToListAsync();
            TypeMaintenancesList = await _db.TypeMaintenances.ToListAsync();
            OperationMaintenancesList = await _db.OperationMaintenances.ToListAsync();
            FournisseursList = await _db.Fournisseurs.ToListAsync();
            VehiculesList = await _db.Vehicules.ToListAsync();
            ChauffeursList = await _db.Chauffeurs.ToListAsync();
            ChequesList = await _db.Cheques.Where(chq => chq.Action == _action).ToListAsync();
            VirementsList = await _db.Virements.Where(v => v.Action == _action).ToListAsync();
            CreditsList = await _db.Credits.Where(c => c.Action == _action).ToListAsync();
        }
    }
}
