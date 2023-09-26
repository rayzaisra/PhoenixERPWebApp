using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.StatementOfAccountPayable
{
    public class StatementOfAccountPayableList : FinanceEntityBase
    {
        public string Id { get; set; }
        public string Vendor { get; set; }
        public string Brand { get; set; }
        public string EvidenNo { get; set; }
        public DateTime Date { get; set; }
        public string JobId { get; set; }
        public string Division { get; set; }
        public string Support { get; set; }
        public string Description { get; set; }
        public decimal Debet { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
    }
}
