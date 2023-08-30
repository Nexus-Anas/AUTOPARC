using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUTOPARC.Models;
using AUTOPARC.Models.ViewsModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AspNetCore.Reporting;

namespace AUTOPARC.Pages.SuiviDepense
{
    public class IndexModel : PageModel
    {
        private readonly DBC _db;
        private readonly IWebHostEnvironment _env;
        public IndexModel(DBC db , IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
       

        public List<Suividepense> Suividepenses { get; set; }
        public List<ModePaiments> ModePaiments { get; set; }

        public async Task OnGet()
        {
            Suividepenses = await _db.Suividepense.ToListAsync();
            ModePaiments = await _db.ModePaiments.ToListAsync();
        }
        public  async Task<IActionResult> OnPostImprimer()
        {
            Suividepenses = await _db.Suividepense.ToListAsync();
            var ListModelReportSuiviDepenses = new List<ModelReportSuiviDepense>();
            if (Suividepenses.Count != 0)
            {
                foreach (var item in Suividepenses)
                {
                    ModelReportSuiviDepense modeletat = new ModelReportSuiviDepense
                    {
                        NumDepense = item.NumDepense,
                        DateDepense = item.DateDepense,
                        Montant = item.Montant,
                        Objet = item.Objet
                    };
                    ListModelReportSuiviDepenses.Add(modeletat);
                }
            }
            string mimtype = "";
            int extension = 1;
            var path = $"{_env.WebRootPath}\\Etats\\Report_SuiviDepense.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", ListModelReportSuiviDepenses);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);
            return File(result.MainStream, "application/pdf");

        }
   }
}
