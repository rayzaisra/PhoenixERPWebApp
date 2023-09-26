using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models
{
    public class RptExpensesAffiliationFilter
    {
        public string AffiliationId { get; set; }
        public DateTime? EndingDate { get; set; }
        public string CurrencyId { get; set; }
        //public bool DisplayZeroBalance { get; set; }
        public string CoaLevel { get; set; }
    }
}
