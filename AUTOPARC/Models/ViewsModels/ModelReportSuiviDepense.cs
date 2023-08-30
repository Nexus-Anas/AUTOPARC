using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AUTOPARC.Models.ViewsModels
{
    public class ModelReportSuiviDepense
    {
        public int NumDepense { get; set; }
        public DateTime DateDepense { get; set; }
        public string Objet { get; set; }
        public decimal Montant { get; set; }
    }
}
