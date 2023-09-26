using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models
{
    public class RptInterCompanyReconciliation
    {
        public Guid? id { get; set; }
        public string company_name { get; set; }
        public string legal_entity_name { get; set; }
        public decimal? ap { get; set; }
        public decimal? ar { get; set; }
        public decimal? balance { get; set; }
        public string remarks { get; set; }
    }

    public class RptInterCompanyReconciliationFilter
    {
        public DateTime? EndingDate { get; set; }
        public string CurrencyId { get; set; }
        public List<string> MultiLegalEntityId { get; set; }
        public List<string> MultiCompanyId { get; set; }
    }
}
