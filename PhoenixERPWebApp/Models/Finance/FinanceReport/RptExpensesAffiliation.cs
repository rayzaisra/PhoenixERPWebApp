using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class RptExpensesAffiliationFilter
    {
        [DisplayName("Affiliation")]
        public string AffiliationId { get; set; }

        [DisplayName("Type")]
        public string Type { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndingDate { get; set; }

        [DisplayName("Currency")]
        public string CurrencyId { get; set; }
        //public bool DisplayZeroBalance { get; set; }

        [DisplayName("COA Level")]
        public string CoaLevel { get; set; }
    }
}
