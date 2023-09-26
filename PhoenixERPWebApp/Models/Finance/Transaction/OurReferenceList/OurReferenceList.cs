using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction
{
    public class OurReferenceList
    {
        public string Id { get; set; }
        public string MCSNumber { get; set; }
        public DateTime? Date { get; set; }
        public string InvoiceNumber { get; set; }
    }
}
