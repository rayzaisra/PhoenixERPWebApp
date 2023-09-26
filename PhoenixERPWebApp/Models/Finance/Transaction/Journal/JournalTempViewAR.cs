using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction
{
    public class JournalTempViewAR
    {
        [DisplayName("Account Code")]
        public string AccountCode { get; set; }
        [DisplayName("Account Name")]
        public string AccountName { get; set; }
        [DisplayName("Description")]
        public string AccountDescription { get; set; }
        [DisplayName("Division")]
        public string DivisionName { get; set; }
        [DisplayName("Legal Entity")]
        public string LegalName { get; set; }

        public decimal? Debet { get; set; }
        public decimal? Credit { get; set; }
    }
}

